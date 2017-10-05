using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///   공용으로 사용되는 함수 처리들 모음
/// </summary>
public class Utils {

    /// <summary>
    ///   가위 바위 보 판정
    /// </summary>
    /// <returns> _a 에 대한 결과 </returns>
    public static eRpsCompareResult CompareRps(eRps _a, eRps _b)
    {
        //. 바위 일 때
        if(_a == eRps.Rock)
        {
            switch(_b)
            {
                case eRps.Rock: return eRpsCompareResult.Draw;
                case eRps.Paper: return eRpsCompareResult.Lose;
                case eRps.Scissors: return eRpsCompareResult.Win;
            }
        }
        //. 보 일때
        else if(_a == eRps.Paper)
        {
            switch (_b)
            {
                case eRps.Rock: return eRpsCompareResult.Win;
                case eRps.Paper: return eRpsCompareResult.Draw;
                case eRps.Scissors: return eRpsCompareResult.Lose;
            }
        }
        //. 가위 일때
        else if(_a == eRps.Scissors)
        {
            switch (_b)
            {
                case eRps.Rock: return eRpsCompareResult.Lose;
                case eRps.Paper: return eRpsCompareResult.Win;
                case eRps.Scissors: return eRpsCompareResult.Draw;
            }
        }

        Log.PrintError(eLogFilter.Normal, "CompareRps >> couldn't compare rps");
        return eRpsCompareResult.Draw;
    }

    /// <summary>
    ///   랜덤으로 Rps를 반환해준다.
    /// </summary>
    public static eRps DecisionRps()
    {
        int randomValue = Random.Range((int)eRps.Rock, (int)eRps.Scissors + 1);
        return (eRps)randomValue;
    }
}
