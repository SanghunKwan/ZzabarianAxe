using UnityEngine;

public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;




    public static IngameManager _instance => _uniqueInstance;

    [SerializeField] UIManager _uiManager;



    private void Awake()
    {
        _uniqueInstance = this;

        //юс╫ц
        TableManager._Instance.AllLoadTable();
        //===
    }

    public void CharacterHpChanged(float currentHpRate)
    {
        _uiManager.SetTargetRate(currentHpRate);
    }
}
