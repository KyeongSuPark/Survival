using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Vector3 m_Offset;    ///< Offset
    public Transform m_Target;  ///< 타겟
    public float m_Speed;        ///< 카메라 이동 속도
    public bool m_bSmooth;      ///< 보간을 사용 할것인가?

    public Transform m_Player;  ///< 테스트 임시 기능

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(m_Target)
        {
            Vector3 newPos = transform.position;
            newPos.x = m_Target.position.x + m_Offset.x;
            newPos.y = m_Target.position.y + m_Offset.y;
            newPos.z = m_Target.position.z + m_Offset.z;

            if (m_bSmooth)
                transform.position = Vector3.Lerp(transform.position, newPos, m_Speed * Time.deltaTime);
            else
                transform.position = newPos;
        }

        //. 테스트 기능
        if (Input.GetMouseButtonDown(0) && m_Player)
        {
            Vector3 screenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            m_Player.transform.position = Camera.main.ScreenToWorldPoint(screenPos); 
        }
	}
}
