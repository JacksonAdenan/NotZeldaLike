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


    private float damagedCounter = 0.0f; 
    public enum BasicDecisions
    {
        WANDER,
        CHASE,
        ATTACK,
        DAMAGED,
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
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) < controller.attackDistance)
            {
                currentState = BasicDecisions.CHASE;
            }

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
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) >= controller.attackDistance)
            {
                currentState = BasicDecisions.WANDER;
            }

            agent.SetDestination(player.transform.position);

            if (hasReachedDestination)
            {
                hasReachedDestination = false;
            }


            if (!agent.pathPending)
            {
                if (Vector3.Distance(agent.transform.position, player.transform.position) <= 2)
                {
                    hasReachedDestination = true;
                    Debug.Log("Agent completed chase path.");
                    currentState = BasicDecisions.ATTACK;
                    agent.ResetPath();
                }
            }
        }

        else if (currentState == BasicDecisions.ATTACK)
        {
            Debug.Log("Agent is attacking");
            Vector3 lookPosition = player.transform.position - agent.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);
            if (Vector3.Distance(agent.transform.position, player.transform.position) > 1)
            {
                currentState = BasicDecisions.CHASE;
            }
        }

        else if (currentState == BasicDecisions.DAMAGED)
        {
            Debug.Log("Agent is damaged");
            agent.ResetPath();
            agent.updatePosition = false;
            DamagedTimer();
            //Vector3 lookPosition = player.transform.position - agent.transform.position;
            //lookPosition.y = 0;
            //Quaternion rotation = Quaternion.LookRotation(lookPosition);
            //agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);
            //if (Vector3.Distance(agent.transform.position, player.transform.position) > 1)
            //{
            //    currentState = BasicDecisions.CHASE;
            //}
        }
    }

    private void DamagedTimer()
    {
        //if (currentState == BasicDecisions.DAMAGED)
        //{
            damagedCounter += Time.deltaTime;
            if (damagedCounter >= 2)
            {
                damagedCounter = 0;
            // After it's not stunned anymore just make it wander.
                agent.updatePosition = true;
                currentState = BasicDecisions.WANDER;
            }
        //}
    }

    public void SetState(BasicDecisions newState)
    {
        switch (newState)
        {
            case BasicDecisions.ATTACK:
                currentState = BasicDecisions.ATTACK;
                break;
            case BasicDecisions.CHASE:
                currentState = BasicDecisions.CHASE;
                break;
            case BasicDecisions.WANDER:
                currentState = BasicDecisions.WANDER;
                break;
            case BasicDecisions.DAMAGED:
                currentState = BasicDecisions.DAMAGED;
                break;
        }
    }
}
