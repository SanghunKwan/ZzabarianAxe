using DefineEnums;
using UnityEngine;

public class Zzabarian : CharacterBase
{
    [SerializeField] GameObject _weaponObj;

    //참조변수
    CharacterController _charController;

    //정보변수
    AniState _nowState;
    float _moveSpeed;

    bool _isArmed;


    //임시
    private void Awake()
    {
        InitCharacter();
        SetArmed(false);
        _moveSpeed = _runSpeed;
    }
    //==

    public void SetArmed(bool isSet)
    {
        _isArmed = isSet;
        _weaponObj.SetActive(_isArmed);
    }

    public void InitCharacter()
    {
        InitSetBase(1, 1, 5, 5);
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        if (Input.GetButtonDown("WeaponEquip"))
            SetArmed(!_isArmed);

        Ray downRay = new Ray(transform.position + Vector3.up, Vector3.down);

        //if (Physics.Raycast(downRay, 1.2f))

        if (_charController.isGrounded)
        {

            float mz = Input.GetAxis("Vertical");
            float mx = Input.GetAxis("Horizontal");

            Vector3 dir = new Vector3(mx, 0, mz);
            dir = dir.magnitude > 1 ? dir.normalized : dir;

            _charController.SimpleMove(transform.rotation * dir * _moveSpeed);
            //_charController.Move(dir * _moveSpeed * Time.deltaTime);
        }
    }
    public override void ExchangeAnimation(AniState state)
    {
        _nowState = state;
    }

}
