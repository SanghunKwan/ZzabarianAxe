using DefineEnums;
using UnityEngine;

public class Zzabarian : CharacterBase
{
    [SerializeField] GameObject _weaponObj;
    [SerializeField] GameObject _decoWeaponObj;
    [SerializeField] BoxCollider _weaponCollider;
    [SerializeField] BoxCollider _kickCollider;

    //참조변수
    CharacterController _charController;

    //정보변수
    AniState _nowState;
    float _moveSpeed;

    bool _isArmed;
    bool _isRun;
    bool _isAttack;


    //임시
    private void Awake()
    {
        InitCharacter();

        _moveSpeed = _runSpeed;
    }
    //==
    public void InitCharacter()
    {
        _charController = GetComponent<CharacterController>();

        InitSetBase(1.2f, 0.8f, 4.1f, 2.9f);

        SetArmed(false);
        DisableArmed();
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
        SetActionKeyProc();

        if (_isAttack) return;

        if (Input.GetButtonDown("WeaponEquip"))
            SetArmed(!_isArmed);

        if (Input.GetButton("Run"))
        {
            _isRun = true;
        }
        else
            _isRun = false;


        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(downRay, 1.2f))

        //if (_charController.isGrounded)
        {

            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");

            SetAniDirection(mx, mz);

            Vector3 dir = new Vector3(mx, 0, mz);
            dir = dir.magnitude > 1 ? dir.normalized : dir;


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

        //임시
        //1=>2 연계 시 aniState 무시하도록 했으니 이후 확인 필요.
        if (Input.GetButtonDown("Fire1"))
        {
            ExchangeAnimation(AniState.Attack);
            _aniController.SetTrigger("StdAttack1");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            ExchangeAnimation(AniState.Attack);
            _aniController.SetTrigger("StdAttack2");
        }
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
            case AniState.Walk:
                if (_aniController.GetFloat("FNB") < 0)
                    _moveSpeed = _backWalkSpeed;
                else
                    _moveSpeed = _walkSpeed;
                break;
            case AniState.Run:
                if (_aniController.GetFloat("FNB") < 0)
                    _moveSpeed = _backRunSpeed;
                else
                    _moveSpeed = _runSpeed;
                break;
            case AniState.Attack:
                _isAttack = true;
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

    public void OnHitting()
    {
        int count = (int)AttackName.Max;

        for (int i = 0; i < count; i++)
            _aniController.ResetTrigger(((AttackName)i).ToString());


    }
}
