using UnityEngine;

// シーンをまたいで設定を保存するクラス
public static class GameSettings
{
    // ============================
    // 👤 プレイヤー人数
    // ============================

    public static int playerCount = 2;

    // ============================
    // 🎮 ゲームモード
    // ============================

    public static string gameMode = "VS";

    // ============================
    // 🗺️ ステージ番号
    // ============================

    public static int stageIndex = 0;

    // ============================
    // 👤 キャラクター選択
    // ============================

    public static int player1CharacterIndex = 0;
    public static int player2CharacterIndex = 0;

    // ============================
    // 🎁 アイテム選択
    // ============================

    public static int[] player1Items = { -1, -1 };
    public static int[] player2Items = { -1, -1 };

    // ============================
    // READY状態
    // ============================

    public static bool player1Ready = false;
    public static bool player2Ready = false;

    // ============================
    // 🏆 リザルト表示
    // ============================

    public static string winnerText = "Player Wins!";

    // ============================
    // リセット用
    // ============================

    public static void ResetSelections()
    {
        player1Items[0] = -1;
        player1Items[1] = -1;

        player2Items[0] = -1;
        player2Items[1] = -1;

        player1Ready = false;
        player2Ready = false;
    }
}