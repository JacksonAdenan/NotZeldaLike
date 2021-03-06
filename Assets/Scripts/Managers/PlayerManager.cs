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
    [HideInInspector]
    public int health;
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
    private PlayerController controller;


    private GameManager gameManager;

    // Getting animation controller.
    [HideInInspector]
    public Animator playerAnimator;

    Renderer leahRenderer;

    [HideInInspector]
    public float attackAnimLength = 0;

    public enum PlayerAnimation 
    { 
        Idle,
        Idle_Low,
        Running,
        Death,
        Knockback,
        Slash,
    }

    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.GetInstance();
        //SpawnPlayer();

        // Have to do this whole thing because the components are shared between different children of Player.
        GameObject leah = player.transform.Find("Leah").gameObject;
        leahRenderer = leah.transform.Find("Leahv1").GetComponent<Renderer>();
        originalMaterial = leahRenderer.sharedMaterial;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        controller = gameObject.GetComponent<PlayerController>();

        foreach (Transform heart in GameManager.GetInstance().healthUIParent.transform)
            hearts.Add(heart.GetComponent<HeartUI>());
        foreach (Transform armour in GameManager.GetInstance().armourUIParent.transform)
            armours.Add(armour.GetComponent<ArmourUI>());
        AddMaxHealth(startingMaxHealth);
        AddHealth(startingHealth);
        AddMaxArmour(startingMaxArmour);
        AddArmour(startingArmour);

        gameManager = GameManager.GetInstance();

        playerAnimator = leah.GetComponent<Animator>();
        attackAnimLength = GetAttackDuration();
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

    public void AddHealth(int add)
    {
        for (int i = 0; i < add; i++)
            if (health < maxHealth)
                health++;

        for (int i = 0; i < health; i++)
            hearts[i].heartToggle = HeartUI.HeartStates.Full;
    }

    public void DecreaseHealth(int remove)
    {


        for (int i = 1; i < remove + 1; i++)
        {
            // This is stop stop indexing out of range.
            if (health - i < 0)
            {
                break;
            }
            hearts[health - i].heartToggle = HeartUI.HeartStates.Empty;
        }

        for (int i = 0; i < remove; i++)
            if (health > 0)
                health--;
    }

    public void AddMaxHealth(int add)
    {
        for (int i = 0; i < add; i++)
            if (maxHealth < 10)
            {
                maxHealth++;
                hearts[maxHealth - 1].heartToggle = HeartUI.HeartStates.Empty;
            }
    }

    public void AddArmour(int add)
    {
        for (int i = 0; i < add; i++)
            if (armour < maxArmour)
                armour++;

        for (int i = 0; i < armour; i++)
            armours[i].armourToggle = ArmourUI.ArmourStates.Full;
    }

    public void DecreaseArmour(int remove)
    {

        for (int i = 1; i < remove + 1; i++)
        {
            if (armour - i < 0)
            {
                // This is here to remove health if the hit breaks armour plus more.
                DecreaseHealth((remove + 1) - i);
                break;
            }

            armours[armour - i].armourToggle = ArmourUI.ArmourStates.Empty;
        }

        for (int i = 0; i < remove; i++)
            if (armour > 0)
                armour--;     
    }

    public void AddMaxArmour(int add)
    {
        for (int i = 0; i < add; i++)
            if (maxArmour < 6)
            {
                maxArmour++;
                armours[maxArmour - 1].armourToggle = ArmourUI.ArmourStates.Empty;
            }
    }

    public void AddKnockback(int add)
    {
        meleeKnockbackForce += add;
    }

    public void AddMovementSpeed(int add)
    {
        controller.speed += add;
    }

    public void AddDamage(int add)
    {
        meleeDamage += add;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyMeleeZone" && !isHit)
        {
            AgentController enemy;
            if (other.transform.parent.tag == "Bullet")
            {
                BulletData data = other.transform.parent.GetComponent<BulletData>();
                enemy = data.agent;
            }
            else
            {
                enemy = other.transform.parent.GetComponent<AgentController>();
            }

            HitPlayer(enemy.meleeDamage);
            Vector3 pushDirection = transform.position - other.transform.parent.position;
            pushDirection = Vector3.Normalize(pushDirection);

            // Only knock back if they don't have armour left.
            if (armour <= 0)
                playerRigidbody.AddForce(pushDirection * enemy.knockback, ForceMode.Impulse);

            //gameObject.transform.position += pushDirection;

            isHit = true;
            leahRenderer.sharedMaterial = damageMaterial;

            //gameObject.transform.position += pushDirection * enemy.knockback;


            Debug.Log("Player took damage.");
        }

        
    }

	private void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.tag == "Gate")
        {
            if (gameManager.hasKey || gameManager.hasMonsterKey)
            {
                Destroy(collision.gameObject);
            }
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
                leahRenderer.sharedMaterial = originalMaterial;
            }
        }
    }

    public void SetAnimation(PlayerAnimation animation)
    {
        switch (animation)
        {
            case PlayerAnimation.Idle:
                playerAnimator.SetBool("Idle", true);
                playerAnimator.SetBool("Idle Low Health", false);
                playerAnimator.SetBool("Running", false);
                break;
            case PlayerAnimation.Idle_Low:
                playerAnimator.SetBool("Idle Low Health", true);
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Running", false);
                break;
            case PlayerAnimation.Running:
                playerAnimator.SetBool("Running", true);
                playerAnimator.SetBool("Idle", false);
                playerAnimator.SetBool("Idle Low Health", false);
                break;
            case PlayerAnimation.Slash:
                playerAnimator.SetTrigger("Slash");
                break;
        }
        
    }

    private float GetAttackDuration()
    {
        float attackDuration = 0;

        // We load all animation clips into this array, I'm hoping C# cleans up this because I only want the "Attack" clip for now.
        AnimationClip[] playerAnimationClips = playerAnimator.runtimeAnimatorController.animationClips;

        for (int i = 0; i < playerAnimationClips.Length; i++)
        {
            if (playerAnimationClips[i].name == "Slash")
            { 
                attackDuration = playerAnimationClips[i].length;
                break;
            }
        }

        return attackDuration;
    }
}
