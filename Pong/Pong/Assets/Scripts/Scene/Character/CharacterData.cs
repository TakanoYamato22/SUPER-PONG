using UnityEngine;

[CreateAssetMenu(
    fileName = "CharacterData",
    menuName = "Pong/Character Data"
)]
public class CharacterData : ScriptableObject
{
    // ==================================================
    // 基本情報
    // ==================================================

    [Header("基本情報")]
    public string characterName;

    public Sprite icon;

    public Color paddleColor = Color.white;

    // ==================================================
    // パドル能力
    // ==================================================

    [Header("パドル能力")]

    [Tooltip("パドルの上下移動速度")]
    [Min(0.1f)]
    public float moveSpeed = 5f;

    [Tooltip("パドルの縦サイズ倍率。1が通常サイズ")]
    [Min(0.1f)]
    public float paddleHeightMultiplier = 1f;

    [Tooltip("最大HP")]
    [Min(1f)]
    public float maxHP = 100f;

    // ==================================================
    // 通常反射
    // ==================================================

    [Header("通常反射")]

    [Tooltip("通常反射後のボール速度倍率。1が通常")]
    [Min(0.01f)]
    public float normalHitSpeedMultiplier = 1f;

    // ==================================================
    // スマッシュ
    // ==================================================

    [Header("スマッシュ")]

    [Tooltip("スマッシュ時の威力倍率。1が通常")]
    [Min(0.01f)]
    public float smashPowerMultiplier = 1f;

    [Tooltip("スマッシュ後のボール速度倍率。1未満で減速")]
    [Min(0.01f)]
    public float smashBallSpeedMultiplier = 1f;

    [Tooltip("スマッシュ成功後のクールタイム")]
    [Min(0f)]
    public float smashCooldown = 2f;

    [Tooltip("スマッシュ時にパドルが前へ出る距離")]
    [Min(0f)]
    public float smashMoveDistance = 0.5f;

    [Tooltip("スマッシュ移動の速さ")]
    [Min(0.1f)]
    public float smashMoveSpeed = 12f;
}