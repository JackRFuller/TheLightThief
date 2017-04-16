using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class LevelManager : MonoSingleton<LevelManager>
{
    [Header("Levels")]
    [SerializeField]
    private Level currentLevel;
    public Level CurrentLevel { get { return currentLevel; } }
    private int levelIndex;
    private GameObject currentLevelObj;

    [SerializeField]
    private Level[] levelList;

    [Header("Player")]
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject player;
    public Transform Player { get { return player.transform; } }

    private int numberOfLevelsPlayed;

    private List<GameObject> oldLevels = new List<GameObject>();


    public void StartUpNewGame()
    {
        levelIndex = 0;
        numberOfLevelsPlayed = 0;

        CameraMovementHandler.Instance.InitCameraMovement(false);
        StartCoroutine(WaitToLoadInLevel());
    }

    public void LoadInGame()
    {
        numberOfLevelsPlayed = 0;
        levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        Debug.Log(levelIndex);
        for(int i = 0; i < oldLevels.Count; i++)
        {
            Destroy(oldLevels[i]);
        }

        oldLevels.Clear();

        CameraMovementHandler.Instance.InitCameraMovement(false);
        StartCoroutine(WaitToLoadInLevel());
    }

    public void LoadInNextLevel()
    {
        //Check if we're at the last level
        if(levelIndex < levelList.Length - 1)
        {
            levelIndex++;
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            Analytics.CustomEvent("Started Level", new Dictionary<string, object>
        {
            {"Level",levelIndex}
        });
            CameraMovementHandler.Instance.InitCameraMovement(false);
            StartCoroutine(WaitToLoadInLevel());
        }
        else
        {
            Analytics.CustomEvent("Completed Game", new Dictionary<string, object>
        {
            {"Completed Game",1}
        });
            //Show EndScreen
            CameraMovementHandler.Instance.InitCameraMovement(false);
            MainMenuHandler.Instance.BringInEndScreen();
            PlayerPrefs.SetInt("HasCompletedGame", 1);
        }
    }

    IEnumerator WaitToLoadInLevel()
    {
        yield return new WaitForSeconds(5.0f);
        LoadInLevel();
    }

    public void LoadInLevel()
    {
        currentLevel = levelList[levelIndex];
        numberOfLevelsPlayed++;
        if (currentLevelObj != null)
        {
            currentLevelObj.SetActive(false);
        }

        oldLevels.Add(currentLevelObj);

        Vector3 levelSpawnPos = new Vector3(0, (numberOfLevelsPlayed * -36.0f), 0);
        currentLevelObj = Instantiate(currentLevel.levelPrefab, levelSpawnPos, Quaternion.identity) as GameObject;
        oldLevels.Add(currentLevelObj);
        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

}
