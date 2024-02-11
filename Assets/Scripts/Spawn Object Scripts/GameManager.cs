using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject jar;
    public GameObject vaza_1_imr;
    public GameObject vaza_2_imr;
    public GameObject water_bottle;
    public GameObject wine_bottle;

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("GameManager is null! Creating a new instance...");
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    // Dacă nu există o instanță în scenă, creează una nouă
                    GameObject gameManagerObject = new GameObject("GameManager");
                    instance = gameManagerObject.AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    // Funcția pentru generarea obiectelor noi
    public void SpawnNewObjects()
    {
        Debug.Log("Spawning new objects...");
        jar = GameObject.Find("jar");
        vaza_1_imr = GameObject.Find("vaza_1_imr");
        vaza_2_imr = GameObject.Find("vaza_2_imr");
        water_bottle = GameObject.Find("water-bottle");
        wine_bottle = GameObject.Find("wine-bottle");
        // Verifică dacă obiectul bowl este diferit de null înainte de a încerca să-l instanțiezi
        //TeleportObjectToPosition(bowlForTeleport, new Vector3(2.38f, 1.44f, -6.22f));
        if (jar != null)
        {
            
            TeleportObjectToPosition(jar, new Vector3(2.37f, 1.57f, -8.201f));
            TeleportObjectToPosition(vaza_1_imr, new Vector3(3.563f, 1.57f, -8.201f));
            TeleportObjectToPosition(vaza_2_imr, new Vector3(4.149f, 1.67f, -8.201f));
            TeleportObjectToPosition(water_bottle, new Vector3(4.611f, 1.57f, -8.201f));
            TeleportObjectToPosition(wine_bottle, new Vector3(3.06f, 1.57f, -8.201f));
            ObjectDestroyer.objectCount = 5;
            
        }
        else
        {
            Debug.LogWarning("Object 'bowlForTeleport' is null. Make sure it is assigned in the inspector.");
        }
    }

    public void TeleportObjectToPosition(GameObject objectToTeleport, Vector3 newPosition)
    {

        if (objectToTeleport != null)
        {
            objectToTeleport.transform.position = newPosition;
            Debug.Log($"Object '{objectToTeleport.name}' teleported to position: {newPosition}");
        }
        else
        {
            Debug.LogWarning("Object to teleport is null.");
        }
    }

}
