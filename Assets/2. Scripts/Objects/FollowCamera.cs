using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _hRotationAngle = 360;           //좌우 회전각
    [SerializeField] float _vRotationAngle = 720;           //상하 회전각
    [SerializeField] float _limitedLoopUpAngle = -50;       //최대 위 보기 회전각
    [SerializeField] float _limitedLookDownAngle = 50;      //아래 보기 회전각

    Transform _followCharacter;

    //임시
    private void Start()
    {
        InitCam();
    }
    //==
    public void InitCam()
    {
        _followCharacter = GameObject.FindGameObjectWithTag("Player").transform;


    }

    private void Update()
    {
        float yDelta = Input.GetAxisRaw("Mouse X");
        float xDelta = Input.GetAxisRaw("Mouse Y");

        yDelta *= _hRotationAngle * Time.deltaTime;
        xDelta *= _vRotationAngle * Time.deltaTime;

        Quaternion hRotate = Quaternion.AngleAxis(yDelta, Vector3.up);
        transform.rotation *= hRotate;

        float xCurrent = transform.eulerAngles.x;
        if (xCurrent > 180)
            xCurrent -= 360;
        else if (xCurrent < -180)
            xCurrent += 360;

        float xNextRot = xCurrent + xDelta;

        if (xNextRot > _limitedLookDownAngle)
            xDelta = _limitedLookDownAngle - xCurrent;
        else if (xNextRot < _limitedLoopUpAngle)
            xDelta = _limitedLoopUpAngle - xCurrent;

        transform.rotation *= Quaternion.AngleAxis(xDelta, Vector3.right);

        //transform.position = _followCharacter.position + _followCharacter.rotation * _offset;
        //transform.rotation = _followCharacter.rotation;

        Vector3 targetPosition = _followCharacter.position + _followCharacter.rotation * _offset;

        // 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);


    }

}
