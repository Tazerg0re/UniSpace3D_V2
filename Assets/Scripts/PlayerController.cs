using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Trefferpunktanzeige
    public Slider healthSlider;

    // Text für die verbleibenden Leben
    public Text livesText;

    // Zerstörungsanimation
    public GameObject destructionAnim;

    // Waffe von der Geschossen werden kann
    public GameObject weapons;

    // Objekt, das die Teile des Raumschiffes enhält
    public GameObject parts;

    // verbleibende Trefferpunkte
    public float currentHealth = 100f;

    // Maximale Trefferpunkte
    public float maxHealth = 100f;

    // Verbleibende Leben
    public int lives = 3;

    // Unverwundbarkeit (für Debugging)
    public bool invuln = false;


    private void Awake()
    {
        // Setzt den Trefferpunktebalken auf die maximalen Trefferpunkte
        healthSlider.maxValue = maxHealth;
    }

    // Wird jeden Frame aufgerufen
    private void Update()
    {
        // Setzt den Trefferpunktebalken auf die aktuellen Trefferpunkte
        healthSlider.value = currentHealth;

        // Schreibt die aktuelle Anzahl an Leben in den Text
        livesText.text = "x " + lives.ToString();
    }

    // Wenn eine Kollision erkannt wird
    private void OnCollisionEnter(Collision collision)
    {
        // Wenn die Kollision mit einem Objekt mit den "HealthPack"-Tag erfolgt
        if (collision.transform.tag == "HealthPack")
        {
            // Wenn die addierten Trefferpunkte unter den Maximaltrefferpunkten sind
            if (currentHealth + collision.gameObject.GetComponent<PickUp>().healthValue < maxHealth)
            {
                // Setzt die aktuellen Trefferpunkte auf den addierten Wert
                currentHealth += collision.gameObject.GetComponent<PickUp>().healthValue;
            }
            else
            {
                // Setzt die aktuellen Trefferpunkte auf den Mximalwert (damit man nicht mehr als 100% Trefferpunkte sammeln kann)
                currentHealth = maxHealth;
            }

            // Zerstört das Objekt mit dem Kollidiert wurde
            Destroy(collision.gameObject);
        }

        // Wenn die Kollision mit einem Objekt mit den "Coin"-Tag erfolgt
        if (collision.transform.tag == "Coin")
        {
            // Addiert den Wert der Münze zum aktuellen Score
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() + collision.gameObject.GetComponent<PickUp>().coinValue);

            // Zerstört das Objekt mit dem Kollidiert wurde
            Destroy(collision.gameObject);
        }

        // Wenn die Kollision mit einen GameObject mit dem Tag "Bullet" erfolgt und Unverwundbarkeit deaktiviert ist
        if (collision.transform.tag == "Bullet" && !invuln)
        {
            // aus dem GeschossObjekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponent<BulletController>().damage;

            // Schaden wird von den derzeitigen Leben abgezogen
            currentHealth -= dmg;

            // 50 Punkte Abzug, wenn man getroffen wird
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() - 50);

            // rufe CheckDeath() Methode auf
            CheckDeath();
            return;
        }

        // Wenn die Kollision mit einen GameObject mit dem Tag "Asteroid" erfolgt
        if (collision.transform.tag == "Asteroid" && !invuln)
        {
            // aus dem GeschossObjekt wird ausgelesen, wie viel Schaden dieses verursacht
            float dmg = collision.gameObject.GetComponentInParent<AsteroidController>().damage;

            // Schaden wird von den derzeitigen Leben abgezogen
            currentHealth -= dmg;

            // 50 Punkte Abzug, wenn man getroffen wird
            GameManager.singleton.SetScore(GameManager.singleton.GetScore() - 100);

            // rufe CheckDeath() Methode auf
            CheckDeath();
        }
    }

    // Überprüft ob der spieler gestorben ist
    void CheckDeath()
    {
        // Wenn 0 oder weniger Leben
        if (currentHealth <= 0)
        {
            // Setzt den Trefferpunktebalken auf die aktuellen Trefferpunkte
            healthSlider.value = currentHealth;

            // Animation für Explosion wird erschaffen
            GameObject ded = Instantiate(destructionAnim, transform.position, Quaternion.identity);

            // Zerstört die Explosionsanimation nach 2 sekunden
            Destroy(ded, 2);

            // Wenn die Leben weniger/gleich 1 sind
            if (lives <= 1)
            {
                // Reduziert Leben um 1
                lives--;

                // Schreibt die aktuelle Anzahl an Leben in den Text
                livesText.text = "x " + lives.ToString();

                //Stoppt die Zeit und geht in GameOver Screen
                GameManager.singleton.OnDeath();

                // Enferne Spieler Raumschiff
                Destroy(gameObject);
            }
            else
            {
                // Reduziert Leben um 1
                lives--;

                // Startet Coroutine Reaspawn()
                StartCoroutine(Respawn());
            }

        }
    }

    // Methode, um den Spieler nach Zerstörung wieder zu erzeugen, falls er noch Leben besitzt
    IEnumerator Respawn()
    {
        // Deaktiviert die Waffen
        weapons.SetActive(false);

        // Array, das die Renderer enhält, welche die Schiffsteile auf dem Bildschirm darstellen
        Renderer[] renderers = parts.GetComponentsInChildren<Renderer>();

        // Trefferboxen der Schiffsteile
        Collider[] colliders = parts.GetComponentsInChildren<Collider>();

        // Für jedes Element in colliders
        foreach (var c in colliders)
        {
            // Trefferbox wird deaktiviert (damit man nach dem "Wiederbeleben" nicht sofort wieder getroffen wird)
            c.enabled = false;
        }

        // Trefferpunkte werden wieder auf Max gesetzt
        currentHealth = maxHealth;

        // Für jedes Element in renderers
        foreach (var r in renderers)
        {
            // Darstellung auf dem Bildschirm wird deaktiviert ( Spielerschiff wird unsichtbar)
            r.enabled = false;
        }

        // Warten für 3 Sekunden
        yield return new WaitForSeconds(3);

        // Waffen werden aktiviert
        weapons.SetActive(true);

        // for-Schleife für "Blink-Effekt"
        for (int i = 0; i < 15; i++)
        {
            // Für jedes Element in renderers
            foreach (var r in renderers)
            {
                // wenn das Element nicht dargestellt wird
                if (!r.enabled)
                {
                    // Aktiviert die Darstellung
                    r.enabled = true;
                }
                else
                {
                    // Deaktiviert die Darstellung
                    r.enabled = false;
                }               
            }

            // Warte 0,2 Sekunden
            yield return new WaitForSeconds(0.2f);
        }

        // Aktiviert die Trefferboxen wieder
        foreach (var c in colliders)
        {
            c.enabled = true;
        }
    }
}
