using UnityEngine;

public class BallSmashManager : MonoBehaviour
{

    [SerializeField] private float smashBoost = 5f;

    [Header("Layer")]
    [SerializeField] private string normalLayerName = "Ball";
    [SerializeField] private string smashLayerName = "BallSmash";


    private Ball ball;

    public float smashBoost = 5f;
    public bool isSmashed = false;

    // �X�}�b�V���O�̑��x��ۑ�
    private float beforeSmashSpeed;

    private void Awake()
    {
        ball = GetComponent<Ball>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }

        gameObject.layer = LayerMask.NameToLayer(normalLayerName);

    }

    /// <summary>
    /// �X�}�b�V�������iPaddle �ɓ��������u�ԂɌĂԁj
    /// </summary>
    public void ApplySmash()
    {
        if (isSmashed) return;

        isSmashed = true;


        IsSmashed = true;

        gameObject.layer = LayerMask.NameToLayer(smashLayerName);


        ball.ignoreMaxSpeed = true;

        // �X�}�b�V�����������x���グ��
        ball.IncreaseSpeed(smashBoost);


        SetColor(Color.red);
    }

    public void SmashReturn()
    {
        if (ball == null) return;

        IsSmashed = true;

        gameObject.layer = LayerMask.NameToLayer(smashLayerName);

        ball.ignoreMaxSpeed = true;
        ball.IncreaseSpeed(smashBoost);

        SetColor(Color.red);

    }

    /// <summary>
    /// �X�}�b�V���I���i��莞�Ԍ� or ���̃q�b�g���j
    /// </summary>
    public void EndSmash()
    {
        if (!isSmashed) return;


        IsSmashed = false;

        gameObject.layer = LayerMask.NameToLayer(normalLayerName);


        ball.ignoreMaxSpeed = false;

        // �X�}�b�V���O�̑��x�ɖ߂�
        ball.SetSpeed(beforeSmashSpeed);
    }

    /// <summary>
    /// ���E���h���Z�b�g���ɋ����I��
    /// </summary>
    public void ResetSmash()
    {
        IsSmashed = false;

        gameObject.layer = LayerMask.NameToLayer(normalLayerName);

        if (ball != null)
        {
            ball.ignoreMaxSpeed = false;
        }

        ResetColor();
    }

    private void SetColor(Color color)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    private void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = defaultColor;
        }

    }
}