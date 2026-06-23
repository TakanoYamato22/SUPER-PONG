using System.Collections;
using UnityEngine;

public class PressMachineManager : MonoBehaviour
{
    [SerializeField] private Transform pressMachine;

    [Header("座標")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;

    [SerializeField] private float moveTime = 1.5f;

    private bool eventStarted = false;

    private void Start()
    {
        if (pressMachine != null)
        {
            // 最初は画面外に置く
            pressMachine.position = startPosition;

            // 消さない！
            pressMachine.gameObject.SetActive(true);
        }
    }

    public void StartPressMachineEvent()
    {
        Debug.Log("StartPressMachineEvent 呼ばれた");

        if (eventStarted)
        {
            Debug.Log("でも eventStarted が true だから止まった");
            return;
        }

        eventStarted = true;
        StartCoroutine(PressMachineAppear());
    }

    private IEnumerator PressMachineAppear()
    {
        Debug.Log("PressMachineAppear 開始");

        if (pressMachine == null)
        {
            Debug.LogWarning("PressMachine がセットされていません");
            yield break;
        }

        // ゲーム停止
        Time.timeScale = 0f;

        float timer = 0f;

        while (timer < moveTime)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / moveTime;

            pressMachine.position = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );

            yield return null;
        }

        pressMachine.position = endPosition;

        // ゲーム再開
        Time.timeScale = 1f;
    }
}