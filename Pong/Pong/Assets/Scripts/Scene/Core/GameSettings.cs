public static class GameSettings
{
    // プレイヤー人数
    // 1 = 1人
    // 2 = 2人
    public static int playerCount = 1;

    // ゲームモード
    // VS   = 通常Pong
    // BOSS = ボス戦
    public static GameMode gameMode = GameMode.VS;

    // ステージ番号
    public static int stageIndex = 0;

    // キャラクター選択
    public static int player1CharacterIndex = 0;
    public static int player2CharacterIndex = 0;

    // アイテム選択
    public static int player1ItemIndex = -1;
    public static int player2ItemIndex = -1;
     
    // READY状態     
    public static bool player1Ready = false;
    public static bool player2Ready = false;

    // リザルト
    public static string winnerText = "Player Wins!";

    // 2P選択が必要か
    // VSで2Pを選んだ時だけ2P選択が必要
    // Bossも2Pプレイなら2P選択が必要
    public static bool NeedPlayer2Selection()
    {
        return playerCount == 2;
    }

    // VSでCPU相手か
    public static bool IsVSCPU()
    {
        return gameMode == GameMode.VS && playerCount == 1;
    }

    // VSで2P対戦か

    public static bool IsVS2P()
    {
        return gameMode == GameMode.VS && playerCount == 2;
    }

    // ボス戦か
    public static bool IsBoss()
    {
        return gameMode == GameMode.BOSS;
    }

    public static void ResetCharacterSelections()
    {
        player1CharacterIndex = 0;
        player2CharacterIndex = 0;

        player1Ready = false;
        player2Ready = false;
    }

    public static void ResetItemSelections()
    {
        player1ItemIndex = -1;
        player2ItemIndex = -1;

        player1Ready = false;
        player2Ready = false;
    }
}