namespace MoesBodyShop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class MoeLevelController : MonoBehaviour
    {
        [SerializeField] private Transform spawnLocation;
        [SerializeField] private MoeController moeController;
        [SerializeField] private Interactable exitDoorInteractable;
        [SerializeField] private Interactable[] generatorInteractables;

        private const byte TotalGenerators = 3;

        private byte _generatorsOn = 0;

        public void GeneratorActivated()
        {
            _generatorsOn++;
            if(_generatorsOn == TotalGenerators)
            {
                //open the exit.
            }
        }

        public void PlayerDied()
        {
            //Reset Moe
            //Respawn player
            //Reset generators
        }
    }
}