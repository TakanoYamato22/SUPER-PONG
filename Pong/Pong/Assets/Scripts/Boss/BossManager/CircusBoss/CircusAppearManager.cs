
using System.Collections;
using UnityEngine;

public class CircusAppearManager : MonoBehaviour
{
    // ========================================
    // 参照
    // ========================================

    [Header("サーカスボス")]
    [SerializeField]
    private CircusBossController bossController;

    [Header("ルーレット")]
    [SerializeField]
    private CircusRoulette roulette;

    // ========================================
    // 戦闘開始
    // ========================================

    [Header("戦闘開始までの時間")]
    [SerializeField]
    private float startDelay = 1.5f;

    private IEnumerator Start()
    {
        if (bossController == null)
        {
            Debug.LogError(
                "CircusBossControllerが未設定です！",
                this
            );

            yield break;
        }

        // 戦闘開始前は停止
        bossController.SetBattleStarted(false);

        if (roulette != null)
        {
            roulette.SetPaused(true);
        }

        // 登場演出などの待機時間
        yield return new WaitForSeconds(startDelay);

        // 戦闘開始
        bossController.SetBattleStarted(true);

        if (roulette != null)
        {
            roulette.StartRoulette();
            roulette.SetPaused(false);
        }

        Debug.Log("Circus Boss 戦闘開始！");
    }

    private void Update()
    {
        if (bossController == null ||
            roulette == null)
        {
            return;
        }

        // 戦闘中以外はルーレット停止
        bool shouldPause =
            !bossController.BattleStarted ||
            bossController.IsDead ||
            bossController.IsPhaseChanging;

        roulette.SetPaused(shouldPause);
    }
}
