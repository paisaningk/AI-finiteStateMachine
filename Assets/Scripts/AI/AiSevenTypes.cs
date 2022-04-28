using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AI
{
    public enum Type
    {
        Seek,
        Flee,
        Pursuit,
        Evade,
        Wander,
        Hide,
        CleverHide,
        Finite
    }
    public class AiSevenTypes : MonoBehaviour
    {
        public NavMeshAgent navMeshAgent;
        public GameObject target;
        public Type type;
        public GameObject[] hides;
        private Vector3 wanderTarget = Vector3.zero;
        public TextMesh textStatus;
        
        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            TargetCanSeeMe();
            var targetPosition = target.transform.position;
            switch (type)
            {
                case Type.Seek:
                    Seek(targetPosition);
                    break;
                case Type.Flee:
                    Flee(targetPosition);
                    break;
                case Type.Pursuit:
                    Pursuit();
                    break;
                case Type.Evade:
                    Evade();
                    break;
                case Type.Wander:
                    Wander();
                    break;
                case Type.Hide:
                    Hide();
                    break;
                case Type.CleverHide:
                    if (CanSeeTarget() && TargetCanSeeMe())
                    {
                        Pursuit();
                    }
                    else
                    {
                        CleverHide();
                    }
                    break;
                case Type.Finite:
                    Finite();
                    break;
            }
        }

        void Finite()
        {
            var distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < 10 && TargetCanSeeMe())
            {
                CleverHide();
                textStatus.text = "Clever Hide";
            }

            if (distance < 10 && TargetCanSeeMe() == false)
            {
                Pursuit();
                textStatus.text = "Pursuit";
            }

            if (distance > 15)
            {
                Wander();
                textStatus.text = "Wander";
            }
        }

        void Seek(Vector3 position)
        {
            navMeshAgent.SetDestination(position);
        }

        void Flee(Vector3 position)
        {
            var transformPosition = transform.position;
            navMeshAgent.SetDestination(transformPosition - position + transformPosition);
        }

        void Pursuit()
        {
            Seek(target.transform.position + target.transform.forward *  LookAhead());
        }

        void Evade()
        {
           Flee(target.transform.position + target.transform.forward *  LookAhead());
        }

        void Wander()
        {
            float wanderRadius = 10;
            float wanderDistance = 10;
            float wanderJitter = 1;

            wanderTarget += new Vector3(Random.Range(-1, 1) * wanderJitter, 0, Random.Range(-1, 1) * wanderJitter);
            
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
            Vector3 targetWord = transform.InverseTransformVector(targetLocal);
            
            Seek(targetWord);
        }

        void Hide()
        {
            float farFactor = 5;
            float lastDist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;

            foreach (var position in hides)
            {
                Vector3 hideDir = position.transform.position - target.transform.position;
                Vector3 hidePos = position.transform.position + hideDir.normalized * farFactor;

                float dist = Vector3.Distance(transform.position, hidePos);
                if (dist < lastDist)
                {
                    lastDist = dist;
                    chosenSpot = hidePos;
                }
            }
            
            Seek(chosenSpot);
        }

        void CleverHide()
        {
            float farFactor = 5;
            float lastDist = Mathf.Infinity;
            Vector3 chosenSpot = Vector3.zero;
            Vector3 chosenDir = Vector3.zero;
            GameObject chosenHide = hides[0];

            foreach (var hidePosition in hides)
            {
                Vector3 hideDir = hidePosition.transform.position - target.transform.position;
                Vector3 hidePos = hidePosition.transform.position + hideDir.normalized * farFactor;

                float dist = Vector3.Distance(transform.position, hidePos);
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
            Seek(info.point + chosenDir.normalized);
        }

        bool CanSeeTarget()
        {
            RaycastHit raycastInfo;
            Vector3 rayToTarget = target.transform.position - this.transform.position;
            if(Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
            {
                if (raycastInfo.transform.gameObject.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }

        bool TargetCanSeeMe()
        {
            bool result = false;
            Vector3 toAgent = this.transform.position - target.transform.position;
            float lookingAngle = Vector3.Angle(target.transform.forward, toAgent);

            if(lookingAngle < 60)
            {
                result = true;
            }
            
            Debug.DrawLine(transform.position,target.transform.position);
            Debug.Log(lookingAngle);
            return result;
            
        }

        private float LookAhead()
        {
            var transformPlayer = target.transform;
            Vector3 targetDir = transformPlayer.position - this.transform.position;
            float playerSpeed = target.GetComponent<NavMeshAgent>().velocity.magnitude;
            float lookAhead = targetDir.magnitude / (navMeshAgent.speed + playerSpeed);
            return lookAhead;
        }
    }
}
