using UnityEngine;

[CreateAssetMenu(menuName = "Pong/ScoreWallRule")]
public class ScoreWallRuleData : ScriptableObject
{
    [Header("発動条件（プレイヤースコア）")]
    public int scoreThreshold;        // 3点, 5点, 7点 など

    [Header("このスコアで出す壁の種類")]
    public WallTypeData[] wallTypes;  // 壁の種類（1種類でも複数でもOK）

    [Header("生成数")]
    public int spawnCount = 1;        // 出す数（1,2,3）
}
