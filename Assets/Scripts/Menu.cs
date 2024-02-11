using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{ 
    // Called when we click the "Play" button.
    public void OnPlayButton()
    {
        if (Mode.GameMode == 3)
        {
            // Dacă GameMode este 3, atunci încărcăm scena de recuperare
            Debug.Log("Teleporting player for recovery..." + Mode.GameMode);
            SceneManager.LoadScene(3);
        }
        else
        {
            // Altfel, încărcăm scena normală
            SceneManager.LoadScene(2);
        }
    }

    public void OnModeButton()
    {
        SceneManager.LoadScene(1);
    }

    // Called when we click the "Quit" button.
    public void OnQuitButton()
    {
        // Verificăm dacă suntem în modul de editare în editor (nu poți închide aplicația în modul editor)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
