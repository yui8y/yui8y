using UnityEngine;

public class ComeCustomer : MonoBehaviour
{
    Transform target;
    public float speed = 2f;
    
    void Start()
    {
        target = GameManager.instance.counterPoint;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("会話開始！");
        }
    }

}
