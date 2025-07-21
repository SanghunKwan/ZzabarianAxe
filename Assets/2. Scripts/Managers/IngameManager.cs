using UnityEngine;

public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;




    public static IngameManager _instance => _uniqueInstance;




    private void Awake()
    {
        _uniqueInstance = this;
    }
}
