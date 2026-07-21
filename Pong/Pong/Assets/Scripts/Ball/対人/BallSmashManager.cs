using UnityEngine;

[RequireComponent(typeof(Ball))]
public class BallSmashManager : MonoBehaviour
{
    [Header("スマッシュ設定")]
    [SerializeField]
    private float smashBoost = 5f;

    [Header("スマッシュ専用Trigger")]
    [SerializeField]
    private GameObject smashTrigger;

    [Header("Layer名")]
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

        spriteRenderer =
            GetComponent<SpriteRenderer>();

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

        if (smashTrigger == null)
        {
            Debug.LogError(
                "Smash Triggerが設定されていません。",
                this
            );
        }

        if (spriteRenderer != null)
        {
            defaultColor =
                spriteRenderer.color;
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

        /*
         * 親のBallだけ変更する。
         * SmashTriggerのLayerは変更しない。
         */
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