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

    // Getting stun time from AgentController.
    public float stunTime;


    // Skeleton and other melee agent stuff.
    private float attackWindUpTimer = 0.0f;
    private bool isAttackReady = false;


    // Thromp stuff.
    private float chargeWindupTimer = 0.0f;
    private bool isChargeReady = false;
    private float chargeDurationTimer = 0.0f;

    private Vector3 storedPlayerPos;

    public enum BasicDecisions
    {
        WANDER,
        CHASE,
        ATTACK,
        DAMAGED,
        FLEE,
        TRACK
    }
    public AIStateMachine(MobType type, NavMeshAgent agent, AgentController control)
    {
        stateMachineType = type;
        this.agent = agent;
        controller = control;

        PlayerManager playerManager = PlayerManager.GetInstance();
        player = playerManager.player;

        this.stunTime = control.stunnedTime;
    }

    public void RunStateMachine()
    {
        switch (stateMachineType)
        {
            case MobType.Skeleton:
                StateMachine1();
                break;
            case MobType.Thromp:
                StateMachineThromp();
                break;
            case MobType.Zombie:
                StateMachine1();
                break;
            case MobType.DekuShooter:
                break;
        }

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

    private void StateMachineThromp()
    {
        if (currentState == BasicDecisions.WANDER)
        {
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) <= controller.chaseDistance)
            {
                currentState = BasicDecisions.TRACK;
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

        else if (currentState == BasicDecisions.TRACK)
        {
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) >= controller.chaseDistance)
            {
                currentState = BasicDecisions.WANDER;
                return;
            }

            // Getting the thromp to face the player.
            Vector3 lookPosition = player.transform.position - agent.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);

            // Loading up charge.
            RunChargeTimer();

            if (isChargeReady)
            { 
                Debug.Log("Agent is beginning to charge.");
                SetState(BasicDecisions.ATTACK);
                storedPlayerPos = player.transform.position;
            }          

        }

        else if (currentState == BasicDecisions.ATTACK)
        {
            if (isChargeReady)
            { 
                Debug.Log("Agent is charging.");

                agent.SetDestination(storedPlayerPos);
                controller.Attack();
                ChargeDuration();

                
            }



            //else if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            //{
            //    SetState(BasicDecisions.WANDER);
            //    // If the agent has an attack winded up or charge, reset it to 0.
            //    attackWindUpTimer = 0;
            //    chargeWindupTimer = 0;
            //    agent.ResetPath();
            //    return;
            //}

            else if (Vector3.Distance(agent.transform.position, player.transform.position) >= controller.attackDistance)
            {
                // If the agent has an attack winded up or charge, reset it to 0.
                attackWindUpTimer = 0;
                chargeWindupTimer = 0;
                currentState = BasicDecisions.WANDER;

                agent.ResetPath();
                return;
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

    private void StateMachineShooter()
    {
        if (currentState == BasicDecisions.WANDER)
        {
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) <= controller.chaseDistance)
            {
                currentState = BasicDecisions.TRACK;
                return;
            }

            // Because shooters can't move, they don't need to wander around.

        }

        else if (currentState == BasicDecisions.TRACK)
        {
            if (Vector3.Distance(agent.gameObject.transform.position, player.transform.position) >= controller.chaseDistance)
            {
                currentState = BasicDecisions.WANDER;
                return;
            }

            // Getting the thromp to face the player.
            Vector3 lookPosition = player.transform.position - agent.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, Time.deltaTime * 10);

            // Loading up charge.
            RunChargeTimer();

            if (isChargeReady)
            {
                Debug.Log("Agent is ready to shoot.");
                SetState(BasicDecisions.ATTACK);
                storedPlayerPos = player.transform.position;
            }

        }

        else if (currentState == BasicDecisions.ATTACK)
        {
            if (isChargeReady)
            {
                Debug.Log("Agent has shot.");

                controller.Attack();

                ChargeDuration();


            }

            else if (Vector3.Distance(agent.transform.position, player.transform.position) >= controller.attackDistance)
            {
                // If the agent has an attack winded up or charge, reset it to 0.
                attackWindUpTimer = 0;
                chargeWindupTimer = 0;
                currentState = BasicDecisions.WANDER;

                agent.ResetPath();
                return;
            }


        }

        else if (currentState == BasicDecisions.DAMAGED)
        {
            Debug.Log("Agent is damaged");

            // If the agent has an attack winded up, reset it to 0.
            attackWindUpTimer = 0;

            DamagedTimer();
        }
    }

    private void ChargeDuration()
    {
        chargeDurationTimer += Time.deltaTime;
        if (chargeDurationTimer >= 2)
        {
            isChargeReady = false;
            chargeDurationTimer = 0;
        }
    }
    private void RunChargeTimer()
    {
        chargeWindupTimer += Time.deltaTime;
        if (chargeWindupTimer >= 2)
        {
            isChargeReady = true;
            chargeWindupTimer = 0;

        }
    }
    private void DamagedTimer()
    {

        damagedCounter += Time.deltaTime;
        if (damagedCounter >= stunTime)
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
