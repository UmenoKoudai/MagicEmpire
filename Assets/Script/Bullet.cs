using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("弾の移動速度")]
    private float _speed = 20;

    public Vector3 Direction { get; set; }

    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        transform.forward = Direction;
        rb.AddForce(Direction * _speed, ForceMode.Impulse);
    }


}
