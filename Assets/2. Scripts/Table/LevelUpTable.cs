using LitJson;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpTable : TableBase
{
    enum INDEX
    {
        Index,
        TargetXP,
        STR,
        INT,
        VIT,
        DEX,
        MEN,
        MovSpeedScale,
        AttSpeedScale,

        Max
    }

    public override void LoadJson(in string strData)
    {
        JsonReader reader = new JsonReader(strData);

        (INDEX, string) tempData;
        Dictionary<string, string> tempDic = new();
        while (reader.Read())
        {
            if (reader.Value == null) continue;

            if (Enum.TryParse(reader.Value.ToString(), out tempData.Item1))
            {
                while (reader.Read())
                {
                    if (reader.Value == null) continue;
                    tempData.Item2 = reader.Value.ToString();
                    tempDic.Add(tempData.Item1.ToString(), tempData.Item2);
                    break;
                }
            }

            if (tempDic.Count == (int)INDEX.Max)
            {
                _tableData.Add(int.Parse(tempDic[INDEX.Index.ToString()]), tempDic);
                tempDic = new();
            }
        }
    }

    public override void LoadTxt(in string str)
    {
        string[] record = str.Split("\n");
        for (int i = 0; i < record.Length; i++)
        {
            string[] values = record[i].Split("|");
            if (values.Length != (int)INDEX.Max)
            {
                Debug.LogErrorFormat("MonsterTable 컬럼 수가 맞지 않습니다 {0}", values.Length);
            }

            Dictionary<string, string> dic = new Dictionary<string, string>();
            int dicIndex = int.Parse(values[0]);
            for (int j = 0; j < values.Length; j++)
            {
                dic.Add(((INDEX)j).ToString(), values[j]);
            }
            _tableData.Add(dicIndex, dic);
        }
    }

}
