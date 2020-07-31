namespace MoesBodyShop
{
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerTrigger : MonoBehaviour
    {
        [Header("Trigger Settings")]
        public UnityEvent TriggerEnterEvent;
        public UnityEvent TriggerExitEvent;

        [SerializeField] private bool triggerOnce = false;

        private const string PlayerTag = "Player";

        private bool _hasFiredEnter = false;
        private bool _hasFiredExit = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == PlayerTag)
            {
                if(triggerOnce && _hasFiredEnter)
                {
                    return;
                }

                TriggerEnterEvent.Invoke();
                _hasFiredEnter = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == PlayerTag)
            {
                if(triggerOnce && _hasFiredExit)
                {
                    return;
                }

                TriggerExitEvent.Invoke();
                _hasFiredExit = true;
            }
        }
    }
}