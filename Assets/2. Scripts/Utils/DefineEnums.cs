using UnityEngine;

namespace DefineEnums
{
    #region [캐릭터]
    public enum EnemyPersonality
    {
        Lazy,                               //12
        LaidBack,                           //35
        General,                            //55
        Impatient,                          //86

        Max
    }
    public enum RoamType
    {
        Random,
        Inorder,
        TwoPoint,
        BackNForth
    }
    public enum AniState
    {
        Idle,
        Walk,
        Run,
        Attack,
        JustGuard,
        BackHome,

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

    public static class GameDefaultValue
    {
        public static int[] _personality = new int[] { 12, 35, 55, 86 };
    }


}
