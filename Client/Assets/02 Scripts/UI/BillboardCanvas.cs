using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour {
    public Transform m_Parent;      ///< 자신의 parent Transform
    private Transform m_MainCam;    ///< Main Camera Transform  

	// Use this for initialization
	void Start () {
        m_MainCam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        m_Parent.LookAt(m_Parent.position + m_MainCam.rotation * Vector3.forward);
	}
}
