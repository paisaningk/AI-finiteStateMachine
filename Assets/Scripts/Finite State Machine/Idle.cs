using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Idle : State
    {
        protected internal Idle(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
            
        }
        
        protected override void Enter()
        {
            timeStamp = Time.time + 5;
            base.Enter();
        }
        
        protected override void Update()
        {
            
        }

        protected override void IsObjective()
        {
            if (timeStamp <= Time.time)
            {
                if (playerMode == PlayerMode.Power)
                {
                    nextState = new Seek(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
                else
                {
                    nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
            }
        }

        protected override void IsKiller()
        {
            if (timeStamp <= Time.time)
            {
                if (playerMode == PlayerMode.Power)
                {
                    nextState = new Evade(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
                if (DistancePlayer() > 10)
                {
                    nextState = new Pursuit(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
                else
                {
                    nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
            }
        }
        
        protected override void IsDog()
        {
            if (timeStamp <= Time.time)
            {
                if (playerMode == PlayerMode.Power)
                {
                    nextState = new Hide(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
                if (DistancePlayer() > 10)
                {
                    nextState = new Patrol(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
                else
                {
                    nextState = new Seek(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                    evenstate = EvenState.Exit;
                }
            }
        }
        
    }
}