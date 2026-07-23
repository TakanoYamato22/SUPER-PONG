using UnityEngine;

public static class InputConfig
{
    // =========================
    // 選択画面用
    // =========================

    public static KeyCode p1SelectLeft = KeyCode.A;
    public static KeyCode p1SelectRight = KeyCode.D;
    public static KeyCode p1Ready = KeyCode.W;

    public static KeyCode p2SelectLeft = KeyCode.LeftArrow;
    public static KeyCode p2SelectRight = KeyCode.RightArrow;
    public static KeyCode p2Ready = KeyCode.UpArrow;

    // =========================
    // ゲーム中用
    // =========================

    public static KeyCode p1Smash = KeyCode.D;
    public static KeyCode p1UseItem = KeyCode.A;

    public static KeyCode p2Smash = KeyCode.LeftArrow;
    public static KeyCode p2UseItem = KeyCode.RightArrow;
}