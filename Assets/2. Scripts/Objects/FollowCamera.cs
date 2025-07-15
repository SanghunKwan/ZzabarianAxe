using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _moveSpeed = 5;
    [SerializeField] float _hDeltaRotationScale = 2.5f;           //좌우 회전각
    [SerializeField] float _vDeltaRotationScale = 2;           //상하 회전각
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

        transform.position = _followCharacter.position + _followCharacter.rotation * _offset;
        transform.rotation = _followCharacter.rotation;

        _followCharacter.SendMessage("SetFollowCam", transform);

    }

    private void Update()
    {

        if (Input.GetButton("Fire2"))
        {
            float yDelta = Input.GetAxisRaw("Mouse X") * _hDeltaRotationScale;
            float xDelta = -Input.GetAxisRaw("Mouse Y") * _vDeltaRotationScale;

            Vector2 mouseDelta = new Vector2(yDelta, -xDelta);
            Vector3 camAngle = transform.rotation.eulerAngles;

            float x = camAngle.x - mouseDelta.y;
            if (x < 180)
                x = Mathf.Clamp(x, -1f, _limitedLookDownAngle);
            else
                x = Mathf.Clamp(x, 360 + _limitedLoopUpAngle, 361f);

            transform.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
        }



        //yDelta *= _hRotationAngle * Time.deltaTime;
        //xDelta *= _vRotationAngle * Time.deltaTime;

        //Quaternion hRotate = Quaternion.AngleAxis(yDelta, Vector3.up);
        //transform.rotation *= hRotate;

        //float xCurrent = transform.eulerAngles.x;
        //if (xCurrent > 180)
        //    xCurrent -= 360;
        //else if (xCurrent < -180)
        //    xCurrent += 360;

        //float xNextRot = xCurrent + xDelta;

        //if (xNextRot > _limitedLookDownAngle)
        //    xDelta = _limitedLookDownAngle - xCurrent;
        //else if (xNextRot < _limitedLoopUpAngle)
        //    xDelta = _limitedLoopUpAngle - xCurrent;

        //transform.rotation *= Quaternion.AngleAxis(xDelta, Vector3.right);

        //transform.position = _followCharacter.position + _followCharacter.rotation * _offset;
        //transform.rotation = _followCharacter.rotation;

        Vector3 targetPosition = _followCharacter.position + _followCharacter.rotation * _offset;

        // 이동
        transform.position = Vector3.Lerp(transform.position, targetPosition, _moveSpeed * Time.deltaTime);
    }

}
