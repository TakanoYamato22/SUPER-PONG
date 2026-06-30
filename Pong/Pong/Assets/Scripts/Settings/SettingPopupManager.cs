using UnityEngine;
using UnityEngine.EventSystems;

public class SettingPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject settingPopup;
    [SerializeField] private GameObject firstSelectedUI;
    [SerializeField] private GameObject closedSelectedUI;

    public void OpenPopup()
    {
        settingPopup.SetActive(true);
        if (EventSystem.current != null && firstSelectedUI != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedUI);
        }
    }

    public void ClosePopup()
    {
        settingPopup.SetActive(false);
        if (EventSystem.current != null && closedSelectedUI != null)
        {
            EventSystem.current.SetSelectedGameObject(closedSelectedUI);
        }
    }
}