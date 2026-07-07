using UnityEngine;

public class PlayerSmashController : BaseSmashController
{
    [SerializeField] private SmashZone smashZone;

<<<<<<< HEAD
    private bool wasCharging = false;

    private void Update()
    {
        bool left = Input.GetKey(KeyCode.LeftArrow);
=======
    private void Update()
    {
        bool left = Input.GetKey(KeyCode.A);
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)

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

=======
        }
        else
        {
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)
            StopCharge();
        }
    }

<<<<<<< HEAD
    private void TrySmashOnRelease()
    {
        if (smashZone != null && smashZone.CanSmash)
        {
            Smash();
        }
=======
    public bool CanSmashNow()
    {
        return isCharging && smashZone != null && smashZone.CanSmash;
    }

    public void DoSmash()
    {
        Smash();
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)
    }
}