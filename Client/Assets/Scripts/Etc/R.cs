using UnityEngine;
using System.Collections;

/// <summary>
/// Player 상태 값
/// </summary>
public enum ePlayerState
{
    None,           ///< 아무 상태도 아님
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
    }
}