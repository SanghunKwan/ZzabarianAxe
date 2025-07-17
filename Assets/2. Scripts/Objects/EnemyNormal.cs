using DefineEnums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyNormal : CharacterBase
{
    const float _distanceOffset = 0.1f;
    const float _maxWaitTime = 15;
    const float _minWaitTime = 3;

    //스탯
    float _attackDistance = 1.8f;
    EnemyPersonality _myPersonality;
    RoamType _myRoamType;

    //참조 변수
    NavMeshAgent _navAgent;

    //정보 변수
    AniState _nowState;
    public List<Vector3> _roamPointList;
    int _nowRoamIndex;
    bool _isSelected;
    float _nowWaitTime;


    Action _destroyAction;


    public void InitCharacter(RoamType roam, in Action onDestoryAction, List<Transform> posTF, int index)
    {
        _navAgent = GetComponent<NavMeshAgent>();
        InitSetBase(1.4f, 1.4f, 4.4f, 4.4f);
        _destroyAction = onDestoryAction;

        _myRoamType = roam;
        _myPersonality = (EnemyPersonality)UnityEngine.Random.Range(0, (int)EnemyPersonality.Max);

        _roamPointList = new List<Vector3>(posTF.Count);
        foreach (var item in posTF)
        {
            _roamPointList.Add(item.position);
        }
        SelectDefaultAutomaticAction();
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
                _navAgent.stoppingDistance = _attackDistance - _distanceOffset;
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

    void SetGoalLocation(Vector3 location, AniState state = AniState.Walk)
    {
        _navAgent.SetDestination(location);
        ExchangeAnimation(state);
    }
    void SelectDefaultAutomaticAction()
    {
        if (_isSelected) return;

        bool selectiveVariable = (UnityEngine.Random.Range(0, 2) == 0) ? true : false;
        if (selectiveVariable)
        {
            _nowWaitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
            ExchangeAnimation(AniState.Idle);
            Debug.LogFormat("{0}:{1:F}sec", _nowState, _nowWaitTime);
        }
        else
        {
            _nowRoamIndex = UnityEngine.Random.Range(0, _roamPointList.Count);
            SetGoalLocation(_roamPointList[_nowRoamIndex]);
            Debug.LogFormat("{0}:[{1}]{2}", _nowState, _nowRoamIndex, _roamPointList[_nowRoamIndex]);
        }
        _isSelected = true;
    }

    public void SetIdle()
    {
        if (_nowState >= AniState.Attack)
            _nowState = AniState.Idle;
    }

    private void Update()
    {
        switch (_nowState)
        {
            case AniState.Idle:
                _nowWaitTime -= Time.deltaTime;
                if (_nowWaitTime <= 0)
                    _isSelected = false;
                break;
            case AniState.Walk:
                if (_navAgent.remainingDistance < _navAgent.stoppingDistance + _walkSpeed * Time.deltaTime + _distanceOffset)
                {
                    _isSelected = false;
                }
                break;
        }

        SelectDefaultAutomaticAction();
    }
    private void OnDestroy()
    {
        _destroyAction?.Invoke();
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
