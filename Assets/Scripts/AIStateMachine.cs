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
    public Transform player;

    private BasicDecisions currentState = BasicDecisions.WANDER;

    enum BasicDecisions
    { 
        WANDER,
        ATTACK,
        FLEE
    }
    public AIStateMachine(MobType type)
    {
        stateMachineType = type;
    }

    public void RunStateMachine()
    { 
        
    }

    private void StateMachine1()
    {
        if (currentState == BasicDecisions.WANDER)
        {
            bool hasReachedDestination = false;

            Vector3 destination;
            float ranX = Random.Range(0, 10);
            float ranZ = Random.Range(0, 10);

            destination.x = ranX;
            destination.z = ranZ;
            destination.y = 0;

            while (!hasReachedDestination)
            { 
                agent.SetDestination(destination);

                if (agent.path.status == NavMeshPathStatus.PathComplete)
                {
                    hasReachedDestination = true;
                    Debug.Log("Agent completed wandering path.");
                }
            }
        }
    }
}
