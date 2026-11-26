
using System.Collections;
using UnityEngine;

public class ForestBossAppearManager : MonoBehaviour
{
    [Header("森ステージのボス")]
    [SerializeField]
    private ForestBossController bossController;

    [Header("戦闘開始までの時間")]
    [SerializeField]
    private float startDelay = 1.5f;



    private IEnumerator Start()
    {
        if (bossController == null)
        {
            Debug.LogError(
                "古代樹が設定されていません！",
                this
            );

            yield break;
        }

        // 戦闘開始前
        bossController.SetBattleStarted(false);

        // 指定時間待機
        yield return new WaitForSeconds(startDelay);

        // 戦闘開始
        bossController.SetBattleStarted(true);

        Debug.Log(
            "森ステージ：古代樹との戦闘開始！",
            this
        );
    }
}
