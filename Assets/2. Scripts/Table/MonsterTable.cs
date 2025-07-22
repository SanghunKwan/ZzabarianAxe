using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTable : TableBase
{

    Dictionary<int, Dictionary<INDEX, string>> _data = new();

    enum INDEX
    {
        Index,
        Name,
        Level,
        STR,
        INT,
        VIT,
        DEX,
        MEN,
        PrefabName,
        XP,
        RewardIndex,

        Max
    }

    public override void Load(in string strData)
    {
        JsonReader reader = new JsonReader(strData);

        (INDEX, string) tempData;
        Dictionary<INDEX, string> tempDic = new();
        while (reader.Read())
        {
            if (reader.Value == null) continue;

            if (Enum.TryParse(reader.Value.ToString(), out tempData.Item1))
            {
                while (reader.Read())
                {
                    if (reader.Value == null) continue;
                    tempData.Item2 = reader.Value.ToString();
                    tempDic.Add(tempData.Item1, tempData.Item2);
                    break;
                }
            }

            if (tempDic.Count == (int)INDEX.Max)
            {
                _data.Add(int.Parse(tempDic[INDEX.Index]), tempDic);
                tempDic = new Dictionary<INDEX, string>();
            }
        }

        foreach (var data in _data.Values)
        {
            foreach (var item in data)
            {
                Debug.Log(item.Key + " : " + item.Value);
            }
        }

        //foreach (var item in _data)
        //{
        //    Debug.Log(item.Key.ToString() + "  " + item.Value);
        //}



        //string[] record = fileName.Split("|");

        //for (int i = 0; i < record.Length; i++)
        //{
        //    string[] values = record[i].Split("|");
        //    if (values.Length != (int)INDEX.Max)
        //        Debug.LogErrorFormat("ChapterInfoList 컬럼 수가 맞지 않습니다 {0}", values.Length);

        //    for (int j = 0; j < values.Length; j++)
        //    {
        //        Add(values[0], ((Index)j).ToString(), values[j]);
        //    }
        //}
    }

}
