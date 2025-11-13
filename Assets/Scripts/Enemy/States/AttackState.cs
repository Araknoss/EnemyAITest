using System;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    private float attackTimer;
    public static Action<bool> OnChargedAttack;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();

        Vector3 attackDestination = _context.directionToPlayer() * _context.attackDistance + _context.transform.position;
        Debug.DrawRay(_context.transform.position, _context.directionToPlayer() * _context.attackDistance, Color.blue, 6f);
        
        _context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;  //Desactivar la evasión de obstáculos

        _context.agent.SetDestination(attackDestination);
        _context.agent.speed = _context.attackRunSpeed;
        _context.attackCollider.gameObject.SetActive(true);

        OnChargedAttack?.Invoke(true);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
       if(_context.agent.remainingDistance < 0.2f)
       {            
            attackTimer += Time.deltaTime;            
            if(attackTimer >= _context.attackCooldown)
            {
                _context.isPatrolling = true;
            }                          
       }      

       CheckAttackCollision();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer = 0f;        

        _context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance; //Para que pueda volver a evitar obstáculos
        _context.agent.avoidancePriority = 50;
        _context.attackCollider.gameObject.SetActive(false);

        OnChargedAttack?.Invoke(false);
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
                _context.isCollisioning = true;
                if(hit.gameObject.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(10); 
                    Debug.Log("AtaqueGolpeaJugador");
                    _context.attackCollider.gameObject.SetActive(false);
                }
                break;
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
