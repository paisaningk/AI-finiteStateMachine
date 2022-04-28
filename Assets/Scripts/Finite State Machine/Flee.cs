using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Flee : State
    {
        protected internal Flee(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
        }
        
        protected override void Enter()
        {
            timeStamp = Time.time + 10;
            navMeshAgent.speed = 10;
            base.Enter();
        }
        
        protected override void Update()
        {
            navMeshAgent.SetDestination(FleeCalculationFormula(player.transform.position));
        }
        
        protected override void IsObjective()
        {
            if (timeStamp <= Time.time)
            {
                navMeshAgent.speed = 5;
                nextState = new CleverHide(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
            base.IsObjective();
        }
    }
}