using UnityEngine;
using System.Collections;

public class DragonAppearManager : MonoBehaviour
{
    [SerializeField] private BossController bossController;

    private IEnumerator Start()
    {
        if (bossController != null)
            bossController.SetBattleStarted(false);

        yield return new WaitForSeconds(1.5f);

        if (bossController != null)
            bossController.SetBattleStarted(true);
    }
}