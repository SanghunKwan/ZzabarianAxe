using UnityEngine;

public class MyPointGizmos : MonoBehaviour
{
    public Color _gizmosColor { get; set; } = Color.yellow;

    public float _radius { get; set; }


    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
