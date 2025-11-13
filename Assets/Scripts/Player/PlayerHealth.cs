using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Take Damage" +damageAmount); //Implementaría lógica de daño aquí
    }
}
