using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallState : MonoBehaviour
{
    public Rigidbody m_RigidBody;
    public float m_AirDrag, m_GroundDrag;
    
    private RaycastHit m_RaycastHit;
    private float m_MaxDistance;
    private bool m_IsGround;

    [HideInInspector] public bool m_Controlable = true;

    void Start()
    {
        m_MaxDistance = transform.localScale.y * 0.5f + 0.1f;
    }

    void Update()
    {
        Debug.Log(m_IsGround);

        if (m_IsGround) {
            m_RigidBody.drag = m_GroundDrag;
        }
        else {
            m_RigidBody.drag = m_AirDrag;
        }

        if (m_RigidBody.velocity == Vector3.zero) {
            m_Controlable = true;
        }
    }

    // Check Collision with Ground
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") {
            m_IsGround = true;
        }
    }
     
     void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") {
            m_IsGround = false;
        }
    }
}

