﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Player 에 관한 이것저것 설정
/// </summary>
public class PlayerOption : MonoBehaviour {
    public float m_RotateLerpOffset;        ///< 회전시에 얼마나 빨리 보간할것인가?
    public float m_WalkVelocityOffset;      ///< 걷기 속도 Offset
    public float m_SneakVelocityOffset;     ///< 기어가기 속도 Offset
}
