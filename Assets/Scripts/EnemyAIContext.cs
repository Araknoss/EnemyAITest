using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyAIContext : MonoBehaviour 
{
    public NavMeshAgent agent;
    public Transform player;
    [SerializeField] private Animator animator;

    [Header("Patrol Settings")]
    public float patrolSpeed = 3.5f;
    public List<Transform> patrolPoints;
    public bool isPatrolling;

    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float detectionConeRange = 20f;
    public float detectionAngle = 40f;
    public bool isDetected;    
    public LayerMask obstacleMask;

    [Header("Chase Settings")]
    public float chaseSpeed = 5.5f;

    [Header("Charge Settings")]
    public bool isInChargeRange;
    public float rotationSpeed = 5f;
    public GameObject chargeEffect;    

    [Header("Attack Settings")]
    public float attackRange = 5f; 
    public float attackCooldown = 2f;
    public float attackDistance = 10f;
    public float attackRunSpeed = 8f;
    public bool isCollisioning=false;
    public BoxCollider attackCollider;
    public AnimationCurve attackMovementCurve;

    [Header("Stun Settings")]
    public float stunDuration = 2f;    

    [Header("Defeat Settings")]
    public float defeatDuration = 2f;
    public BoxCollider defeatCollider;


    private void Awake()
    {
        animator=gameObject.GetComponent<Animator>();
        attackCollider.enabled = false;
    }
    private void Update()
    {
        if(animator != null)
        {
            SetAnimatorParameters();
        }             
    }
    private void SetAnimatorParameters()
    {
        animator.SetBool("isDetected", isDetected);
        animator.SetBool("isInChargeRange", isInChargeRange);
        animator.SetBool("isCollisioning", isCollisioning);
        animator.SetBool("isPatrolling", isPatrolling);
    }
    public Vector3 directionToPlayer()
    {
        return (player.position - transform.position).normalized;
    }

    public float distanceToPlayer()
    {
        return Vector3.Distance(player.position, transform.position);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionConeRange);

        Gizmos.color = Color.blue;
        Vector3 rightBoundary = Quaternion.Euler(0, detectionAngle / 2, 0) * transform.forward * detectionConeRange;
        Vector3 leftBoundary = Quaternion.Euler(0, -detectionAngle / 2, 0) * transform.forward * detectionConeRange;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    }
}
