using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCooldownUI : MonoBehaviour
{
    [Header("Player Number")]
    [SerializeField] private int playerNumber = 1;

    [Header("UI References")]
    [SerializeField] private Image itemIcon;

    [Tooltip("アイコンの上に重ねるクールタイム用Image")]
    [SerializeField] private Image cooldownFillImage;

    [SerializeField] private TMP_Text cooldownText;

    [SerializeField] private TMP_Text itemNameText;

    [SerializeField] private TMP_Text keyGuideText;

    public int PlayerNumber
    {
        get { return playerNumber; }
    }

    private void Awake()
    {
        Clear();
    }

    public void Setup(ItemData item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = true;
            itemIcon.color = Color.white;
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
        }

        if (keyGuideText != null)
        {
            keyGuideText.text =
                playerNumber == 1
                ? "Aキーで使用"
                : "→キーで使用";
        }

        SetCooldown(0f, item.cooldown);
    }

    public void SetCooldown(
        float remainingTime,
        float totalCooldown
    )
    {
        float ratio = 0f;

        if (totalCooldown > 0f)
        {
            ratio = remainingTime / totalCooldown;
        }

        ratio = Mathf.Clamp01(ratio);

        if (cooldownFillImage != null)
        {
            cooldownFillImage.fillAmount = ratio;
            cooldownFillImage.enabled = ratio > 0f;
        }

        if (cooldownText != null)
        {
            if (remainingTime > 0f)
            {
                cooldownText.text =
                    Mathf.CeilToInt(remainingTime).ToString();

                cooldownText.enabled = true;
            }
            else
            {
                cooldownText.text = "";
                cooldownText.enabled = false;
            }
        }

        if (itemIcon != null)
        {
            itemIcon.color =
                remainingTime > 0f
                ? new Color(0.45f, 0.45f, 0.45f, 1f)
                : Color.white;
        }
    }

    public void Clear()
    {
        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (cooldownFillImage != null)
        {
            cooldownFillImage.fillAmount = 0f;
            cooldownFillImage.enabled = false;
        }

        if (cooldownText != null)
        {
            cooldownText.text = "";
            cooldownText.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "アイテムなし";
        }

        if (keyGuideText != null)
        {
            keyGuideText.text = "";
        }
    }

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}