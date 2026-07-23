using UnityEngine;

public class VolcanoBallGimmickRunner :
    BallGimmickRunner
{
    [Header("共通ギミック")]
    [SerializeField]
    private BossBallGimmick gimmick;

    [SerializeField]
    private Ball ball;

    private void Awake()
    {
        if (ball == null)
        {
            ball =
                FindObjectOfType<Ball>();
        }
    }

    private void Update()
    {
        if (gimmick == null ||
            ball == null)
        {
            return;
        }

        gimmick.OnUpdate(
            ball
        );
    }

    public override void OnBossHpChanged(
        float currentHp,
        float maxHp
    )
    {
        if (maxHp <= 0f)
        {
            Debug.LogWarning(
                "Bossの最大HPが0以下です。",
                this
            );

            return;
        }

        float hpRate =
            Mathf.Clamp01(
                currentHp / maxHp
            );

        Debug.Log(
            $"BossHP：{currentHp} / {maxHp} " +
            $"割合：{hpRate:P0}",
            this
        );

        if (gimmick == null ||
            ball == null)
        {
            return;
        }

        gimmick.OnBossHpChanged(
            ball,
            Mathf.RoundToInt(
                currentHp
            ),
            Mathf.RoundToInt(
                maxHp
            )
        );
    }

    public void SetGimmick(
        BossBallGimmick newGimmick
    )
    {
        gimmick =
            newGimmick;
    }
}