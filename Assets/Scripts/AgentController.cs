using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform goal;
    NavMeshAgent agent;

    private AIStateMachine stateMachine;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        stateMachine = new AIStateMachine(MobType.Goblin, agent);
        //agent.destination = goal.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
