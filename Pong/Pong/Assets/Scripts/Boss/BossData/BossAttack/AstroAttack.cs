using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Attack/Astro")]
public class AstroAttackPattern : BossAttackPattern
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 5f;

    public override void ExecuteAttack(BossController boss)
    {
        
    }
}
