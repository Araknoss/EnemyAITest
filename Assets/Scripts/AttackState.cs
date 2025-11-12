using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    private float attackTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();

        Vector3 attackDestination = _context.directionToPlayer() * _context.attackDistance + _context.transform.position;
        Debug.DrawRay(_context.transform.position, _context.directionToPlayer() * _context.attackDistance, Color.blue, 6f);

        // Desactivar la evasión de obstáculos para que el agente atraviese obstáculos
        _context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;        

        _context.agent.SetDestination(attackDestination);
        _context.agent.speed = _context.attackRunSpeed;
        _context.attackCollider.enabled = true;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {       
       if(_context.agent.remainingDistance < 0.2f)
       {
            _context.agent.isStopped = true;
            attackTimer += Time.deltaTime;            
            if(attackTimer >= _context.attackCooldown)
            {
                animator.SetTrigger("Patrol");
                _context.agent.isStopped = false;
            }                          
       }
       //if(_context.isCollisioning)
       //{
       //     animator.SetBool("Stun");   
       //}

        CheckAttackCollision();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackTimer = 0f;
        // (Opcional) Restaurar la evasión de obstáculos
        _context.agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        _context.agent.avoidancePriority = 50;
        _context.attackCollider.enabled = false;
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
