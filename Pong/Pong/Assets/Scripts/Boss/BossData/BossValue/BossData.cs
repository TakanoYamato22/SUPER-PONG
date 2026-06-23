using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossData")]
public class BossData : ScriptableObject
{
    [Header("基本情報")]
    public string bossName;

    [Header("ステータス")]
    public float maxHP = 100f;

    [Header("移動設定")]
    public float moveSpeed = 3f;
    public float moveRangeX = 7f;
    public float moveRangeY = 4f;

    [Header("攻撃パターン")]
    public BossAttackPattern attackPattern;
    public BossBallGimmick gimmick;// ScriptableObject（後で作る）

    [Header("演出")]
    public AudioClip bossBGM;                 // ← BGM

    [Header("見た目")]
    public GameObject bossPrefab;             // 当たり判定・Sprite・Animator を持つ
}
