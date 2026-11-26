
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Ball))]
public class BallBurn : MonoBehaviour
{
    [Header("やけど設定")]
    [SerializeField] private float damageInterval = 10f;
    [SerializeField] private int burnDamage = 10;

    [Header("ダメージ対象")]
    [SerializeField] private PlayerHealth player1Health;
    [SerializeField] private PlayerHealth player2Health;

    [Header("炎エフェクト")]
    [SerializeField] private GameObject burnEffect;

    private BallSmashManager smashManager;
    private Coroutine burnCoroutine;

    private bool isBurning;

    public bool IsBurning => isBurning;

    private void Awake()
    {
        smashManager = GetComponent<BallSmashManager>();

        if (burnEffect != null)
        {
            burnEffect.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isBurning)
            return;

        // スマッシュ状態になったら解除
        if (smashManager != null && smashManager.IsSmashed)
        {
            RemoveBurn();
        }
    }

    // 噴火に触れたとき
    public void ApplyBurn()
    {
        // すでにやけど中なら何もしない
        // ダメージのカウントもリセットしない
        if (isBurning)
            return;

        // スマッシュ中はやけどしない
        if (smashManager != null && smashManager.IsSmashed)
            return;

        isBurning = true;

        if (burnEffect != null)
        {
            burnEffect.SetActive(true);
        }

        burnCoroutine = StartCoroutine(BurnRoutine());

        Debug.Log("Ballがやけど状態になった！");
    }


    private IEnumerator BurnRoutine()
    {
        while (isBurning)
        {
            yield return new WaitForSeconds(damageInterval);

            if (!isBurning)
                yield break;

            // 1Pにやけどダメージ
            if (player1Health != null)
            {
                player1Health.TakeDamage(burnDamage);
            }

            // 2Pにやけどダメージ
            if (player2Health != null &&
                player2Health != player1Health)
            {
                player2Health.TakeDamage(burnDamage);
            }

            Debug.Log(
                $"やけどダメージ発生！各プレイヤーに{burnDamage}ダメージ"
            );
        }
    }


    // スマッシュでやけど解除
    public void RemoveBurn()
    {
        if (!isBurning)
            return;

        isBurning = false;

        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
            burnCoroutine = null;
        }

        if (burnEffect != null)
        {
            burnEffect.SetActive(false);
        }

        Debug.Log("スマッシュでやけど解除！");
    }

    private void OnDisable()
    {
        if (isBurning)
        {
            RemoveBurn();
        }
    }
}
