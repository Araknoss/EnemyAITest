using UnityEngine;

public class StunnedState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private float stunTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context= animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;
    }    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stunTimer += Time.deltaTime;
        if(stunTimer >= _context.stunDuration)
        {
            animator.SetTrigger("Patrol");
            stunTimer = 0f;
            _context.agent.isStopped = false;
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
