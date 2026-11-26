
using UnityEngine;
using System.Collections;

public class SpaceBossAppearManager : MonoBehaviour
{
    [SerializeField]
    private SpaceBossController bossController;

    private IEnumerator Start()
    {
        if (bossController == null)
        {
            Debug.LogError("宇宙ボスが設定されていません！");
            yield break;
        }

        bossController.SetBattleStarted(false);

        yield return new WaitForSeconds(1.5f);

        bossController.SetBattleStarted(true);

        Debug.Log("宇宙ボス戦闘開始！");
    }
}
