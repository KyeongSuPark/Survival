using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorPlayModeEvent
{
    static EditorPlayModeEvent()
    {
        Debug.Log("EditorPlayModeEvent created");
        EditorPlayMode.PlayModeChanged += OnChangedPlayMode;
    }

    private static void OnChangedPlayMode(PlayModeState _curPlayMode, PlayModeState _newPlayMode)
    {
        if(_newPlayMode == PlayModeState.Stopped)
        {
            Debug.Log("stopped");
            if (DevelopOptions.Save())
                Debug.Log("develop options saved");
        }
        else if(_newPlayMode == PlayModeState.Playing)
        {
            Debug.Log("playing");
        }
        else if(_newPlayMode == PlayModeState.Paused)
        {
            Debug.Log("paused");
        }
    }
}

