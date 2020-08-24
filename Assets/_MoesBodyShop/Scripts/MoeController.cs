namespace MoesBodyShop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    public class MoeController : MonoBehaviour
    {
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private NavMeshAgent navAgent;

        private const float WaypointThreshold = 3f;

        private Transform _targetWaypoint;
        private int _waypointIndex = -1;

        private void Awake()
        {
            _targetWaypoint = GetClosestWaypoint();
            navAgent.SetDestination(_targetWaypoint.position);
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, _targetWaypoint.position) < WaypointThreshold)
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
    }
}