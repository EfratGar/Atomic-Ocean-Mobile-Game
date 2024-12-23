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
            return;
        }

        AsyncOperation loadLevelOperation = 
            SceneManager.LoadSceneAsync(GetLevelSceneName(levelIndex), LoadSceneMode.Additive);
        loadLevelOperation.completed += (_) => CalculateNumberOfMonstersInLevel();
    }

    private async void OnMonsterDied()
    {
        _numberOfMonsters--;
        CalculateNumberOfMonstersInLevel();
        if (_numberOfMonsters <= 0)
        {
            await Task.Delay(2000);
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
    }

    private bool DoesSceneExist(int levelIndex)
    {
        string scenePath = SceneUtility.GetScenePathByBuildIndex(levelIndex);
        int loadedSceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath);
        return loadedSceneBuildIndex > 0;
    }
}

