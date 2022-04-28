using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class CleverHide : State
    {
        protected internal CleverHide(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
            
        }

        protected override void Enter()
        {
            timeStamp = Time.time + 10;
            base.Enter();
        }
    
        protected override void Exit()
        {
            base.Exit();
        }
    
        protected override void Update()
        {
            float farFactor = 5;
            float lastDist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDir = Vector3.zero;
            GameObject chosenHide = SetupManager.Instance.hides[0];

            foreach (var hidePosition in SetupManager.Instance.hides)
            {
                Vector3 hideDir = hidePosition.transform.position - player.transform.position;
                Vector3 hidePos = hidePosition.transform.position + hideDir.normalized * farFactor;

                float dist = Vector3.Distance(npc.transform.position, hidePos);
                if (dist < lastDist)
                {
                    lastDist = dist;
                    chosenSpot = hidePos;
                    chosenDir = hideDir;
                    chosenHide = hidePosition;
                }
            }

            Collider hideCol = chosenHide.GetComponent<Collider>();
            Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
            RaycastHit info;
            float distance = 250;
            hideCol.Raycast(backRay, out info, distance);
            
            navMeshAgent.SetDestination(info.point + chosenDir.normalized);
        }
        
        protected override void IsObjective()
        {
            if (timeStamp <= Time.time)
            {
                nextState = new Wander(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
            base.IsObjective();
        }
        
    }
}
