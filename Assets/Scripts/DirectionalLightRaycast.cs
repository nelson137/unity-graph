using UnityEngine;

/// <summary>
/// Draw a debug ray along the path of the directional light. Remember that the ray is only visible
/// in-game when gizmos are on.
/// </summary>
[ExecuteInEditMode]
public class DirectionalLightRaycast : MonoBehaviour
{
    void Update()
    {
        var dir = transform.TransformDirection(Vector3.forward) * 20;
        Debug.DrawRay(transform.position, dir, Color.red);
    }
}
