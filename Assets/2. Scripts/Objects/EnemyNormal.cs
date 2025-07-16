using DefineEnums;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNormal : CharacterBase
{
    //스탯
    float _attackDistance = 1.8f;

    //참조 변수
    NavMeshAgent _navAgent;

    //정보 변수
    AniState _nowState;

    private void Awake()
    {
        InitCharacter();
    }

    public void InitCharacter()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        InitSetBase(1.4f, 1.4f, 4.4f, 4.4f);
    }

    public override void ExchangeAnimation(AniState state)
    {
        switch (state)
        {
            case AniState.Walk:
                //_aniController.speed = _walkSpeed * 2;
                _navAgent.speed = _walkSpeed;
                break;
            case AniState.Run:
                //_aniController.speed = _runSpeed * 2;
                _navAgent.speed = _runSpeed;
                _navAgent.stoppingDistance = _attackDistance;
                break;
            case AniState.Attack:
                break;
            case AniState.Dead:
                _aniController.SetTrigger("Death");
                break;

        }

        _nowState = state;

        _aniController.SetInteger("AniState", (int)state);
    }
    public void OnAttackStart()
    {
    }
    public void OnAttackEnd()
    {
        ExchangeAnimation(AniState.Idle);
    }

    void SetGoalLocation(Vector3 location)
    {
        _navAgent.SetDestination(location);
        ExchangeAnimation(AniState.Run);
    }
    public void SetIdle()
    {
        _nowState = AniState.Idle;
    }

    private void Update()
    {
        switch (_nowState)
        {
            case AniState.Walk:
            case AniState.Run:
                if (Vector3.Distance(_navAgent.destination, transform.position) < 0.1f)
                {

                    ExchangeAnimation(AniState.Idle);
                }
                break;

        }
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
