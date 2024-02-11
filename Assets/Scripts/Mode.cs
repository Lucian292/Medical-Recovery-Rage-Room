using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mode : MonoBehaviour
{
    public static int GameMode; // Acum avem GameMode ca variabilă statică

    // Called when we click the "Play" button.
    public void OnNormalButton()
    {
        GameMode = 0;
        SceneManager.LoadScene(0);
        Debug.Log("regim = " + GameMode);
    }

    public void OnHardButton()
    {
        GameMode = 1;
        SceneManager.LoadScene(0);
        Debug.Log("regim = " + GameMode);
    }

    // Called when we click the "Quit" button.
    public void OnRecoveryButton()
    {
        GameMode = 3;
        SceneManager.LoadScene(0);
        Debug.Log("regim = " + GameMode);
    }
}
