using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    //Called when the player acquires a key
    public static string CollectedKey = "CollectedKey";

    //Called from Platforms have changed position
    public static string RecalibrateNodes = "RecalibrateNodes";

    //Used to determine whether the player can start moving or not
    public static string EnablePlayerMovement = "EnablePlayerMovement";
    public static string DisablePlayerMovement = "DisablePlayerMovement";
	
}
