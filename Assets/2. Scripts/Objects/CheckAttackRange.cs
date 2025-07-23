using UnityEngine;

public class CheckAttackRange : MonoBehaviour
{
    CharacterBase _owner;


    public void InitSetRange(CharacterBase ower)
    {
        _owner = ower;
    }

    public T GetOwner<T>() where T : CharacterBase
    {
        return (T)_owner;
    }

}
