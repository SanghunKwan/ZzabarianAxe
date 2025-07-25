using DefineEnums;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


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
    [SerializeField] SimpleStatusWnd _wnd;

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
    int _hp, _nowHp;
    int _nowRoamIndex;
    int _personalityRate;
    bool _isSelected;
    bool _isBack;
    bool _isBattle;
    float _nowWaitTime;
    float _attackWaitTime;

    Vector3 startPos;

    Action _destroyAction;

    public float _hpRate => ((float)_nowHp) / _hp;
    public float _attackTimeRate => (_nowWaitTime / _attackDelayTime);
    public int _finalAttPow
    {
        get
        {
            if (_attMethod == MethodAttack.Physics)
                return (int)(_str + _vit * 0.5f);
            else
                return (int)(_int * 1.5f + _men * 0.5f);
        }
    }
    public override int GetFinalDefPow(MethodAttack ma)
    {
        if (ma == MethodAttack.Physics)
            return (int)(_vit + _str * 0.4f);
        else
            return (int)(_men + _int * 0.7f);
    }

    public void InitCharacter(RoamType roam, in Action onDestoryAction, List<Transform> posTF, int roamIndex, int enemyIndex)
    {
        _navAgent = GetComponent<NavMeshAgent>();
        TableBase table = TableManager._Instance.Tables[TableType.MonsterTable];

        string n = table.ToStr(enemyIndex, "Name");
        int l = table.ToInt(enemyIndex, "Level");
        int s = table.ToInt(enemyIndex, "STR");
        int i = table.ToInt(enemyIndex, "INT");
        int v = table.ToInt(enemyIndex, "VIT");
        int d = table.ToInt(enemyIndex, "DEX");
        int m = table.ToInt(enemyIndex, "MEN");

        float fw = table.ToFloat(enemyIndex, "Fwalk");
        float bw = table.ToFloat(enemyIndex, "Bwalk");
        float fr = table.ToFloat(enemyIndex, "Frun");
        float br = table.ToFloat(enemyIndex, "Brun");

        InitSetBase(n, fw, bw, fr, br, l, s, i, v, d, m);
        _destroyAction = onDestoryAction;
        _nowHp = _hp = (int)((_vit * 1.3f + _str) * 8);

        _sensingArea.InitSet(this, _sightRange);
        _myPersonality = (EnemyPersonality)UnityEngine.Random.Range(0, (int)EnemyPersonality.Max);
        //_myPersonality = EnemyPersonality.Impatient;
        Debug.Log(_myPersonality);
        _personalityRate = GameDefaultValue._personality[(int)_myPersonality];
        _nowRoamIndex = roamIndex;
        _myRoamType = roam;

        for (int j = 0; j < _attackZone.Length; j++)
        {
            _attackZone[j].GetComponent<CheckAttackRange>().InitSetRange(this);
        }

        _roamPointList = new List<Vector3>(posTF.Count);
        foreach (var item in posTF)
        {
            _roamPointList.Add(item.position);
        }
        AllZoneDisable();


        SelectDefaultAutomaticAction();
        //_isSelected = true;
        startPos = transform.position;

        _wnd.OpenSimpleWnd(_name);
        _wnd.CloseSimpleWnd();
    }

    public override void ExchangeAnimation(AniState state)
    {
        if (_isDeath) return;

        switch (state)
        {
            case AniState.Walk:
#if UNITY_EDITOR
                //_aniController.speed = _walkSpeed ;
                _navAgent.speed = _walkSpeed * _speedScale;
                _aniController.speed = _speedScale;
                _navAgent.stoppingDistance = 0;
#else
                _navAgent.speed = _walkSpeed;
#endif
                break;
            case AniState.Run:
                _aniController.speed = _runSpeed;
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
                _isDeath = true;
                _aniController.SetTrigger("Death");
                break;
            case AniState.BackHome:
                _aniController.speed = _runSpeed * 2;
                _navAgent.speed = _runSpeed * 2;
                _navAgent.stoppingDistance = 0;
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
        if (_isDead) return;

        switch (_nowState)
        {
            case AniState.Idle:
                if (_isBattle)
                {
                    if (Vector3.Distance(transform.position, _targetCharacter.position) > _navAgent.stoppingDistance + _runSpeed * Time.deltaTime + _distanceOffset + _attackDistance)
                    {
                        SetGoalLocation(_targetCharacter.position, AniState.Run);
                    }
                    else
                    {
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
                    SetGoalLocation(_roamPointList[_nowRoamIndex], AniState.BackHome);
                    break;
                }

                if (Vector3.Distance(transform.position, _targetCharacter.position) <= _navAgent.stoppingDistance + _runSpeed * Time.deltaTime + _distanceOffset + _attackDistance)
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
                    transform.rotation = Quaternion.LookRotation(_targetCharacter.transform.position - transform.position, Vector3.up);
                }
                else
                {
                    SetGoalLocation(_targetCharacter.position, AniState.Run);
                }
                break;
            case AniState.BackHome:
                if (_navAgent.remainingDistance <= _navAgent.stoppingDistance + _runSpeed * Time.deltaTime * 2 + _distanceOffset)
                {
                    ExchangeAnimation(AniState.Idle);
                }
                break;
        }
        SelectDefaultAutomaticAction();
    }

    private void OnDestroy()
    {
        _destroyAction?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PWeapon"))
        {
            CheckAttackRange ar = other.GetComponent<CheckAttackRange>();

            Zzabarian ply = ar.GetOwner<Zzabarian>();
            int def = GetFinalDefPow(ply._methodAttack);
            int damage = ply._finalAttPow;

            int avoidance = (int)((1 - _dex) * 100f / (_level + _dex));
            int finishDamage = damage - def;

            if (ply._methodAttack == MethodAttack.Physics)
            {
                if (avoidance >= Random.Range(0, 100)) return;

                finishDamage *= (int)(damage * (avoidance * 0.01f));
            }

            finishDamage = finishDamage < 1 ? 1 : finishDamage;

            if ((_nowHp -= finishDamage) <= 0)
            {
                _nowHp = 0;
                ExchangeAnimation(AniState.Dead);
                AllZoneDisable();
                GetComponent<BoxCollider>().enabled = false;
            }

            _wnd.SetHPRate(_hpRate);
            //Debug.LogFormat("{0}[{1}:{2}]", _name, _nowHp, _hp);
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
