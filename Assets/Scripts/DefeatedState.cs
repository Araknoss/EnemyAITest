using UnityEngine;

public class DefeatedState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private Material material;
    private Color initialColor;
    private float defeatTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        material = animator.gameObject.GetComponent<Renderer>().material;
        initialColor = material.color;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(defeatTimer < _context.defeatDuration)
        {
            defeatTimer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, defeatTimer / _context.defeatDuration);
            material.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
        }
        else
        {
            animator.gameObject.SetActive(false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
