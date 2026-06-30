using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSelectMenu : MonoBehaviour
{
    [Header("Item Database")]
    [SerializeField] private ItemData[] items;

    [Header("Player1 UI")]
    [SerializeField] private Image p1Icon;
    [SerializeField] private TMP_Text p1NameText;
    [SerializeField] private TMP_Text p1ReadyText;
    [SerializeField] private Image p1Panel;

    [Header("Player2 UI")]
    [SerializeField] private GameObject p2PanelObject;
    [SerializeField] private Image p2Icon;
    [SerializeField] private TMP_Text p2NameText;
    [SerializeField] private TMP_Text p2ReadyText;
    [SerializeField] private Image p2Panel;

    [Header("List UI")]
    [SerializeField] private TMP_Text listText;

    [Header("Start Manager")]
    [SerializeField] private GameStartManager gameStartManager;

    private int p1Index = 0;
    private int p2Index = 0;

    private void Start()
    {
        GameSettings.ResetItemSelections();

        if (!GameSettings.NeedPlayer2Selection())
        {
            p2PanelObject.SetActive(false);
            GameSettings.player2Ready = true;
        }
        else
        {
            p2PanelObject.SetActive(true);
            GameSettings.player2Ready = false;
        }

        ApplyItem(1);
        ApplyItem(2);

        UpdateReadyUI();
        UpdateListUI();
    }

    private void Update()
    {
        HandlePlayer1Input();

        if (GameSettings.NeedPlayer2Selection())
            HandlePlayer2Input();
    }

    private void HandlePlayer1Input()
    {
        if (GameSettings.player1Ready)
        {
            if (Input.GetKeyDown(InputConfig.p1Ready))
                ToggleP1Ready();

            return;
        }

        if (Input.GetKeyDown(InputConfig.p1SelectLeft))
            ChangeP1(-1);

        if (Input.GetKeyDown(InputConfig.p1SelectRight))
            ChangeP1(1);

        if (Input.GetKeyDown(InputConfig.p1Ready))
            ToggleP1Ready();
    }

    private void HandlePlayer2Input()
    {
        if (GameSettings.player2Ready)
        {
            if (Input.GetKeyDown(InputConfig.p2Ready))
                ToggleP2Ready();

            return;
        }

        if (Input.GetKeyDown(InputConfig.p2SelectLeft))
            ChangeP2(-1);

        if (Input.GetKeyDown(InputConfig.p2SelectRight))
            ChangeP2(1);

        if (Input.GetKeyDown(InputConfig.p2Ready))
            ToggleP2Ready();
    }

    private void ChangeP1(int direction)
    {
        p1Index = WrapIndex(p1Index + direction);
        ApplyItem(1);
    }

    private void ChangeP2(int direction)
    {
        p2Index = WrapIndex(p2Index + direction);
        ApplyItem(2);
    }

    private int WrapIndex(int index)
    {
        int count = items.Length;

        if (index < 0)
            return count - 1;

        if (index >= count)
            return 0;

        return index;
    }

    private void ApplyItem(int player)
    {
        if (items.Length == 0)
        {
            Debug.LogError("ItemSelectMenuにアイテムが登録されていません");
            return;
        }

        if (player == 1)
        {
            ItemData data = items[p1Index];

            GameSettings.player1ItemIndex = p1Index;

            p1Icon.sprite = data.icon;
            p1Icon.enabled = true;
            p1NameText.text = data.itemName;
        }
        else
        {
            if (!GameSettings.NeedPlayer2Selection())
                return;

            ItemData data = items[p2Index];

            GameSettings.player2ItemIndex = p2Index;

            p2Icon.sprite = data.icon;
            p2Icon.enabled = true;
            p2NameText.text = data.itemName;
        }
    }

    private void ToggleP1Ready()
    {
        GameSettings.player1Ready = !GameSettings.player1Ready;

        UpdateReadyUI();
        CheckStart();
    }

    private void ToggleP2Ready()
    {
        if (!GameSettings.NeedPlayer2Selection())
            return;

        GameSettings.player2Ready = !GameSettings.player2Ready;

        UpdateReadyUI();
        CheckStart();
    }

    private void UpdateReadyUI()
    {
        p1ReadyText.text = GameSettings.player1Ready ? "READY ✓" : "NOT READY";
        p1Panel.color = GameSettings.player1Ready ? new Color(0.4f, 1f, 0.4f) : new Color(0.5f, 0.8f, 1f);

        if (GameSettings.NeedPlayer2Selection())
        {
            p2ReadyText.text = GameSettings.player2Ready ? "READY ✓" : "NOT READY";
            p2Panel.color = GameSettings.player2Ready ? new Color(0.4f, 1f, 0.4f) : new Color(1f, 0.5f, 0.5f);
        }
    }

    private void UpdateListUI()
    {
        if (listText == null)
            return;

        string text = "";

        for (int i = 0; i < items.Length; i++)
        {
            text += items[i].itemName + "\n";
        }

        listText.text = text;
    }

    private void CheckStart()
    {
        if (GameSettings.player1Ready && GameSettings.player2Ready)
        {
            gameStartManager.StartGame();
        }
    }
}