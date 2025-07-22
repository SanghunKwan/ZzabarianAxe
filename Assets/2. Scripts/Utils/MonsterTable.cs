using UnityEngine;

public class MonsterTable
{
    public enum INDEX
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

    //public void Load(string txtData)
    //{
    //    string[] record = txtData.Split("\r\n");

    //    for (int i = 0; i < record.Length; i++)
    //    {
    //        string[] values = record[i].Split("|");
    //        if (values.Length != (int)INDEX.Max)
    //            Debug.LogErrorFormat("ChapterInfoList 컬럼 수가 맞지 않습니다 {0}", values.Length);

    //        for (int j = 0; j < values.Length; j++)
    //        {
    //            Add(values[0], ((Index)j).ToString(), values[j]);
    //        }
    //    }
    //}

}
