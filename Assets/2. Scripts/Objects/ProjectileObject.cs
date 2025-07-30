using DefineEnums;
using System;
using UnityEngine;

public class ProjectileObject : MonoBehaviour
{
    ProjectileTargetType _type;
    Vector3 _movingDirection;
    GameObject _targetObject;
    float _speed;
    float _lifeTime;
    float _currentTime;
    CharacterBase _owner;


    public void InitProjectile(ProjectileTargetType projectileType, in Vector3 targetPosition, float speed, float lifeTime, EnemyMagic owner)
    {
        _type = projectileType;
        _movingDirection = (targetPosition + Vector3.up - transform.position).normalized;
        _targetObject = null;
        _speed = speed;
        _lifeTime = lifeTime;

        _owner = owner;
    }
    public void InitProjectile(ProjectileTargetType projectileType, GameObject targetObject, float speed, float lifeTime, EnemyMagic owner)
    {
        _type = projectileType;
        _targetObject = targetObject;
        _speed = speed;
        _lifeTime = lifeTime;

        _owner = owner;
    }


    private void Update()
    {
        float deltaTime = Time.deltaTime;
        float deltaSpeed = _speed * deltaTime;
        _currentTime += deltaTime;

        if (_currentTime >= _lifeTime)
        {
            Expired();
            return;
        }


        switch (_type)
        {
            case ProjectileTargetType.Vector:
                transform.position += _movingDirection * deltaSpeed;
                break;
            case ProjectileTargetType.GameObject:
                transform.position = Vector3.MoveTowards(transform.position, _targetObject.transform.position, deltaSpeed);
                break;
        }
    }

    public T GetOwner<T>() where T : CharacterBase
    {
        return (T)_owner;
    }
    public void Expired()
    {
        Destroy(gameObject);
    }

    public void CollideWithCharacter()
    {
        Destroy(gameObject);
    }
}
