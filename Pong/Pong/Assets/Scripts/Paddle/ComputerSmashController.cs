using UnityEngine;

public class ComputerSmashController : BaseSmashController
{
    public float triggerDistance = 2.5f;
    public float smashChance = 0.25f;
    public float missChance = 0.15f;
    public float lateReactionChance = 0.2f;

    private bool delayedSmash = false;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        float distX = Mathf.Abs(ball.transform.position.x - transform.position.x);

        if (distX < triggerDistance)
        {
            if (!isCharging && Random.value < smashChance)
            {
                bool wrongDir = Random.value < missChance;

                int dir = (ball.velocity.x < 0) ? -1 : +1;
                if (wrongDir) dir *= -1;

                StartCharge(dir);

                delayedSmash = (Random.value < lateReactionChance);
            }
        }
        else
        {
            StopCharge();
            return;
        }

        if (isCharging && charge >= chargeTime)
        {
            if (delayedSmash)
            {
                Invoke(nameof(DoDelayedSmash), 0.1f);
                delayedSmash = false;
                return;
            }

            Smash();
        }
    }

    private void DoDelayedSmash()
    {
        Smash();
    }
}
