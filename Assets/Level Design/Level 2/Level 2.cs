using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PDollarGestureRecognizer;


public class Level2 : MonoBehaviour
{
    public List<GameObject> spawnableObjects;
    public GameObject pillar;

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
            parent = GameObject.Find("level2");
            Debug.LogWarning("Parent is null. Make sure it is assigned in the inspector.");
        }

        movementRecognizer = GameObject.Find("Movement Recognizer").GetComponent<MovementRecognizer>();
        levelManager = FindObjectOfType<LevelManager>();

        //transformarea pozitie parent in 0
        parent.transform.position = Vector3.zero;
        CurentLevel = GameObject.Find("Mesaje").transform.Find("CurentLevel").GetComponent<TextMeshProUGUI>();
        CurentTask = GameObject.Find("Mesaje").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        CurentActions = GameObject.Find("Mesaje").transform.Find("CurentActions").GetComponent<TextMeshProUGUI>();

        AfiseazaMesajLevel("Level 2");
        AfiseazaMesajTask("Destroy the object with the sword");
        AfiseazaMesajActiuni("Destroy all objects with a single vertical move");

        DestroyAllPillars();
        InstantiateNewPillars();

        objectDestroyer = GameObject.Find("ObjectDestroyer").GetComponent<ObjectDestroyer>();
        ObjectDestroyer.objectCount = 4;
        DeleteObjectsWithTag();
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
            if (ObjectDestroyer.objectCount < 4 && ObjectDestroyer.objectCount > 0)
            {
                AfiseazaMesajActiuni("The movement was not good enough. Try again!");
                Invoke("StergeMesajActions", 2f);
                ObjectDestroyer.objectCount = 4;
                CheckAndDestroyObjects();
                SpawnNewObjects();
            }
            if (ObjectDestroyer.objectCount <= 0)
            {
                Result gestureResult = movementRecognizer.gestureResult;
                if (gestureResult.GestureClass == "Vertical Line" && gestureResult.Score >= 0.7f)
                {
                    AfiseazaMesajActiuni("Congratulations! The move was good enought!");
                    Invoke("StergeMesajActions", 2f);
                    AfiseazaMesajLevel("Loading next level...");
                    Invoke("StergeMesajLevel", 2f);
                    Invoke("LoadNextLevelWithDelay", 2f);
                }
                else
                {
                    if (gestureResult.GestureClass != "Vertical Line")
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
    public void DeleteObjectsWithTag()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("SliceObject");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj,2f);
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
        float coordonateY = 0f;

        foreach (GameObject spawnableObject in spawnableObjects)
        {
            // Folosește Instantiate în loc de TeleportObjectToPosition
            GameObject spawnedObject = Instantiate(spawnableObject, GetSpawnPosition(coordonateY), Quaternion.Euler(-90f, 0f, 0f), parent.transform);
            Debug.Log($"Object '{spawnableObject.name}' spawned on position: {spawnedObject.transform.position}");
            coordonateY += 0.4f;
        }

        ObjectDestroyer.objectCount = spawnableObjects.Count;
    }

    Vector3 GetSpawnPosition(float coordonateY)
    {
        return new Vector3(15.391f, 1.452f + coordonateY, -3.528f);
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

    void DestroyAllPillars()
    {
        // Găsește obiectul "Postament" în scenă
        GameObject postament = GameObject.Find("Postament");

        if (postament != null)
        {
            // Parcurge toți copiii "pillar" ai obiectului "Postament"
            foreach (Transform child in postament.transform)
            {
                if (child.name.Contains("pillar"))
                {
                    // Șterge fiecare obiect "pillar"
                    Destroy(child.gameObject);
                }
            }
        }
    }
    void InstantiateNewPillars()
    {
        GameObject postament = GameObject.Find("Postament");

        // Coordonatele și quaternionii pentru instantierea noilor "pillar"
        Vector3[] positions = { new Vector3(14.7f, 1.6f, -3.5f), new Vector3(14.7f, 2f, -3.5f), new Vector3(14.7f, 2.4f, -3.5f), new Vector3(14.7f, 2.8f, -3.5f),
                                new Vector3(16.1f, 1.6f, -3.5f), new Vector3(16.1f, 2f, -3.5f), new Vector3(16.1f, 2.4f, -3.5f), new Vector3(16.1f, 2.8f, -3.5f) };

        Quaternion[] rotation = {Quaternion.Euler(0,-90,0), Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -90, 0),
                                Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -90, 0)};

        // Instantiați noi obiecte "pillar" la coordonatele specificate
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject newPillar = Instantiate(pillar, positions[i], rotation[i]);
            newPillar.transform.parent = postament.transform;
        }
    }
}
