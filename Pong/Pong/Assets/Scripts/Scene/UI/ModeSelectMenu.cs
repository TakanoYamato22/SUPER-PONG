using UnityEngine;

// ÉÇÅ[ÉhëIë
public class ModeSelectMenu : MonoBehaviour
{
    public void SelectCPU()
    {
        GameSettings.gameMode = "CPU";
    }

    public void SelectVS()
    {
        GameSettings.gameMode = "VS";
    }

    public void SelectBoss()
    {
        GameSettings.gameMode = "BOSS";
    }
}