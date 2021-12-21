using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    privat Image HealthBar;
    public float CurrentHealth;
    private float MaxHealth = 100f;
    PlayerController_Script Player;


    private void Start()
    {
        //Bitte das Script vom Player in Zeile 18 fügen,
        //da akteull falscher Name
        HealthBar = GetComponent<Image>();
        Player = FindObjectOfType<PlayerController_Script>();
    }

    
    private void Update()
    {
        CurrentHealth = Player.Health;
        HealthBar.fillAmount = CurrentHealth / MaxHealth;
    }
}
