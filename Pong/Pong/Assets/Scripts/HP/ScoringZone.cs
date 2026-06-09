//using UnityEngine;

//public class ScoringZone : MonoBehaviour
//{
//    public bool isPlayerGoal;
//    // true  = プレイヤーHPを減らす（左右の壁）
//    // false = Boss HP を減らす（Boss）

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!collision.TryGetComponent(out Ball ball)) return;

//        if (isPlayerGoal)
//        {
//            HPManager.Instance.DamagePlayer(ball.velocity.magnitude);
//        }
//        else
//        {
//            HPManager.Instance.DamageBoss(ball.velocity.magnitude);
//        }

//        ball.ResetAndStartWithDelay(1.0f);
//    }
//}
