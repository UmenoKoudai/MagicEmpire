using UnityEngine;

public class FireBall : MonoBehaviour, IAbility
{

    public void Ability(Transform muzzle, Player plyaer)
    {

        var magic = (GameObject)Resources.Load("");
        Instantiate(magic, muzzle.position, Quaternion.identity);
    }
}
