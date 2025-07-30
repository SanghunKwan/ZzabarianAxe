using UnityEngine;

public class ArrivePosition : MonoBehaviour
{
    [SerializeField] SphereCollider _collider;



    public void ActivateArrivePosition()
    {
        if (_collider.enabled) return;

        _collider.enabled = true;
        Instantiate(Resources.Load<GameObject>("Effects/ArriveEffect"), transform);
    }

}
