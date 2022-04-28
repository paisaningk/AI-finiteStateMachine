using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Hide : State
    {
        protected internal Hide(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
        }
        
        protected override void Update()
        {
            float farFactor = 5;
            float lastDist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            foreach (var position in SetupManager.Instance.hides)
            {
                Vector3 hideDir = position.transform.position - player.transform.position;
                Vector3 hidePos = position.transform.position + hideDir.normalized * farFactor;

                float dist = Vector3.Distance(npc.transform.position, hidePos);
                if (dist < lastDist)
                {
                    lastDist = dist;
                    chosenSpot = hidePos;
                }
            }
            navMeshAgent.SetDestination(chosenSpot);
        }
        
        protected override void IsDog()
        {
            if (playerMode == PlayerMode.Normal)
            {
                nextState = new Patrol(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
        }
    }
}