using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private static TextMeshProUGUI sharedTextMeshPro;
    private static float score = 0;
    private void Awake()
    {
        // Ensure the sharedTextMeshPro is assigned during Awake
        if (sharedTextMeshPro == null)
        {
            // Find the GameObject with the name "ScoreText"
            GameObject scoreTextObject = GameObject.Find("Score text");

            // Check if the GameObject is found
            if (scoreTextObject != null)
            {
                // Get the TextMeshProUGUI component from the found GameObject
                sharedTextMeshPro = scoreTextObject.GetComponent<TextMeshProUGUI>();

                // Check if the TextMeshProUGUI is found
                if (sharedTextMeshPro == null)
                {
                    Debug.LogError("TextMeshProUGUI component not found on the GameObject named 'ScoreText'!");
                }
            }
            else
            {
                Debug.LogError("GameObject named 'ScoreText' not found in the scene!");
            }
        }
    }
    public static void IncrementScore(int scoreToAdd)
    {
        // Note: You can't create a new TextMeshProUGUI instance here, it won't be connected to any UI in the scene.
        // Instead, you should use the reference to the UI element passed to the script.

        score += scoreToAdd;
        Debug.Log(score);
        if (sharedTextMeshPro != null)
        {
            sharedTextMeshPro.text = score.ToString();
            sharedTextMeshPro.text = string.Format("{0} Score", score);
        }
        else
        {
            Debug.Log(score);
            Debug.LogError("textMeshProUGUI is not assigned in the Inspector!");
        }
    }
}
