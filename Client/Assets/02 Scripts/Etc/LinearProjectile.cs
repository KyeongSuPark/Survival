using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearProjectile : MonoBehaviour {
    [SerializeField]
    private float m_Velocity;       ///< 속도
	
	// Update is called once per frame
	void Update () {
        //. 쭈욱 직진 ( 생성시에 방향이 결정된다.)
        transform.position += transform.forward * m_Velocity * Time.deltaTime;
	}
}
