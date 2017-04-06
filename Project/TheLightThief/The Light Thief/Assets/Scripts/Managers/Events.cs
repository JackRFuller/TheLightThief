using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    //Called from Platforms have changed position
    public static string RecalibrateNodes = "RecalibrateNodes";

    //Used to determine whether the player can start moving or not
    public static string EnablePlayerMovement = "EnablePlayerMovement";
    public static string DisablePlayerMovement = "DisablePlayerMovement";
	
}
