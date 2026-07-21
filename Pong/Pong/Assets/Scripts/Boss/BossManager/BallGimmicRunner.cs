using UnityEngine;

public abstract class BallGimmickRunner : MonoBehaviour
{
    public abstract void OnBossHpChanged(float currentHp, float maxHp);
}