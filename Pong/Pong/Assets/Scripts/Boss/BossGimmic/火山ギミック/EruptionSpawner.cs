
using System.Collections;
using UnityEngine;

public class EruptionSpawner : MonoBehaviour
{
    [Header("•¬‰ÎPrefab")]
    [SerializeField] private GameObject eruptionPrefab;

    [Header("•¬‰Î‚Ì¶¬”ÍˆÍ")]
    [SerializeField] private float minX = -7f;
    [SerializeField] private float maxX = 7f;
    [SerializeField] private float spawnY = -4f;

    [Header("•¬‰ÎI—¹Œã‚Ì‘Ò‹@ŠÔ")]
    [SerializeField] private float spawnInterval = 15f;

    [Header("•¬‰Î‚É‚©‚©‚éŠÔ")]
    [SerializeField] private float eruptionDuration = 7f;

    private Coroutine spawnCoroutine;
    private bool isSpawning = false;

    public void StartSpawning()
    {
        if (isSpawning) return;

        if (eruptionPrefab == null)
        {
            Debug.LogError("•¬‰ÎPrefab‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return;
        }

        isSpawning = true;
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        isSpawning = false;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning)
        {
            float randomX = Random.Range(minX, maxX);

            Vector3 spawnPosition = new Vector3(
                randomX,
                spawnY,
                0f
            );

            Instantiate(
                eruptionPrefab,
                spawnPosition,
                Quaternion.identity
            );

            yield return new WaitForSeconds(eruptionDuration);

            yield return new WaitForSeconds(spawnInterval);
        }

        spawnCoroutine = null;
    }
}
