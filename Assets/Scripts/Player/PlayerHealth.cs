using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private bool isInvulnerable = false;
    [SerializeField] private float invulnerabilityDuration = 1f;
    public void TakeDamage(int damageAmount)
    {
        if (isInvulnerable) return;
        Debug.Log("Take Damage" +damageAmount); //Implementaría lógica de daño aquí, seguramente lanzaría un evento a los distintos
                                                //sistemas como UI, sonido, efectos visuales, etc.
        StartCoroutine(InvulnerabilityCo());
    }

    IEnumerator InvulnerabilityCo()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}
