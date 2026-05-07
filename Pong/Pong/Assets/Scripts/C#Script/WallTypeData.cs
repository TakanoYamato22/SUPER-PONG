using UnityEngine;

[CreateAssetMenu(menuName = "Pong/WallType")]
public class WallTypeData : ScriptableObject
{
    [Header("•Ç‚ÌŠî–{İ’è")]
    public string wallName = "Wall";
    public int hp = 1;
    public Color color = Color.white;
    public Vector2 size = new Vector2(1f, 1f);

    [Header("’µ‚Ë•Ô‚è‹­‰»i”CˆÓj")]
    public bool useBounceBoost = false;
    public float bounceMultiplier = 1f;

    [Header("Œ©‚½–Ú‚ÌPrefab")]
    public GameObject prefab;
}
