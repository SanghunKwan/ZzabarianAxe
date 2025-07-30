using UnityEngine;


public class EnemyNormal : EnemyBase
{
    [Header("캐릭터 Resource Link")]
    [SerializeField] BoxCollider[] _attackZone;

    protected override void InitDetails()
    {
        for (int i = 0; i < _attackZone.Length; i++)
        {
            _attackZone[i].GetComponent<CheckAttackRange>().InitSetRange(this);
        }
    }


    protected override void AllZoneDisable()
    {
        for (int i = 0; i < _attackZone.Length; i++)
        {
            _attackZone[i].enabled = false;
        }
    }
    void SetZoneEnable(int index)
    {
        if (index < 0 || index >= _attackZone.Length)
        {
            Debug.Log(index + "는 배열의 outofRange입니다.");
            return;
        }
        _attackZone[index].enabled = true;
    }
    void SetZoneDisable(int index)
    {
        if (index < 0 || index >= _attackZone.Length)
        {
            Debug.Log(index + "는 배열의 outofRange입니다.");
            return;
        }
        _attackZone[index].enabled = false;
    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 100, 50), "Idle"))
    //    {
    //        ExchangeAnimation(AniState.Idle);
    //    }
    //    if (GUI.Button(new Rect(110, 0, 100, 50), "Walk"))
    //    {
    //        ExchangeAnimation(AniState.Walk);
    //    }
    //    if (GUI.Button(new Rect(220, 0, 100, 50), "Run"))
    //    {
    //        ExchangeAnimation(AniState.Run);
    //    }
    //    if (GUI.Button(new Rect(0, 70, 100, 50), "Attack1"))
    //    {
    //        ExchangeAnimation(AniState.Attack);
    //        _aniController.SetTrigger("Attack1");
    //    }
    //    if (GUI.Button(new Rect(110, 70, 100, 50), "Attack2"))
    //    {
    //        ExchangeAnimation(AniState.Attack);
    //        _aniController.SetTrigger("Attack2");

    //        if (GUI.Button(new Rect(20, 70, 100, 50), "Hit"))
    //        {
    //            ExchangeAnimation(AniState.Idle);
    //            _aniController.SetTrigger("Hitting");
    //        }
    //        GUIStyle style = new GUIStyle();

    //        style.fontSize = 80;
    //        GUI.Box(new Rect(0, 160, 300, 100), _nowState.ToString(), style);
    //    }
    //}
}
