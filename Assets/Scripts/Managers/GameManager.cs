using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;

    public static GameManager GetInstance()
    {
        if (GameManager.instance != null)
        {
            return GameManager.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on GameManager, but GameManager has no instance.");
            return null;
        }

    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created GameManager multiple times!");
        }
        else
        {
            instance = this;
        }
    }


    public float cameraZAxis = -3;
    public float cameraYAxis = 10;

    // We have pointers to these rooms so that we can use them to spawn the player.
    [HideInInspector]
    public GameObject currentEntranceRoom;
    [HideInInspector]
    public GameObject currentExitRoom;


    // Replace this with loading it in from the resources folder.
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Put SpawnCamera() here to check the camera y axis and z axis in real time. Useful for positioning it just right.
        //SpawnCamera();
    }

    public void SpawnPlayer()
    {
        player = Instantiate(player);
        player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);

        SpawnCamera();

    }

    public void SpawnCamera()
    {
        Camera theCamera = Camera.main;

        theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    }
}
