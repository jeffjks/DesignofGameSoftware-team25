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

    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameManager.instance_gm;
    }

    void Update()
    {
        float xRot = Input.GetAxisRaw("Mouse X");

        PreformCameraRotation(xRot);

        if (m_BallState.m_Controlable) {
            if (Input.GetButtonDown("Fire1")) {
                m_BallState.m_ShotPower = 0;
            }
            else if (Input.GetButton("Fire1")) {
                m_BallState.m_ShotPower++;
            }
            else if (Input.GetButtonUp("Fire1")) {
                PerformShot();
            }

            if (m_BallState.m_ShotPower >= m_BallState.MAX_SHOT_POWER) {
                PerformShot();
            }
        }
    }
    
    private void PreformCameraRotation(float xRot)
    {
        m_CameraRotation += xRot * m_CameraRotationSpeed;
        m_BallCamera.eulerAngles = new Vector3(0f, m_CameraRotation, 0f);
        //m_BallCamera.Rotate(Vector3.up * m_CameraRotationSpeed * xRot, Space.World); Mathf.Sin(Mathf.pi*m_BallState.m_ShotPower/100) + 1
    }

    private void PerformShot()
    {
        m_BallState.m_Controlable = false;
        Vector3 force = m_BallCamera.forward * m_BallState.m_ShotPower * 0.01f + new Vector3(0f, m_BallState.m_ShotPower*0.01f, 0f); // Sine 함수로 변경 예정
        //Debug.Log(force);
        m_RigidBody.AddForce(force, ForceMode.Impulse);
        m_BallState.m_MinimalDelay = false;
        m_GameManager.AddTry();
        Invoke("MinimalDelay", 1f);
    }

    private void MinimalDelay() {
        m_BallState.m_MinimalDelay = true;
    }
}

