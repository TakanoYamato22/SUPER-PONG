public static class GameSettings
{
    public static int playerCount = 1;
    public static GameMode gameMode = GameMode.CPU;
    public static int stageIndex = 0;

    public static int player1CharacterIndex = 0;
    public static int player2CharacterIndex = 0;

    public static int player1ItemIndex = -1;
    public static int player2ItemIndex = -1;

    public static bool player1Ready = false;
    public static bool player2Ready = false;

    public static string winnerText = "Player Wins!";

    public static bool NeedPlayer2Selection()
    {
        return playerCount == 2 && gameMode != GameMode.CPU;
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