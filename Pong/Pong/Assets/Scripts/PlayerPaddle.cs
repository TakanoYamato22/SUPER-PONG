using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector2 direction;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // 慣性ゼロのクラシック操作
        if (direction.sqrMagnitude != 0)
        {
            transform.position += (Vector3)(direction * speed * Time.fixedDeltaTime);
        }

        // --- パドルの特殊効果があれば適用 ---
        //activeEffect?.UpdateEffect(this);
    }
}