using System;
using System.Collections;
using UnityEngine;

public class DefeatedState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private RendererController _rendererController;
    private Rigidbody body;
    private float defeatedTimer;
    public static Action OnJumpDefeat;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;

        _rendererController = animator.GetComponentInChildren<RendererController>();
        _rendererController.FadeOutMaterialColor(_context.defeatDuration);

        body = animator.GetComponent<Rigidbody>();
        body.isKinematic = true;              

        OnJumpDefeat?.Invoke();
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
        _context.agent.isStopped = false;

        _rendererController.ResetMaterialColor();
        body.isKinematic = false;
    }
      
}
