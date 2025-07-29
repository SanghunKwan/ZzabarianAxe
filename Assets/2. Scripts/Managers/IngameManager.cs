using UnityEngine;

public class IngameManager : MonoBehaviour
{
    static IngameManager _uniqueInstance;




    public static IngameManager _instance => _uniqueInstance;

    [SerializeField] int _targetKillCount;
    ArrivePosition _arrivePosition;

    public int _killCount { get; private set; }





    [SerializeField] UIManager _uiManager;



    private void Awake()
    {
        _uniqueInstance = this;

        //임시
        TableManager._Instance.AllLoadTable();
        //===

    }
    private void Start()
    {
        _arrivePosition = GameObject.FindGameObjectWithTag("ArrivePosition").GetComponent<ArrivePosition>();
    }

    public void InitCharacters(in string tempName)
    {
        _uiManager.InitUI(tempName);
        CharacterHpChanged(1);
    }

    public void CharacterHpChanged(float currentHpRate)
    {
        _uiManager.SetTargetRate(currentHpRate);
    }


    public void MonsterDead()
    {
        _killCount++;
        _uiManager.SetKillCountText(_killCount);

        if (_killCount >= _targetKillCount)
        {
            //목적지 활성화.
            _arrivePosition.ActivateArrivePosition();
        }
    }
}
