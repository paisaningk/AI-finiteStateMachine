using System.Collections.Generic;
using UnityEngine;

namespace Finite_State_Machine
{
    public class SetupManager : MonoBehaviour
    { 
        public GameObject[] hides;
        public List<GameObject> waypoint;
        public GameObject player;
        public static SetupManager Instance { get; private set; }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            hides = GameObject.FindGameObjectsWithTag("hide");
        }
    }
}
