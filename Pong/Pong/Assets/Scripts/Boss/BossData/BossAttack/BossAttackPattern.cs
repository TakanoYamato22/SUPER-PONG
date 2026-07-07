using UnityEngine;

public abstract class BossAttackPattern : ScriptableObject
{
    [Header("攻撃間隔（秒）")]
    public float attackInterval = 2f;

    // ボスごとの攻撃内容はここをオーバーライドして書く
    public abstract void ExecuteAttack(BossController boss);
}