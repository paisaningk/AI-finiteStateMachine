using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Evade : State
    {
        protected internal Evade(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
            
        }
        
        protected override void Update()
        {
            navMeshAgent.SetDestination(FleeCalculationFormula(CalculateMoveDirection()));
        }
        
        protected override void IsKiller()
        {
            if (playerMode == PlayerMode.Normal)
            {
                nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
        }
    }
}