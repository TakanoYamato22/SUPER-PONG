using UnityEngine;

public class FactoryBallGimmickRunner : BallGimmickRunner
{
    [SerializeField] private BossBallGimmick gimmick;
    [SerializeField] private Ball ball;

    [Header("Press Machine")]
    [SerializeField] private PressMachineManager[] pressMachineManagers;

    [Header("Moving Drone")]
    [SerializeField] private GameObject dronePrefab;
    [SerializeField] private int droneCount = 3;
    [SerializeField] private float spawnRangeX = 8f;
    [SerializeField] private float spawnRangeY = 8f;
    [SerializeField] private float droneSpawnInterval = 10f;

    [Header("Fixed Drone")]
    [SerializeField] private GameObject fixedDronePrefab;
    [SerializeField] private Vector2[] fixedDronePositions;

    private bool pressMachineSpawned = false;
    private bool fixedDroneSpawned = false;

    private void Awake()
    {
        if (ball == null)
            ball = FindObjectOfType<Ball>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMovingDrones), 0f, droneSpawnInterval);
    }

    private void Update()
    {
        if (gimmick == null || ball == null)
            return;

        gimmick.OnUpdate(ball);
    }

    public override void OnBossHpChanged(float currentHp, float maxHp)
    {
        Debug.Log($"BossHP : {currentHp} / {maxHp}");

        if (gimmick != null && ball != null)
            gimmick.OnBossHpChanged(ball, (int)currentHp, (int)maxHp);

        if (!pressMachineSpawned && currentHp <= maxHp * 0.5f)
        {
            pressMachineSpawned = true;
            StartPressMachines();
            Debug.Log("50% PressMachine 出現！");
        }

        if (!fixedDroneSpawned && currentHp <= maxHp * 0.3f)
        {
            fixedDroneSpawned = true;
            SpawnFixedDrones();
            Debug.Log("30% Fixed Drone 出現！");
        }
    }

    private void StartPressMachines()
    {
        foreach (PressMachineManager manager in pressMachineManagers)
        {
            if (manager != null)
                manager.StartPressMachineEvent();
        }
    }

    private void SpawnMovingDrones()
    {
        if (dronePrefab == null)
            return;

        for (int i = 0; i < droneCount; i++)
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(-spawnRangeX, spawnRangeX),
                Random.Range(-spawnRangeY, spawnRangeY));

            Instantiate(dronePrefab, spawnPos, Quaternion.identity);
        }
    }

    private void SpawnFixedDrones()
    {
        if (fixedDronePrefab == null)
            return;

        foreach (Vector2 pos in fixedDronePositions)
        {
            Instantiate(fixedDronePrefab, pos, Quaternion.identity);
        }
    }

    public void SetGimmick(BossBallGimmick newGimmick)
    {
        gimmick = newGimmick;
    }
}