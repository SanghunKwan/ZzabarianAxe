using UnityEngine;
using UnityEngine.AI;

public class TestScene : MonoBehaviour
{
    [SerializeField] GameObject _target;

    private void Start()
    {
        //_targetAgent = _target.GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;

            Ray r = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out hit))
            {
                //_targetAgent.SetDestination(hit.point);
                _target.SendMessage("SetGoalLocation", hit.point);
            }
        }
    }

}
