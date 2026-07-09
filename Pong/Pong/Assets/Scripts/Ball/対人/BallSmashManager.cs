using UnityEngine;

public class BallSmashManager : MonoBehaviour
{
    [SerializeField] private float smashBoost = 5f;

    [Header("Layer")]
    [SerializeField] private string normalLayerName = "Ball";
    [SerializeField] private string smashLayerName = "BallSmash";

    private Ball ball;
    private SpriteRenderer spriteRenderer;

    private Color defaultColor;
    private float beforeSmashSpeed;

    public bool IsSmashed { get; private set; }

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

    public void ApplySmash()
    {
        if (ball == null) return;

        if (!IsSmashed)
        {
            beforeSmashSpeed = ball.currentSpeed;
        }

        IsSmashed = true;

        gameObject.layer = LayerMask.NameToLayer(smashLayerName);

        ball.ignoreMaxSpeed = true;
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

    public void EndSmash()
    {
        if (ball == null) return;

        IsSmashed = false;

        gameObject.layer = LayerMask.NameToLayer(normalLayerName);

        ball.ignoreMaxSpeed = false;
        ball.SetSpeed(beforeSmashSpeed);

        ResetColor();
    }

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