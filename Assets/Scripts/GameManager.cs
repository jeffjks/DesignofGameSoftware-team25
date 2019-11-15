using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text m_TryStateText;
    public Image m_PowerImage;

    private byte m_Stage = 0;
    private byte m_Try;
    private byte[] m_MaxTry = {5, 3, 7};

    public static GameManager instance_gm = null;
    
    void Awake()
    {
        if (instance_gm != null) {
            Destroy(this.gameObject);
            return;
        }
        instance_gm = this;
        
        DontDestroyOnLoad(gameObject);

        InitTryState();
    }

    public void InitTryState() {
        m_TryStateText.text = m_Try + " / " + m_MaxTry[m_Stage];
    }

    public void AddTry() {
        m_Try++;
        InitTryState();
    }

    public bool CheckTry() {
        if (m_Try < m_MaxTry[m_Stage]) {
            return true;
        }
        else {
            return false;
        }
    }

    public void StageClear() {
        StartCoroutine(NextStage());
    }

    private IEnumerator NextStage() {
        if (m_Stage < 2)
            SceneManager.LoadScene("Stage" + (m_Stage + 1));
        m_Stage++;
        InitTryState();
        yield break;
    }
}
