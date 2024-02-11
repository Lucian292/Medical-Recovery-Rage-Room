using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int currentLevel = 1;

    void Start()
    {
        // Aici poți face inițializări sau acțiuni specifice nivelului curent.
        SwitchToLevel(currentLevel);
    }

    void SwitchToLevel(int level)
    {
        // Poți face acțiuni specifice când se schimbă nivelul (exemplu: dezactivezi/activezi obiecte, ajustezi setările nivelului etc.).
        Debug.Log($"Treci la nivelul {level}");

        // În acest exemplu, dezactivez toate obiectele subordonate pentru a arăta schimbarea nivelului.
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // Activează obiectul specific nivelului curent.
        Transform currentLevelObject = transform.Find($"level{level}");
        if (currentLevelObject != null)
        {
            currentLevelObject.gameObject.SetActive(true);
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevel >= 4)
        {
            Debug.Log("Ai terminat toate nivelele!");
            return;
        }
        currentLevel++;
        SwitchToLevel(currentLevel);
    }
}
