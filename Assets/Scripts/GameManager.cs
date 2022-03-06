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
    public GameObject dontDestroy;
    public GameObject volSlider;
    public GameObject cam;
    public GameObject muteButton;
    public GameObject unmuteButton;
    public InputField selectName;
    public Text namePlaceholder;
    public Text gameOverName;
    public Text gameOverScore;
    public Text[] highScoreNames;
    public Text[] highScoreValues;

    // singleton sorgt dafür, dass auf Methoden des GameManagers einfach aus anderen Skripten zugegriffen werden kann 
    public static GameManager singleton;
    public Text CurrentScore;
    public Text PlayTime;
    private int score = 0;

    // Score der zum Debugging am Anfang addiert wird
    public int debugScore = 10000;

    // Score der über die Zeit addiert wird
    public int timeScore;

    // Zeit zwischen addierung des timeScore
    public float scoreInterval = 10f;

    // Ist das Spiel pausier?
    private bool isPaused;
    private string playerName;

    // Wurde das Spiel gestartet?
    private bool hasStarted;

    // Objekt, das bei neuladen der Szene nicht Zerstört wird
    private DontDestroy ddScript;

    private void Awake()
    {   
        // Speilzeit wird angehalten (Spiel hinter dem Menü wird pausiert)
        Time.timeScale = 0f;

        // Sorgt dafür, dass nur ein GameManager existieren kann, auch bei neuladen der Szene
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

        // weist ddScript das Skript des dontDestroy Objektes zu
        ddScript = dontDestroy.transform.GetComponent<DontDestroy>();

        // Wenn in den PlayerPrefs kein Key namens "Mute" vorhanden ist  oder der Wert für Mute auf unmuted steht
        if (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetString("Mute") == "unmuted")
        {
            // Aufrufen der Unmute() Methode
            UnMute();
        }
        else
        {
            // Aufrufen der Mute() Methode
            Mute();
        }

        // Wenn in den PlayerPrefs kein Key namens "Volume" vorhanden ist       
        if (!PlayerPrefs.HasKey("Volume"))
        {
            // Setzt in den PlayPrefs den Wert 1 für den Key "Volume"
            PlayerPrefs.SetFloat("Volume", 1);

            // Aufrufen der Loadsettings Methode
            LoadSettings();
        }
        else
        {
            LoadSettings();
        }

        // Wenn der Wert für den Key "highScoreName_1" leer ist
        if (PlayerPrefs.GetString("highScoreName_1") == "")
        {
            ResetHighScores();
        }

        // Wenn der Spielername vorher schonmal eingegeben wurde
        if (ddScript.GetPlayerName() != null)
        {
            // Schreibt den vorher schonmal eingegeben Namen in das Inputfeld
            selectName.text = ddScript.GetPlayerName();
        }

        // Wenn das Spiel mit neuer Versuch neu gestartet wurde
        if (ddScript.GetRestart())
        {
            OnClickStart();

            // Setzt den Restart Wert wieder auf false
            ddScript.SetRestart(false);
        }

        // Setzt die HighScores
        SetHighscores();
    }

    private void SetHighscores()
    {
        // for jeden der 5 Anzeigbaren HighScores
        for (int i = 1; i <= 5; i++)
        {
            // Wenn der erreichte Score hocher ist als der HighScore an der Position i
            if(GetScore() > PlayerPrefs.GetInt("highScoreValue_" + i))
            {
                // Für jeden HighScore unterhalt von i
                for (int x = 5; x > i; x--)
                {
                    // Nimmt den Wert des HighScore unterhalb des erreichten HighScore
                    int value = PlayerPrefs.GetInt("highScoreValue_" + (x - 1));
                    
                    // Nimmt den Namen des HighScore unterhalb des erreichten HighScore
                    string name = PlayerPrefs.GetString("highScoreName_" + (x - 1));

                    // Setzt den "verdrängten" HighScore einen Platz runter
                    PlayerPrefs.SetInt("highScoreValue_" + x, value);

                    // Setzt den "verdrängten" HighScore-Namen einen Platz runter
                    PlayerPrefs.SetString("highScoreName_" + x, name);
                }

                // Setzt den erreichten Score an die Position i
                PlayerPrefs.SetInt("highScoreValue_" + i, GetScore());

                // Setzt den Spielernamen an Position i
                PlayerPrefs.SetString("highScoreName_" + i, playerName);

                // verlässt die Schleife
                break;
            }            
        }

        // Schreibt die HighScores und Namen in die Textfelder
        for (int i = 1; i <= 5; i++)
        {
            highScoreValues[i - 1].text = PlayerPrefs.GetInt("highScoreValue_" + i).ToString();
            highScoreNames[i - 1].text = PlayerPrefs.GetString("highScoreName_" + i);
        }
    }

    // Standartwerte für vorgefertigten HighScore
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

        // Schreibt die HighScores und Namen in die Textfelder
        for (int i = 1; i <= 5; i++)
        {
            highScoreValues[i - 1].text = PlayerPrefs.GetInt("highScoreValue_" + i).ToString();
            highScoreNames[i - 1].text = PlayerPrefs.GetString("highScoreName_" + i);
        }
    }

    // Lädt die Szene Neu
    public void OnClickReset()
    {
        SceneManager.LoadScene(0);
    }

    // Setzt Restart bevor die Szene neu geladen wird (Überspringt dadurch das Hauptmenu und die Namenseingabe)
    public void OnClickRestart()
    {
        ddScript.SetRestart(true);
        SceneManager.LoadScene(0);
    }

    // Wenn Spiel Starten gedrückt wird
    public void OnClickStart()
    {
        // Wenn kein Name eingegeben wurde
        if (selectName.text == "")
        {
            // Ändert Textfarbe zu rot
            namePlaceholder.color = Color.red;
        }// Wenn ein Name eingegeben wurde
        else
        {
            // Wenn das Spiel über neuer Versuch neu gestartet wurde
            if (ddScript.GetRestart())
            {
                // Nimmt den Spielernamen der vorher schon eingegeben wurde
                playerName = ddScript.GetPlayerName();
            }
            else
            {
                // Setzt den Spielernamen auf die Eingabe aus dem Inputfeld
                playerName = selectName.text;

                // Speichert den Spielernamen in das Objekt, dass bei neu laden erhalten bleibt
                ddScript.SetPlayerName(playerName);
            }

            backgroundImage.SetActive(false);
            mainMenu.SetActive(false);
            werSpielt.SetActive(false);
            gameUi.SetActive(true);

            // Startet die Spielzeit (Spiel wird unpausiert)
            Time.timeScale = 1f;

            // Spiel ist nichtmehr pausiert
            isPaused = false;

            // Der DebugScore wird zum Score addiert
            score += debugScore;

            // ScoreUpWithTime() Methode wird immerwieder nach dem angegebenen Intervall ausgeführt
            InvokeRepeating("ScoreUpWithTime", scoreInterval, scoreInterval);

            // Spiel wurde gestartet
            hasStarted = true;
        }
            
       
    }

    // Wird jeden Frame aufgerufen
    private void Update()
    {
        // Zeigt vergangene Zeit im Spiel
        ShowTime();

        // Zeigt erreichten Score im Spiel
        ShowScore();

        // Wenn Esc gedrückt wird
        OnEscapePauseOrResume();
    }

    // Addiert die Punktzahl über Zeit mit dem aktuellen Score
    public void ScoreUpWithTime()
    {
        SetScore(GetScore() + timeScore);
    }

    public void OnEscapePauseOrResume()
    {
        // Wenn Esc gedrückt wurde und das Spiel gestartet ist und das Süiel nicht bereits pausiert ist
        if (Input.GetKeyDown(KeyCode.Escape) && hasStarted == true && isPaused == false)
        {
            Pause();
        }// wenn Wenn Esc gedrückt wurde und das Spiel bereits pausiert ist
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            OnClickResume();
        }
    }

    // Pausiert die Spielzeit und aktiviert das Pause Menü
    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    // Unpausiert die Spielzeit und deaktiviert das Pause Menü
    public void OnClickResume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // Wenn im Pausemenü auf zurück zum Hauptmenü geklickt wird, werden die HighScores neu gesetzt (Im Falle, dass der Spieler zu diesem Zeitpunkt einen HighScore erreicht hat) und die Szene neu geladen
    public void OnClickToMainMenu()
    {
        SetHighscores();
        SceneManager.LoadScene(0);        
    }

    // Spielzeit seit dem Spielstart wird im Spiel UI ausgegeben
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

    // Setzt den aktuellen Score
    public void SetScore(int value)
    {
        score = value;
    }

    // Gibt den aktuellen Score zurück
    public int GetScore()
    {
        return score;
    }

    // Schreibt den actuellen Score im Spiel UI
    public void ShowScore()
    {
        CurrentScore.text = "Score: " + score; 
    }

    // Nach 2 Sekunden wird GameOver() aufgerufen
    public void OnDeath()
    {
        Invoke("GameOver", 2);        
    }

    // Wird aufgerufen wenn der Spieler alle Leben verloren hat
    public void GameOver()
    {
        // Deaktiviert das Spiel UI
        gameUi.SetActive(false);

        // Aktiviert den GameOver Screen
        gameOverScreen.SetActive(true);

        // Hält die Spielzeit an
        Time.timeScale = 0f;

        // Zeigt den Spielernamen und den erreichten Score im GameOver Screen an
        gameOverName.text = playerName;
        gameOverScore.text = GetScore().ToString();

        // Setzt die HighScores und hasStarted wieder auf false
        SetHighscores();
        hasStarted = false;
    }

    // Setzt den Wert des Lautstärkereglers auf die Lautstärke des AudioListeners und speichert die Einstellung
    public void ChangeVol(float newValue)
    {
        AudioListener.volume = volSlider.GetComponent<Slider>().value;
        SaveSetting();
    }

    // Pausiert den AudioListener und speichert die Einstellung in den PlayerPrefs
    public void Mute()
    {
        AudioListener.pause = true;
        PlayerPrefs.SetString("Mute", "muted");

        // Deaktiviert das Mute Icon und aktiviert das unmute Icon
        unmuteButton.SetActive(true);
        muteButton.SetActive(false);
    }

    // Unpausiert den AudioListener und speichert die Einstellung in den PlayerPrefs
    public void UnMute()
    {
        AudioListener.pause = false;
        PlayerPrefs.SetString("Mute", "unmuted");

        // Deaktiviert das Unmute Icon und aktiviert das Mute Icon
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
    }

    // Lädt die gespeicherten Lautstärkeeinstellungen aus den PlayerPrefs
    public void LoadSettings()
    {
        volSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume");
    }

    // Speichert die Lautstärkeeinstellungen in den PlayerPrefs
    private void SaveSetting()
    {
        PlayerPrefs.SetFloat("Volume", volSlider.GetComponent<Slider>().value);
    }

    // Beendet die Applikation
    public void ExitGame()
    {
        Application.Quit();
    }
}
