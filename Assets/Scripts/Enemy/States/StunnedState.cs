using UnityEngine;
using System.Collections.Generic;
using Cinemachine;
public class StunnedState : StateMachineBehaviour
{
    private EnemyAIContext _context;
    private CinemachineImpulseSource _impulse;
    private RendererController _renderer;
    private Rigidbody body;
    private float stunTimer;
    //private Material enemyMaterial;
    private Color initialColor;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;
        _context.agent.velocity = Vector3.zero;        

        _impulse = animator.GetComponent<CinemachineImpulseSource>();
        if(_impulse != null)
        {
            _impulse.GenerateImpulse();
        }

        body = animator.GetComponent<Rigidbody>();
        body.isKinematic = true;
              
        _renderer = animator.GetComponentInChildren<RendererController>(); //Le cambiamos el color para que se entienda que esta aturdido, aunque le pondría un efecto
        _renderer.SetColor(Color.red);
    }    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stunTimer += Time.deltaTime;
        if(stunTimer > 0.2f) //Pequeño delay para poder recibir daño
        {
            _context.defeatCollider.gameObject.SetActive(true);
        }
        if(stunTimer >= _context.stunDuration)
        {
            _context.isPatrolling = true;
            stunTimer = 0f;            
        }        
        
        CheckDefeatCollision(animator);
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context.agent.isStopped = false;
        _context.isCollisioning = false;
        _context.defeatCollider.gameObject.SetActive(false);

        _renderer.ResetMaterialColor();

        body.isKinematic = false;       
    }

    public void CheckDefeatCollision(Animator animator)
    {
        if (_context.attackCollider == null)
        {
            return;
        }

        Vector3 center = _context.defeatCollider.bounds.center;
        Vector3 halfExtents = _context.defeatCollider.bounds.extents;
        Quaternion orientation = _context.defeatCollider.transform.rotation;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, orientation);

        _context.isCollisioning = false;
        for (int i = 0; i < hits.Length; i++)
        {
            Collider hit = hits[i];
            if (hit.gameObject.CompareTag("Jump"))
            {
                animator.SetTrigger("Defeat");
                break;
            }
        }
    }  
}
