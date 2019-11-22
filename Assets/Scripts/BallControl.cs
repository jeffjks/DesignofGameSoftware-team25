﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    public BallState m_BallState;
    public Rigidbody m_RigidBody;
    public Transform m_BallCamera;
    public float m_CameraRotationSpeed;
    public AudioSource m_AudioGolfHit;
    
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

        if (m_GameManager.m_Stage == 0) {
            m_GameManager.m_MapPlayer.anchoredPosition = new Vector2(transform.position.x*3f + 30f, - 300f + transform.position.z*3f - 30f);
        }
        else if (m_GameManager.m_Stage == 1) {
            m_GameManager.m_MapPlayer.anchoredPosition = new Vector2(transform.position.x*1.8f + 30f, - 540f + transform.position.z*1.8f - 30f);
        }
    }
    
    private void PreformCameraRotation(float xRot)
    {
        m_CameraRotation += xRot * m_CameraRotationSpeed;
        m_BallCamera.eulerAngles = new Vector3(0f, m_CameraRotation, 0f);
        //m_BallCamera.Rotate(Vector3.up * m_CameraRotationSpeed * xRot, Space.World);
    }

    private void PerformShot()
    {
        m_BallState.m_Controlable = false;
        float power = m_BallState.m_ShotPower;
        float force_height = (Mathf.Sin((power - 50f)*1.8f*Mathf.Deg2Rad) + 1) * 0.5f;
        
        Vector3 force = m_BallCamera.forward * power * 0.01f + new Vector3(0f, force_height, 0f);
        m_RigidBody.AddForce(force, ForceMode.Impulse);
        m_BallState.m_MinimalDelay = false;
        m_GameManager.AddTry();
        if (m_BallState.m_ShotPower > 15f)
            m_RigidBody.angularVelocity = Random.insideUnitSphere;
        m_AudioGolfHit.Play();
        Invoke("MinimalDelay", 1f);
    }

    private void MinimalDelay() {
        m_BallState.m_MinimalDelay = true;
    }
}

