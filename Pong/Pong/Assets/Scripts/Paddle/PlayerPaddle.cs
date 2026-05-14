using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector2 direction;

    public float limitY = 3.5f; // ← 上下の移動制限を追加

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
        if (direction.sqrMagnitude != 0)
        {
            float newY = transform.position.y + direction.y * speed * Time.fixedDeltaTime;

            // 上下の移動制限（これが超重要）
            newY = Mathf.Clamp(newY, -limitY, limitY);

            transform.position = new Vector2(transform.position.x, newY);
        }
    }
}
