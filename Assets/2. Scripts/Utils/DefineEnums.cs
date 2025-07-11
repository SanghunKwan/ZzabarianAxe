using UnityEngine;

namespace DefineEnums
{
    #region [캐릭터]
    public enum AniState
    {
        Idle,
        Walk,
        Run,
        Attack,
        JustGuard,

        Dead = 50
    }

    public enum AttackName
    {
        StdAttack1,
        StdAttack2,
        SDAttack,
        SUAttack,
        Kick,

        Max
    }



    #endregion [캐릭터]




}
