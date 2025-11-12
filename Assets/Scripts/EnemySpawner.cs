using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;    
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            enemy.SetActive(true);
            //enemy.GetComponent<Animator>().SetTrigger("Patrol");
        }
    }
}
