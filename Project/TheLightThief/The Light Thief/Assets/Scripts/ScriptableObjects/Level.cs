using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level,", order = 1)]
public class Level : ScriptableObject
{
    public GameObject levelPrefab;
    [Range(0, 5)]
    public int numberOfKeysToCollect;


}
