using UnityEngine;
using Utils;

public class SerchRangeGizmo : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        try
        {
            var enemy = GetComponent<EnemyBase>();
            Gizmos.color = Color.blue;
            GizmosExtensions.DrawWireCircle(transform.position, enemy.SerchRange);
        } 
        catch
        {
            Debug.LogError("EnemyBaseÇ™ë∂ç›ÇµÇ‹ÇπÇÒ");
        }
    }
}
