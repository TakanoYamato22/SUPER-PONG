using UnityEngine;

public class PlayerSmashController : BaseSmashController
{
    [SerializeField] private SmashZone smashZone;

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
        if (smashZone != null && smashZone.CanSmash)
        {
            Smash();
        }
    }
}