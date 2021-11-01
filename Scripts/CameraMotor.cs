using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    public Vector3 offset = new Vector3(0,5f,-10f);
    public Vector3 rotation = new Vector3(35, 0, 0);

    public bool IsMoving { get; set; }
    void Start()
    {
      //  transform.position = lookAt.position + offset;

    }   

    // Update is called once per frame
    void LateUpdate()
    {
        if (!IsMoving)
            return;
         
        Vector3 desiredPos = lookAt.position + offset;
        desiredPos.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * 2);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation),0.1f); // convert vector3 into quaternion
    }
}
