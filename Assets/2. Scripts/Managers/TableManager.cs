using DefineEnums;
using System.Collections.Generic;
using UnityEngine;

public class TableManager : MonoBehaviour
{
    static TableManager _uniqueInstance;

    Dictionary<TableType, string> _tables;


    public static TableManager Instance => _uniqueInstance;
    public IReadOnlyDictionary<TableType, string> Tables => _tables;



    private void Awake()
    {
        _uniqueInstance = this;
        _tables = new Dictionary<TableType, string>();
        LoadTable<MonsterTable>(TableType.MonsterTable);
    }

    void LoadTable<T>(TableType tableType) where T : TableBase, new()
    {
        TextAsset asset = Resources.Load<TextAsset>("Tables/" + tableType.ToString());

        _tables.Add(tableType, asset.text);
        T classIntance = new T();

        classIntance.Load(asset.text);
    }
}
