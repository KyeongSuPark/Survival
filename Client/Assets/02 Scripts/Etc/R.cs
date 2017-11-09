using UnityEngine;
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
///   가위 바위 보 대결 결과
/// </summary>
public enum eRpsCompareResult
{
    Win,        ///< 승리
    Lose,       ///< 패배
    Draw,       ///< 무승부
}

/// <summary>
///   국가
/// </summary>
public enum eCountry
{
    Korea,
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
        public static int NORMAL_DAMAGE = 1;    ///< 일반 공격 Damage
        public static int SPECIAL_DAMAGE = 2;   ///< Special 공격 Damage
        public static int SHUFFLE_FREQUENCY = 60;///< Shuffle 주기 (주어진 시간동안 Shuffle되는 횟수)

        public static float RESET_TIME = 3.0f;   ///< 가위 바위 보 졌을 때 reset 시간 (s)
        public static float RESET_CYCLE = 30.0f; ///< 주기적으로 reset 시간 (s)
    }

    /// <summary>
    ///   Anim Layer 상수 Index들
    /// </summary>
    public class AnimLayer
    {
        public static int BASE = 0;         ///< Base
    }

    public class String
    {
        /// AnimTrigger 
        public static string ANIM_TRIGGER_RUN = "Run";           ///< Run

        /// Input 
        public static string INPUT_RUN = "Run";                 ///< Run
        public static string INPUT_WALK = "Walk";               ///< Walk
        public static string INPUT_SNEAK = "Sneak";             ///< Sneak
        public static string INPUT_JUMP = "Jump";               ///< Jump (Roll)

        /// Tag       

        /// Scene
        public static string SCENE_LOGIN = "Login";             ///< 로그인
        public static string SCENE_LOBBY = "Lobby";             ///< 로비
        public static string SCENE_GAME = "Game";               ///< Lv

        /// Anim State
        public static string ANIM_STATE_ROLL = "Roll";          ///< 구르기 상태
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

        /// 국가 아이콘 패스
        public static string COUNTRY_ICON_FOLDER = @"Sprites/CountryIcons/";
    }

    public class AnimHash
    {
        public static int STATE = Animator.StringToHash("State");
        public static int MOVE_STATE = Animator.StringToHash("MoveState");
        public static int ROLL = Animator.StringToHash("Roll");
        public static int DIE = Animator.StringToHash("Die");
    }
}