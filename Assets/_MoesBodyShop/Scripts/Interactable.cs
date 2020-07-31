namespace MoesBodyShop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;

    public class Interactable : PlayerTrigger
    {
        [Header("Interact Settings")]
        public UnityEvent OnPlayerInteract;
        [Tooltip("Leave empty to use default message")]
        [SerializeField] private string interactMessage;
        [SerializeField] private bool interactOnce = false;

        private bool _isPlayerInTrigger = false;
        private bool _hasInteracted = false;

        private const string InteractButton = "Interact";

        private void Awake()
        {
            TriggerEnterEvent.AddListener(OnPlayerEnter);
            TriggerExitEvent.AddListener(OnPlayerExit);
        }

        private void Update()
        {
            if (_isPlayerInTrigger && Input.GetButtonDown(InteractButton))
            {
                if(interactOnce && _hasInteracted)
                {
                    return;
                }

                OnPlayerInteract.Invoke();
                _hasInteracted = true;

                if (interactOnce)
                {
                    TriggerExitEvent.Invoke();
                    RemoveListeners();
                }
            }
        }

        private void RemoveListeners()
        {
            TriggerEnterEvent.RemoveListener(OnPlayerEnter);
            TriggerExitEvent.RemoveListener(OnPlayerExit);
        }

        private void OnPlayerEnter()
        {
            _isPlayerInTrigger = true;

            if (interactMessage == null || interactMessage == string.Empty)
            {
                GameManager.instance.SetInteractText(true);
            }
            else
            {
                GameManager.instance.SetInteractText(true, interactMessage);
            }
        }

        private void OnPlayerExit()
        {
            _isPlayerInTrigger = false;
            GameManager.instance.SetInteractText(false);
        }
    }
}