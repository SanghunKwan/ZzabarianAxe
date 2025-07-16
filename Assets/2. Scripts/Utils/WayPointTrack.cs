using UnityEngine;

public class WayPointTrack : MonoBehaviour
{
    [SerializeField] Color _lineColor = Color.yellow;
    Transform[] _points;


    private void OnDrawGizmos()
    {
        Gizmos.color = _lineColor;
        _points = GetComponentsInChildren<Transform>();
        
        if (_points.Length <= 2) return;

        int nextIndex = 1;

        Vector3 currPos = _points[nextIndex].position;
        Vector3 nextPos;

        for (int i = 0; i < _points.Length; i++)
        {
            nextPos = (++nextIndex >= _points.Length) ? _points[1].position : _points[nextIndex].position;
            Gizmos.DrawLine(currPos, nextPos);
            currPos = nextPos;
        }

    }


}
