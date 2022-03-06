using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Statische Instanz um sicher zu stellen, dass es auch beim neu laden der Szene nur einmal dieses Objekt gibt
    private static DontDestroy instance;

    // Spielername von der Nutzereingabe
    public static string playername;

    // Ob das Spiel neu gestartet wurde
    public static bool restart = false;

    private void Awake()
    {
        // Wenn es noch keine Instanz von diesem Objekt gibt
        if (instance == null)
        {
            // Setzt dieses Objekt als Instanz
            instance = this;

            // behalte dieses Objekt, auch wenn neu geladen wird
            DontDestroyOnLoad(instance);
        }
        else // Wenn es schon eine Instanz des  Objektes gibt
        {
            // Zerstöre das nach dem neu laden erstellte Objekt
            Destroy(gameObject);
        }
    }

    // Methode zum setzten des Wertes, ob das Spiel neu gestartet wurde
    public void SetRestart(bool r)
    {
        restart = r;
    }

    // Methode zum Abrufen des restart Wertes
    public bool GetRestart()
    {
        return restart;
    }

    // Methode zum setzen des Spielernamen
    public void SetPlayerName(string name)
    {
        playername = name;
    }

    // Methode zum Abrufen des Spielernamen
    public string GetPlayerName()
    {
        return playername;
    }
}
