using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PDollarGestureRecognizer;

public class Level3 : MonoBehaviour
{
    public List<GameObject> spawnableObjectsDiagonal;
    public List<GameObject> spawnableObjectsRevDiagonal;
    public GameObject pillar;

    public GameObject parent;
    public LevelManager levelManager; // Adaugă o referință la LevelManager
    public TextMeshProUGUI CurentLevel;
    public TextMeshProUGUI CurentTask;
    public TextMeshProUGUI CurentActions;

    public ObjectDestroyer objectDestroyer;
    public MovementRecognizer movementRecognizer;

    bool RevDiagonal = false;

    void Start()
    {
        if (spawnableObjectsDiagonal == null || spawnableObjectsRevDiagonal == null)
        {
            spawnableObjectsDiagonal = new List<GameObject>();
            spawnableObjectsRevDiagonal = new List<GameObject>();
        }

        if (parent == null)
        {
            parent = GameObject.Find("level3");
            Debug.LogWarning("Parent is null. Make sure it is assigned in the inspector.");
        }

        movementRecognizer = GameObject.Find("Movement Recognizer").GetComponent<MovementRecognizer>();
        levelManager = FindObjectOfType<LevelManager>();

        //transformarea pozitie parent in 0
        parent.transform.position = Vector3.zero;
        CurentLevel = GameObject.Find("Mesaje").transform.Find("CurentLevel").GetComponent<TextMeshProUGUI>();
        CurentTask = GameObject.Find("Mesaje").transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
        CurentActions = GameObject.Find("Mesaje").transform.Find("CurentActions").GetComponent<TextMeshProUGUI>();

        AfiseazaMesajLevel("Level 3");
        AfiseazaMesajTask("Destroy the object with the sword");
        AfiseazaMesajActiuni("Destroy all objects with a single diagonal move");

        DestroyAllPillars();
        InstantiateNewPillars1();

        objectDestroyer = GameObject.Find("ObjectDestroyer").GetComponent<ObjectDestroyer>();
        ObjectDestroyer.objectCount = 4;
        DeleteObjectsWithTag();
        CheckAndDestroyObjects(spawnableObjectsDiagonal);
        CheckAndDestroyObjects(spawnableObjectsRevDiagonal);
        SpawnNewObjectsDiagonal(spawnableObjectsDiagonal);

        Invoke("StergeMesajLevel", 2f);
        Invoke("StergeMesajActions", 4f);
        Invoke("StergeMesajTask", 4f);
    }

