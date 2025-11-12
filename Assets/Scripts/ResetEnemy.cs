using UnityEngine;

public class ResetEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private Vector3 startPosition;

    private void Awake()
    {
        startPosition = enemy.transform.position;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            enemy.SetActive(true);
            enemy.GetComponent<Animator>().SetTrigger("Reset");
            enemy.transform.position = startPosition;
        }
    }
}
