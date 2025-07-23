using DefineEnums;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    static TableManager _uniqueInstance;

    Dictionary<TableType, TableBase> _tables;


    public static TableManager _Instance => _uniqueInstance;
    public IReadOnlyDictionary<TableType, TableBase> Tables => _tables;



    private void Awake()
    {
        _uniqueInstance = this;
        _tables = new Dictionary<TableType, TableBase>();
    }

    void LoadTableJson<T>(TableType tableType) where T : TableBase, new()
    {
        TextAsset asset = Resources.Load<TextAsset>("Tables/" + tableType.ToString());

        T classIntance = new T();
        classIntance.LoadJson(asset.text);

        _tables.Add(tableType, classIntance);
    }
    void LoadTableTxt<T>(TableType tableType) where T : TableBase, new()
    {
        TextAsset asset = Resources.Load<TextAsset>("Tables/Txt/" + tableType.ToString());

        T classIntance = new T();
        classIntance.LoadTxt(asset.text);

        _tables.Add(tableType, classIntance);
    }
    public void AllLoadTable()
    {
        //LoadTableJson<MonsterTable>(TableType.MonsterTable);
        //LoadTableJson<LevelUpTable>(TableType.LevelUpTable);

        LoadTableTxt<MonsterTable>(TableType.MonsterTable);
        LoadTableTxt<LevelUpTable>(TableType.LevelUpTable);
    }

}
