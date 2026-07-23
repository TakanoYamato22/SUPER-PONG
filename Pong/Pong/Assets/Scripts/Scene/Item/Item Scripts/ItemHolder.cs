using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [Header("Player Number")]
    [SerializeField] private int playerNumber = 1;

    [Header("References")]
    [SerializeField] private ItemDatabase itemDatabase;

    [SerializeField] private ItemCooldownUI cooldownUI;

    private ItemData currentItem;

    private float cooldownTimer;

    public ItemData CurrentItem
    {
        get { return currentItem; }
    }

    public float CooldownRemaining
    {
        get { return cooldownTimer; }
    }

    public bool CanUseItem
    {
        get
        {
            return
                currentItem != null &&
                cooldownTimer <= 0f;
        }
    }

    private void Start()
    {
        FindReferences();
        LoadSelectedItem();
    }

    private void Update()
    {
        UpdateCooldown();
        HandleInput();
    }

    private void FindReferences()
    {
        if (itemDatabase == null)
        {
            itemDatabase =
                FindFirstObjectByType<ItemDatabase>();
        }

        if (cooldownUI == null)
        {
            ItemCooldownUI[] allUI =
                FindObjectsByType<ItemCooldownUI>(
                    FindObjectsSortMode.None
                );

            foreach (ItemCooldownUI ui in allUI)
            {
                if (ui.PlayerNumber == playerNumber)
                {
                    cooldownUI = ui;
                    break;
                }
            }
        }
    }

    private void LoadSelectedItem()
    {
        // 1PモードのCPUにはアイテムを持たせない
        if (
            playerNumber == 2 &&
            GameSettings.playerCount == 1
        )
        {
            currentItem = null;

            if (cooldownUI != null)
            {
                cooldownUI.SetVisible(false);
            }

            enabled = false;

            Debug.Log(
                gameObject.name +
                " はCPUなのでアイテムなし"
            );

            return;
        }

        if (itemDatabase == null)
        {
            Debug.LogError(
                gameObject.name +
                " がItemDatabaseを取得できません"
            );

            return;
        }

        int itemIndex =
            playerNumber == 1
            ? GameSettings.player1ItemIndex
            : GameSettings.player2ItemIndex;

        Debug.Log(
            gameObject.name +
            " の選択アイテム番号: " +
            itemIndex
        );

        currentItem =
            itemDatabase.GetItem(itemIndex);

        if (currentItem == null)
        {
            if (cooldownUI != null)
            {
                cooldownUI.Clear();
            }

            return;
        }

        cooldownTimer = 0f;

        if (cooldownUI != null)
        {
            cooldownUI.SetVisible(true);
            cooldownUI.Setup(currentItem);
        }

        Debug.Log(
            gameObject.name +
            " が持っているアイテム: " +
            currentItem.itemName
        );
    }

    private void HandleInput()
    {
        if (playerNumber == 1)
        {
            if (
                Input.GetKeyDown(
                    InputConfig.p1UseItem
                )
            )
            {
                TryUseItem();
            }

            return;
        }

        if (
            playerNumber == 2 &&
            GameSettings.playerCount == 2
        )
        {
            if (
                Input.GetKeyDown(
                    InputConfig.p2UseItem
                )
            )
            {
                TryUseItem();
            }
        }
    }

    private void UpdateCooldown()
    {
        if (currentItem == null)
            return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0f)
            {
                cooldownTimer = 0f;
            }
        }

        if (cooldownUI != null)
        {
            cooldownUI.SetCooldown(
                cooldownTimer,
                currentItem.cooldown
            );
        }
    }

    private void TryUseItem()
    {
        if (currentItem == null)
        {
            Debug.Log(
                gameObject.name +
                " はアイテムを持っていません"
            );

            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log(
                currentItem.itemName +
                " はクールタイム中。残り " +
                cooldownTimer.ToString("F1") +
                " 秒"
            );

            return;
        }

        if (ItemEffectManager.Instance == null)
        {
            Debug.LogError(
                "ItemEffectManagerがSceneにありません"
            );

            return;
        }

        bool effectActivated =
            ItemEffectManager.Instance.ApplyEffect(
                currentItem,
                gameObject
            );

        if (!effectActivated)
        {
            Debug.LogWarning(
                currentItem.itemName +
                " の効果を発動できませんでした"
            );

            return;
        }

        cooldownTimer =
            Mathf.Max(0f, currentItem.cooldown);

        if (cooldownUI != null)
        {
            cooldownUI.SetCooldown(
                cooldownTimer,
                currentItem.cooldown
            );
        }

        Debug.Log(
            gameObject.name +
            " が " +
            currentItem.itemName +
            " を使用。CT: " +
            currentItem.cooldown +
            " 秒"
        );
    }
}