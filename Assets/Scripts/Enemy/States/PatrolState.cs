using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PatrolState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    private List<Transform> patrolPoints = new List<Transform>();
    private int currentPatrolIndex = 0;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context= animator.GetComponent<EnemyAIContext>();       
        _context.agent.speed = _context.patrolSpeed; //En cada estado se configura la velocidad del agente segun el comportamiento

        GetPatrolPoints();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (!_context.agent.pathPending && _context.agent.remainingDistance < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count; //Loop entre todos los puntos de patrulla
            _context.agent.SetDestination(patrolPoints[currentPatrolIndex].position);

            Debug.Log("Next Patrol Point: " +currentPatrolIndex);
        }

        DetectionCheck(animator);        
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {        
        patrolPoints.Clear();        
    }

    private void GetPatrolPoints()
    {
        patrolPoints.Clear();
        for (int i = 0; i < _context.patrolPoints.Count; i++)
        {
            patrolPoints.Add(_context.patrolPoints[i]);
        }
        currentPatrolIndex = 0;
        if (patrolPoints.Count > 0)
        {
            _context.agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    private void DetectionCheck(Animator animator)
    {        
        Vector3 forward = _context.transform.forward;           
        float distanceToTarget = _context.distanceToPlayer();
        Vector3 origin = _context.transform.position;

        if (distanceToTarget <= _context.detectionRange) //Deteccion por rango simple
        {           
            _context.isDetected = true;
            return;
        }
        else if (distanceToTarget <= _context.detectionConeRange) //Deteccion por cono de vision
        {
            Vector3 directionToTarget = _context.directionToPlayer();

            if (Vector3.Angle(forward, directionToTarget) < _context.detectionAngle / 2)
            {
                Debug.DrawRay(origin, directionToTarget * distanceToTarget, Color.red);
                if (!Physics.Raycast(origin, directionToTarget, distanceToTarget, _context.obstacleMask)) //Si no hay obstaculos entre el enemigo y el jugador
                {
                    _context.isDetected = true;
                }
                else
                {
                    _context.isDetected = false;
                }
            }
            else
            {
                _context.isDetected = false;
            }
        }
        else
        {
            _context.isDetected = false;
        }
    }    
}
