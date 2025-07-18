using UnityEngine;

[RequireComponent(typeof(MyPointGizmos))]
public class SensingArea : MonoBehaviour
{
    CharacterBase _owner;
    SphereCollider _collider;




    public void InitSet(CharacterBase owner, float range)
    {
        _owner = owner;
        MyPointGizmos pointGizmos = GetComponent<MyPointGizmos>();
        _collider = GetComponent<SphereCollider>();
        pointGizmos._radius = _collider.radius = range;
        pointGizmos._gizmosColor = new Color(1, 1, 0, 0.2f);
    }

    public void ColliderOnoff(bool isOn)
    {
        _collider.enabled = isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _owner.CheckedOpponent(other.gameObject);
        }
    }
}
