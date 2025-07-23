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

    protected bool _isDeath;

    protected MethodAttack _attMethod;
    //기초 스탯
    protected int _level;
    protected int _str;
    protected int _vit;
    protected int _dex;
    protected int _int;
    protected int _men;


    public abstract int GetFinalDefPow(MethodAttack ma);
    public string _myName => _name;
    public bool _isDead => _isDeath;
    public MethodAttack _methodAttack => _attMethod;



    protected void InitSetBase(in string name, float walk, float backWalk, float run, float backRun,
                               int level, int s, int i, int v, int d, int m)
    {
        _isDeath = false;

        _name = name;
        _walkSpeed = walk;
        _backWalkSpeed = backWalk;
        _runSpeed = run;
        _backRunSpeed = backRun;

        _level = level;
        _str = s;
        _int = i;
        _vit = v;
        _dex = d;
        _men = m;

        _aniController = GetComponent<Animator>();
    }

    public abstract void ExchangeAnimation(AniState state);

    public virtual void CheckedOpponent(GameObject hostileObject)
    {
        Debug.Log("상대 감지");
    }
}
