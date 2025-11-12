using System.Collections;
using UnityEngine;

public class DefeatedState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private RendererController _rendererController;
    private Rigidbody body;
    private float defeatedTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();

        _rendererController = animator.GetComponent<RendererController>();

        body = animator.GetComponent<Rigidbody>();
        body.isKinematic = true;

        _rendererController.FadeOutMaterialColor(_context.defeatDuration);
    }    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        defeatedTimer+=Time.deltaTime;
        if(defeatedTimer >= _context.defeatDuration)
        {
            _context.gameObject.SetActive(false);
            defeatedTimer = 0f;
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {     
        _rendererController.ResetMaterialColor();
        body.isKinematic = false;
    }
      
}
