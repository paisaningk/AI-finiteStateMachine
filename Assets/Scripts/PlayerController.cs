using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum PlayerMode
{
    Normal,
    Power
}

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    public PlayerMode playerMode;
    public int life = 3;

    // Update is called once per frame
    private void Awake()
    {
        playerMode = PlayerMode.Normal;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (playerMode == PlayerMode.Power)
        {
            StartCoroutine(nameof(TimerRoutine));
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }

    private IEnumerator TimerRoutine()
    {
        agent.speed = 10;
        yield return new WaitForSeconds(7);
        agent.speed = 5;
        playerMode = PlayerMode.Normal;
        StopAllCoroutines();

    }
}
