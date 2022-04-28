using UnityEngine;
using UnityEngine.AI;

namespace Finite_State_Machine
{
   public class State
   { 
      protected enum EvenState
      {
         Enter, Update, Exit
      }

      public enum NameBot
      {
         Objective,   
         Killer,
         Dog
      }
      
      protected NameBot nameBot;
      protected EvenState evenstate;
      protected GameObject npc;

      protected GameObject player;
      protected State nextState;
      protected NavMeshAgent navMeshAgent;
      protected bool iCanSeePlayer;
      protected float timeStamp;
      protected PlayerMode playerMode;
      
      protected State(GameObject npc,NavMeshAgent navMeshAgent, GameObject player,bool iCanSeePlayer,NameBot nameBot)
      {
         this.npc = npc;
         this.navMeshAgent = navMeshAgent;
         this.evenstate = EvenState.Enter;
         this.player = player;
         this.iCanSeePlayer = iCanSeePlayer;
         this.nameBot = nameBot;
         timeStamp = 0;
         playerMode = player.GetComponent<PlayerController>().playerMode;
         navMeshAgent.isStopped = false;
         navMeshAgent.ResetPath();
      }

      protected virtual void Enter()
      {
         evenstate = EvenState.Update;
      }

      protected virtual void Update()
      {
         evenstate = EvenState.Update;
      }

      protected virtual void Exit()
      {
         evenstate = EvenState.Exit;
      }
   
      public State Process(bool isCanSeePlayer,PlayerMode playerMode)
      {
         if (evenstate == EvenState.Exit)
         {
            Exit();
            return nextState;
         }
         if (evenstate == EvenState.Enter)
         {
            Enter();
         }
         if (evenstate == EvenState.Update)
         {
            Update();
            iCanSeePlayer = isCanSeePlayer;
            this.playerMode = playerMode;
            CheckName(nameBot);
         }
         return this;
      }

      protected void CheckName(NameBot namebot)
      {
         if (namebot == NameBot.Objective)
         {
            IsObjective();
         }
         else if (namebot == NameBot.Dog)
         {
            IsDog();
         }
         else if (namebot == NameBot.Killer)
         {
            IsKiller();
         }
      }

      protected virtual void IsObjective()
      {
         if (playerMode != PlayerMode.Power) return;
         nextState = new Seek(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
         evenstate = EvenState.Exit;
      }
      
      protected virtual void IsDog()
      {
         if (playerMode != PlayerMode.Power) return;
         nextState = new Hide(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
         evenstate = EvenState.Exit;
      }
      
      protected virtual void IsKiller()
      {
         if (playerMode != PlayerMode.Power) return;
         nextState = new Evade(npc, navMeshAgent, player, iCanSeePlayer, nameBot);
         evenstate = EvenState.Exit;
      }


      protected float DistancePlayer()
      {
         return Vector3.Distance(npc.transform.position,player.transform.position);
      }
   
      protected float LookAhead()
      {
         var transformPlayer = player.transform;
         Vector3 targetDir = transformPlayer.position - npc.transform.position;
         float playerSpeed = player.GetComponent<NavMeshAgent>().velocity.magnitude;
         float lookAhead = targetDir.magnitude / (navMeshAgent.speed + playerSpeed);
         return lookAhead;
      }
      
      protected Vector3 FleeCalculationFormula(Vector3 target)
      {
         // npc.position + (-1 *(player.position - npc.position))
         // Adjust the above equation to npc.position - player.position + npc.position
         
         var npcTransform = npc.transform.position;
         return npcTransform - target + npcTransform;
      }

      //For Pursuit , Evade
      protected Vector3 CalculateMoveDirection()
      {
          return player.transform.position + player.transform.forward * LookAhead();
      }
   }
}

