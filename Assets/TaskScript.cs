using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskScript : MonoBehaviour
{
    public Text textToDisappear;

    void Start()
    {
        // Call the DisappearText() function after 10 seconds
        Invoke("DisappearText", 10f);
    }

    void DisappearText()
    {
        // Set the text object to be inactive (disappear)
        textToDisappear.gameObject.SetActive(false);
    }
}
