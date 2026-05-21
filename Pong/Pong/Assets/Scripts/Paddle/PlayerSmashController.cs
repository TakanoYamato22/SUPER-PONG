using UnityEngine;

public class PlayerSmashController : BaseSmashController
{
    private bool wasCharging = false;

    private void Update()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow);

        if (left)
        {
            StartCharge(-1);
            wasCharging = true;
        }
        else
        {
            if (wasCharging)
            {
                TrySmashOnRelease();
                wasCharging = false;
            }

            StopCharge();
        }
    }

    private void TrySmashOnRelease()
    {
        float x = ball.transform.position.x;

        if (x <= -8.2f && x >= -8.5f)
        {
            Smash();
        }
    }

}
