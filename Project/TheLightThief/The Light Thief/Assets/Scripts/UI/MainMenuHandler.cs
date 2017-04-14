using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoSingleton<MainMenuHandler>
{
    //Components
    private Animation mainMenuAnim;

    [Header("UI Elements - Main Menu")]
    [SerializeField]
    private GameObject mainMenuObjs;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Text continueText;

    [Header("UI Elements - End Screen")]
    [SerializeField]
    private GameObject endScreenObjs;

    private int currentLevel = 0;

    private void Start()
    {
        //Get Components
        mainMenuAnim = this.GetComponent<Animation>();

        mainMenuObjs.SetActive(true);
        endScreenObjs.SetActive(false);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        //If we haven't played before - disable continue
        if(currentLevel == 0)
        {
            DisableContinueButton();
        }
    }

    public void NewGame()
    {
        mainMenuAnim.Play("MainMenuUp");
        CameraMovementHandler.Instance.InitCameraMovement();
        LevelManager.Instance.StartUpNewGame();
        LevelManager.Instance.LoadInLevel();
    }

    public void Continue()
    {
        LevelManager.Instance.LoadInGame();
        LevelManager.Instance.LoadInLevel();
    }

    public void Credits()
    {
        if(!mainMenuAnim.isPlaying)
            mainMenuAnim.Play("CreditsIn");
    }

    public void ExitCredits()
    {
        if (!mainMenuAnim.isPlaying)
            mainMenuAnim.Play("CreditsOut");
    }

	public void Quit()
    {
        Application.Quit();
    }

    public void EmailFeedback()
    {
        string email = "jackrfuller@outlook.com";
        string subject = MyEscapeURL("The Light Thief Feedback");

        Application.OpenURL("mailto:" + email + "?subject" + subject);
        Application.Quit();
    }

    private string MyEscapeURL(string url)
    {
        return WWW.EscapeURL(url).Replace("+", "%20");
    }

    ///////
    private void DisableContinueButton()
    {
        continueButton.enabled = false;
        continueText.color = Color.grey;
    }
    
    #region EndScreen

    public void BringInEndScreen()
    {
        StartCoroutine(WaitToBringInEndScreen());
    }

    IEnumerator WaitToBringInEndScreen()
    {
        yield return new WaitForSeconds(4.0f);
        endScreenObjs.SetActive(true);
        mainMenuAnim.Play("EndScreenIn");
    }

    public void RemoveEndScreen()
    {
        mainMenuAnim.Play("EndScreenOut");
        StartCoroutine(WaitToBringInMainMenu());
    }

    IEnumerator WaitToBringInMainMenu()
    {
        yield return new WaitForSeconds(3.0f);
        endScreenObjs.SetActive(true);
        mainMenuAnim.Play("MainMenuDown");
    }

    #endregion
}
