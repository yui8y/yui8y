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
        if (Vector2.Distance(transform.position, target.position) > 1f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );
        }
    }

}
