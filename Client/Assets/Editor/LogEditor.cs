using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

/// <summary>
/// Log 커스텀 에디터 
/// Runtime에서 LogFilter Check 동적으로 하기 위해 
/// </summary>
[CustomEditor(typeof(Log))]
public class LogEditor : Editor
{
    string[] mEnumNames = Enum.GetNames(typeof(eLogFilter));

    /// <summary>
    /// Inspector GUI 변경
    /// Enum값 대로 체크박스 노출되도록 수정
    /// </summary>
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (Log.Instance == null)
            return;

        // 체크 박스 생성.
        foreach(eLogFilter value in Enum.GetValues(typeof(eLogFilter)))
        {
            int i = (int)value;
            if (i < 0 || i >= Log.Instance.Filters.Length)
                break;

            Log.Instance.Filters[i] = EditorGUILayout.Toggle(mEnumNames[i], Log.Instance.Filters[i]);            
        }

        //. Clear Button
        if(GUILayout.Button("Clear All"))
            ClearAll();

        //. Check Button
        if (GUILayout.Button("Check All"))
            CheckAll();
            
        // GUI 변경 되었으면 타겟을 다시 렌더링하도록 dirty 마크.
        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    /// <summary>
    ///   로그 설정을 Clear 한다.
    /// </summary>
    private void ClearAll()
    {
        if (Log.Instance == null)
            return;

        for (int i = 0; i < Log.Instance.Filters.Length; ++i )
            Log.Instance.Filters[i] = false;
    }

    /// <summary>
    ///   로그 설정을 선부 Check한다.
    /// </summary>
    private void CheckAll()
    {
        if (Log.Instance == null)
            return;

        for (int i = 0; i < Log.Instance.Filters.Length; ++i)
            Log.Instance.Filters[i] = true;
    }
}
