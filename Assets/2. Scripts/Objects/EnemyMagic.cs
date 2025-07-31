using UnityEngine;
using DefineEnums;

public class EnemyMagic : EnemyBase
{
    [SerializeField] Transform _projectileSpawnPosition;

    GameObject _prefabProjectile;
    GameObject _controlProjectile;

    protected override void InitDetails()
    {
        _prefabProjectile = Resources.Load<GameObject>("Effects/FireBall");
        _attackDistance = 8;
    }

    protected override void AllZoneDisable()
    {
        if (_controlProjectile == null) return;

        _controlProjectile.GetComponent<ProjectileObject>().Expired();
        OnProjectileDestoryEvent();
    }

    public void LaunchAttack()
    {
        //공격 이펙트 Scene에 생성.
        //계속 추적, Enemy 하나 당 하나까지 생성.
        _controlProjectile = Instantiate(_prefabProjectile, _projectileSpawnPosition.position + Vector3.up, Quaternion.identity);
        _controlProjectile.GetComponent<ProjectileObject>().InitProjectile(ProjectileTargetType.Vector, _targetCharacter.position, 5, 3, this, OnProjectileDestoryEvent);

        _isAttackStop = true;
    }
    public void Launch2Attack()
    {
        //공격 이펙트 Scene에 생성.
        //유도공격.
        _controlProjectile = Instantiate(_prefabProjectile, _projectileSpawnPosition.position + Vector3.up, Quaternion.identity);
        _controlProjectile.GetComponent<ProjectileObject>().InitProjectile(ProjectileTargetType.GameObject, _targetCharacter.gameObject, 5, 10, this, OnProjectileDestoryEvent);

        _isAttackStop = true;
    }

    public void OnProjectileDestoryEvent()
    {
        _controlProjectile = null;
        _isAttackStop = false;
    }
}
