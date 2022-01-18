using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject destructionAnim;
    public float currentHealth = 100f;
    public float maxHealth = 100f;


    private void Awake()
    {
        healthSlider.maxValue = maxHealth;
        
    }


    private void Update()
    {
        healthSlider.value = currentHealth;
    }

    // Wenn eine Kollision erkannt wird
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "HealthPack")
        {
            if (currentHealth + collision.gameObject.GetComponent<PickUp>().healthValue < maxHealth)
            {
                currentHealth += collision.gameObject.GetComponent<PickUp>().healthValue;
            } else
            {
                currentHealth = 100;
            }
            
            Destroy(collision.gameObject);
        }

        if (collision.transform.tag == "Coin")
        {
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() + collision.gameObject.GetComponent<PickUp>().coinValue);
            Destroy(collision.gameObject);
        }

        // Wenn die Kollision mit einen GameObject mit dem Tag "Bullet" erfolgt
        if (collision.transform.tag == "Bullet")
        {
            // aus dem GeschossObjekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponent<BulletController>().damage;

            // Schaden wird von den derzeitigen Leben abgezogen
            currentHealth -= dmg;

            // 50 Punkte Abzug, wenn man getroffen wird
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() - 50);

            // Wenn 0 oder weniger Leben
            if (currentHealth <= 0)
            {
                healthSlider.value = currentHealth;

                // Animation für Explosion wird erschaffen
                GameObject ded = Instantiate(destructionAnim, transform.position, Quaternion.identity);

                // Zerstört die Explosionsanimation nach 2 sekunden
                Destroy(ded, 2);

                //Stoppt die Zeit und geht in GameOver Screen
                GameManager.singleton.OnDeath();

                // Enferne Spieler Raumschiff
                Destroy(gameObject);

          
            }
        }
    }
}
