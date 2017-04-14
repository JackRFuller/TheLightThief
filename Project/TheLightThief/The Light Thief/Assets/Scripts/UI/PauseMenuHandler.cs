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
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuObjs.activeInHierarchy)
                pauseMenuObjs.SetActive(false);
            else
                pauseMenuObjs.SetActive(true);
        }
	}

    public void Resume()
    {
        pauseMenuObjs.SetActive(true);
    }

    public void ReturnToMainMenu()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
