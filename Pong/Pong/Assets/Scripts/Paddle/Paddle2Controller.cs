using UnityEngine;

public class Player2Paddle : Paddle
{
    [Header("Player2 Key")]
    [SerializeField] private KeyCode upKey = KeyCode.UpArrow;
    [SerializeField] private KeyCode downKey = KeyCode.DownArrow;

    public float limitY = 3.5f;

    private void FixedUpdate()
    {
        float move = 0f;

        if (Input.GetKey(upKey))
            move = 1f;
        else if (Input.GetKey(downKey))
            move = -1f;

        float newY = transform.position.y + move * speed * Time.fixedDeltaTime;

        newY = Mathf.Clamp(newY, -limitY, limitY);

        transform.position = new Vector2(transform.position.x, newY);
    }
}