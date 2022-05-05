using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotion : MonoBehaviour
{
    [SerializeField]
    Transform graph;

    // Update is called once per frame
    void Update()
    {
        var target = graph.position;
        var axis = Vector3.up;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(target, axis, -10f);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(target, axis, 10f);
        }

        if (Input.GetMouseButton(0))
        {
            float angle = 300f * Time.deltaTime * Input.GetAxis("Mouse X");
            transform.RotateAround(target, axis, angle);
        }
    }
}
