namespace MoesBodyShop
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.AI;

    public class MoeController : MonoBehaviour
    {
        [SerializeField] private MoeLevelController levelController;
        [Header("Moe Settings")]
        [SerializeField] private float detectionThreshold = 25f;
        [SerializeField] private float visionThreshold = 25f;
        [SerializeField] private float attackThreshold = 5f;
        [SerializeField] private float fov = 30f;
        [Header("References")]
        [SerializeField] private Transform[] waypoints;
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Animator moeAnimator;

        private const float WaypointThreshold = 3f;
        private const string PlayerTag = "Player";
        private const float WalkSpeed = 3.75f;
        private const float ChaseSpeed = 9f;
        private const string RoarParam = "Roar";
        private const string SwipeParam = "SwipeAttack";
        private const string PunchParam = "PunchAttack";
        private const float AttackDelay = .2f;
        private const float SwipeDuration = 2.666f;
        private const float PunchDuration = 1.1f;

        private enum State { Searching, Chasing, Alert}

        private Transform _targetTransform;
        private Transform _cameraTransform;
        private Transform _playerTransform;
        private int _waypointIndex = -1;
        private State _moeState = State.Searching;
        private bool _canAttack = true;

        private void OnDrawGizmos()
        {
            //Draw the attack circle
            Handles.color = Color.magenta;
            Handles.DrawWireDisc(transform.position, Vector3.up, attackThreshold);

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

            _targetTransform = GetClosestWaypoint();
            navAgent.SetDestination(_targetTransform.position);
        }

        private void Update()
        {
            if (_moeState == State.Searching)
            {
                if (IsPlayerClose() || IsPlayerVisible())
                {
                    SetChaseMode();
                }
                else if (Vector3.Distance(transform.position, _targetTransform.position) < WaypointThreshold)
                {
                    _waypointIndex++;
                    if (_waypointIndex >= waypoints.Length)
                    {
                        _waypointIndex = 0;
                    }

                    _targetTransform = waypoints[_waypointIndex];
                    navAgent.SetDestination(_targetTransform.position);
                }
            }
            else if (_moeState == State.Chasing)
            {
                if (IsPlayerAttackable())
                {
                    navAgent.SetDestination(transform.position);
                    Attack();
                }
                else if(Vector3.Distance(transform.position, _cameraTransform.position) < visionThreshold)
                {
                    navAgent.SetDestination(_cameraTransform.position);
                }
                else
                {
                    SetSearchMode();
                }
            }
            else if (_moeState == State.Alert)
            {
                if (IsPlayerClose() || IsPlayerVisible())
                {
                    SetChaseMode();
                }
                else if (Vector3.Distance(transform.position, _targetTransform.position) < WaypointThreshold)
                {
                    SetSearchMode();
                }
            }

            IsPlayerVisible();
        }

        public void AlertMoe(Transform alertLocation)
        {
            _targetTransform = alertLocation;
            SetAlertMode();
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

        private bool IsPlayerClose()
        {
            return Vector3.Distance(_cameraTransform.position, transform.position) <= detectionThreshold;
        }

        private bool IsPlayerAttackable()
        {
            return Vector3.Distance(_cameraTransform.position, transform.position) <= attackThreshold;
        }

        private void SetSearchMode()
        {
            Debug.Log("Search mode");
            _moeState = State.Searching;
            navAgent.speed = WalkSpeed;
            _targetTransform = GetClosestWaypoint();
            navAgent.SetDestination(_targetTransform.position);
        }

        private void SetChaseMode()
        {
            Debug.Log("Chase mode");
            _moeState = State.Chasing;
            navAgent.speed = ChaseSpeed;
        }

        private void SetAlertMode()
        {
            Debug.Log("Alert mode");
            _moeState = State.Alert;
            navAgent.speed = ChaseSpeed;
        }

        private void Attack()
        {
            if (_canAttack)
            {
                int choice = Random.Range(0, 2);
                switch (choice)
                {
                    case 0:
                        moeAnimator.SetTrigger(PunchParam);
                        StartCoroutine(AttackRoutine(PunchDuration, AttackDelay));
                        break;
                    case 1:
                        moeAnimator.SetTrigger(SwipeParam);
                        StartCoroutine(AttackRoutine(SwipeDuration, AttackDelay));
                        break;
                }
            }

            _canAttack = false;
        }

        private IEnumerator AttackRoutine(float duration, float delay)
        {
            float timer = duration;
            float hitTimingRange = duration - delay;
            bool canHit = true;

            while (timer >= 0)
            {
                if (timer < hitTimingRange && Vector3.Distance(transform.position, _cameraTransform.position) < attackThreshold && canHit)
                {
                    Debug.Log("Hit");
                    levelController.PlayerHit();
                    canHit = false;
                }

                timer -= Time.deltaTime;
                yield return null;
            }

            _canAttack = true;
        }
    }
}