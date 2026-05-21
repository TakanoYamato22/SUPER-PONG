using UnityEngine;

// l”‘I‘ğ
public class TitleMenu : MonoBehaviour
{
    public void Select1P()
    {
        GameSettings.playerCount = 1;
    }

    public void Select2P()
    {
        GameSettings.playerCount = 2;
    }
}