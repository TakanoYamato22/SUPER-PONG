using UnityEngine;

public class ScoreWallSpawner : MonoBehaviour
{
    public ScoreManager scoreManager;
    public ScoreWallRuleData[] rules;   // �� ���ꂪ�d�v�I

    private void OnEnable()
    {
        if (scoreManager != null)
            scoreManager.onPlayerScoreChanged.AddListener(HandleScore);
    }

    private void OnDisable()
    {
        if (scoreManager != null)
            scoreManager.onPlayerScoreChanged.RemoveListener(HandleScore);
    }

    private void HandleScore(int score)
    {
        foreach (var rule in rules)
        {
            if (score == rule.scoreThreshold)
            {
                SpawnWalls(rule);
                break;
            }
        }
    }

    private void SpawnWalls(ScoreWallRuleData rule)
    {
        for (int i = 0; i < rule.spawnCount; i++)
        {
            int index = Random.Range(0, rule.wallTypes.Length);
            WallTypeData type = rule.wallTypes[index];

            GameObject wall = Instantiate(type.prefab, GetRandomPosition(), Quaternion.identity);
            wall.GetComponent<WallBlock>().data = type;
        }
    }

    private Vector2 GetRandomPosition()
    {
        //float camHeight = Camera.main.orthographicSize;
        //float camWidth = camHeight * Camera.main.aspect;

        //float x = Random.Range(-camWidth + 0.5f, camWidth - 0.5f);
        //float y = Random.Range(-camHeight + 0.5f, camHeight - 0.5f);
         
        main

        float camHeight = Camera.main.orthographicSize;

        float y = Random.Range(-camHeight + 0.5f, camHeight - 0.5f);

        return new Vector2(0f, y);   // �� x �� 0 �ɌŒ�@�^�񒆌Œ�̏c�̂�
    }

}

