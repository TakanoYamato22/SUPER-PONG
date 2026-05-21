using UnityEngine;

// キャラクター1体分の能力を保存するデータ
[CreateAssetMenu(fileName = "CharacterData", menuName = "Pong/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("基本情報")]
    public string characterName;
    public Sprite icon;

    [Header("移動関連")]
    public float moveSpeed = 5f;
    public float paddleHeight = 2f;

    [Header("スマッシュ")]
    public float smashPower = 2.0f;
    public float smashDashDistance = 0.5f;

    [Header("特殊能力")]
    public int shrinkThreshold = 3;
}