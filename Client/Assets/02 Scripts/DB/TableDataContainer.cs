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

    public TableDataContainer(eTableType _type)
    {
        m_Datas = new Dictionary<int, TblBase>();
        m_eType = _type;
    }

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
            do
	        {
                //. value 부분 읽어 온다.
	            line = reader.ReadLine();
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

                //. field 에 값을 채운다.
                Type t = row.GetType();
                FieldInfo[] fields = t.GetFields();
                foreach(var field in fields)
                {
                    if(lookUp.ContainsKey(field.Name) == false)
                        continue;
                    
                    string csvValue = lookUp[field.Name]; //. csv 값
                    if (csvValue == string.Empty)
                        continue;

                    object variable = field.GetValue(row); //. 변수
                    if(variable is int)
                    {
                        field.SetValue(row, int.Parse(csvValue));
                    }
                    else if(variable is string)
                    {
                        field.SetValue(row, csvValue);
                    }
                    else if(variable is float)
                    {
                        field.SetValue(row, float.Parse(csvValue));
                    }
                    else if(variable is Enum)
                    {
                        foreach(var enumValue in Enum.GetValues(variable.GetType()))
                        {
                            if(csvValue.Equals(enumValue.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                field.SetValue(row, enumValue);
                                break;
                            }
                        }
                    }
                }

                //. 캐쉬
                m_Datas.Add(row.Id, row);
            } while (line != null); 
            
        }
    }

    public TblBase Find(int _id)
    {
        if (m_Datas.ContainsKey(_id))
            return m_Datas[_id];
        return null;
    }

    private TextAsset GetTextAsset(eTableType _type)
    {
        string path = string.Format("{0}{1}", R.Path.TABLE_DATA_FOLDER, _type.ToString());
        var testResult = Resources.Load(path);

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
