using UnityEngine;
using CnControls;

public class InputManager : MonoBehaviour {
    public static InputManager Instance = null;
    private bool m_bReverse;                        ///< 반전 입력 여부
    private bool m_Enable;                          ///< 입력 가능 여부

    public static bool Reverse {
        get { return Instance.m_bReverse; }
        set { Instance.m_bReverse = value; }
    }

    public static bool Enable
    {
        get { return Instance.m_Enable; }
        set { Instance.m_Enable = value; }
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_Enable = true;
        m_bReverse = false;
    }
	
    /// <summary>
    /// 이동 방향 반환
    /// </summary>
	public static Vector3 GetMoveDir()
    {
        if (Enable == false)
            return Vector3.zero;

        // Vector3 dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 dir = new Vector3(CnInputManager.GetAxisRaw("Horizontal"), 0, CnInputManager.GetAxisRaw("Vertical")).normalized;
        //. 방향 반전
        if (Reverse)
            dir *= -1.0f;

        return dir;
    }

    /// <summary>
    /// 버튼 입력 여부
    /// </summary>
    public static bool GetButtonDown(string _buttonName)
    {
        if(Enable)
            return CnInputManager.GetButtonDown(_buttonName);
        return false;
    }
}
