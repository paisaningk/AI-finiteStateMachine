using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Pursuit : State
    {
        protected internal Pursuit(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
        }
        

        protected override void Update()
        {
            navMeshAgent.SetDestination(CalculateMoveDirection());
        }

        protected override void IsKiller()
        {
            if (DistancePlayer() < 5 && iCanSeePlayer)
            {
                player.GetComponent<PlayerController>().life -= 1;
                nextState = new Idle(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
                
            if (DistancePlayer() > 10)
            {
                nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
        }
    }
}
