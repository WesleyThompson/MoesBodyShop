namespace MoesBodyShop
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.AI;

    public class MoeAgentController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private Animator animator;

        private const string VelX = "velx";
        private const string VelY = "vely";
        private const string RoarTrigger = "Roar";
        private const string SwipeAttackTrigger = "SwipeAttack";
        private const string PunchAttackTrigger = "PunchAttack";

        private void Update()
        {
            animator.SetFloat(VelX, navAgent.velocity.x);
            animator.SetFloat(VelY, navAgent.velocity);
        }
    }
}