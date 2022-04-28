using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Wander : State
    {
        protected internal Wander(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
            
        }

       
    
        protected override void Update()
        {
            Vector3 wanderTarget = Vector3.zero;
            float wanderRadius = 10;
            float wanderDistance = 10;
            float random = 30;

            wanderTarget += new Vector3(Random.Range(-random, random) * wanderDistance, 0, 
                Random.Range(-random, random) * wanderDistance);
            
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
            Vector3 targetWord = npc.transform.InverseTransformVector(targetLocal);
            
            navMeshAgent.SetDestination(targetWord);
        }

        protected override void IsObjective()
        {
           
            if (iCanSeePlayer)
            {
                nextState = new Flee(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
            base.IsObjective();
        }

        protected override void IsKiller()
        {
            if (iCanSeePlayer)
            {
                nextState = new Pursuit(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
            base.IsKiller();
        }
    }
}
