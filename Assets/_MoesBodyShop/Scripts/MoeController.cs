namespace MoesBodyShop
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.AI;

    public class MoeController : MonoBehaviour
    {
        [Header("Moe Settings")]
        [SerializeField] private float detectionThreshold = 25f;
        [SerializeField] private float visionThreshold = 25f;
        [SerializeField] private float fov = 30f;
        [Header("References")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private NavMeshAgent navAgent;

        private const float WaypointThreshold = 3f;
        private const string PlayerTag = "Player";

        private enum State { Searching, Chasing}

        private Transform _targetWaypoint;
        private Transform _cameraTransform;
        private Transform _playerTransform;
        private int _waypointIndex = -1;
        private State _moeState = State.Searching;

        private void OnDrawGizmos()
        {
            //Draw the detect circle
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position, Vector3.up, detectionThreshold);

            //Draw vision arc
            Handles.color = Color.yellow;
            Vector3 startDirection = Quaternion.AngleAxis(-(fov / 2), Vector3.up) * transform.forward;
            Handles.DrawLine(transform.position, (startDirection * visionThreshold) + transform.position);
            Handles.DrawWireArc(transform.position, Vector3.up, startDirection, fov, visionThreshold);
            startDirection = Quaternion.AngleAxis(fov, Vector3.up) * startDirection;
            Handles.DrawLine(transform.position, (startDirection * visionThreshold) + transform.position);
        }

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            _playerTransform = _cameraTransform.parent;

            _targetWaypoint = GetClosestWaypoint();
            navAgent.SetDestination(_targetWaypoint.position);
        }

        private void Update()
        {
            if (_moeState == State.Searching)
            {
                if (Vector3.Distance(_cameraTransform.position, transform.position) <= detectionThreshold)
                {
                    SetChaseMode();
                }
                else if (Vector3.Distance(transform.position, _targetWaypoint.position) < WaypointThreshold)
                {
                    _waypointIndex++;
                    if (_waypointIndex >= waypoints.Length)
                    {
                        _waypointIndex = 0;
                    }

                    _targetWaypoint = waypoints[_waypointIndex];
                    navAgent.SetDestination(_targetWaypoint.position);
                }
            }
            
            if (_moeState == State.Chasing)
            {
                navAgent.SetDestination(_cameraTransform.position);
            }

            IsPlayerVisible();
        }

        private Transform GetClosestWaypoint()
        {
            float smallestDistance = float.MaxValue;

            for (int i = 0; i < waypoints.Length; i++)
            {
                float currentDistance = Vector3.Distance(transform.position, waypoints[i].position);
                if (smallestDistance > currentDistance)
                {
                    _waypointIndex = i;
                    smallestDistance = currentDistance;
                }
            }

            return waypoints[_waypointIndex];
        }

        private bool IsPlayerVisible()
        {
            bool playerVisible = false;

            if (Vector3.Distance(_playerTransform.position, transform.position) < visionThreshold)
            {
                Vector3 directionToPlayer = _playerTransform.position - transform.position;
                float angle = Vector3.Angle(directionToPlayer, transform.forward);
                if (angle < fov / 2)
                {
                    RaycastHit hit;
                    Debug.DrawRay(transform.position, directionToPlayer, Color.blue);
                    if (Physics.SphereCast(transform.position, .5f, directionToPlayer, out hit, visionThreshold))
                    {
                        if (hit.transform.tag == PlayerTag)
                        {
                            playerVisible = true;
                        }
                    }
                }
            }

            return playerVisible;
        }

        private void SetSearchMode()
        {
            _moeState = State.Searching;

        }

        private void SetChaseMode()
        {
            _moeState = State.Chasing;
        }
    }
}