﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);

            SpawnPlayer();
        }


    }


    private bool hasResetPlayer = false;

    public float cameraZAxis = -3;
    public float cameraYAxis = 10;

    // We have pointers to these rooms so that we can use them to spawn the player.
    //[HideInInspector]
    public GameObject currentEntranceRoom;
    //[HideInInspector]
    public GameObject currentExitRoom;


    // Replace this with loading it in from the resources folder.
    public GameObject player;

    private PlayerManager playerManager;
    private GameObject playerInstance;

    private NavMeshBaker navmeshBaker;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnPlayer();
        SetPlayerPos();
        SpawnCamera();

        playerManager = PlayerManager.GetInstance();
        playerInstance = playerManager.player;

        navmeshBaker = NavMeshBaker.GetInstance();

    }

    // Update is called once per frame
    void Update()
    {
        // Put SpawnCamera() here to check the camera y axis and z axis in real time. Useful for positioning it just right.
        //SpawnCamera();


        if (Input.GetKeyDown(KeyCode.X))
            LoadNewLevel();


        
    }

	private void FixedUpdate()
	{

        // I think resetting the player pos has to be here because when we reload the scene we have to wait for DungeonGenerators Awake() function to be called before we re position the player.
        if (!hasResetPlayer && currentEntranceRoom != null)
        {
            Debug.Log("!NOTE!========= NAV MESH REBAKED ==========!NOTE!");
            ResetPlayer();
            navmeshBaker.ResetBaker();
        }
    }

	public void SpawnPlayer()
    {
        player = Instantiate(player);
        //player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);

        //SpawnCamera();

    }
    
    // SetPlayerPos() is used once at the start of the game. ResetPlayerPos() is used on level reload.
    public void SetPlayerPos()
    {
        player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);
    }
    public void ResetPlayerPos()
    {

        // WARNING --------- Have no idea but when the player is not kinematic and gets stuck inside a thing on spawn, you can change its position like this. So my temporary solution
        // is just to make him kinematic for a split second then change him back.

        playerManager.playerRigidbody.isKinematic = true;
        playerInstance.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);
        playerManager.playerRigidbody.isKinematic = false;
    }

    public void SpawnCamera()
    {
        Camera theCamera = Camera.main;

        theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    }


    public void LoadNewLevel()
    {
        SceneManager.LoadScene("DanielsTestScene");
        hasResetPlayer = false;
        currentEntranceRoom = null;

        
    }

    public void ResetPlayer()
    {
        
        ResetPlayerPos();
        SpawnCamera();
        hasResetPlayer = true;
    }
}