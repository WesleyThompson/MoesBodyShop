namespace MoesBodyShop
{
    using UnityEngine;
    using UnityEngine.Events;

    public class PlayerTrigger : MonoBehaviour
    {
        public UnityEvent TriggerEnterEvent;
        public UnityEvent TriggerExitEvent;

        [SerializeField] private bool fireOnce = false;

        private const string PlayerTag = "Player";

        private bool _hasFiredEnter = false;
        private bool _hasFiredExit = false;

        private void OnTriggerEnter(Collider other)
        {
            if(fireOnce && _hasFiredEnter == false)
            {
                if (other.tag == PlayerTag)
                {
                    TriggerEnterEvent.Invoke();
                    _hasFiredEnter = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (fireOnce && _hasFiredExit == false)
            {
                if (other.tag == PlayerTag)
                {
                    TriggerExitEvent.Invoke();
                    _hasFiredExit = true;
                }
            }
        }
    }
}