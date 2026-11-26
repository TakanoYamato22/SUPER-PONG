
using UnityEngine;
using System.Collections;

public class AtlantisAppearManager : MonoBehaviour
{
    [Header("Atlantis Boss")]
    [SerializeField] private BossController bossController;

    [Header("戦闘開始までの時間")]
    [SerializeField] private float startDelay = 1.5f;

    private IEnumerator Start()
    {
        if (bossController == null)
        {
            Debug.LogError(
                "AtlantisのBossControllerが設定されていません！"
            );

            yield break;
        }

        // 最初は戦闘停止
        bossController.SetBattleStarted(false);

        // 指定時間待機
        yield return new WaitForSeconds(startDelay);

        // 戦闘開始
        bossController.SetBattleStarted(true);

        Debug.Log("Atlantis Boss 戦闘開始！");
    }
}
