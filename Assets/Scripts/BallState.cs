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
    private bool m_IsGround, IsGoal;
    private Vector3 m_StartPos; // 시작 위치
    private Vector3 m_PreviousPos; // 이전 위치 (y좌표 3이하로 떨어지면 이전 위치로 이동)
    private float m_GoalDistance;
    private float m_UnderHeightTimer;
    private Vector2 m_Wind;

    private const float HEIGHT_DEATH = 1f;
    private const float MAX_WIND = 8f;
    private const float WIND_FACTOR = 0.01f;

    [HideInInspector] public bool m_MinimalDelay; // 버그 방지용
    [HideInInspector] public bool m_Controlable; // 현재 볼 컨트롤 가능 여부
    [HideInInspector] public int m_ShotPower;

    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameManager.instance_gm;
        m_MaxDistance = transform.localScale.y * 0.5f + 0.1f;
        m_StartPos = transform.position;
        m_PreviousPos = transform.position;
        SetWind();
    }

    void Update()
    {
        if (m_IsGround) {
            m_RigidBody.drag = m_GroundDrag;
        }
        else {
            m_RigidBody.drag = m_AirDrag;
            if (!m_Controlable) {
                m_RigidBody.AddForce(new Vector3(m_Wind.x * WIND_FACTOR, 0f, m_Wind.y * WIND_FACTOR), ForceMode.Force);
            }
        }

        //Debug.Log(m_RigidBody.velocity.magnitude);
        if (m_RigidBody.velocity == Vector3.zero) {
            if (m_MinimalDelay) {
                if (!m_Controlable) {
                    if (transform.position.y >= HEIGHT_DEATH) {
                        if (m_GameManager.CheckTry()) {
                            m_PreviousPos = transform.position;
                            m_Controlable = true;
                            m_ShotPower = 0;
                            SetWind();
                        }
                    }
                }
            }
        }
        else if (m_RigidBody.velocity.magnitude < 0.1f) {
            m_RigidBody.velocity = Vector3.zero;
        }
        
        else if (transform.position.y < HEIGHT_DEATH) {
            m_UnderHeightTimer += Time.deltaTime;
        }
        else {
            m_UnderHeightTimer = 0f;
        }

        if (m_UnderHeightTimer > 2f) {
            Retry();
        }
        
        m_GameManager.m_PowerImage.fillAmount = Mathf.Lerp(0, 1, (float) m_ShotPower / (float) MAX_SHOT_POWER);
    }

    private void SetWind() {
        m_Wind = Random.insideUnitCircle * MAX_WIND;
        m_GameManager.m_CompassArrow.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, m_Wind));
        m_GameManager.m_WindSpeed.text = Mathf.Round(m_Wind.magnitude * 10)/10  + " m/s";
    }

    private void Retry() {
        m_UnderHeightTimer = 0f;
        transform.position = m_PreviousPos;
        m_RigidBody.velocity = Vector3.zero;
        SetWind();
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

    // Check Collision with Goal
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Goal") {
            if (!IsGoal) {
                IsGoal = true;
                Debug.Log("Goal");
                m_GameManager.StageClear();
            }
        }
    }
}

