using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ItemSelectMenu : MonoBehaviour
{
    // ==================================================
    // アイテムデータ一覧
    // ==================================================

    [Header("Item Database")]
    [SerializeField] private ItemData[] items;

    // ==================================================
    // 1P表示
    // ==================================================

    [Header("Player1 Slots")]
    [SerializeField] private Image p1Slot1;
    [SerializeField] private Image p1Slot2;

    // ==================================================
    // 2P表示
    // ==================================================

    [Header("Player2 Slots")]
    [SerializeField] private Image p2Slot1;
    [SerializeField] private Image p2Slot2;

    // ==================================================
    // READY表示
    // ==================================================

    [Header("Ready Text")]
    [SerializeField] private TMP_Text p1ReadyText;
    [SerializeField] private TMP_Text p2ReadyText;

    // ==================================================
    // READYボタン文字
    // ==================================================

    [Header("Ready Button Text")]
    [SerializeField] private TMP_Text p1ButtonText;
    [SerializeField] private TMP_Text p2ButtonText;

    // ==================================================
    // プレイヤーパネル
    // ==================================================

    [Header("Player Panels")]
    [SerializeField] private Image player1Panel;
    [SerializeField] private Image player2Panel;

    // ==================================================
    // 現在選択中プレイヤー
    // ==================================================

    private int currentPlayer = 1;

    // ==================================================
    // 選択数
    // ==================================================

    private int p1Count = 0;
    private int p2Count = 0;

    // ==================================================
    // 初期化
    // ==================================================

    private void Start()
    {
        GameSettings.ResetSelections();

        Debug.Log("登録アイテム数 : " + items.Length);

        // READY表示初期化
        p1ReadyText.text = "NOT READY";
        p2ReadyText.text = "NOT READY";

        p1ButtonText.text = "準備完了";
        p2ButtonText.text = "準備完了";

        UpdateTurnDisplay();
    }

    // ==================================================
    // アイテム選択
    // ボタンから呼ばれる
    // ==================================================

    public void SelectItem(int itemIndex)
    {
        Debug.Log("選択アイテム番号 : " + itemIndex);

        // 存在チェック
        if (itemIndex < 0 || itemIndex >= items.Length)
        {
            Debug.LogError("存在しないアイテム番号");
            return;
        }

        //------------------------------------------------
        // 1P選択中
        //------------------------------------------------

        if (currentPlayer == 1)
        {
            if (GameSettings.player1Ready)
                return;

            if (p1Count >= 2)
            {
                Debug.Log("1Pは既に2個選択済み");
                return;
            }

            GameSettings.player1Items[p1Count] = itemIndex;

            if (p1Count == 0)
                p1Slot1.sprite = items[itemIndex].icon;

            if (p1Count == 1)
                p1Slot2.sprite = items[itemIndex].icon;

            p1Count++;

            Debug.Log("1P選択数 : " + p1Count);
        }

        //------------------------------------------------
        // 2P選択中
        //------------------------------------------------

        else
        {
            if (GameSettings.player2Ready)
                return;

            if (p2Count >= 2)
            {
                Debug.Log("2Pは既に2個選択済み");
                return;
            }

            GameSettings.player2Items[p2Count] = itemIndex;

            if (p2Count == 0)
                p2Slot1.sprite = items[itemIndex].icon;

            if (p2Count == 1)
                p2Slot2.sprite = items[itemIndex].icon;

            p2Count++;

            Debug.Log("2P選択数 : " + p2Count);
        }
    }

    // ==================================================
    // 1P READY
    // 同じボタンでON/OFF
    // ==================================================

    public void TogglePlayer1Ready()
    {
        //----------------------------------------
        // READYにする
        //----------------------------------------

        if (!GameSettings.player1Ready)
        {
            if (p1Count < 2)
            {
                Debug.Log("1Pはアイテムを2個選んでください");
                return;
            }

            GameSettings.player1Ready = true;

            p1ReadyText.text = "READY ✓";
            p1ButtonText.text = "キャンセル";

            // 次は2P
            currentPlayer = 2;
        }

        //----------------------------------------
        // READY解除
        //----------------------------------------

        else
        {
            GameSettings.player1Ready = false;

            p1ReadyText.text = "NOT READY";
            p1ButtonText.text = "準備完了";

            currentPlayer = 1;
        }

        UpdateTurnDisplay();
        CheckStart();
    }

    // ==================================================
    // 2P READY
    // ==================================================

    public void TogglePlayer2Ready()
    {
        //----------------------------------------
        // READYにする
        //----------------------------------------

        if (!GameSettings.player2Ready)
        {
            if (p2Count < 2)
            {
                Debug.Log("2Pはアイテムを2個選んでください");
                return;
            }

            GameSettings.player2Ready = true;

            p2ReadyText.text = "READY ✓";
            p2ButtonText.text = "キャンセル";
        }

        //----------------------------------------
        // READY解除
        //----------------------------------------

        else
        {
            GameSettings.player2Ready = false;

            p2ReadyText.text = "NOT READY";
            p2ButtonText.text = "準備完了";

            currentPlayer = 2;
        }

        UpdateTurnDisplay();
        CheckStart();
    }

    // ==================================================
    // 現在選択中プレイヤー表示
    // ==================================================

    private void UpdateTurnDisplay()
    {
        // 1P選択中
        if (currentPlayer == 1)
        {
            player1Panel.color = new Color(0.5f, 0.8f, 1f);
            player2Panel.color = Color.white;
        }

        // 2P選択中
        else
        {
            player1Panel.color = Color.white;
            player2Panel.color = new Color(1f, 0.5f, 0.5f);
        }
    }

    // ==================================================
    // 両方READYでゲーム開始
    // ==================================================

    private void CheckStart()
    {
        if (
            GameSettings.player1Ready &&
            GameSettings.player2Ready
        )
        {
            Debug.Log("全員READY");

            SceneManager.LoadScene("GameScene");
        }
    }
    // ==================================
    // 1P Slot1削除
    // ==================================

    public void RemoveP1Slot1()
    {
        RemovePlayerItem(1, 0);
    }

    // ==================================
    // 1P Slot2削除
    // ==================================

    public void RemoveP1Slot2()
    {
        RemovePlayerItem(1, 1);
    }

    // ==================================
    // 2P Slot1削除
    // ==================================

    public void RemoveP2Slot1()
    {
        RemovePlayerItem(2, 0);
    }

    // ==================================
    // 2P Slot2削除
    // ==================================

    public void RemoveP2Slot2()
    {
        RemovePlayerItem(2, 1);
    }
    private void RemovePlayerItem(int player, int slot)
    {
        //--------------------------------
        // 1P
        //--------------------------------

        if (player == 1)
        {
            if (GameSettings.player1Items[slot] == -1)
                return;

            if (GameSettings.player1Ready)
            {
                GameSettings.player1Ready = false;

                p1ReadyText.text = "NOT READY";
                p1ButtonText.text = "準備完了";
            }

            GameSettings.player1Items[slot] = -1;

            RearrangePlayer1();
        }

        //--------------------------------
        // 2P
        //--------------------------------

        else
        {
            if (GameSettings.player2Items[slot] == -1)
                return;

            if (GameSettings.player2Ready)
            {
                GameSettings.player2Ready = false;

                p2ReadyText.text = "NOT READY";
                p2ButtonText.text = "準備完了";
            }

            GameSettings.player2Items[slot] = -1;

            RearrangePlayer2();
        }
    }
    private void RearrangePlayer1()
    {
        int[] temp = new int[2]
        {
        -1,
        -1
        };

        int index = 0;

        for (int i = 0; i < 2; i++)
        {
            if (GameSettings.player1Items[i] != -1)
            {
                temp[index] = GameSettings.player1Items[i];
                index++;
            }
        }

        GameSettings.player1Items = temp;

        p1Count = index;

        RefreshPlayer1UI();
    }
    private void RearrangePlayer2()
    {
        int[] temp = new int[2]
        {
        -1,
        -1
        };

        int index = 0;

        for (int i = 0; i < 2; i++)
        {
            if (GameSettings.player2Items[i] != -1)
            {
                temp[index] = GameSettings.player2Items[i];
                index++;
            }
        }

        GameSettings.player2Items = temp;

        p2Count = index;

        RefreshPlayer2UI();
    }
    private void RefreshPlayer1UI()
    {
        p1Slot1.sprite = null;
        p1Slot2.sprite = null;

        if (GameSettings.player1Items[0] != -1)
            p1Slot1.sprite =
                items[GameSettings.player1Items[0]].icon;

        if (GameSettings.player1Items[1] != -1)
            p1Slot2.sprite =
                items[GameSettings.player1Items[1]].icon;
    }
    private void RefreshPlayer2UI()
    {
        p2Slot1.sprite = null;
        p2Slot2.sprite = null;

        if (GameSettings.player2Items[0] != -1)
            p2Slot1.sprite =
                items[GameSettings.player2Items[0]].icon;

        if (GameSettings.player2Items[1] != -1)
            p2Slot2.sprite =
                items[GameSettings.player2Items[1]].icon;
    }
}