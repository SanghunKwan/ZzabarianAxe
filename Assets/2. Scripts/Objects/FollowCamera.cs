using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _hRotationAngle = 360;           //�¿� ȸ����
    [SerializeField] float _vRotationAngle = 720;           //���� ȸ����
    [SerializeField] float _limitedLoopUpAngle = -50;       //�ִ� �� ���� ȸ����
    [SerializeField] float _limitedLookDownAngle = 50;      //�Ʒ� ���� ȸ����

    Transform _followCharacter;

    //�ӽ�
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

        // �̵�
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);


    }

}
