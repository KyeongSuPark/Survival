﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Player 상태 값
/// 순서 변경시 Animator Transition 설정값 주의하자
/// </summary>
public enum ePlayerState
{
    Idle,           ///< 대기 상태
    Move,           ///< 이동 상태
    Roll,           ///< 구르기 상태
    Abnormal,       ///< 상태 이상
}

/// <summary>
///   이동 상태 값
/// </summary>
public enum eMoveState
{
    Run,            ///< 뛰다
    Walk,           ///< 걷는다
    Sneak,          ///< 살금살금 걷는다.
}

/// <summary>
/// Animation Event 코드값
/// </summary>
public enum eAnimationEvent
{
    AnimationEnd,   ///< 애니메이션 종료
}

/// <summary>
///  Sns 타입
/// </summary>
public enum eSocialPlatform
{
    Facebook,       ///< 페이스북
}

/// <summary>
///  Hidden 이 되는 이유
/// </summary>
public enum eHiddenReason
{
    Bush,       ///< 부쉬에 들어갔다.
    Item,       ///< 아이템 사용
}

/// <summary>
///   가위 바위 보 
///     Rock Paper Scissors
/// </summary>
public enum eRps
{
    Rock,       ///< 바위
    Paper,      ///< 보
    Scissors,   ///< 가위
}

/// <summary>
/// 전역 상수값들 선언
/// 내부 클래스는 카테고리처럼 사용
/// </summary>
public class R
{
    /// <summary>
    /// 일반 상수 값들
    /// </summary>
    public class Const
    {
        public static int INDEX_NONE = -1;

    }

    public class String
    {
        /// AnimTrigger 
        public static string ANIM_TRIGGER_RUN = "Run";           ///< Run

        /// Input 

        /// Tag       

        /// Scene
        public static string SCENE_LOGIN = "Login";             ///< 로그인
        public static string SCENE_LOBBY = "Lobby";             ///< 로비
        public static string SCENE_GAME = "Game";               ///< Lv
    }

    /// <summary>
    ///   데이터 Path들
    /// </summary>
    public class Path
    {
        /// Path
        public static string EDITOR_DATA = Application.dataPath + @"/Editor/Data/";

        /// Toon Lit Ramp Texture
        public static string RESOURCE_TOON_LIT_RAMP_TEX = @"Textures/ToonLitRamp";
    }

    public class AnimHash
    {
        public static int STATE = Animator.StringToHash("State");
        public static int MOVE_STATE = Animator.StringToHash("MoveState");
    }
}