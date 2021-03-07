using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using UnityEditor.AI;

public class AIStateMachine
{
    public MobType stateMachineType;
    public NavMeshAgent agent;
    // We need to know where the player is so we can lookat/attack him.
    public GameObject player;

    private BasicDecisions currentState = BasicDecisions.WANDER;


    private AgentController controller;

    bool hasReachedDestination = true;
    enum BasicDecisions
    {
        WANDER,
        CHASE,
        FLEE
    }
    public AIStateMachine(MobType type, NavMeshAgent agent, AgentController control)
    {
        stateMachineType = type;
        this.agent = agent;
        controller = control;

        PlayerManager playerManager = PlayerManager.GetInstance();
        player = playerManager.player;
    }

    public void RunStateMachine()
    {
        StateMachine1();

        
    }

    private void StateMachine1()
    {
        if (currentState == BasicDecisions.WANDER)
        {
            //if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) < controller.attackDistance)
            //{
            //    //currentState = BasicDecisions.CHASE;
            //}

            if (hasReachedDestination)
            {
                Vector3 destination;
                float ranX = Random.Range(-controller.xWanderDistance, controller.xWanderDistance);
                float ranZ = Random.Range(-controller.yWanderDistance, controller.yWanderDistance);

                destination.x = agent.gameObject.transform.position.x + ranX;
                destination.z = agent.gameObject.transform.position.z + ranZ;
                destination.y = 0;

                agent.SetDestination(destination);
                hasReachedDestination = false;
            }

            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        hasReachedDestination = true;
                        Debug.Log("Agent completed wandering path.");
                    }
                }
            }

        }

        else if (currentState == BasicDecisions.CHASE)
        { 
            
        }
    }
}
