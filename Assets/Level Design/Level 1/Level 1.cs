using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PDollarGestureRecognizer;

public class Level1 : MonoBehaviour
{

    public List<GameObject> spawnableObjects;

    public GameObject parent;
    public LevelManager levelManager; // Adaugă o referință la LevelManager
    public TextMeshProUGUI CurentLevel;
    public TextMeshProUGUI CurentTask;
    public TextMeshProUGUI CurentActions;

    public ObjectDestroyer objectDestroyer;

    public MovementRecognizer movementRecognizer;


    void Start()
    {
        if (spawnableObjects == null)
        {
            spawnableObjects = new List<GameObject>();
        }

        if (parent == null)
        {
            parent = GameObject.Find("level1");
            Debug.LogWarning("Parent is null. Make sure it is assigned in the inspector.");
        }

        //ii atribui variabilei  movementRecognizer componenta Movement Recognizer din scena
        movementRecognizer = GameObject.Find("Movement Recognizer").GetComponent<MovementRecognizer>();

        // Obține referința la LevelManager
        levelManager = FindObjectOfType<LevelManager>();

        // Transformarea poziției părintelui la 0
        parent.transform.position = Vector3.zero;
        CurentLevel = GameObject.Find("Mesaje").transform.Find("CurentLevel").GetComponent<TextMeshProUGUI>();
        CurentTask = GameObject.Find("Mesaje").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        CurentActions = GameObject.Find("Mesaje").transform.Find("CurentActions").GetComponent<TextMeshProUGUI>();

        AfiseazaMesajLevel("Level 1");
        AfiseazaMesajTask("Destroy the object with the sword");
        AfiseazaMesajActiuni("Destroy all objects with a single horizontal move");

        objectDestroyer = GameObject.Find("ObjectDestroyer").GetComponent<ObjectDestroyer>();
        ObjectDestroyer.objectCount = 5;
        CheckAndDestroyObjects();
        SpawnNewObjects();

        Invoke("StergeMesajLevel", 2f);
        Invoke("StergeMesajActions", 4f);
        Invoke("StergeMesajTask", 4f);
    }

    void Update()
    {
        if (movementRecognizer.isMovementRecognized)
        {
            if (ObjectDestroyer.objectCount < 5 && ObjectDestroyer.objectCount > 0)
            {
                AfiseazaMesajActiuni("The movement was not good enough. Try again!");
                Invoke("StergeMesajActions", 2f);
                ObjectDestroyer.objectCount = 5;
                CheckAndDestroyObjects();
                SpawnNewObjects();
            }

            if (ObjectDestroyer.objectCount <= 0)
            {
                Result gestureResult = movementRecognizer.gestureResult;
                if (gestureResult.GestureClass == "Orizontal Line" && gestureResult.Score >= 0.7f)
                {
                    AfiseazaMesajActiuni("Congratulations! The move was good enought!");
                    Invoke("StergeMesajActions", 2f);
                    AfiseazaMesajLevel("Loading next level...");
                    Invoke("StergeMesajLevel", 2f);
                    Invoke("LoadNextLevelWithDelay", 2f);
                }
                else
                {
                    if (gestureResult.GestureClass != "Orizontal Line")
                    {
                        AfiseazaMesajActiuni("The movement was not precise enough. Try again!");
                        Invoke("StergeMesajActions", 2f);
                        CheckAndDestroyObjects();
                        SpawnNewObjects();
                    }
                }
            }
            movementRecognizer.isMovementRecognized = false;
        }
    }

    void CheckAndDestroyObjects()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("SliceObject");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj, 2f);
        }

        foreach (GameObject spawnableObject in spawnableObjects)
        {
            GameObject objectToDestroy = GameObject.Find(spawnableObject.name + "(Clone)");
            if (objectToDestroy != null)
            {
                Destroy(objectToDestroy);
            }
            else if (objectToDestroy == null)
            {
                Debug.Log("Object to destroy is null");
            }
        }
    }


    public void SpawnNewObjects()
    {
        Debug.Log("Spawning new objects...");
        float coordonateX = 0f;

        foreach (GameObject spawnableObject in spawnableObjects)
        {
            // Folosește Instantiate în loc de TeleportObjectToPosition
            GameObject spawnedObject = Instantiate(spawnableObject, GetSpawnPosition(coordonateX), Quaternion.Euler(-90f, 0f, 0f), parent.transform);
            Debug.Log($"Object '{spawnableObject.name}' spawned on position: {spawnedObject.transform.position}");
            coordonateX += 0.4f;
        }

        ObjectDestroyer.objectCount = spawnableObjects.Count;
    }

    Vector3 GetSpawnPosition(float coordonateX)
    {
        return new Vector3(14.68f + coordonateX, 2f, -3.528f);
    }

    void AfiseazaMesajLevel(string mesaj)
    {
        // Setează textul în componenta TextMeshPro
        CurentLevel.text = mesaj;
    }

    void AfiseazaMesajTask(string mesaj)
    {
        // Setează textul în componenta TextMeshPro
        CurentTask.text = mesaj;
    }

    void AfiseazaMesajActiuni(string mesaj)
    {
        // Setează textul în componenta TextMeshPro
        CurentActions.text = mesaj;
    }

    void StergeMesajLevel()
    {
        // Șterge textul mesajului după 2 secunde
        CurentLevel.text = "";
    }
    void StergeMesajTask()
    {
        // Șterge textul mesajului după 2 secunde
        CurentTask.text = "";
    }
    void StergeMesajActions()
    {
        // Șterge textul mesajului după 2 secunde
        CurentActions.text = "";
    }

    void LoadNextLevelWithDelay()
    {
        levelManager.LoadNextLevel();
    }
}
