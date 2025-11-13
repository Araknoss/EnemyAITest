using UnityEngine;

public class DoDamageOnCollision : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
    }       
}
