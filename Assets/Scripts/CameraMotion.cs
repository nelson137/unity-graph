using UnityEngine;

/// <summary>
/// Swivel the camera around the graph on user input.
/// </summary>
public class CameraMotion : MonoBehaviour
{
    /// <summary>
    /// The graph around which to swivel.
    /// </summary>
    [SerializeField]
    Transform graph;

    void Update()
    {
        const float mouseAngleFactor = 400f;
        Vector3 target = graph.position;
        bool isMouseDown = Input.GetMouseButton(0);

        // Swivel left/right.
        Vector3 axisLR = Vector3.up;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target, axisLR, 0.5f);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target, axisLR, -0.5f);
        }
        if (isMouseDown)
        {
            float angle = mouseAngleFactor * Time.deltaTime * Input.GetAxis("Mouse X");
            transform.RotateAround(target, axisLR, angle);
        }

        // Swivel up/down.
        Vector3 axisUD = transform.TransformDirection(Vector3.left);
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.RotateAround(target, axisUD, 0.5f);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(target, axisUD, -0.5f);
        }
        if (isMouseDown)
        {
            float angle = mouseAngleFactor * Time.deltaTime * Input.GetAxis("Mouse Y");
            transform.RotateAround(target, axisUD, angle);
        }
    }
}
