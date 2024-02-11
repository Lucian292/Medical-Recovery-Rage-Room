using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectDestroyer : MonoBehaviour
{
    public static int objectCount { get; set; } = 5;

    public List<GameObject> spawnableObjects;

    public GameObject parent;

    void Start()
    {
        if (spawnableObjects == null)
        {
            spawnableObjects = new List<GameObject>();
        }

        if (parent == null)
        {
            parent = GameObject.Find("New Object Spawner");
            Debug.LogWarning("Parent is null. Make sure it is assigned in the inspector.");
        }

        //transformarea pozitie parent in 0
        parent.transform.position = Vector3.zero;

    }
}
