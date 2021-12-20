using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject backgroundImage;
    public GameObject mainMenu;
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void OnClickReset()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickStart()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {

    }
}
