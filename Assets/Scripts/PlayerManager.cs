using System.Collections;
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


	public int health;
    public int armour;
    public int maxHealth;
    public int maxArmour;

    public int meleeDamage = 1;
    public float meleeKnockbackForce = 7.0f;

    // We have pointers to these rooms so that we can use them to spawn the player.
    [HideInInspector]
    public GameObject currentEntranceRoom;
    [HideInInspector]
    public GameObject currentExitRoom;

    [HideInInspector]
    public bool isAlive;


    // Reference to the UIManager so we can take away and add hearts and stuff to the screen.
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.GetInstance();
        SpawnPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        // Put SpawnCamera() here to check the camera y axis and z axis in real time. Useful for positioning it just right.
        //SpawnCamera();


        if (Input.GetKeyDown(KeyCode.F))
        {
            HitPlayer(1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddHealth(1);
        }
    }

    public void SpawnPlayer()
    {
        player = Instantiate(player);
        player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);

        SpawnCamera();
        
    }
    public void HitPlayer(int damageAmount)
    {
        // The player will lose 1 peice of armour if they have it, if the player has no armour the damage goes to their health.
        if (armour > 0)
        {
            armour -= 1;
        }
        else
        { 
            health -= damageAmount;
            for (int i = 0; i < damageAmount; i++)
            { 
                uiManager.RemoveIcon(UIManager.IconType.HealthIcon);
            }

            if (health < 0)
                health = 0;
        }
    }
    public void AddArmour(int armourAmount)
    {
        armour += armourAmount;
        // Making it so that the player never has over the max amount.
        if (armour > maxArmour)
        {
            armour = maxArmour;
        }
    }
    public void AddHealth(int healthAmount)
    {
        health += healthAmount;

        for (int i = 0; i < healthAmount; i++)
        {
            if((health - healthAmount) + 1 <= maxHealth)
                uiManager.AddIcon(UIManager.IconType.HealthIcon, health);
        }


        // Making it so that the player never has over the max amount.
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SpawnCamera()
    {
        Camera theCamera = Camera.main;

        theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    }
}
