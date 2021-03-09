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


    // This is public just for debugging purposes.
    public BasicDecisions currentState = BasicDecisions.WANDER;


    private AgentController controller;

    bool hasReachedDestination = true;


    private float damagedCounter = 0.0f;


    private float attackWindUpTimer = 0.0f;
    private bool isAttackReady = false;
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
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) <= controller.chaseDistance)
            {
                currentState = BasicDecisions.CHASE;
                return;
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
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) >= controller.chaseDistance)
            {
                currentState = BasicDecisions.WANDER;
                return;
            }

            if (!agent.pathPending)
            {
                if (Vector3.Distance(agent.transform.position, player.transform.position) < controller.attackDistance)
                {
                    hasReachedDestination = true;
                    Debug.Log("Agent completed chase path.");
                    currentState = BasicDecisions.ATTACK;
                    //agent.ResetPath();
                    return;
                }
            }


            agent.SetDestination(player.transform.position);

            if (hasReachedDestination)
            {
                hasReachedDestination = false;
            }


            
        }

        else if (currentState == BasicDecisions.ATTACK)
        {

            Debug.Log("Agent is attacking");
            Vector3 lookPosition = player.transform.position - agent.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);
            if (Vector3.Distance(agent.transform.position, player.transform.position) >= controller.attackDistance)
            {
                // If the agent has an attack winded up, reset it to 0.
                attackWindUpTimer = 0;
                currentState = BasicDecisions.CHASE;
                return;
            }

            // Actual attack wind up.
            AttackTimer();
            if (isAttackReady)
            {
                isAttackReady = false;
                attackWindUpTimer = 0;
                controller.Attack();
            }
        }

        else if (currentState == BasicDecisions.DAMAGED)
        {
            Debug.Log("Agent is damaged");
            //agent.ResetPath();
            //agent.updatePosition = false;

            // If the agent has an attack winded up, reset it to 0.
            attackWindUpTimer = 0;

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

        damagedCounter += Time.deltaTime;
        if (damagedCounter >= 2)
        {
            damagedCounter = 0;
            // After it's not stunned anymore just make it wander.
            //agent.updatePosition = true;
            //agent.nextPosition = agent.transform.position;



            currentState = BasicDecisions.WANDER;
            // IMPORTANT. Set the agent's velocity to zero otherwise he will move really quickly when swapping to the next state.
            agent.velocity = Vector3.zero;
            agent.angularSpeed = controller.originalAngularSpeed;
        }
  
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


    private void AttackTimer()
    {
        attackWindUpTimer += Time.deltaTime;
        if (attackWindUpTimer >= 1)
        {
            isAttackReady = true;
        }
    }
}
