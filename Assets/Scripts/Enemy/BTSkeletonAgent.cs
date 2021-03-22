using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhiteWillow;
using System;
using UnityEngine.AI;

public class BTSkeletonAgent : MonoBehaviour
{
    private float m_MoveSpeed = 0.0f;
    private float m_TurnAngle = 0.0f;

    public Transform m_Target;

    private Animator m_Animator;
    private BehaviourTree m_Tree;



    // ------- Non Behaviour Tree Stuff ------- //
    NavMeshAgent agent;

    PlayerManager playerManager;

    float attackWindupCounter = 0;
    bool isAttackReady = false;
    BoxCollider meleeZone;

    // ------- Enemy Stats ------- //
    [Header("Enemy Stats")]
    public float health = 5;
    public int meleeDamage = 3;
    public float knockback = 3;

    [Header("Other")]
    public GameObject monsterExplosion;
    public Material damagedMaterial;
    private Material originalMaterial;
    private Renderer agentRenderer;

    private float originalAngularSpeed;

    [Header("Behavioural Stats")]
    public float stunTime;
    public float stunCounter = 0;
    private bool isEnemyHit = false;

    private void Awake() => Initialise();
    private void Update()
    {
        m_Tree.Tick();
        m_MoveSpeed = Mathf.Clamp(m_MoveSpeed, 0.0f, 1.0f);
        //Animate();
    }

    private void Initialise()
    {
        playerManager = PlayerManager.GetInstance();

        SetTarget(PlayerManager.GetInstance().player.transform);
        meleeZone = FindMeleeZone();

        agent = this.GetComponent<NavMeshAgent>();

        originalAngularSpeed = agent.angularSpeed;

        agentRenderer = transform.GetComponent<Renderer>();
        originalMaterial = agentRenderer.material;

        m_Tree = new BehaviourTree();
        m_Animator = GetComponentInChildren<Animator>();

        Func<bool> atPosition = () => Vector3.Distance(transform.position, m_Target.position) < 0.5f;

        LeafNode moveTo = new LeafNode(m_Tree, "Move To Position", () => MoveToPosition(m_Target.position), () => !atPosition());
        LeafNode wander = new LeafNode(m_Tree, "Wander", () => agent.SetDestination(RandomNavMeshLocation(5)), () => HasCompleteWander());
        LeafNode idle = new LeafNode(m_Tree, "Idle", DoNothing, atPosition);
        LeafNode chase = new LeafNode(m_Tree, "Chase", () => agent.SetDestination(m_Target.position), () => TargetWithinRange(5));
        LeafNode attack = new LeafNode(m_Tree, "Attack", () => Attack(), () => TargetWithinRange(1));
        LeafNode damaged = new LeafNode(m_Tree, "Damaged", () => StunEnemy(stunTime), () => CheckIsEnemyHit());

        SelectorNode atTarget = new SelectorNode(m_Tree, "At Target", attack);

        SequenceNode noTargetNear = new SequenceNode(m_Tree, "No Target Near", wander);
        SelectorNode targetNear = new SelectorNode(m_Tree, "Target near", atTarget, chase);


        SelectorNode notDamaged = new SelectorNode(m_Tree, "Not damaged", targetNear, noTargetNear);

        SelectorNode start = new SelectorNode(m_Tree, "Start", damaged, notDamaged);

        //attack.SetParent(atTarget);
        //atTarget.SetParent(targetNear);
        //chase.SetParent(targetNear);
        //targetNear.SetParent(notDamaged);
        //
        //wander.SetParent(noTargetNear);
        //noTargetNear.SetParent(notDamaged);
        //
        //notDamaged.SetParent(start);
        //damaged.SetParent(start);

        SelectorNode testStart = new SelectorNode(m_Tree, "Test Start", damaged);
        damaged.SetParent(testStart);


        m_Tree.SetRootNode(testStart);
    }

    private void Animate()
    {
        m_Animator.SetFloat("MoveSpeed", m_MoveSpeed);
    }

    private void DoNothing()
    {
        m_MoveSpeed -= 2.0f * Time.deltaTime;
    }

    private void MoveToPosition(Vector3 position)
    {
        m_TurnAngle = Vector3.Dot(transform.forward, Vector3.Cross(transform.position, position));
        m_Animator.SetFloat("MoveX", m_TurnAngle);
        m_MoveSpeed += 2.0f * Time.deltaTime;

        Vector3 lookDirection = Vector3.RotateTowards(transform.forward, position - transform.position, 2.5f * Time.deltaTime, 2.5f);
        transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    private Vector3 RandomNavMeshLocation(float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        else // The random number generated isn't on the navmesh, so re-generate a new number
        {
            finalPosition = transform.position;
        }
        return finalPosition;
    }

    private bool HasCompleteWander()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("Agent completed wandering path.");
                    return true;
                }
            }
        }
        return false;
    }

    private void SetTarget(Transform target) { m_Target = target; }

    private bool TargetWithinRange(float rangeRequired)
    {
        if (Vector3.Distance(transform.position, m_Target.position) <= rangeRequired)
            return true;
        else
            return false;
    }
    private void LookAtTarget()
    {
        Vector3 lookPosition = m_Target.position - transform.position;
        lookPosition.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPosition);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);
    }

    private void Attack()
    {
        LookAtTarget();

        attackWindupCounter += Time.deltaTime;
        if (attackWindupCounter >= 1)
            isAttackReady = true;

        if (isAttackReady)
        {
            isAttackReady = false;
            attackWindupCounter = 0;
            meleeZone.gameObject.SetActive(true);
        }
        else
        {
            meleeZone.gameObject.SetActive(false);
        }
    }

    private BoxCollider FindMeleeZone()
    {
        BoxCollider collider = transform.Find("MeleeZone").GetComponent<BoxCollider>();
        return collider;
    }

    private void TakeDamage()
    {
        isEnemyHit = true;

        if(agent.hasPath)
            agent.ResetPath();

        //agent.velocity = Vector3.zero;
        agent.angularSpeed = 0;

        RemoveHealth();
        KnockBackEnemy(m_Target.position);

        SetMaterial(damagedMaterial);


        StunEnemy(stunTime);
    }
    private void RemoveHealth()
    {
        this.health -= playerManager.meleeDamage;
        if (this.health <= 0)
        {
            gameObject.SetActive(false);
            DefeatEnemy();
            Debug.Log("Killed an enemy");
        }
    }
    private void DefeatEnemy()
    {
        GameObject explosion = Instantiate(monsterExplosion);
        explosion.transform.position = this.transform.position;
    }

    private void KnockBackEnemy(Vector3 pushersPosition)
    {
        Vector3 pushDirection = transform.position - pushersPosition;
        pushDirection = Vector3.Normalize(pushDirection);

        agent.velocity = pushDirection * playerManager.meleeKnockbackForce;
    }

	public void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Melee")
        {
            TakeDamage();
        }
	}

    private void SetMaterial(Material mat) { agentRenderer.material = mat; }

    private void StunEnemy(float stunTime)
    {
        Debug.Log("StunEnemy() called.");

        stunCounter += Time.deltaTime;
        if (stunCounter >= stunTime)
        {
            stunCounter = 0;
            isEnemyHit = false;
            agentRenderer.material = originalMaterial;
            agent.angularSpeed = originalAngularSpeed;

            agent.velocity = Vector3.zero;
        }
    }
    private bool CheckIsEnemyHit() { return (isEnemyHit); }

}
