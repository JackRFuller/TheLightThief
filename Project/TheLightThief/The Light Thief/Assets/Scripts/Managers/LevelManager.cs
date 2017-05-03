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

    //Level Spawning Attributes
    private Vector3 levelSpawnPosition;
    private Vector3 levelSpawnSize;

    public void LoadInNextLevel()
    {
        //Get Level Attributes
        Transform levelSpawn = LevelHandler.Instance.NextLevelTarget;
        levelSpawnPosition = new Vector3(levelSpawn.position.x, levelSpawn.position.y, 0);
        levelSpawnSize = LevelHandler.Instance.NextLevelSpawnSize;

        //Destroy Current Level
        Destroy(LevelHandler.Instance.gameObject);

        //Spawn In Next Level - Temp Logic
        currentLevelObj = Instantiate(currentLevel.levelPrefab, levelSpawnPosition, Quaternion.identity) as GameObject;

        currentLevelObj.GetComponent<LevelHandler>().InitGrowLevel(levelSpawnSize);
        
    }
}
