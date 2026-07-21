using UnityEngine;

public class PlayerPaddle : Paddle
{
    [SerializeField]
    private SmashController smashController;

    public float limitY = 3.5f;

    private float moveInput;

    private void Update()
    {
        // スマッシュ中は入力を停止
        if (smashController != null &&
            smashController.IsCharging)
        {
            moveInput = 0f;
            return;
        }

        if (Input.GetKey(KeyCode.W))
        {
            moveInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveInput = -1f;
        }
        else
        {
            moveInput = 0f;
        }
    }

    private void FixedUpdate()
    {
        // スマッシュ中は慣性も止める
        if (smashController != null &&
            smashController.IsCharging)
        {
            StopInertia();
            return;
        }

        float velocity =
            CalculateMoveVelocity(
                moveInput,
                Time.fixedDeltaTime
            );

        float newY =
            transform.position.y +
            velocity *
            Time.fixedDeltaTime;

        newY =
            Mathf.Clamp(
                newY,
                -limitY,
                limitY
            );

        transform.position =
            new Vector2(
                transform.position.x,
                newY
            );

        // 壁に到達したら溜まった慣性を止める
        if (newY <= -limitY ||
            newY >= limitY)
        {
            StopInertia();
        }
    }
}