using UnityEngine;

public class PlayerSmashController : BaseSmashController
{
    [SerializeField] private SmashZone smashZone;

    private void Update()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow);

        if (left)
        {
            StartCharge(-1);
        }
        else
        {
            StopCharge();
        }
    }

    public bool CanSmashNow()
    {
        return isCharging && smashZone != null && smashZone.CanSmash;
    }

    public void DoSmash()
    {
        Smash();
    }
}