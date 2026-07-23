using UnityEngine;

// ゲーム中に使用するキャラクター能力を保持する
public class CharacterRuntimeStats : MonoBehaviour
{
    [Header("通常反射")]
    [SerializeField] private float normalHitSpeedMultiplier = 1f;

    [Header("スマッシュ")]
    [SerializeField] private float smashPowerMultiplier = 1f;
    [SerializeField] private float smashBallSpeedMultiplier = 1f;

    public float NormalHitSpeedMultiplier
    {
        get { return normalHitSpeedMultiplier; }
    }

    public float FinalSmashSpeedMultiplier
    {
        get
        {
            return smashPowerMultiplier * smashBallSpeedMultiplier;
        }
    }

    public void ApplyCharacterData(CharacterData data)
    {
        if (data == null)
            return;

        normalHitSpeedMultiplier =
            Mathf.Max(0.01f, data.normalHitSpeedMultiplier);

        smashPowerMultiplier =
            Mathf.Max(0.01f, data.smashPowerMultiplier);

        smashBallSpeedMultiplier =
            Mathf.Max(0.01f, data.smashBallSpeedMultiplier);
    }
}