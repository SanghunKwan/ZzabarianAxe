using DefineEnums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyNormal : CharacterBase
{
    //Debug용
    const float _speedScale = 5;
    //==
    const float _distanceOffset = 0.1f;
    const float _maxWaitTime = 15;
    const float _minWaitTime = 3;

    [Header("캐릭터 Resource Link")]
    [SerializeField] BoxCollider[] _attackZone;
    [SerializeField] SensingArea _sensingArea;

    //스탯
    float _attackDistance = 1.8f;
    float _sightRange = 8;
    float _attackDelayTime = 1;
    float _followDistance = 50;
    EnemyPersonality _myPersonality;
    RoamType _myRoamType;

    //참조 변수
    NavMeshAgent _navAgent;
    Transform _targetCharacter;

    //정보 변수
    AniState _nowState;
    public List<Vector3> _roamPointList;
    int _nowRoamIndex;
    int _personalityRate;
    bool _isSelected;
    bool _isBack;
    bool _isBattle;
    float _nowWaitTime;
    float _attackWaitTime;

    Vector3 startPos;

    Action _destroyAction;


    public void InitCharacter(in string name, RoamType roam, in Action onDestoryAction, List<Transform> posTF, int index)
    {
        _navAgent = GetComponent<NavMeshAgent>();
        InitSetBase(name, 1.4f, 1.4f, 4.4f, 4.4f);
        _destroyAction = onDestoryAction;

        _sensingArea.InitSet(this, _sightRange);
        _myPersonality = (EnemyPersonality)UnityEngine.Random.Range(0, (int)EnemyPersonality.Max);
        //_myPersonality = EnemyPersonality.Impatient;
        Debug.Log(_myPersonality);
        _personalityRate = GameDefaultValue._personality[(int)_myPersonality];
        _nowRoamIndex = index;
        _myRoamType = roam;

        _roamPointList = new List<Vector3>(posTF.Count);
        foreach (var item in posTF)
        {
            _roamPointList.Add(item.position);
        }
        AllZoneDisable();

        _isSelected = true;

        SelectDefaultAutomaticAction();

        startPos = transform.position;
    }

    public override void ExchangeAnimation(AniState state)
    {
        switch (state)
        {
            case AniState.Walk:
#if UNITY_EDITOR
                //_aniController.speed = _walkSpeed * 2;
                _navAgent.speed = _walkSpeed * _speedScale;
                _aniController.speed = _speedScale;
#else
                _navAgent.speed = _walkSpeed;
#endif
                break;
            case AniState.Run:
                //_aniController.speed = _runSpeed * 2;
                _navAgent.speed = _runSpeed;
                _navAgent.stoppingDistance = _attackDistance - _distanceOffset;
                break;
            case AniState.Attack:
                if (UnityEngine.Random.Range(0, 2) == 0)
                    _aniController.SetTrigger("Attack1");
                else
                    _aniController.SetTrigger("Attack2");
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

    public override void CheckedOpponent(GameObject hostileObject)
    {
        base.CheckedOpponent(hostileObject);
        _targetCharacter = hostileObject.transform;
        _isBattle = true;
        _sensingArea.ColliderOnoff(false);

        if (Vector3.Distance(_targetCharacter.position, transform.position) > _attackDistance)
            SetGoalLocation(_targetCharacter.position, AniState.Run);
        else
            ExchangeAnimation(AniState.Attack);
    }

    void AllZoneDisable()
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
    void SetGoalLocation(Vector3 location, AniState state = AniState.Walk)
    {
        _navAgent.SetDestination(location);
        ExchangeAnimation(state);
    }
    void SelectDefaultAutomaticAction()
    {
        if (_isSelected) return;

        bool selectiveVariable = UnityEngine.Random.Range(0, 100) >= _personalityRate;
        //bool selectiveVariable = false;
        if (selectiveVariable)
        {
            _nowWaitTime = UnityEngine.Random.Range(_minWaitTime, _maxWaitTime);
            ExchangeAnimation(AniState.Idle);
            Debug.LogFormat("{0}:{1:F}sec", _nowState, _nowWaitTime);
        }
        else
        {
            SelectPointByRoam();
        }

        _isSelected = true;
    }
    void SelectPointByRoam()
    {
        int index;
        switch (_myRoamType)
        {
            case RoamType.Random:
                _nowRoamIndex = UnityEngine.Random.Range(0, _roamPointList.Count);
                index = _nowRoamIndex;
                break;
            case RoamType.Inorder:
                _nowRoamIndex = (_nowRoamIndex + 1) % _roamPointList.Count;
                index = _nowRoamIndex;
                break;
            case RoamType.TwoPoint:
                _nowRoamIndex = (_nowRoamIndex + 2) % _roamPointList.Count;
                index = _nowRoamIndex;
                break;
            case RoamType.BackNForth:
                if (_isBack)
                {
                    _nowRoamIndex--;
                    if (_nowRoamIndex < 0)
                    {
                        _isBack = false;
                        _nowRoamIndex = 1;
                    }
                }
                else
                {
                    _nowRoamIndex++;
                    if (_nowRoamIndex == _roamPointList.Count)
                    {
                        _isBack = true;
                        _nowRoamIndex = _roamPointList.Count - 2;
                    }
                }


                //int lastIndex = _roamPointList.Count - 1;
                //_nowRoamIndex = (_nowRoamIndex + 1) % (lastIndex * 2);
                //index = (_nowRoamIndex < _roamPointList.Count) ? _nowRoamIndex :
                //                                                 (_roamPointList.Count - 1) * 2 - _nowRoamIndex;
                //index = (int)Mathf.PingPong(_nowRoamIndex, lastIndex);
                break;
            default:
                Debug.Log("RoamType 값이 올바르지 않습니다.");
                index = 0;
                break;
        }
        SetGoalLocation(_roamPointList[_nowRoamIndex]);
        Debug.LogFormat("{0}:[{1}]{2}", _nowState, _nowRoamIndex, _roamPointList[_nowRoamIndex]);
    }
    public void SetIdle()
    {
        if (_nowState >= AniState.Attack)
            _nowState = AniState.Idle;
    }

    private void Update()
    {
        Debug.Log(_nowState);
        switch (_nowState)
        {
            case AniState.Idle:
                if (_isBattle)
                {
                    SetGoalLocation(_targetCharacter.position, AniState.Run);

                    if (_attackWaitTime < _attackDelayTime)
                    {
                        _attackWaitTime += Time.deltaTime;
                    }
                    else
                    {
                        ExchangeAnimation(AniState.Attack);
                        _attackWaitTime = 0;
                    }
                }
                else
                {
                    _nowWaitTime -= Time.deltaTime;
                    if (_nowWaitTime <= 0)
                        _isSelected = false;
                }
                break;
            case AniState.Walk:
                if (_navAgent.remainingDistance < _navAgent.stoppingDistance + _walkSpeed * Time.deltaTime + _distanceOffset)
                {
                    _isSelected = false;
                }
                break;
            case AniState.Run:
                if (_attackWaitTime < _attackDelayTime)
                {
                    _attackWaitTime += Time.deltaTime;
                }

                if (Vector3.Distance(transform.position, startPos) > _followDistance)
                {
                    _isBattle = false;
                    SetGoalLocation(_roamPointList[_nowRoamIndex]);
                    break;
                }

                if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + _runSpeed * Time.deltaTime + _distanceOffset + _attackDistance)
                {
                    if (_attackWaitTime >= _attackDelayTime)
                    {
                        ExchangeAnimation(AniState.Attack);
                        _attackWaitTime = 0;
                    }
                }
                else
                {
                    SetGoalLocation(_targetCharacter.position, AniState.Run);
                }
                break;
            case AniState.Attack:
                if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + _runSpeed * Time.deltaTime + _distanceOffset + _attackDistance)
                //플레이어 쪽 방향을 보고 있어야 한다.
                {
                    transform.LookAt(_targetCharacter);
                }
                else
                {
                    SetGoalLocation(_targetCharacter.position, AniState.Run);
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
