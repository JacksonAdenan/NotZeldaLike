using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public MobType mob;

    [Header("Base mob stats")]
    public int health = 5;
    public float knockback = 5.0f;
    public int meleeDamage = 1;
    public float stunnedTime = 0.75f;

    [Header("AI Decision Making Values")]
    public float xWanderDistance = 2;
    public float yWanderDistance = 2;
    public float chaseDistance = 3;
    [Tooltip("Changes this probably requires you to change the melee trigger zone size!")]
    public float attackDistance = 1;

    [Header("Other")]
    public Material damageMaterial;
    private Material originalMaterial;

    public GameObject monsterExplosion;

    private AIStateMachine stateMachine;

    private Rigidbody agentRigidbody;
    private PlayerManager playerManager;


    private float tookDamageCounter = 0.0f;
    private bool isHit = false;

    private Collider meleeZone = null;

    [HideInInspector]
    public bool isAttacking = false;

    // For shooters.
    [Header("Values for shooter types")]
    public GameObject ammunition;
    public float bulletSpeed;
    public float shootCooldown = 1;
    
    // Tracks how long the attack has been up for.
    private float attackCounter = 0.0f;


    [HideInInspector]
    public float originalAngularSpeed;

    // Just for debugging purposes
    //[HideInInspector]
    [Header("Debugging Purposes")]
    public AIStateMachine.BasicDecisions currentState;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(mob, agent, this);

        agentRigidbody = this.gameObject.GetComponent<Rigidbody>();

        playerManager = PlayerManager.GetInstance();

        // Saving a reference to the original material.
        originalMaterial = this.gameObject.GetComponent<Renderer>().material;

        // Getting the meleeZone reference. If we are a shooter, we don't have a melee zone so don't look for one.
        if(mob != MobType.DekuShooter)
            meleeZone = this.gameObject.transform.Find("MeleeZone").GetComponent<BoxCollider>();
        if (meleeZone == null)
        {
            Debug.Log("Couldn't find melee zone for monster.");
        }

        // Storing agents original angular speed so we can modify it.
        originalAngularSpeed = agent.angularSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.RunStateMachine();
        DamageTimer();

        if(meleeZone != null)
            AttackTimer();


        // Debugging purposes:
        currentState = stateMachine.currentState;

    }

    void DamageTimer()
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

    void TakeDamage()
    {
        this.health -= playerManager.meleeDamage;
        if (this.health <= 0)
        {
            gameObject.SetActive(false);
            DefeatEnemy();
            Debug.Log("Killed an enemy");
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Melee" && !isHit)
        {
            TakeDamage();
            Vector3 pushDirection = this.transform.position - playerManager.player.transform.position;
            //this.transform.position += Vector3.Normalize(pushDirection);
            pushDirection = Vector3.Normalize(pushDirection);
            
            stateMachine.SetState(AIStateMachine.BasicDecisions.DAMAGED);
            //agentRigidbody.AddForce(pushDirection * playerManager.meleeKnockbackForce, ForceMode.Impulse);
            //agentRigidbody.velocity = pushDirection * playerManager.meleeKnockbackForce;
            isHit = true;
            this.gameObject.GetComponent<Renderer>().material = damageMaterial;

            // We set the agents state to damaged which is like a "stun" phase. This is so the agent doesn't try to go towards the player while also getting pushed back.
            //agent.acceleration = 0;

            // We have to ResetPath() here because although we are doing it in the state machine when DAMAGED is the current state, the state machine hasn't neccessarily caught up to swapping to
            // the DAMAGE state. To make sure no path's are active we make sure by resetting here.
            if (agent.hasPath)
            { 
                agent.ResetPath();
                //agent.acceleration = 0;
            }
            agent.velocity = pushDirection * playerManager.meleeKnockbackForce;
            //agent.

            agent.angularSpeed = 0;

            Debug.Log("Enemy took damage.");
        }
	}

    public void Attack()
    {
        meleeZone.gameObject.SetActive(true);
        isAttacking = true;
    }

    public void Shoot(Vector3 target)
    {
        GameObject newBullet = Instantiate(ammunition);

        // Giving the bullet a reference to the controller it came from and the controllers collider.
        BulletData thisData = newBullet.GetComponent<BulletData>();
        thisData.agent = this;
        thisData.agentsCollider = this.gameObject.GetComponent<Collider>();

        newBullet.transform.position = this.transform.position;
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        Vector3 shotDirection = target - newBullet.transform.position;
        shotDirection = Vector3.Normalize(shotDirection);
        shotDirection *= bulletSpeed;
        bulletRigidbody.AddForce(shotDirection, ForceMode.Impulse);
    }

    void AttackTimer()
    {
        if (isAttacking)
        {
            attackCounter += Time.deltaTime;
            if (attackCounter >= 0.3f)
            {
                isAttacking = false;
                meleeZone.gameObject.SetActive(false);
                attackCounter = 0;
            }
        }

        // Safety incase melee box doesn't go away like it's supposed to.
        if (!isAttacking && meleeZone.gameObject.activeSelf)
        {
            meleeZone.gameObject.SetActive(false);
        }
    }

    private void DefeatEnemy()
    {
        GameObject explosion = Instantiate(monsterExplosion);
        explosion.transform.position = this.transform.position;
    }
}
