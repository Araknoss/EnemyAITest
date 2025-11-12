using System;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    private float cooldownTimer;    
    private Vector3 attackTarget;
    private float distanceToTarget;

    private float attackTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;

        attackTarget = _context.directionToPlayer() * _context.attackDistance + _context.transform.position;
        attackTarget = new Vector3(attackTarget.x, _context.transform.position.y, attackTarget.z);
        //Debug.DrawRay(_context.transform.position, _context.directionToPlayer() * _context.attackDistance, Color.blue, 6f);

        //_context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;  //Desactivar la evasión de obstáculos

        //_context.agent.SetDestination(attackDestination);
        //_context.agent.speed = _context.attackRunSpeed;
        //_context.attackCollider.gameObject.SetActive(true);
        cooldownTimer = 0f;
        attackTimer = 0f;
        distanceToTarget = 5f;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
       if(distanceToTarget<0.5f)/*_context.agent.remainingDistance < 0.2f*/
       {            
            cooldownTimer += Time.deltaTime;            
            if(cooldownTimer >= _context.attackCooldown)
            {
                _context.isPatrolling = true;
            }                          
       }
       else
       {
            MoveToTarget();
       }
            

        CheckAttackCollision();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context.agent.isStopped = false;
        cooldownTimer = 0f;
        attackTimer = 0f;

        //_context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance; //Para que pueda volver a evitar obstáculos
        //_context.agent.avoidancePriority = 50;
        //_context.attackCollider.gameObject.SetActive(false);
    }    
    private void MoveToTarget()
    {      
        attackTimer += Time.deltaTime;

        // Movimiento manual usando Lerp y la curva de movimiento
        float t = Mathf.Clamp01(attackTimer / 50f);        
        _context.transform.position = Vector3.Lerp(_context.transform.position, attackTarget, _context.attackMovementCurve.Evaluate(t));

        distanceToTarget = Vector3.Distance(_context.transform.position, attackTarget);
    }

    public void CheckAttackCollision()
    {
        if (_context.attackCollider == null)
        {
            return;
        }

        Vector3 center = _context.attackCollider.bounds.center;
        Vector3 halfExtents = _context.attackCollider.bounds.extents;
        Quaternion orientation = _context.attackCollider.transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, orientation);

        _context.isCollisioning = false;
        for (int i = 0; i < hits.Length; i++)
        {
            Collider hit = hits[i];
            if (hit.gameObject.CompareTag("Player") || hit.gameObject.CompareTag("Wall"))
            {                
                _context.agent.isStopped = true;
                _context.isCollisioning = true;
                break;
            }
        }
    }
}


