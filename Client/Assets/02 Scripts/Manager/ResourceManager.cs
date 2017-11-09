using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    public static ResourceManager Instance = null;

    private Dictionary<eCountry, Sprite> m_CountryIcons;     ///< 국가 아이콘

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Log has two instances");

        m_CountryIcons = new Dictionary<eCountry, Sprite>();
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
}
