using UnityEngine;

public class ChargeState : StateMachineBehaviour
{
    private EnemyAIContext _context;    
    private float chargeTimer;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context = animator.GetComponent<EnemyAIContext>();
        _context.agent.isStopped = true;   

        chargeTimer = 0f;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        chargeTimer += Time.deltaTime;
        if (chargeTimer >= 1f)
        {
            animator.SetTrigger("Attack");
            chargeTimer = 0f;
        }

        LookAtPlayer();
        _context.chargeEffect.SetActive(true);
        _context.exclamationEffect.SetActive(true);

    }    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _context.agent.isStopped = false;
        _context.isInChargeRange = false;   
        _context.chargeEffect.SetActive(false);
        _context.exclamationEffect.SetActive(false);
    }
    private void LookAtPlayer()
    {
        Vector3 direction = _context.directionToPlayer();
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        _context.transform.rotation = Quaternion.Slerp(_context.transform.rotation, lookRotation, Time.deltaTime * _context.rotationSpeed);
    }
}
