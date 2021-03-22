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

    private void Awake() => Initialise();
    private void Update()
    {
        m_Tree.Tick();
        m_MoveSpeed = Mathf.Clamp(m_MoveSpeed, 0.0f, 1.0f);
        //Animate();
    }

    private void Initialise()
    {
        SetTarget(PlayerManager.GetInstance().player.transform);

        agent = this.GetComponent<NavMeshAgent>();


        m_Tree = new BehaviourTree();
        m_Animator = GetComponentInChildren<Animator>();

        Func<bool> atPosition = () => Vector3.Distance(transform.position, m_Target.position) < 0.5f;

        LeafNode moveTo = new LeafNode(m_Tree, "Move To Position", () => MoveToPosition(m_Target.position), () => !atPosition());
        LeafNode wander = new LeafNode(m_Tree, "Wander", () => agent.SetDestination(RandomNavMeshLocation(5)), () => CompletedWander());
        LeafNode idle = new LeafNode(m_Tree, "Idle", DoNothing, atPosition);
        SelectorNode atTarget = new SelectorNode(m_Tree, "At Target", idle, wander);

        m_Tree.SetRootNode(atTarget);
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
        Vector3 randomDirection = UnityEngine.Random.insideUnitCircle * radius;
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

    private bool CompletedWander()
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
}
