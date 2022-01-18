using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject backgroundImage;
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject werSpielt;
    public GameObject gameUi;
    public GameObject gameOverScreen;
    public InputField selectName;
    public Text namePlaceholder;
    public Text gameOverName;
    public Text gameOverScore;
    public Text[] highScoreNames;
    public Text[] highScoreValues;
    public static GameManager singleton;
    public Text CurrentScore;
    public Text PlayTime;
    private int score = 0;
    public int debugScore = 10000;
    public int timeScore;
    public float scoreInterval = 10f;
    private bool isPaused;
    private string playerName;
    private bool hasStarted;

    

    private void Awake()
    {        
        Time.timeScale = 0f;
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != null)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        hasStarted = false;
        for(int i = 1; i <= 5; i++)
        {
            Debug.Log(PlayerPrefs.GetString("highScoreName_" + i));
            Debug.Log(PlayerPrefs.GetInt("highScoreValue_" + i).ToString());            
        }

        if (PlayerPrefs.GetString("highScoreName_1") == "")
        {
            ResetHighScores();
        }
        SetHighscores();
    }

    private void SetHighscores()
    {
        for (int i = 1; i <= 5; i++)
        {
            if(GetScore() > PlayerPrefs.GetInt("highScoreValue_" + i))
            {
                for (int x = 5; x > i; x--)
                {
                    int value = PlayerPrefs.GetInt("highScoreValue_" + (x - 1));
                    string name = PlayerPrefs.GetString("highScoreName_" + (x - 1));
                    PlayerPrefs.SetInt("highScoreValue_" + x, value);
                    PlayerPrefs.SetString("highScoreName_" + x, name);
                    Debug.Log("Test" + x);
                }

                PlayerPrefs.SetInt("highScoreValue_" + i, GetScore());
                PlayerPrefs.SetString("highScoreName_" + i, playerName);
                break;
            }            
        }

        for (int i = 1; i <= 5; i++)
        {
            highScoreValues[i - 1].text = PlayerPrefs.GetInt("highScoreValue_" + i).ToString();
            highScoreNames[i - 1].text = PlayerPrefs.GetString("highScoreName_" + i);
        }
    }

    public void ResetHighScores()
    {
        PlayerPrefs.SetString("highScoreName_1", "Sarah");
        PlayerPrefs.SetInt("highScoreValue_1", 10000);

        PlayerPrefs.SetString("highScoreName_2", "Paul");
        PlayerPrefs.SetInt("highScoreValue_2", 9000);

        PlayerPrefs.SetString("highScoreName_3", "Andreas");
        PlayerPrefs.SetInt("highScoreValue_3", 8000);

        PlayerPrefs.SetString("highScoreName_4", "Chris");
        PlayerPrefs.SetInt("highScoreValue_4", 7000);

        PlayerPrefs.SetString("highScoreName_5", "Eva");
        PlayerPrefs.SetInt("highScoreValue_5", 6000);

        for (int i = 1; i <= 5; i++)
        {
            highScoreValues[i - 1].text = PlayerPrefs.GetInt("highScoreValue_" + i).ToString();
            highScoreNames[i - 1].text = PlayerPrefs.GetString("highScoreName_" + i);
        }
    }

    public void OnClickReset()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickStart()
    {
        if (selectName.text == "")
        {
            Debug.Log("Name is empty");
            namePlaceholder.color = Color.red;
        }
        else
        {
            playerName = selectName.text;
            backgroundImage.SetActive(false);
            mainMenu.SetActive(false);
            werSpielt.SetActive(false);
            gameUi.SetActive(true);
            Time.timeScale = 1f;
            isPaused = false;
            score += debugScore;
            InvokeRepeating("ScoreUpWithTime", scoreInterval, scoreInterval);
            hasStarted = true;
        }
            
       
    }

    private void Update()
    {
        ShowTime();
        ShowScore();

        OnEscapePauseOrResume();      
    }

    public void ScoreUpWithTime()
    {
        SetScore(GetScore() + timeScore);
    }

    public void OnEscapePauseOrResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && hasStarted == true && isPaused == false)
        {
            Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            OnClickResume();
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    public void OnClickResume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OnClickToMainMenu()
    {
        SceneManager.LoadScene(0);        
    }

    public void ShowTime()
    {
        int min = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60);
        int sec = Mathf.FloorToInt(Time.timeSinceLevelLoad % 60);
        
        if (sec < 10)
        {
            PlayTime.text = "Time: " + min + ":0" + sec;
        }
        else
        {
            PlayTime.text = "Time: " + min + ":" + sec;
        }       
    }

    public void SetScore(int value)
    {
        score = value;
    }
    public int GetScore()
    {
        return score;
    }
    public void ShowScore()
    {
        CurrentScore.text = "Score: " + score; 
    }

    public void OnDeath()
    {
        Invoke("GameOver", 2);        
    }

    public void GameOver()
    {
        gameUi.SetActive(false);
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;

        gameOverName.text = playerName;
        gameOverScore.text = GetScore().ToString();

        SetHighscores();
        hasStarted = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
