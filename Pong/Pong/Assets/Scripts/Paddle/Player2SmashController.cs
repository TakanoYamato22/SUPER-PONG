using UnityEngine;

public class Player2SmashController : BaseSmashController
{
    [SerializeField] private SmashZone smashZone;

    private void Update()
    {
        bool right = Input.GetKey(KeyCode.RightArrow);

        if (right)
        {
            StartCharge(1);
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