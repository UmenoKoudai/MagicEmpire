using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("’e‚ÌˆÚ“®‘¬“x")]
    private float _speed = 20;

    public Vector3 Direction { get; set; }

    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(Direction * _speed, ForceMode.Impulse);
    }


}
