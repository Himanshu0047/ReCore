using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform playerAttach;
    public Transform target;
    public Transform floor;
    //public float smoothing = 5f;

    Vector3 offset;

    void Start()
    {
        Input.gyro.enabled = true;
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
       
        playerAttach.position = target.position;
        Vector3 previousEulerAngle = playerAttach.eulerAngles;
        Vector3 gyroInput = -Input.gyro.rotationRateUnbiased;
        Vector3 targetEulerAngle = previousEulerAngle + gyroInput * Time.deltaTime * Mathf.Rad2Deg;
        targetEulerAngle.x = 0;
        targetEulerAngle.z = 0;
        playerAttach.eulerAngles = targetEulerAngle;
        floor.eulerAngles = targetEulerAngle;

        Vector3 targetCamPos = target.position + offset;
        //transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
