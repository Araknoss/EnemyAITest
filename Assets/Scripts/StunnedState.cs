using UnityEngine;
using System.Collections.Generic;
public class StunnedState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private float stunTimer;
    private Material enemyMaterial;
    private Color initialColor;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context= animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;
        _context.agent.velocity = Vector3.zero;
        enemyMaterial = animator.gameObject.GetComponent<Renderer>().material;
        initialColor = enemyMaterial.color;
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
        if(Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("Defeat");
            stunTimer = 0f;
        }        
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Restaurar el alpha original al salir del estado
        if (enemyMaterial != null)
        {
            enemyMaterial.color = initialColor;
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
