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
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }    
	}

    // Replace this with loading it in from the resources folder.
    public GameObject player;

    public float cameraZAxis = -3;
    public float cameraYAxis = 10;


    /*public int health;
    public int armour;
    public int maxHealth;
    public int maxArmour;*/
    [Header("Stats")]
    public int startingHealth;
    public int startingMaxHealth;
    public int startingArmour;
    public int startingMaxArmour;
    int health;
    int maxHealth;
    int armour;
    int maxArmour;
    public List<HeartUI> hearts = new List<HeartUI>();
    public List<ArmourUI> armours = new List<ArmourUI>();

    public int meleeDamage = 1;
    public float meleeKnockbackForce = 7.0f;

    //// We have pointers to these rooms so that we can use them to spawn the player.
    //[HideInInspector]
    //public GameObject currentEntranceRoom;
    //[HideInInspector]
    //public GameObject currentExitRoom;

    [HideInInspector]
    public bool isAlive;


    public Material damageMaterial;
    private Material originalMaterial;


    private float tookDamageCounter = 0;
    private bool isHit = false;

    // Reference to the UIManager so we can take away and add hearts and stuff to the screen.
    private UIManager uiManager;

    [HideInInspector]
    public Rigidbody playerRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.GetInstance();
        //SpawnPlayer();

        originalMaterial = player.GetComponent<Renderer>().material;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();

        foreach (Transform heart in GameManager.GetInstance().healthUIParent.transform)
            hearts.Add(heart.GetComponent<HeartUI>());
        foreach (Transform armour in GameManager.GetInstance().armourUIParent.transform)
            armours.Add(armour.GetComponent<ArmourUI>());
        AddMaxHealth(startingMaxHealth);
        AddHealth(startingHealth);
        AddMaxArmour(startingMaxArmour);
        AddArmour(startingArmour);
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
            AddMaxHealth(1);

        if (Input.GetKeyDown(KeyCode.H))
            AddHealth(1);

        if (Input.GetKeyDown(KeyCode.J))
            DecreaseHealth(1);
        if (Input.GetKeyDown(KeyCode.B))
            AddMaxArmour(1);
        if (Input.GetKeyDown(KeyCode.N))
            AddArmour(1);
        if (Input.GetKeyDown(KeyCode.M))
            DecreaseArmour(1);



            TookDamagerTimer();
    }

    //public void SpawnPlayer()
    //{
    //    player = Instantiate(player);
    //    player.transform.position = new Vector3(currentEntranceRoom.transform.position.x, 1, currentEntranceRoom.transform.position.y);
    //
    //    SpawnCamera();
    //    
    //}
    public void HitPlayer(int damageAmount)
    {
        if (armour > 0)
            DecreaseArmour(damageAmount);
        else
            DecreaseHealth(damageAmount);
        // The player will lose 1 peice of armour if they have it, if the player has no armour the damage goes to their health.
        /*if (armour > 0)
        {
            armour -= 1;
            for (int i = 0; i < damageAmount; i++)
            {
                uiManager.RemoveIcon(UIManager.IconType.ArmourIcon);
            }

            if (armour < 0)
                armour = 0;
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
        }*/
    }
    /*public void AddArmour(int armourAmount)
    {
        armour += armourAmount;
        for (int i = 0; i < armourAmount; i++)
        {
            if ((armour - armourAmount) + 1 <= maxArmour)
                uiManager.AddIcon(UIManager.IconType.ArmourIcon, armour);
        }

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
    }*/

    //public void SpawnCamera()
    //{
    //    Camera theCamera = Camera.main;
    //
    //    theCamera.transform.position = new Vector3(player.transform.position.x, cameraYAxis, cameraZAxis);
    //}

    void AddHealth(int add)
    {
        for (int i = 0; i < add; i++)
            if (health < maxHealth)
                health++;

        for (int i = 0; i < health; i++)
            hearts[i].heartToggle = HeartUI.HeartStates.Full;
    }

    void DecreaseHealth(int remove)
    {
        for (int i = 0; i < remove; i++)
            if (health > 0)
                health--;

        for (int i = 0; i < remove; i++)
            hearts[health - i].heartToggle = HeartUI.HeartStates.Empty;
    }

    void AddMaxHealth(int add)
    {
        for (int i = 0; i < add; i++)
            if (maxHealth < 10)
            {
                maxHealth++;
                hearts[maxHealth - 1].heartToggle = HeartUI.HeartStates.Empty;
            }
    }

    void AddArmour(int add)
    {
        for (int i = 0; i < add; i++)
            if (armour < maxArmour)
                armour++;

        for (int i = 0; i < armour; i++)
            armours[i].armourToggle = ArmourUI.ArmourStates.Full;
    }

    void DecreaseArmour(int remove)
    {
        for (int i = 0; i < remove; i++)
            if (armour > 0)
                armour--;

        for (int i = 0; i < remove; i++)
            armours[armour - i].armourToggle = ArmourUI.ArmourStates.Empty;
    }

    void AddMaxArmour(int add)
    {
        for (int i = 0; i < add; i++)
            if (maxArmour < 6)
            {
                maxArmour++;
                armours[maxArmour - 1].armourToggle = ArmourUI.ArmourStates.Empty;
            }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyMeleeZone" && !isHit)
        {
            AgentController enemy = other.transform.parent.GetComponent<AgentController>();

            HitPlayer(enemy.meleeDamage);
            Vector3 pushDirection = transform.position - other.transform.parent.position;
            pushDirection = Vector3.Normalize(pushDirection);
            playerRigidbody.AddForce(pushDirection * enemy.knockback, ForceMode.Impulse);
            //gameObject.transform.position += pushDirection;

            isHit = true;
            this.gameObject.GetComponent<Renderer>().material = damageMaterial;

            //gameObject.transform.position += pushDirection * enemy.knockback;


            Debug.Log("Player took damage.");
        }
    }

    void TookDamagerTimer()
    {
        if (isHit)
        {
            tookDamageCounter += Time.deltaTime;
            if (tookDamageCounter >= 0.2f)
            {
                isHit = false;
                tookDamageCounter = 0;
                this.gameObject.GetComponent<Renderer>().material = originalMaterial;
            }
        }
    }
}
