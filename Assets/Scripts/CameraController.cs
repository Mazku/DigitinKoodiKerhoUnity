using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Transform player;
    public float speed = 0.1f;

    new Camera camera;

    public void Start()
    {
        camera = GetComponent<Camera>();
    }

    public void Update()
    {
        camera.orthographicSize = (Screen.height / 100f) / 0.7f;

        if (player)
        {
            transform.position = Vector3.Lerp(transform.position, player.position, speed) + new Vector3(0, 0, -12);
        }
    }
}
