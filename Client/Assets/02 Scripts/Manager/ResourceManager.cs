using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance = null;

    [SerializeField]
    private SpriteAtlas m_ItemAtlas;

    private Animator m_TransformedPlayerAnimator;            ///< 변신 했을 때 사용될 플레이어 애니메이터

    private Dictionary<string, Sprite> m_ItemIcons;          ///< 아이템 아이콘 < key:IconName >
    private Dictionary<eCountry, Sprite> m_CountryIcons;     ///< 국가 아이콘
    private Dictionary<string, GameObject> m_ItemPrefabs;    ///< 아이템 프리팹 < key:PrefabName >
    private List<GameObject> m_TransformableObjects;         ///< 변신 가능한 목록들

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_CountryIcons = new Dictionary<eCountry, Sprite>();
        m_ItemPrefabs = new Dictionary<string, GameObject>();
        m_ItemIcons = new Dictionary<string, Sprite>();
        m_TransformableObjects = new List<GameObject>();
    }

    /// <summary>
    ///   국가 아이콘 검색 후 없으면 추가
    ///   네이밍 규칙 : Sprites/CountryIcons/CI_[국가명]
    /// </summary>
    public Sprite FindOrLoadCountryIcon(eCountry _country)
    {
        if (m_CountryIcons.ContainsKey(_country))
            return m_CountryIcons[_country];

        string path = string.Format("{0}CI_{1}", R.Path.COUNTRY_ICON_FOLDER, _country.ToString());
        Sprite icon = Resources.Load<Sprite>(path);
        if (icon == null)
            return null;

        m_CountryIcons.Add(_country, icon);
        return icon;
    }

    /// <summary>
    /// 아이템 아이콘 이름으로 검색
    /// </summary>
    public Sprite FindItemIcon(string _iconName)
    {
        //. Cache된게 있다면 그걸로 사용
        if (m_ItemIcons.ContainsKey(_iconName))
            return m_ItemIcons[_iconName];

        //. 없으면 Sprite에서 가져온다.        
        Sprite icon = m_ItemAtlas.GetSprite(_iconName);
        if (icon == null)
            return null;

        //. 잘 가져왔으면 캐쉬해둔다.
        m_ItemIcons.Add(_iconName, icon);
        return icon;
    }

    /// <summary>
    /// 아이템 프리팹 검색 (없으면 로드한 후 캐쉬한다.)
    /// </summary>
    public GameObject FindOrLoadItemPrefab(string _prefabName)
    {
        if (m_ItemPrefabs.ContainsKey(_prefabName))
            return m_ItemPrefabs[_prefabName];

        string path = string.Format("{0}{1}", R.Path.ITEM_PREFAB_FOLDER, _prefabName);
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
            return null;

        m_ItemPrefabs.Add(_prefabName, prefab);
        return prefab;
    }

    /// <summary>
    /// 변신 가능한 목록에 추가한다.
    /// </summary>
    public static void AddTransformableObject(GameObject _object)
    {
        Instance.m_TransformableObjects.Add(_object);
    }

    /// <summary>
    /// 변신 가능한 목록 중에 랜덤으로 하나 선택한다.
    /// </summary>
    public static GameObject PickTransformableObject()
    {
        int randomIdx = Random.Range(0, Instance.m_TransformableObjects.Count);
        return Instance.m_TransformableObjects[randomIdx];
    }
}