    void Update()
    {
        if (movementRecognizer.isMovementRecognized && RevDiagonal == false)
        {
            if (ObjectDestroyer.objectCount < 4 && ObjectDestroyer.objectCount > 0)
            {
                AfiseazaMesajActiuni("The movement was not good enough. Try again!");
                Invoke("StergeMesajActions", 2f);
                ObjectDestroyer.objectCount = 4;
                CheckAndDestroyObjects(spawnableObjectsDiagonal);
                SpawnNewObjectsDiagonal(spawnableObjectsDiagonal);
            }
            if (ObjectDestroyer.objectCount <= 0)
            {
                Result gestureResult = movementRecognizer.gestureResult;
                if (gestureResult.GestureClass == "Reverse Diagonal" && gestureResult.Score >= 0.7f)
                {
                    AfiseazaMesajActiuni("Congratulations! The move was good enought!");
                    Invoke("StergeMesajActions", 2f);
                    DestroyAllPillars();
                    AfiseazaMesajTask("Now destroy all objects with a single reverse diagonal move");
                    Invoke("StergeMesajTask", 2f);
                    RevDiagonal = true;
                    DestroyAllPillars();
                    InstantiateNewPillars2();
                    CheckAndDestroyObjects(spawnableObjectsDiagonal);
                    SpawnNewObjectsRevDiagonal(spawnableObjectsRevDiagonal);
                }
                else
                {
                    if (gestureResult.GestureClass != "Reverse Diagonal")
                    {
                        AfiseazaMesajActiuni("The movement was not precise enough. Try again!");
                        Invoke("StergeMesajActions", 2f);
                        CheckAndDestroyObjects(spawnableObjectsDiagonal);
                        SpawnNewObjectsDiagonal(spawnableObjectsDiagonal);
                    }
                }
            }
            movementRecognizer.isMovementRecognized = false;
        }
        else
        {
            if (movementRecognizer.isMovementRecognized && RevDiagonal == true)
            {
                if (ObjectDestroyer.objectCount < 4 && ObjectDestroyer.objectCount > 0)
                {
                    AfiseazaMesajActiuni("The movement was not good enough. Try again!");
                    Invoke("StergeMesajActions", 2f);
                    ObjectDestroyer.objectCount = 4;
                    CheckAndDestroyObjects(spawnableObjectsRevDiagonal);
                    SpawnNewObjectsRevDiagonal(spawnableObjectsRevDiagonal);
                }
                if (ObjectDestroyer.objectCount <= 0)
                {
                    Debug.Log("S-a intrat in iful final");
                    Result gestureResult = movementRecognizer.gestureResult;
                    if (gestureResult.GestureClass == "Diagonal" && gestureResult.Score >= 0.7f)
                    {
                        Debug.Log("S-a intrat in iful final 2");
                        AfiseazaMesajActiuni("Congratulations! The move was good enought!");
                        Invoke("StergeMesajActions", 2f);
                        AfiseazaMesajLevel("Well done you have completed all the levels!");
                        Invoke("StergeMesajLevel", 4f);
                        Invoke("LoadNextLevelWithDelay", 4f);
                    }
                    else
                    {
                        if (gestureResult.GestureClass != "Diagonal")
                        {
                            AfiseazaMesajActiuni("The movement was not precise enough. Try again!");
                            Invoke("StergeMesajActions", 2f);
                            CheckAndDestroyObjects(spawnableObjectsRevDiagonal);
                            SpawnNewObjectsRevDiagonal(spawnableObjectsRevDiagonal);
                        }
                    }
                }
                movementRecognizer.isMovementRecognized = false;
            }
        }
    }
    public void DeleteObjectsWithTag()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("SliceObject");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj, 2f);
        }
    }

    void CheckAndDestroyObjects(List<GameObject> objects)
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("SliceObject");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj, 2f);
        }

        foreach (GameObject spawnableObject in objects)
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
    public void SpawnNewObjectsDiagonal(List<GameObject> objects)
    {
        Debug.Log("Spawning new objects...");
        float coordonateY = 0f;
        float coordonateX = 0f;

        foreach (GameObject spawnableObject in objects)
        {
            // Folosește Instantiate în loc de TeleportObjectToPosition
            GameObject spawnedObject = Instantiate(spawnableObject, GetSpawnPosition(coordonateX, coordonateY), Quaternion.Euler(-90f, 0f, 0f), parent.transform);
            Debug.Log($"Object '{spawnableObject.name}' spawned on position: {spawnedObject.transform.position}");
            coordonateY += 0.25f;
            coordonateX += 0.45f;

        }

        ObjectDestroyer.objectCount = objects.Count;
    }

    public void SpawnNewObjectsRevDiagonal(List<GameObject> objects)
    {
        Debug.Log("Spawning new objects...");
        float coordonateY = 0f;
        float coordonateX = 0f;

        foreach (GameObject spawnableObject in objects)
        {
            // Folosește Instantiate în loc de TeleportObjectToPosition
            GameObject spawnedObject = Instantiate(spawnableObject, GetSpawnPositionRevDiagonal(coordonateX, coordonateY), Quaternion.Euler(-90f, 0f, 0f), parent.transform);
            Debug.Log($"Object '{spawnableObject.name}' spawned on position: {spawnedObject.transform.position}");
            coordonateY += -0.25f;
            coordonateX += 0.45f;

        }

        ObjectDestroyer.objectCount = objects.Count;
    }

    Vector3 GetSpawnPosition(float coordonateX, float coordonateY)
    {
        return new Vector3(14.979f + coordonateX, 1.952f + coordonateY, -3.528f);
    }

    Vector3 GetSpawnPositionRevDiagonal(float coordonateX, float coordonateY)
    {
        return new Vector3(14.989f + coordonateX, 2.736f + coordonateY, -3.52f);
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
    void InstantiateNewPillars1()
    {
        GameObject postament = GameObject.Find("Postament");

        // Coordonatele și quaternionii pentru instantierea noilor "pillar"
        Vector3[] positions = { new Vector3(15f, 1.312f, -3.528f), new Vector3(15.437f, 1.469f, -3.528f), new Vector3(15.891f, 1.586f, -3.528f), new Vector3(16.338f, 1.721f, -3.528f)};

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        // Scalarea pentru fiecare obiect "pillar"
        Vector3[] scales = { new Vector3(12, 12, 12), new Vector3(12, 12, 14), new Vector3(12, 12, 17), new Vector3(12, 12, 20) };

        // Instantiați noi obiecte "pillar" la coordonatele specificate
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject newPillar = Instantiate(pillar, positions[i], rotation);
            newPillar.transform.localScale = scales[i];
            newPillar.transform.parent = postament.transform;
        }
    }

    void InstantiateNewPillars2()
    {
        GameObject postament = GameObject.Find("Postament");

        // Coordonatele și quaternionii pentru instantierea noilor "pillar"
        Vector3[] positions = { new Vector3(15f, 1.721f, -3.528f), new Vector3(15.445f, 1.586f, -3.528f), new Vector3(15.894f, 1.469f, -3.528f), new Vector3(16.339f, 1.312f, -3.528f)};

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);

        // Scalarea pentru fiecare obiect "pillar"
        Vector3[] scales = { new Vector3(12, 12, 20), new Vector3(12, 12, 17), new Vector3(12, 12, 14), new Vector3(12, 12, 12) };

        // Instantiați noi obiecte "pillar" la coordonatele specificate
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject newPillar = Instantiate(pillar, positions[i], rotation);
            newPillar.transform.localScale = scales[i];
            newPillar.transform.parent = postament.transform;
        }
    }
}
