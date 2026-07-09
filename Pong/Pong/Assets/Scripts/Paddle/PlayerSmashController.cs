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
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)
    private void Update()
    {
        bool left = Input.GetKey(KeyCode.A);

<<<<<<< HEAD

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

=======
        if (left)
        {
            StartCharge(-1);
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
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)
    public bool CanSmashNow()
    {
        return isCharging && smashZone != null && smashZone.CanSmash;
    }

    public void DoSmash()
    {
        Smash();
<<<<<<< HEAD


=======
>>>>>>> parent of 50accef (Merge branch 'main' into micchi-)
    }
}