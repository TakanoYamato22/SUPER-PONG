using UnityEngine;

public class PlayerSmashController : BaseSmashController
{
    [SerializeField] private SmashZone smashZone;



    private bool wasCharging = false;

    private void Update()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow);

    private void Update()
    {
        bool left = Input.GetKey(KeyCode.A);


        if (left)
        {
            StartCharge(-1);
<<<<<<< HEAD

            wasCharging = true;
        }
        else
        {
            if (wasCharging)
            {
                TrySmashOnRelease();
                wasCharging = false;
            }

       
        }
        else
        {

            StopCharge();
        }
    }



    private void TrySmashOnRelease()
    {
        if (smashZone != null && smashZone.CanSmash)
        {
            Smash();
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