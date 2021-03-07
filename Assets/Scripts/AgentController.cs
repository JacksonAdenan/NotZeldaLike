using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    NavMeshAgent agent;
    public float xWanderDistance = 2;
    public float yWanderDistance = 2;
    public float attackDistance = 3;

    private AIStateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(MobType.Skeleton, agent, this);
        //agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.RunStateMachine();
    }
}
