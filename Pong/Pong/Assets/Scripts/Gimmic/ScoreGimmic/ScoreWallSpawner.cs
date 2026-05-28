//using UnityEngine;

//public class ScoreWallSpawner : MonoBehaviour
//{
//    public HPManager hpManager;
//    public ScoreWallRuleData[] rules;

//    private void OnEnable()
//    {
//        if (hpManager != null)
//            hpManager.onPlayerHPChanged.AddListener(HandleHP);
//    }

//    private void OnDisable()
//    {
//        if (hpManager != null)
//            hpManager.onPlayerHPChanged.RemoveListener(HandleHP);
//    }

//    private void HandleHP(float hp)
//    {
//        foreach (var rule in rules)
//        {
//            if (hp == rule.scoreThreshold)   // scoreThreshold → HP閾値として再利用
//            {
//                SpawnWalls(rule);
//                break;
//            }
//        }
//    }

//    private void SpawnWalls(ScoreWallRuleData rule)
//    {
//        for (int i = 0; i < rule.spawnCount; i++)
//        {
//            int index = Random.Range(0, rule.wallTypes.Length);
//            WallTypeData type = rule.wallTypes[index];

//            GameObject wall = Instantiate(type.prefab, GetRandomPosition(), Quaternion.identity);
//            wall.GetComponent<WallBlock>().data = type;
//        }
//    }

//    private Vector2 GetRandomPosition()
//    {
//        float camHeight = Camera.main.orthographicSize;
//        float y = Random.Range(-camHeight + 0.5f, camHeight - 0.5f);

//        return new Vector2(0f, y);
//    }
//}
