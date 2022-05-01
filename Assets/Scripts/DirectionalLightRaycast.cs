using UnityEngine;

[ExecuteInEditMode]
public class DirectionalLightRaycast : MonoBehaviour
{
    void Update()
    {
        var dir = transform.TransformDirection(Vector3.forward) * 100;
        Debug.DrawRay(transform.position, dir, Color.red, Time.deltaTime, true);
    }
}
