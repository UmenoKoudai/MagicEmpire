using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out EnemyBase enemy))
        {
            var player = FindObjectOfType<Player>();
            //player.HitStop();
            enemy.Hit(player.Attack);

        }
    }
}
