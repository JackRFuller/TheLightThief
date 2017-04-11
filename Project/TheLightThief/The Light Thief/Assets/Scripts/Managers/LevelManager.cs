using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private Level currentLevel;
    public Level CurrentLevel { get { return currentLevel; } }
    private int levelIndex;
    private GameObject currentLevelObj;

    [SerializeField]
    private Level[] levelList;

    public void LoadInLevel()
    {
        if(currentLevelObj != null)
        {
            currentLevelObj.SetActive(false);
        }

        Vector3 levelSpawnPos = new Vector3(0, (levelIndex * -20.0f) - 20.0f, 0);
        currentLevelObj = Instantiate(currentLevel.levelPrefab, levelSpawnPos, Quaternion.identity) as GameObject;

        EventManager.TriggerEvent(Events.RecalibrateNodes);
    }

}
