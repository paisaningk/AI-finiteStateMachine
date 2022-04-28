using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Seek : State
    {
        protected internal Seek(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
        }
        
        
        protected override void Update()
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

        protected override void IsObjective()
        {
            if (DistancePlayer() < 5 && iCanSeePlayer)
            {
                var playerController = player.GetComponent<PlayerController>();
                playerController.life -= 1;
                //playerController.playerMode = PlayerMode.Normal;
                nextState = new Idle(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }

            if (playerMode == PlayerMode.Normal)
            {
                nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
        }
        
        protected override void IsDog()
        {
            if (DistancePlayer() > 10)
            {
                nextState = new Patrol(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
                
            if (DistancePlayer() < 5 && iCanSeePlayer)
            {
                player.GetComponent<PlayerController>().life -= 1;
                nextState = new Idle(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
        }
    }
}