using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   Player 에 관한 이것저것 설정
/// </summary>
public class PlayerOption : MonoBehaviour {
    public float m_RotateLerpOffset;        ///< 회전시에 얼마나 빨리 보간할것인가?
    public float m_WalkAccelOffset;         ///< 걷기 가속도 Offset
    public float m_SneakAccelOffset;        ///< 기어가기 가속도 Offset
    public float m_RollAccelOffset;         ///< 구르기 가속도 Offset
    public AnimationCurve m_RollDecelCurve;///< 구르기 감쇠속도 Curve
}
