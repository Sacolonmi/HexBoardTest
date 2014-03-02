using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    void Update()
    {
        Vector3 move = new Vector3(
            transform.position.x + Input.GetAxis("Horizontal"),
            transform.position.y,
            transform.position.z + Input.GetAxis("Vertical")
            );

        transform.position = move;
    }
}
