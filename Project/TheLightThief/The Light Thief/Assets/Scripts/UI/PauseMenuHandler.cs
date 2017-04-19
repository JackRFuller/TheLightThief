using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour {

    [SerializeField]
    private GameObject pauseMenuObjs;

	// Use this for initialization
	void Start ()
    {
        pauseMenuObjs.SetActive(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!MainMenuHandler.isInMainMenuMode)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (pauseMenuObjs.activeInHierarchy)
                {
                    pauseMenuObjs.SetActive(false);
                    Time.timeScale = 1;
                }
                else
                {
                    pauseMenuObjs.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
	}

    public void Resume()
    {
        pauseMenuObjs.SetActive(false);
    }
 
    public void QuitGame()
    {
        Application.Quit();
    }
}
