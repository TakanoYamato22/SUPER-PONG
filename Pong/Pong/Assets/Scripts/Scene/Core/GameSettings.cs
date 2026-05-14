using UnityEngine;

// シーンをまたいで設定を保存するクラス
public static class GameSettings
{
  
    // 👤 プレイヤー人数
    public static int playerCount = 1;

    // ============================
    // 🎮 ゲームモード
    public static string gameMode = "CPU";

    // ============================
    // 🗺️ ステージ番号
    public static int stageIndex = 1;

    // ============================
    // 🏆 リザルト画面に表示する文字
    public static string winnerText = "Player Wins!";
}