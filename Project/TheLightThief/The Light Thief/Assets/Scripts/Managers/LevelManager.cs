using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private Level currentLevel;
    public Level CurrentLevel { get { return currentLevel; } }

    [SerializeField]
    private Level[] levelList;

}
