using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class MainMenuHandler : MonoSingleton<MainMenuHandler>
{
    //Components
    private Animation mainMenuAnim;
    private AudioSource mainMenuAudio;

    [Header("UI Elements - Main Menu")]
    [SerializeField]
    private GameObject mainMenuObjs;
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Text continueText;
    [SerializeField]
    private GameObject sendFeedbackButton;

    [Header("UI Elements - End Screen")]
    [SerializeField]
    private GameObject endScreenObjs;

    [Header("Pause Screen")]
    [SerializeField]
    private GameObject pauseScreenPrefab;
    private GameObject pauseScreen;

    public static bool isInMainMenuMode;

    private int currentLevel = 0;

    private void Start()
    {
        //Get Components
        mainMenuAnim = this.GetComponent<Animation>();
        mainMenuAudio = this.GetComponent<AudioSource>();

        mainMenuObjs.SetActive(true);
        endScreenObjs.SetActive(false);

        currentLevel = PlayerPrefs.GetInt("CurrentLevel");

        //If we haven't played before - disable continue
        if(currentLevel == 0)
        {
            DisableContinueButton();
        }

        if(PlayerPrefs.GetInt("HasCompletedGame") == 1)
        {
            sendFeedbackButton.SetActive(true);
        }
        else
        {
            sendFeedbackButton.SetActive(false);
        }

        isInMainMenuMode = true;
    }

    public void NewGame()
    {
        //Load in Pause Screen
        if (pauseScreen == null)
            pauseScreen = Instantiate(pauseScreenPrefab) as GameObject;

        mainMenuAnim.Play("MainMenuUp");        
        LevelManager.Instance.StartUpNewGame();        

        isInMainMenuMode = false;
        Analytics.CustomEvent("NewGame", new Dictionary<string, object>
        {
            {"New Game",1}
        }
        );
    }

    public void Continue()
    {
        //Load in Pause Screen
        if (pauseScreen == null)
            pauseScreen = Instantiate(pauseScreenPrefab) as GameObject;

        mainMenuAnim.Play("MainMenuUp");

        LevelManager.Instance.LoadInGame();
        mainMenuAudio.Play();
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

    public IEnumerator WaitToBringInMainMenu()
    {
        yield return new WaitForSeconds(3.0f);
        endScreenObjs.SetActive(true);
        mainMenuAnim.Play("MainMenuDown");
        isInMainMenuMode = true;
    }

    #endregion
}
