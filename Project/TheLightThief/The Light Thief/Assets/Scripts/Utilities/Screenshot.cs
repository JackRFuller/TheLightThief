using System.Collections;
using UnityEngine;
using System.IO;

public class Screenshot : MonoBehaviour
{
    private int count;

	// Use this for initialization
	void Start ()
    {
        count = PlayerPrefs.GetInt("ScreenshotCount"); 	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F12))
            TakeScreenShot();

	}

    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();

        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        yield return 0;

        byte[] bytes = texture.EncodeToPNG();

        File.WriteAllBytes(Application.dataPath + "/../Screenshot " + count + ".png", bytes);
        count++;

        PlayerPrefs.SetInt("ScreenshotCount", count);
        DestroyObject(texture);
    }
}
