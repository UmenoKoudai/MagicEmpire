using UnityEngine;

public class FireBall : MonoBehaviour, IAbility
{

    public void Ability(Player player)
    {
        player.Anim.Play("Attack1");
        var magic = (GameObject)Resources.Load("FireBall");
        Instantiate(magic, player.MuzzleRight.position, Quaternion.identity);
    }
}
