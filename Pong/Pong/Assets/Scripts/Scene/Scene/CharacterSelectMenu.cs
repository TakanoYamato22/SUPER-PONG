using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectMenu : MonoBehaviour
{
    [Header("Database")]
    [SerializeField] private CharacterDatabase database;

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

    private int p1Index = 0;
    private int p2Index = 0;

    private void Awake()
    {
        if (database == null)
            database = GetComponent<CharacterDatabase>();
    }

    private void Start()
    {
        GameSettings.ResetCharacterSelections();

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

        ApplyCharacter(1);
        ApplyCharacter(2);

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
        ApplyCharacter(1);
    }

    private void ChangeP2(int direction)
    {
        p2Index = WrapIndex(p2Index + direction);
        ApplyCharacter(2);
    }

    private int WrapIndex(int index)
    {
        int count = database.characters.Length;

        if (index < 0)
            return count - 1;

        if (index >= count)
            return 0;

        return index;
    }

    private void ApplyCharacter(int player)
    {
        if (database.characters.Length == 0)
        {
            Debug.LogError("CharacterDatabaseにキャラが登録されていません");
            return;
        }

        if (player == 1)
        {
            CharacterData data = database.GetCharacter(p1Index);

            GameSettings.player1CharacterIndex = p1Index;

            p1Icon.sprite = data.icon;
            p1Icon.color = data.paddleColor;
            p1Icon.enabled = true;

            p1NameText.text = data.characterName;
        }
        else
        {
            if (!GameSettings.NeedPlayer2Selection())
                return;

            CharacterData data = database.GetCharacter(p2Index);

            GameSettings.player2CharacterIndex = p2Index;

            p2Icon.sprite = data.icon;
            p2Icon.color = data.paddleColor;
            p2Icon.enabled = true;

            p2NameText.text = data.characterName;
        }
    }

    private void ToggleP1Ready()
    {
        GameSettings.player1Ready = !GameSettings.player1Ready;

        UpdateReadyUI();
        CheckNext();
    }

    private void ToggleP2Ready()
    {
        if (!GameSettings.NeedPlayer2Selection())
            return;

        GameSettings.player2Ready = !GameSettings.player2Ready;

        UpdateReadyUI();
        CheckNext();
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

        for (int i = 0; i < database.characters.Length; i++)
        {
            text += database.characters[i].characterName + "\n";
        }

        listText.text = text;
    }

    private void CheckNext()
    {
        if (GameSettings.player1Ready && GameSettings.player2Ready)
        {
            SceneManager.LoadScene("ItemSelectScene");
        }
    }

    public void BackToStageSelect()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}