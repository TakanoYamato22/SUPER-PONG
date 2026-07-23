using UnityEngine;

public class BallRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 360f; // 1ïbä‘Ç…180ìx

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}