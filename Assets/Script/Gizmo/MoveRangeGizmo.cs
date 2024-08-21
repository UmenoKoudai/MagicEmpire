using UnityEngine;
using Utils;

public class MoveRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        try
        {
            var enemy = GetComponent<EnemyBase>();
            Gizmos.color = Color.red;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.MoveRange);
        }
        catch
        {
            Debug.LogError("EnemyBaseÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
        }
    }
}
