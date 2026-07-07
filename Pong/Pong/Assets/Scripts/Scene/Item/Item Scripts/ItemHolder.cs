using UnityEngine;

// 選択したアイテムを保持して、キー入力で発動する
public class ItemHolder : MonoBehaviour
{
    [Header("Player Number")]
    [SerializeField] private int playerNumber = 1;
    // 1 = Player Paddle
    // 2 = Computer Paddle / Player2 Paddle

    [Header("Database")]
    [SerializeField] private ItemDatabase itemDatabase;

    private ItemData currentItem;

    private float cooldownTimer = 0f;

    private void Start()
    {
        if (itemDatabase == null)
        {
            itemDatabase = FindFirstObjectByType<ItemDatabase>();
        }

        LoadSelectedItem();
    }

    private void Update()
    {
        UpdateCooldown();
        HandleInput();
    }

    private void LoadSelectedItem()
    {
        int itemIndex;

        if (playerNumber == 1)
        {
            itemIndex = GameSettings.player1ItemIndex;
        }
        else
        {
            itemIndex = GameSettings.player2ItemIndex;
        }

        currentItem = itemDatabase.GetItem(itemIndex);

        if (currentItem != null)
        {
            Debug.Log(name + " が持っているアイテム: " + currentItem.itemName);
        }
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0f)
            {
                cooldownTimer = 0f;
            }
        }
    }

    private void HandleInput()
    {
        if (playerNumber == 1)
        {
            if (Input.GetKeyDown(InputConfig.p1UseItem))
            {
                TryUseItem();
            }
        }
        else if (playerNumber == 2)
        {
            // 1Pモードでは2Pアイテムは使わない
            if (GameSettings.playerCount != 2)
                return;

            if (Input.GetKeyDown(InputConfig.p2UseItem))
            {
                TryUseItem();
            }
        }
    }

    private void TryUseItem()
    {
        if (currentItem == null)
        {
            Debug.Log(name + " はアイテムを持っていません");
            return;
        }

        if (cooldownTimer > 0f)
        {
            Debug.Log(currentItem.itemName + " はクールタイム中: " + cooldownTimer.ToString("F1"));
            return;
        }

        ItemEffectManager.Instance.ApplyEffect(currentItem, gameObject);

        cooldownTimer = currentItem.cooldown;

        Debug.Log(name + " が " + currentItem.itemName + " を使用");
    }
}