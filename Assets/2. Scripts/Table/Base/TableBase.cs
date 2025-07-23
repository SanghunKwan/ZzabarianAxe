using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class TableBase
{
    protected Dictionary<int, Dictionary<string, string>> _tableData = new Dictionary<int, Dictionary<string, string>>();


    public abstract void LoadJson(in string str);
    public abstract void LoadTxt(in string str);

    public int ToInt(int index, string key) => int.Parse(_tableData[index][key]);
    public string ToStr(int index, string key) => _tableData[index][key];
    public float ToFloat(int index, string key) => float.Parse(_tableData[index][key]);

}
