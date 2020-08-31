namespace MoesBodyShop
{
    using UnityEngine;

    public class MoeLevelController : MonoBehaviour
    {
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private MoeController moeController;
        [SerializeField] private Interactable exitDoorInteractable;
        [SerializeField] private Interactable[] generatorInteractables;

        private const byte TotalGenerators = 3;
        private const byte MaxPlayerHealth = 2;

        private byte _generatorsOn = 0;
        private byte _playerHealth = 2;
        private GeneratorController[] _generatorControllers;

        private void Awake()
        {
            _playerHealth = MaxPlayerHealth;
            _generatorsOn = 0;
            exitDoorInteractable.enabled = false;
            _generatorControllers = FindObjectsOfType<GeneratorController>();
        }

        public void GeneratorActivated()
        {
            _generatorsOn++;
            if(_generatorsOn == TotalGenerators)
            {
                //open the exit.
                Debug.Log("Exit door opened");
                exitDoorInteractable.enabled = true;
            }
        }

        public void PlayerHit()
        {
            _playerHealth--;

            if(_playerHealth == 0)
            {
                PlayerDied();
            }
        }

        public void ExitFloor()
        {

        }

        private void PlayerDied()
        {
            Debug.Log("Player died");
            //Reset Moe

            //Respawn player
            GameManager.instance.TeleportPlayer(spawnLocation);
            _playerHealth = MaxPlayerHealth;

            //Reset generators
            _generatorsOn = 0;

            for (int i = 0; i < generatorInteractables.Length; i++)
            {
                generatorInteractables[i].ResetInteraction();
                _generatorControllers[i].ResetGenerator();
            }
        }
    }
}