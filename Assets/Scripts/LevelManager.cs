using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int _numberOfMonsters;
    [SerializeField] private int levelIndex;
    private const string BaseLevelSceneName = "Level";
    private const int GameOverSceneIndex = 8;
    [SerializeField] private float delayBetweenLevels;

    public event Action LevelCompleted = delegate { };
    public event Action GameOverSceneLoaded = delegate { };

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Monster.OnMonsterDied += OnMonsterDied;
        LoadLevel(levelIndex, 0);
        PlayableCharacter player = FindObjectOfType<PlayableCharacter>();

        if (player != null)
        {
            player.YouDiedPopUp += YouDiedSceneLoad;
        }
    }

    private AsyncOperation UnloadCurrentLevel()
    {
        Debug.Log("The scene that's going to be unloaded is " + GetLevelSceneName(levelIndex));
        AsyncOperation unloadLevelOperation = SceneManager.UnloadSceneAsync(GetLevelSceneName(levelIndex));
        return unloadLevelOperation;
    }

    private void OnMonsterDied()
    {
        _numberOfMonsters--;
        CalculateNumberOfMonstersInLevel();
        if (_numberOfMonsters <= 0)
        {
            LevelCompleted();
            int nextLevelIndex = levelIndex + 1;
            AsyncOperation unloadLevelOperation = UnloadCurrentLevel();
            unloadLevelOperation.completed += (_) => LoadLevel(nextLevelIndex, delayBetweenLevels);
        }
    }

    private async void YouDiedSceneLoad()
    {
        await Task.Delay(2000);
        AsyncOperation loadLevelOperation =
            SceneManager.LoadSceneAsync(GetLevelSceneName(GameOverSceneIndex), LoadSceneMode.Additive);
        loadLevelOperation.completed += (_) =>
        {
            GameOverSceneLoaded();
            RetryButton retryButton = FindFirstObjectByType<RetryButton>();
            retryButton.GameRestarted += () =>
            {
                //SceneManager.UnloadSceneAsync(GetLevelSceneName(GameOverSceneIndex));
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
        };
    }


    private async void LoadLevel(int nextLevelIndex, float delay)
    {
        if(delay > 0)
            await Task.Delay(TimeSpan.FromSeconds(delay));

        levelIndex = nextLevelIndex;
        AsyncOperation loadLevelOperation =
            SceneManager.LoadSceneAsync(GetLevelSceneName(levelIndex), LoadSceneMode.Additive);
        loadLevelOperation.completed += (_) => CalculateNumberOfMonstersInLevel();
    }

    private string GetLevelSceneName(int levelIndex)
    {
        return BaseLevelSceneName + levelIndex;
    }

    private void CalculateNumberOfMonstersInLevel()
    {
        _numberOfMonsters = FindObjectsByType<Monster>(FindObjectsSortMode.None).Length;
    }

    //private bool DoesSceneExist(int levelIndex)
    //{
    //    string scenePath = SceneUtility.GetScenePathByBuildIndex(levelIndex);
    //    int loadedSceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
    //    return loadedSceneBuildIndex > 0;
    //}

    public void MoveToActiveScene(GameObject objectToMove)
    {
        SceneManager.MoveGameObjectToScene(objectToMove, SceneManager.GetActiveScene());
    }
}

