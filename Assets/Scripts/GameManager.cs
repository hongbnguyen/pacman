using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public int _playerScore;

    public static string initials_input;
    public List<string> player_pos = new List<string>();
    public int trialNum;
    public string trialName;
    public List<string> trials;

    private string sceneName;

    public float trialTimer = 0;

    private bool timerIsActive = true;

    public string playerWinOrLose;

    public string combinedString;
    public GameObject player;

    public int inFullScreen;

    private bool doItOnce = false;
    private GameObject pacman;    

    void Awake()
    {
        inFullScreen = 1;
    }

    void Start()
    {
        trialNum = GlobalControl.Instance.trialNum;
        trialName = GlobalControl.Instance.trialName;
        trials = GlobalControl.Instance.trials;
        initials_input = SaveInitials.name;
        pacman = GameObject.Find("pacman");
        _playerScore = 0;

        InvokeRepeating("Player_Y", 0f, 0.5f);  //0 delay, repeat every 500ms
    }

    void Player_Y() {
        float pX = player.transform.position.x;
        float pY = player.transform.position.y;
        player_pos.Add("[" + pX.ToString() + "," + pY.ToString() + "]");
    }
        

    void Update()
    {
        if (Screen.fullScreen == false)
        {
            inFullScreen = 0;
        }

        if (timerIsActive) 
        {
            trialTimer += Time.deltaTime;
        }

        if (doItOnce == false)
        {
            if (pacman == null || _playerScore == 334)
            {
                doItOnce = true;
                ResetRound();
            }
        }

    }

    
    public void SaveGame()
    {
        GlobalControl.Instance.trialNum = trialNum;
        GlobalControl.Instance.trialName = trialName;
        GlobalControl.Instance.trials = trials;
    }


    public void PlayerScores()
    {
        //Tinylytics.AnalyticsManager.LogCustomMetric("Player Score", _playerScore.ToString());
        //_playerScore++;
    }

    private void ResetRound()
    {
        if (pacman == null)
        {
            playerWinOrLose = "lose";
        }
        if (_playerScore == 334)
        {
            playerWinOrLose = "win";
        }
        trialNum = trialNum + 1;

        SaveGame();
        newTrial();
        timerIsActive = false;

        string str = string.Join(",", player_pos);
        //Debug.Log(str);

        Tinylytics.AnalyticsManager.LogCustomMetric(initials_input + "_" + trialNum.ToString() + "_" + trials[trialNum-1] + "_" + inFullScreen.ToString(), _playerScore + "_" + playerWinOrLose + "_" + trialTimer.ToString() + "_" + str);

        //Tinylytics.AnalyticsManager.LogCustomMetric("FullSreen" , inFullScreen.ToString());
    }

    void newTrial()
    {
        Debug.Log(trialNum);
        if (trialNum < trials.Count)
        {
            trialName = trials[trialNum];
            SaveGame();

            sceneName = "interstitial"; //this name is used in the Coroutine, which is basically just a pause timer for 3 seconds.

            StartCoroutine(WaitForSceneLoad());
        }
        else { endGame(); }


    }

    void endGame()
    {
        //if you want to know how lond the entire set of trials took, you can add your tinyLytics call here
        sceneName = "ending"; //this name is used in the Coroutine, which is basically just a pause timer for 3 seconds.
        StartCoroutine(WaitForSceneLoad());

    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(sceneName);
    }

}
