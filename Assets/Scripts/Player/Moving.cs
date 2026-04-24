using UnityEngine;
using UnityEngine.InputSystem;

public class Moving : MonoBehaviour
{
    public float maxSpeed = 5f;      // 最大スピード
    public float acceleration = 20f; // 加速の速さ
    float currentSpeed = 0f;

    //1フレームごとに実行
    void Update()
    {
        float input = 0f;
        if (Keyboard.current.aKey.isPressed) input = -1f;
        if (Keyboard.current.dKey.isPressed) input = 1f;

        //加速移動
        float targetSpeed = input * maxSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        transform.position += new Vector3(currentSpeed * Time.deltaTime, 0f, 0f);


        if (input != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(input) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }

    }
}