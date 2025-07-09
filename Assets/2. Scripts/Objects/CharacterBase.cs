using DefineEnums;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected Animator _aniController;
    protected float _walkSpeed;
    protected float _backWalkSpeed;
    protected float _runSpeed;
    protected float _backRunSpeed;



    protected void InitSetBase(float walk, float backWalk, float run, float backRun)
    {
        _walkSpeed = walk;
        _backWalkSpeed = backWalk;
        _runSpeed = run;
        _backRunSpeed = backRun;

        _aniController = GetComponent<Animator>();
    }

    public abstract void ExchangeAnimation(AniState state);
}
