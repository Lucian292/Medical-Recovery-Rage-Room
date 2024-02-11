using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using TMPro;
public class MovementRecognizer : MonoBehaviour
{
    public KeyCode simulateMovementKey = KeyCode.G;
    //public KeyCode simulateActionKey = KeyCode.G;
    public Transform movementSource;
    private bool isMoving = false;

    public float newPositionThesholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;

    public float recognitionThreshold = 0.7f;
    public Result gestureResult;

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    public UnityStringEvent OnRecognized;

    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionsList = new List<Vector3>();

    public ObjectDestroyer objectDestroyer;
    public TextMeshProUGUI recognizedMove;

    public bool isMovementRecognized = false;

    // Start is called before the first frame update
    void Start()
    {
        
        string[] gestureFiles = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (var item in gestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }

        recognizedMove = GameObject.Find("Mesaje").transform.Find("MiscareRecunoscuta").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the key for simulating movement is pressed
        if (Input.GetKeyDown(simulateMovementKey))
        {
            StartMoving();
        }
        // Check if the key for simulating movement is released
        else if (Input.GetKeyUp(simulateMovementKey))
        {
            EndMoving();
        }
        // Check if the key for simulating movement is held
        else if (Input.GetKey(simulateMovementKey))
        {
            UpdateMoving();
        }

        
    }

    void StartMoving()
    {
        Debug.Log("Start Moving");
        isMoving = true;
        positionsList.Clear();
        positionsList.Add(movementSource.position);
        // if we have a debug cube prefab, instantiate it at the start position and destroy it after 3 seconds
        if (debugCubePrefab)
            Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity),3);
    }

    void EndMoving()
    {
        Debug.Log("End Moving");
        isMoving = false;

        //Create the gesture from the positions list
        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if (creationMode)
        {
            //Add the gesture to the training set
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            //Save the gesture to a file
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        else
        {
            //Recognize the gesture
            gestureResult = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            isMovementRecognized = true;

            //Show the gesture label
            Debug.Log(gestureResult.GestureClass + " " + gestureResult.Score);

            /*if (gestureResult.Score > recognitionThreshold)
            {
                OnRecognized.Invoke(gestureResult.GestureClass);
            }*/
            string formattedScore = (gestureResult.Score).ToString("P0");
            formattedScore = formattedScore.Replace(".", "").Replace(",", "");
            AfiseazaMesaj("Mișcarea recunoscută: " + gestureResult.GestureClass + " cu precizie: " + formattedScore);
            Invoke("StergeMesaj", 1f);
        }
    }

    void UpdateMoving()
    {
        //Debug.Log("Update Moving");
        Vector3 lastPosition = positionsList[positionsList.Count - 1];
        if (Vector3.Distance(lastPosition, movementSource.position) > newPositionThesholdDistance)
        {
            positionsList.Add(movementSource.position);
            if (debugCubePrefab)
                Destroy(Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity), 3);
        }
    }

    void AfiseazaMesaj(string mesaj)
    {
        // Setează textul în componenta TextMeshPro
        recognizedMove.text = mesaj;
    }

    void StergeMesaj()
    {
        // Șterge textul mesajului după 2 secunde
        recognizedMove.text = "";
    }
}
