using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallState : MonoBehaviour
{
    public Rigidbody m_RigidBody;
    public Transform m_Goal;
    public float m_AirDrag, m_GroundDrag;

    public int MAX_SHOT_POWER = 100;
    
    private RaycastHit m_RaycastHit;
    private float m_MaxDistance;
    private bool m_IsGround;
    private Vector3 m_PreviousPos; // 이전 위치 (y좌표 3이하로 떨어지면 이전 위치로 이동)
    private float m_GoalDistance;
    private float m_UnderHeightTimer;

    private const float HEIGHT_DEATH = 3f;

    [HideInInspector] public bool m_MinimalDelay; // 버그 방지용
    [HideInInspector] public bool m_Controlable = true; // 현재 볼 컨트롤 가능 여부
    [HideInInspector] public int m_ShotPower;

    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameManager.instance_gm;
        m_MaxDistance = transform.localScale.y * 0.5f + 0.1f;
        m_PreviousPos = transform.position;
    }

    void Update()
    {
        if (m_IsGround) {
            m_RigidBody.drag = m_GroundDrag;
        }
        else {
            m_RigidBody.drag = m_AirDrag;
        }

        if (m_RigidBody.velocity == Vector3.zero) {
            if (m_MinimalDelay) {
                if (!m_Controlable) {
                    if (m_GameManager.CheckTry()) {
                        m_PreviousPos = transform.position;
                        m_Controlable = true;
                        m_ShotPower = 0;
                    }
                }
            }
        }

        if (transform.position.y < HEIGHT_DEATH) {
            if (GetGoalDistance() > 1f) {
                m_UnderHeightTimer += Time.deltaTime;
            }
            else {
                m_GameManager.StageClear();
            }
        }
        else {
            m_UnderHeightTimer = 0f;
        }

        if (m_UnderHeightTimer > 2f) {
            Retry();
        }
        
        m_GameManager.m_PowerImage.fillAmount = Mathf.Lerp(0, 1, (float) m_ShotPower / (float) MAX_SHOT_POWER);
    }

    private void Retry() {
        transform.position = m_PreviousPos;
        m_RigidBody.velocity = Vector3.zero;
    }

    private float GetGoalDistance() {
        float distance = Vector3.Distance(transform.position, m_Goal.position);
        return distance;
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

