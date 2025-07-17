using DefineEnums;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFactory : MonoBehaviour
{
    //위치, 생산 시간, 정해진 수량
    [Header("Spawn Parameter")]
    [SerializeField] float _spawnDelaytime = 5;             // 생성 대기 시간
    [SerializeField] int _limitGenerateCount = 10;          // 한계 생성 개수(0이면 무한대)
    [SerializeField] int _maxLiveCount = 1;                 // 최대 살아있는 개체수
    [SerializeField] int _spawnSameTime = 1;                // 동시 생성 개수

    [Header("Char Parameter")]
    RoamType _charRoamType;



    //임시
    GameObject _prefabEnemy;
    //==

    bool _isInfinity;
    float _spawnTime;

    List<Transform> _posList;
    List<GameObject> _spawnObject;


    private void Awake()
    {
        //임시
        InitFactory("EnemyLeather");
        //==
    }

    public void InitFactory(in string prefabName)
    {
        //임시
        _prefabEnemy = Resources.Load<GameObject>("Characters/" + prefabName);
        //==

        if (_limitGenerateCount == 0)
            _isInfinity = true;

        Transform root = transform.GetChild(0);
        int length = root.childCount;


        _spawnObject = new List<GameObject>(_maxLiveCount);
        _posList = new List<Transform>(length);

        for (int i = 0; i < length; i++)
        {
            _posList.Add(root.GetChild(i));
        }
    }


    private void Update()
    {
        if ((_spawnObject.Count >= _maxLiveCount) && (!_isInfinity)) return;

        if (_spawnTime >= _spawnDelaytime)
        {
            _spawnTime -= _spawnDelaytime;
            for (int i = 0; i < _spawnSameTime; i++)
            {
                if (_limitGenerateCount <= 0 || _spawnObject.Count >= _maxLiveCount) break;

                int randomIndex = Random.Range(0, _posList.Count);
                GameObject ob = Instantiate(_prefabEnemy, _posList[randomIndex].position, _posList[randomIndex].rotation);
                _spawnObject.Add(ob);

                EnemyNormal enemy = ob.GetComponent<EnemyNormal>();
                enemy.InitCharacter(_charRoamType, () => _spawnObject.Remove(ob), _posList, randomIndex);

                _limitGenerateCount--;
            }
        }
        _spawnTime += Time.deltaTime;
    }
}
