using UnityEngine;

public class ChaseState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        _context.agent.speed = _context.patrolSpeed;   
        
        _context.isDetected = true;
        _context.isInChargeRange = false;
        _context.isPatrolling = false;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context.agent.SetDestination(_context.player.position);

        if (_context.distanceToPlayer() <= _context.attackRange)
        {
            _context.isInChargeRange = true;
        }
        else
        {
            _context.isInChargeRange = false;
        }

        DetectionCheck(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context.isDetected = false;
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