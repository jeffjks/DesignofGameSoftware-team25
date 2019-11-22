using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text m_TryStateText;
    public Image m_PowerImage;
    public RectTransform m_CompassArrow;
    public RectTransform m_MapPlayer;
    public Text m_WindSpeed;
    public GameObject m_GameClear;
    public GameObject[] m_Map;

    [HideInInspector] public bool m_ToNextStage = false;
    public byte m_Stage;

    private byte m_Try;
    private byte[] m_MaxTry = {5, 7};
    private bool m_GameClearState = false;

    public static GameManager instance_gm = null;
    
    void Awake()
    {
        if (instance_gm != null) {
            Destroy(this.gameObject);
            return;
        }
        instance_gm = this;
        
        DontDestroyOnLoad(gameObject);

        UpdateTryText();
    }

    public void UpdateTryText() {
        m_TryStateText.text = m_Try + " / " + m_MaxTry[m_Stage];
    }

    public void AddTry() {
        m_Try++;
        UpdateTryText();
    }

    private void InitTry() {
        m_Try = 0;
        UpdateTryText();
    }

    void Update()
    {
        if (m_GameClearState) {
            if (Input.GetButtonDown("Fire1")) {
                m_GameClear.SetActive(false);
                m_GameClearState = false;
                m_Stage = 0;
                SceneManager.LoadScene("Stage1");
                UpdateTryText();
            }
        }
        
        if (Input.GetButtonDown("Cancel")) {
            Application.Quit();
        }
    }

    public bool CheckTry() {
        if (m_Try < m_MaxTry[m_Stage]) {
            return true;
        }
        else {
            InitTry();
            return false;
        }
    }

    public void StageClear() {
        m_ToNextStage = true;
        StartCoroutine(NextStage());
    }

    private IEnumerator NextStage() {
        yield return new WaitForSeconds(3f);
        if (m_Stage < m_MaxTry.Length - 1) {
            m_Stage++;
            SceneManager.LoadScene("Stage" + (m_Stage + 1));
        }
        else {
            m_GameClear.SetActive(true);
            m_GameClearState = true;
        }
        m_ToNextStage = false;
        InitTry();

        for (int i = 0; i < m_Map.Length; i++) {
            m_Map[i].SetActive(false);
        }
        m_Map[m_Stage].SetActive(true);
        yield break;
    }
}
