using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider healthSlider;
    public GameObject destructionAnim;
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    //PlayerController_Script Player;


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
        // Wenn die Kollision mit einen GameObject mit dem Tag "Bullet" erfolgt
        if (collision.transform.tag == "Bullet")
        {
            // aus dem GeschossObjekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponent<BulletController>().damage;

            // Schaden wird von den derzeitigen Leben abgezogen
            currentHealth -= dmg;            

            // Wenn 0 oder weniger Leben
            if (currentHealth <= 0)
            {
                // Animation für Explosion wird erschaffen
                GameObject ded = Instantiate(destructionAnim, transform.position, Quaternion.identity);

                // Death() Methode Entfernt Schiff und pausiert das Spiel
                Invoke("Death", 1);

                // Zerstört die Explosionsanimation nach 2 sekunden
                Destroy(ded, 0.99f);              
            }
        }
    }

    public void Death()
    {
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
