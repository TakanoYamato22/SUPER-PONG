using UnityEngine;

[RequireComponent(typeof(Ball))]
public class BallSmashManager : MonoBehaviour

{
    [Header("�X�}�b�V���ݒ�")]
    [SerializeField]
    private float smashBoost = 5f;

    [Header("�X�}�b�V����pTrigger")]
    [SerializeField]
    private GameObject smashTrigger;

    [Header("Layer��")]
    [SerializeField]
    private string normalLayerName = "Ball";

    [SerializeField]
    private string smashLayerName = "SmashBall";


    private Ball ball;

    private SpriteRenderer spriteRenderer;

    private Color defaultColor;

    private float beforeSmashSpeed;

    private int normalLayer;
    private int smashLayer;

    public bool IsSmashed
    {
        get;
        private set;
    }

    private void Awake()
    {
        ball = GetComponent<Ball>();

        // 子オブジェクトのBallVisualからSpriteRendererを取得
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        normalLayer =
            LayerMask.NameToLayer(normalLayerName);

        smashLayer =
            LayerMask.NameToLayer(smashLayerName);

        if (normalLayer == -1)
        {
            Debug.LogError(
                $"Layer「{normalLayerName}」がありません。",
                this
            );
        }

        if (smashLayer == -1)
        {
            Debug.LogError(
                $"Layer「{smashLayerName}」がありません。",
                this
            );
        }

        if (spriteRenderer != null)
        {
            defaultColor = spriteRenderer.color;
        }
        else
        {
            Debug.LogError(
                "BallVisualのSpriteRendererが見つかりません。",
                this
            );
        }
    }

    private void Start()
    {
        ResetSmash();
    }

    public void ApplySmash()

    {
        StartSmash();
    }

    public void SmashReturn()

    {
        StartSmash();
    }

    private void StartSmash()
    {
        if (ball == null)
        {
            return;
        }

        if (!IsSmashed)
        {
            beforeSmashSpeed =
                ball.currentSpeed;
        }

        IsSmashed = true;

        SetBallLayer(smashLayer);

        if (smashTrigger != null)
        {
            smashTrigger.SetActive(true);
        }

        ball.ignoreMaxSpeed = true;

        ball.IncreaseSpeed(smashBoost);

        if (spriteRenderer != null)
        {
            spriteRenderer.color =
                Color.red;
        }

        if (ball.smashEffect != null)
        {
            ball.smashEffect.Play();
        }
    }

    public void EndSmash()

    {
        if (ball == null)
        {
            return;
        }

        IsSmashed = false;

        if (smashTrigger != null)
        {
            smashTrigger.SetActive(false);
        }

        SetBallLayer(normalLayer);

        ball.ignoreMaxSpeed = false;

        ball.SetSpeed(beforeSmashSpeed);

        ResetColor();

    }

    public void ResetSmash()

    {

        IsSmashed = false;

        if (smashTrigger != null)
        {
            smashTrigger.SetActive(false);
        }

        SetBallLayer(normalLayer);

        if (ball != null)

        {

            ball.ignoreMaxSpeed = false;

        }

        ResetColor();

    }

    private void SetBallLayer(int layer)
    {
        if (layer == -1)
        {
            return;
        }

        gameObject.layer = layer;
    }

    private void ResetColor()

    {

        if (spriteRenderer != null)

        {
            spriteRenderer.color =
                defaultColor;
        }

    }

}
