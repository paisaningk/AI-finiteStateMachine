using Finite_State_Machine;
using UnityEngine;
using UnityEngine.AI;

namespace Bot
{
    [RequireComponent(typeof(AiSensor))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class KillerBot : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private AiSensor aiSensor;
        private State currentState;
        private readonly State.NameBot nameBot = State.NameBot.Killer;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            aiSensor = GetComponent<AiSensor>();
        }

        void Start()
        {
            currentState = new Wander(this.gameObject,navMeshAgent,SetupManager.Instance.player,aiSensor.iCanSeePlayer,nameBot);
        }

        // Update is called once per frame
        void Update()
        {
            currentState = currentState.Process(aiSensor.iCanSeePlayer,SetupManager.
                Instance.player.GetComponent<PlayerController>().playerMode);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<PlayerController>().playerMode == PlayerMode.Power)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}