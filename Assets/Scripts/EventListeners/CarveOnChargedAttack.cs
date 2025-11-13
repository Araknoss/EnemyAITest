using UnityEngine;
using UnityEngine.AI;

public class CarveOnChargedAttack : MonoBehaviour
{
    private NavMeshObstacle navMeshObstacle;
    private void Awake()
    {
        navMeshObstacle=gameObject.GetComponent<NavMeshObstacle>();
    }
    private void OnEnable()
    {
        AttackState.OnChargedAttack += HandleChargedAttack;
    }

    private void OnDisable()
    {
        AttackState.OnChargedAttack -= HandleChargedAttack;
    }

    private void HandleChargedAttack(bool isAttacking) //Hacemos que durante el ataque cargado el enemigo se pueda chocar con los obstáculos del NavMesh
    {
        if(navMeshObstacle != null)
            navMeshObstacle.carving = !isAttacking;
    }
}
