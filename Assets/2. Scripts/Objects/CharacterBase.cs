using DefineEnums;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected Animator _aniController;
    protected string _name;
    protected float _walkSpeed;
    protected float _backWalkSpeed;
    protected float _runSpeed;
    protected float _backRunSpeed;

    public string _myName => _name;


    protected void InitSetBase(in string name, float walk, float backWalk, float run, float backRun)
    {
        _name = name;
        _walkSpeed = walk;
        _backWalkSpeed = backWalk;
        _runSpeed = run;
        _backRunSpeed = backRun;

        _aniController = GetComponent<Animator>();
    }

    public abstract void ExchangeAnimation(AniState state);

    public virtual void CheckedOpponent(GameObject hostileObject)
    {
        Debug.Log("상대 감지");
    }
}
