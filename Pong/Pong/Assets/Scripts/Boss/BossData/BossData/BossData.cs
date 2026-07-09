using UnityEngine;

[CreateAssetMenu(menuName = "Boss/BossData")]
public class BossData : ScriptableObject
{
    [Header("��{���")]
    public string bossName;

    [Header("�X�e�[�^�X")]
    public float maxHP = 100f;

    [Header("�ړ��ݒ�")]
    public float moveSpeed = 3f;
    public float moveRangeX = 7f;
    public float moveRangeY = 4f;

    [Header("���o")]
    public AudioClip bossBGM;                 // �� BGM

    [Header("������")]
    public GameObject bossPrefab;             // �����蔻��ESprite�EAnimator ������

}
