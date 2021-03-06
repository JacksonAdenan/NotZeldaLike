﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    public static PlayerManager GetInstance()
    {
        if (PlayerManager.instance != null)
        {
            return PlayerManager.instance;
        }
        else
        {
            Debug.Log("You tried to call GetInstance() on PlayerManager, but PlayerManager has no instance.");
            return null;
        }

    }
	private void Awake()
	{
        if (instance != null && instance != this)
        {
            Debug.Log("======= WARNING ======= : You have created PlayerManager multiple times!");
        }
        else
        {
            instance = this;
        }    
	}

    // Replace this with loading it in from the resources folder.
    public GameObject player;

    public float cameraZAxis = -3;
    public float cameraYAxis = 10;


	public float health;
    public int armour;

    // We have pointers to these rooms so that we can use them to spawn the player.
    public GameObject currentEntranceRoom;
    public GameObject currentExitRoom;


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
    public void HitPlayer(float damageAmount)
    {
        // The player will lose 1 peice of armour if they have it, if the player has no armour the damage goes to their health.
        if (armour > 0)
        {
            armour -= 1;
        }
        else
        { 
            health -= damageAmount;
        }
    }
    public void AddArmour(int armourAmount)
    {
        armour += armourAmount;
    }
    public void AddHealth(float healthAmount)
    {
        health += healthAmount;
    }

    public void SpawnCamera()
    {
        Camera theCamera = Camera.main;

        theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    }
}