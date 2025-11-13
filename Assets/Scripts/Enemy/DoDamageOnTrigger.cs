using UnityEngine;

public class DoDamageOnTrigger : MonoBehaviour
{
    [SerializeField] private int damage = 10;  
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
