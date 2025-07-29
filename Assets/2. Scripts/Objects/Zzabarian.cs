using DefineEnums;
using UnityEngine;

public class Zzabarian : CharacterBase
{
    const float _fWalkSpeed = 1.2f;
    const float _bWalkSpeed = 0.9f;
    const float _fRunSpeed = 4.1f;
    const float _bRunSpeed = 2.9f;

    [SerializeField] GameObject _weaponObj;
    [SerializeField] GameObject _decoWeaponObj;
    [SerializeField] BoxCollider _weaponCollider;
    [SerializeField] BoxCollider _kickCollider;

    // 스탯 변수
    float _guardTime = 2;
    float _moveSpeedScale;
    float _attSpeedScale;
    int _hp, _nowHP;

    //참조변수
    CharacterController _charController;
    CheckAttackRange _weaponCheck;

    Transform _followCam;

    //정보변수
    AniState _nowState;
    float _moveSpeed;
    float _protectingTime;

    public int _finalAttPow => (int)((_str + (_dex * 0.5f) + (_vit * 0.2f)) * (((_level / 10) * 0.1f) + 1));
    public override int GetFinalDefPow(MethodAttack ma)
    {
        if (ma == MethodAttack.Physics)
            return (int)(_vit + _str * 0.5f);
        else
            return (int)(_men + _int * 0.8f);
    }


    bool _isArmed;
    bool _isRun;
    bool _isAttack;
    bool _isGuard;



    //임시
    private void Start()
    {
        string tempName = "제이슨";
        InitCharacter(tempName, 1);

        _moveSpeed = _runSpeed;
        IngameManager._instance.InitCharacters(tempName);
    }
    //==
    public void InitCharacter(in string name, int level)
    {
        _charController = GetComponent<CharacterController>();
        _weaponCheck = _weaponCollider.GetComponent<CheckAttackRange>();
        TableBase table = TableManager._Instance.Tables[TableType.LevelUpTable];

        int s = table.ToInt(level, "STR");
        int i = table.ToInt(level, "INT");
        int v = table.ToInt(level, "VIT");
        int d = table.ToInt(level, "DEX");
        int m = table.ToInt(level, "MEN");
        _moveSpeedScale = table.ToFloat(level, "MovSpeedScale");
        _attSpeedScale = table.ToFloat(level, "AttSpeedScale");
        InitSetBase(name, _fWalkSpeed, _bWalkSpeed, _fRunSpeed, _bRunSpeed, level, s, i, v, d, m);

        _nowHP = _hp = (int)((_vit * 1.2f + (_str * 0.6f + _dex * 0.4f)) * 9);
        SetArmed(false);
        DisableArmed();

        _weaponCheck.InitSetRange(this);
    }


    void SetFollowCam(Transform cam)
    {
        _followCam = cam;
        Debug.Log(_followCam.name);
    }

    public void SetArmed(bool isSet)
    {
        _isArmed = isSet;

        if (!_isArmed)
        {
            DisableKickCollider();
            DisableWeaponCollider();
        }

        if (_nowState == AniState.Idle)
        {
            if (isSet)
            {
                _weaponObj.SetActive(_isArmed);
                _decoWeaponObj.SetActive(!_isArmed);
            }
        }
        else
        {
            _weaponObj.SetActive(_isArmed);
            _decoWeaponObj.SetActive(!_isArmed);
        }

        ExchangeAnimation(_nowState);
    }


    public void DisableArmed()
    {
        _weaponObj.SetActive(false);
        _decoWeaponObj.SetActive(true);
    }


    private void Update()
    {
        if (_isDead) return;
        if (_isGuard) return;

        SetActionKeyProc();

        if (_isAttack) return;

        if (Input.GetButtonDown("WeaponEquip"))
            SetArmed(!_isArmed);

        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);
        if (Physics.Raycast(downRay, 1.5f))

