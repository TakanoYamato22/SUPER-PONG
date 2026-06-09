using UnityEngine;

public class BossBallGimmickRunner : MonoBehaviour
{
    [SerializeField] private BossBallGimmick gimmick;
    [SerializeField] private Ball ball;

    void Awake()
    {
        if (ball == null)
            ball = FindObjectOfType<Ball>(); // Åö é©ìÆéÊìæ
    }

    void Update()
    {
        if (gimmick == null || ball == null) return;
        gimmick.OnUpdate(ball);
    }

    public void OnBossHpChanged(float currentHp, float maxHp)
    {

        Debug.Log(ball);
        if (gimmick == null || ball == null) return;

        gimmick.OnBossHpChanged(ball, (int)currentHp, (int)maxHp);
    }

    public void SetGimmick(BossBallGimmick newGimmick)
    {
        gimmick = newGimmick;
    }
}
