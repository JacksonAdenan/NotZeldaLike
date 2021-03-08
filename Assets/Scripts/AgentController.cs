using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public int health = 5;

    public float xWanderDistance = 2;
    public float yWanderDistance = 2;
    public float attackDistance = 3;

    public Material damageMaterial;
    private Material originalMaterial;

    private AIStateMachine stateMachine;

    private Rigidbody agentRigidbody;
    private PlayerManager playerManager;


    private float tookDamageCounter = 0.0f;
    private bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(MobType.Skeleton, agent, this);

        agentRigidbody = this.gameObject.GetComponent<Rigidbody>();

        playerManager = PlayerManager.GetInstance();

        // Saving a reference to the original material.
        originalMaterial = this.gameObject.GetComponent<Renderer>().material;


        //agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.RunStateMachine();
        DamageTimer();
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
            Destroy(this.gameObject);
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
            agentRigidbody.AddForce(pushDirection * playerManager.meleeKnockbackForce, ForceMode.Impulse);

            isHit = true;
            this.gameObject.GetComponent<Renderer>().material = damageMaterial;

            // We set the agents state to damaged which is like a "stun" phase. This is so the agent doesn't try to go towards the player while also getting pushed back.
            //agent.acceleration = 0;
            //agent.velocity = pushDirection * playerManager.meleeKnockbackForce;
         

            Debug.Log("Enemy took damage.");
        }
	}
}
