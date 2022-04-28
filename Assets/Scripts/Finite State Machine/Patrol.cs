using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
    public class Patrol : State
    {
        private List<GameObject> wayPoint;
        private int currentIndex;
        protected internal Patrol(GameObject npc, NavMeshAgent navMeshAgent, GameObject player, bool iCanSeePlayer, NameBot nameBot) : base(npc, navMeshAgent, player, iCanSeePlayer, nameBot)
        {
            navMeshAgent.speed = 2;
            wayPoint = SetupManager.Instance.waypoint;
        }

        protected override void Enter()
        {
            float lastDist = Mathf.Infinity;
            currentIndex = 0;
            for (int i = 0; i < wayPoint.Count; i++)
            {
                GameObject thisWp = wayPoint[i];
                float distance = Vector3.Distance(npc.transform.position, thisWp.transform.position);

                if (distance < lastDist)
                {
                    currentIndex = i - 1;
                    lastDist = distance;
                }
            }
            base.Enter();
        }

        protected override void Update()
        {
            if (navMeshAgent.remainingDistance < 1)
            {
                if (currentIndex >= wayPoint.Count - 1)
                {
                    currentIndex = 0;
                }
                else
                {
                    currentIndex++;
                }

                navMeshAgent.SetDestination(wayPoint[currentIndex].transform.position);
            }
        }

        protected override void IsDog()
        {
            if (iCanSeePlayer)
            {
                nextState = new Seek(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
                evenstate = EvenState.Exit;
            }
            base.IsDog();
        }
    }
}
