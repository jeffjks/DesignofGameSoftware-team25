using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public BallState m_BallState;
    public Rigidbody m_RigidBody;
    public Transform m_BallCamera;
    public float m_CameraRotationSpeed;
    
    private float m_CameraRotation;
    private int m_ShotPower;

    void Update()
    {
        float xRot = Input.GetAxisRaw("Mouse X");

        PreformCameraRotation(xRot);

        if (m_BallState.m_Controlable) {
            if (Input.GetButtonDown("Fire1")) {
                m_ShotPower = 0;
            }
            else if (Input.GetButton("Fire1")) {
                m_ShotPower++;
            }
            else if (Input.GetButtonUp("Fire1")) {
                PerformShot();
            }
        }
    }
    
    void PreformCameraRotation(float xRot)
    {
        m_CameraRotation += xRot * m_CameraRotationSpeed;
        m_BallCamera.eulerAngles = new Vector3(0f, m_CameraRotation, 0f);
        //m_BallCamera.Rotate(Vector3.up * m_CameraRotationSpeed * xRot, Space.World);
    }

    void PerformShot()
    {
        m_BallState.m_Controlable = false;
        Vector3 force = m_BallCamera.forward * m_ShotPower * 0.01f;
        Debug.Log(force);
        m_RigidBody.AddForce(force, ForceMode.Impulse);
    }
}

