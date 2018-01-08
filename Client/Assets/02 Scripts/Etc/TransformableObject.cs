using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 변신 가능한 객체임을 알리는 스크립트
/// 이 대상은 삭제 되지 않는 배경 오브젝트여야 한다.
/// </summary>
public class TransformableObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ResourceManager.AddTransformableObject(gameObject);	
	}

    void OnDestroy()
    {
        Log.PrintError(eLogFilter.Normal, "TransformableObject.OnDestroy() >> invalid usage");
    }
}
