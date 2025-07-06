using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float MoveSpeed = 5.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime ;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += -transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += -transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += new Vector3(0, MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += new Vector3(0, -MoveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetMouseButton(1))
        {
            Vector2 mouseDelta = MoveSpeed * new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
            Quaternion rotation = transform.rotation;
            Quaternion horiz = Quaternion.AngleAxis(mouseDelta.x, Vector3.up);
            Quaternion vert = Quaternion.AngleAxis(mouseDelta.y, Vector3.right);
            transform.rotation = horiz * rotation * vert;
        }
    }
}