        //if (_charController.isGrounded)
        {
            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");

            SetAniDirection(mx, mz);

            Vector3 dir = new Vector3(mx, 0, mz);
            dir = dir.magnitude > 1 ? dir.normalized : dir;

            if (_followCam != null)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, _followCam.eulerAngles.y, transform.eulerAngles.z);
            }
            _charController.SimpleMove(transform.rotation * dir * _moveSpeed);
            //_charController.Move(dir * _moveSpeed * Time.deltaTime);

            if (dir.magnitude == 0)
            {
                ExchangeAnimation(AniState.Idle);
            }
            else
            {
                if (_isRun)
                    ExchangeAnimation(AniState.Run);
                else
                    ExchangeAnimation(AniState.Walk);
            }
        }
        else
        {
            _charController.SimpleMove(Vector3.zero);

        }

        //임시
        //1=>2 연계 시 aniState 무시하도록 했으니 이후 확인 필요.
        //if (Input.GetButtonDown("Fire1"))
        //{
        //    ExchangeAnimation(AniState.Attack);
        //    _aniController.SetTrigger("StdAttack1");
        //}

        //if (Input.GetButtonDown("Fire2"))
        //{
        //    ExchangeAnimation(AniState.Attack);
        //    _aniController.SetTrigger("StdAttack2");
        //}
        //==
    }
    void SetActionKeyProc()
    {
        if (!_isArmed) return;

        int count = (int)AttackName.Max;
        bool active = false;

        string name;
        for (int i = 0; i < count; i++)
        {
            name = ((AttackName)i).ToString();
            if (Input.GetButtonDown(name))
            {
                active = true;
                _aniController.SetTrigger(name);
            }
        }

        if (active)
            ExchangeAnimation(AniState.Attack);

    }

    private void LateUpdate()
    {
        if (_isDeath) return;

        if (Input.GetButtonDown("Run"))
            _isRun = !_isRun;

        if (_isAttack && _isArmed)
        {
            if (Input.GetButton("JGuard"))
            {
                ExchangeAnimation(AniState.JustGuard);
            }
        }
        if (_isGuard)
        {
            _protectingTime += Time.deltaTime;
            if (_protectingTime >= _guardTime)
            {
                _protectingTime = 0;
                ExchangeAnimation(AniState.Idle);
            }
        }


    }
    void DisableAttacked()
    {
        _isAttack = false;
        _nowState = AniState.Idle;
        DisableKickCollider();
        DisableWeaponCollider();
    }

    void EnableWeaponCollider()
    {
        _weaponCollider.enabled = true;
    }
    void DisableWeaponCollider()
    {
        _weaponCollider.enabled = false;
    }
    void EnableKickCollider()
    {
        _kickCollider.enabled = true;
    }
    void DisableKickCollider()
    {
        _kickCollider.enabled = false;
    }

    public override void ExchangeAnimation(AniState state)
    {
        _aniController.SetBool("IsArmed", _isArmed);

        switch (state)
        {
            case AniState.Idle:
                _isGuard = false;
                break;
            case AniState.Walk:
                if (_aniController.GetFloat("FNB") < 0)
                    _moveSpeed = _backWalkSpeed;
                else
                    _moveSpeed = _walkSpeed;

                _moveSpeed *= _moveSpeedScale;
                _aniController.speed *= _moveSpeedScale;
                break;
            case AniState.Run:
                if (_aniController.GetFloat("FNB") < 0)
                    _moveSpeed = _backRunSpeed;
                else
                    _moveSpeed = _runSpeed;

                _moveSpeed *= _moveSpeedScale;
                _aniController.speed *= _moveSpeedScale;
                break;
            case AniState.Attack:
                _isAttack = true;
                _aniController.speed *= _attSpeedScale;
                break;
            case AniState.JustGuard:
                _isGuard = true;
                break;
            case AniState.Dead:
                _isDeath = true;
                _aniController.SetTrigger("Dead");
                break;

        }
        _aniController.SetInteger("AniState", (int)state);
        _nowState = state;
    }

    void SetAniDirection(float x, float z)
    {
        _aniController.SetFloat("RNL", x);
        _aniController.SetFloat("FNB", z);
    }

    public void OnHitting(EnemyNormal en)
    {
        if (_isGuard)
        {
        }
        else
        {
            int count = (int)AttackName.Max;
            for (int i = 0; i < count; i++)
                _aniController.ResetTrigger(((AttackName)i).ToString());

            int damage = en._finalAttPow;
            int def = GetFinalDefPow(en._methodAttack);
            int avoidance = (int)((1 - _dex) * 100f / (_level + _dex));
            int finishDamage = damage - def;

            if (en._methodAttack == MethodAttack.Physics)
            {
                if (avoidance >= Random.Range(0, 100)) return;

                finishDamage *= (int)(damage * (avoidance * 0.01f));
            }

            finishDamage = finishDamage < 1 ? 1 : finishDamage;

            if ((_nowHP -= finishDamage) <= 0)
            {
                _nowHP = 0;
                ExchangeAnimation(AniState.Dead);
            }
            IngameManager._instance.CharacterHpChanged(((float)_nowHP) / _hp);

            Debug.LogFormat("{0}[{1}:{2}]", _name, _nowHP, _hp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EAttackZone"))
        {
            CheckAttackRange car = other.GetComponent<CheckAttackRange>();

            OnHitting(car.GetOwner<EnemyNormal>());
        }

        else if (other.CompareTag("ArrivePosition"))
        {
            //게임 클리어.
        }
    }
}
