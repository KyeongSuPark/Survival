using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 한 타입의 테이블 데이터를 담고 있는 컨테이너
/// </summary>
public class TableDataContainer{
    private eTableType m_eType;
    private Dictionary<int, TblBase> m_Datas;  

    public Dictionary<int, TblBase> Datas {get { return m_Datas; } }

    public TableDataContainer(eTableType _type)
    {
        m_Datas = new Dictionary<int, TblBase>();
        m_eType = _type;
    }

    /// <summary>
    /// textAsset으로 부터 load 하여 캐쉬해둔다.
    /// </summary>
    public void Load()
    {
        //. 문자열 받아 오고
        TextAsset asset = GetTextAsset(m_eType);
        if (asset == null)
            return;

        //. 파싱해서 데이터 구성한다.
        using (StringReader reader = new StringReader(asset.text))
        {
            string scheme = reader.ReadLine();
            if (scheme == null)
                return;

            string [] headers = scheme.Split(',');
            if (headers == null)
                return;

            string line;
            while(true)
	        {
                //. value 부분 읽어 온다.
	            line = reader.ReadLine();
                if (line == null)
                    break;

                string[] values = line.Split(',');

                //. header와 같지 않다면 데이터 문제
                if (values.Length != headers.Length)
                    continue;

                //. Lookup table 구성 - key : header, value : value
                Dictionary<string, string> lookUp = new Dictionary<string, string>();
                for(int i=0; i < headers.Length; ++i)
                    lookUp.Add(headers[i], values[i]);

                //. table row 하나 생성
                TblBase row = Create(m_eType);
                if (row == null)
                    continue;

                //. looup을 참조하여 field 에 값을 채운다.
                Type t = row.GetType();
                FillFields(row, t, ref lookUp);

                //. 캐쉬
                m_Datas.Add(row.Id, row);
            }
        }
    }

    public TblBase Find(int _id)
    {
        if (m_Datas.ContainsKey(_id))
            return m_Datas[_id];
        return null;
    }

    /// <summary>
    /// row 의 field를 type으로 부터 유추하여 
    /// lookup 에 있는 값으로 채워준다.
    /// </summary>
    /// <param name="_row">테이블 row</param>
    /// <param name="_type">실제 인스턴스 클래스 타입</param>
    /// <param name="_lookUp">값이 들어있는 looup 테이블</param>
    private void FillFields(TblBase _row, Type _type, ref Dictionary<string, string> _lookUp)
    {
        FieldInfo[] fields = _type.GetFields();
        foreach (var field in fields)
        {
            if (_lookUp.ContainsKey(field.Name) == false)
            {
                Log.PrintError(eLogFilter.Table, string.Format("field not in lookup type:{0} field:{1}", _type.ToString(), field.Name));
                continue;
            }

            string csvValue = _lookUp[field.Name]; //. csv 값
            if (csvValue == string.Empty)
                continue;

            object variable = field.GetValue(_row); //. 변수
            if (variable is int)
            {
                field.SetValue(_row, int.Parse(csvValue));
            }
            else if (variable is string)
            {
                field.SetValue(_row, csvValue);
            }
            else if (variable is float)
            {
                field.SetValue(_row, float.Parse(csvValue));
            }
            else if (variable is Enum)
            {
                foreach (var enumValue in Enum.GetValues(variable.GetType()))
                {
                    if (csvValue.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        field.SetValue(_row, enumValue);
                        break;
                    }
                }
            }
        }
    }

    private TextAsset GetTextAsset(eTableType _type)
    {
        string path = string.Format("{0}{1}", R.Path.TABLE_DATA_FOLDER, _type.ToString());
        return Resources.Load<TextAsset>(path);
    }

    private TblBase Create(eTableType _type)
    {
        switch (_type)
        {
            case eTableType.Item: return new TblItem();
            case eTableType.ItemEffect: return new TblItemEffect();
        }
        return null;
    }
}
