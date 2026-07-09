using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector2 direction;

    public float limitY = 3.5f;

    private void Update()
    {
        // スマッシュ中は移動禁止
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector2.zero;
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
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
        if (direction.sqrMagnitude == 0)
            return;

        float newY = transform.position.y + direction.y * speed * Time.fixedDeltaTime;

        newY = Mathf.Clamp(newY, -limitY, limitY);

        transform.position = new Vector2(transform.position.x, newY);
    }
}