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
    public InputField selectName;
    public Text namePlaceholder;
    public static GameManager singleton;
    public Text CurrentScore;
    public Text PlayTime;
    public int score;
    public float scoreInterval = 10f;
    public bool isPaused;


    private void Awake()
    {
        score = 0;
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
            backgroundImage.SetActive(false);
            mainMenu.SetActive(false);
            werSpielt.SetActive(false);
            gameUi.SetActive(true);

            Time.timeScale = 1f;
            isPaused = false;
            InvokeRepeating("ScoreUpWithTime", 10, 10);

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
        SetScore(GetScore() + 50);
    }

    public void OnEscapePauseOrResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false && mainMenu.activeSelf == false)
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
}
