//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

//public class HPManager : MonoBehaviour
//{
//    public static HPManager Instance;

//    [Header("HP Settings")]
//    public float playerMaxHP = 100f;
//    public float bossMaxHP = 100f;

//    public float playerHP;
//    public float bossHP;

//    [Header("UI")]
//    public Slider playerHPSlider;
//    public Slider bossHPSlider;

//    // HP変化イベント（ScoreManagerのイベント互換）
//    public UnityEvent<float> onPlayerHPChanged;
//    public UnityEvent<float> onBossHPChanged;

//    private void Awake()
//    {
//        Instance = this;

//        playerHP = playerMaxHP;
//        bossHP = bossMaxHP;

//        playerHPSlider.maxValue = playerMaxHP;
//        bossHPSlider.maxValue = bossMaxHP;

//        playerHPSlider.value = playerHP;
//        bossHPSlider.value = bossHP;
//    }

//    public void DamagePlayer(float amount)
//    {
//        playerHP -= amount;
//        playerHP = Mathf.Max(playerHP, 0);

//        playerHPSlider.value = playerHP;
//        onPlayerHPChanged?.Invoke(playerHP);
//    }

//    public void DamageBoss(float amount)
//    {
//        bossHP -= amount;
//        bossHP = Mathf.Max(bossHP, 0);

//        bossHPSlider.value = bossHP;
//        onBossHPChanged?.Invoke(bossHP);
//    }
//}
