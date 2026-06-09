using UnityEngine;

public abstract class BossBallGimmick : ScriptableObject
{
    public abstract void OnHitBoss(Ball ball);
    public abstract void OnHitWall(Ball ball);
    public abstract void OnUpdate(Ball ball);

    public virtual void OnBossHpChanged(Ball ball, int currentHp, int maxHp)
    {
        // 必要なギミックだけ override する
    }
}
