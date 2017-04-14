using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public void StartUpNewGame()
    {
        levelIndex = 0;
    }

    public void LoadInGame()
    {
        levelIndex = PlayerPrefs.GetInt("CurrentLevel");
    }

    public void LoadInNextLevel()
    {
        //Check if we're at the last level
        if(levelIndex < levelList.Length - 1)
        {
            levelIndex++;
            PlayerPrefs.SetInt("CurrentLevel", levelIndex);
            CameraMovementHandler.Instance.InitCameraMovement();
            StartCoroutine(WaitToLoadInLevel());
        }
        else
        {
            //Show EndScreen
            CameraMovementHandler.Instance.InitCameraMovement();
            MainMenuHandler.Instance.BringInEndScreen();
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

        if(currentLevelObj != null)
        {
            currentLevelObj.SetActive(false);
        }

        Vector3 levelSpawnPos = new Vector3(0, (levelIndex * -36.0f) - 36.0f, 0);
        currentLevelObj = Instantiate(currentLevel.levelPrefab, levelSpawnPos, Quaternion.identity) as GameObject;

        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

}
