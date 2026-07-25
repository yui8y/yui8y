using UnityEngine;
using System.Collections;

public class ComeCustomer : MonoBehaviour
{
    Transform target;
    public float speed = 2f;

    static Vector3 savedPosition = Vector3.zero;  // 静的変数で位置を保存

    [SerializeField] Transform visual;

    void Start()
    {
        target = GameManager.instance.counterPoint;

        // 保存された位置があればそこから開始
        if (GameData.hasCustomerPosition)
        {
            transform.position = savedPosition;    // 保存位置に移動
            GameData.hasCustomerPosition = false;  // フラグリセット
        }

        CounterInteraction counter = FindAnyObjectByType<CounterInteraction>();
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

    // Makingシーンに行く前に呼ぶ
    public static void SavePosition(Vector3 pos)
    {
        savedPosition = pos;               // 位置を保存
        GameData.hasCustomerPosition = true;
    }

    // 退場処理
    public IEnumerator Leave()
    {
        // ComeCustomerのUpdateを止める
        enabled = false;

        float targetX = -20f;

        Vector3 scale = visual.localScale;
        scale.x = -Mathf.Sign(-1) * Mathf.Abs(scale.x);
        visual.localScale = scale;

        while (transform.position.x > targetX)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
        GameManager.instance.customerExists = false;
        CounterInteraction.hasStartedTalk = false;  // ダイアログ開始フラグをリセット
        Debug.Log("お客さん退場！");
    }
}