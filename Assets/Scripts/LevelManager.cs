using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _numberOfMonsters;
    [SerializeField] private int levelIndex;
    private const string BaseLevelSceneName = "Level";
    [SerializeField] private float delayBetweenLevels;
    [SerializeField] private GameObject victoryScreen; // Reference to the victory screen

    public event Action LevelCompleted = delegate { };

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Monster.OnMonsterDied += OnMonsterDied;
        LoadLevel(levelIndex); // Loads the first level
    }


    public void LoadLevel(int levelIndex)
    {
        if (!DoesSceneExist(levelIndex))
        {
            Debug.Log("You won");
            ShowVictoryScreen();
            return;
        }

        AsyncOperation loadLevelOperation = 
            SceneManager.LoadSceneAsync(GetLevelSceneName(levelIndex), LoadSceneMode.Additive);
        loadLevelOperation.completed += (_) => CalculateNumberOfMonstersInLevel();
    }

    private void OnMonsterDied()
    {
        _numberOfMonsters--;
        CalculateNumberOfMonstersInLevel();
        if (_numberOfMonsters <= 0)
        {
            LevelCompleted();
            // If there are no more levels, show the victory screen
            if (!DoesSceneExist(levelIndex + 1))
            {
                ShowVictoryScreen();
                return;
            }
            AsyncOperation unloadLevelOperation = 
                SceneManager.UnloadSceneAsync(GetLevelSceneName(levelIndex));
            unloadLevelOperation.completed += LevelUnloaded;
        }
    }

    private async void LevelUnloaded(AsyncOperation unloadLevelOperation)
    {
        await Task.Delay(TimeSpan.FromSeconds(delayBetweenLevels)); 
        levelIndex++;
        LoadLevel(levelIndex);
    }

    private string GetLevelSceneName(int levelIndex)
    {
        return BaseLevelSceneName + levelIndex;
    }

    private void CalculateNumberOfMonstersInLevel()
    {
        _numberOfMonsters = FindObjectsByType<Monster>(FindObjectsSortMode.None).Length;
        Debug.Log($"Number of monsters in level: {_numberOfMonsters}");
    }

    private bool DoesSceneExist(int levelIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(levelIndex);
        int loadedSceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        return loadedSceneBuildIndex > 0;
    }

    public void MoveToActiveScene(GameObject objectToMove)
    {
        SceneManager.MoveGameObjectToScene(objectToMove, SceneManager.GetActiveScene());
    }

    private void ShowVictoryScreen()
    {
        if (victoryScreen != null)
        {
            victoryScreen.SetActive(true); // Activate the victory screen
        }
        else
        {
            Debug.LogWarning("Victory screen is not assigned in the inspector!");
        }
    }
}

