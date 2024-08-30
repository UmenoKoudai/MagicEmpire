using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private void Update()
    {
        var camera = Camera.main.transform.position;
        var direction = transform.position - camera;

        transform.forward = direction;
    }
}
