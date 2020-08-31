namespace MoesBodyShop
{
    using DigitalRuby.RainMaker;

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.Playables;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    using UnityStandardAssets.Characters.FirstPerson;

    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] private PlayableDirector director;
        [SerializeField] private MainMenuController mainMenu;
        [Header("Player")]
        [SerializeField] private FirstPersonController firstPersonController;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Text interactText;
        [Header("Environment")]
        [SerializeField] private RainScript rain;
        [SerializeField] private Light directionalLight;
        [SerializeField] private Transform undergroundSpawnLocation;

        private const string DefaultInteractMessage = "Press [E] to interact";
        private const string TwitterURL = "https://twitter.com/tooManyWes";
        private const float UndergroundFogDensity = 0.1f;

        private bool _isDisplayingInteractText = false;

        private void Awake()
        {
            instance = GetComponent<GameManager>();
        }

        void Start()
        {
            //Can't change audio during awake :(
            AudioManager.instance.SetSFXVolume(false);
            firstPersonController.SetCursorLock(false);
        }

        public void PlayGame()
        {
            AudioManager.instance.SetSFXVolume(true);
            mainMenu.CloseMenu();
            firstPersonController.SetCursorLock(true);
            director.Play();
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void EndGame()
        {
            SceneManager.LoadScene(0);
            firstPersonController.SetCursorLock(false);
        }

        public void OpenTwitter()
        {
            Application.OpenURL(TwitterURL);
        }

        public void SetInteractText(bool isActive, string interactMessage = DefaultInteractMessage)
        {
            interactText.text = interactMessage;
            interactText.gameObject.SetActive(isActive);
        }

        public void GoUnderground()
        {
            //Set lighting settings
            RenderSettings.fogDensity = UndergroundFogDensity;
            directionalLight.gameObject.SetActive(false);
            Camera.main.clearFlags = CameraClearFlags.SolidColor;

            //Adjust rain
            rain.FollowCamera = false;
            AudioManager.instance.SetIndoorRain();

            //Move player
            TeleportPlayer(undergroundSpawnLocation);
        }

        public void GoAngelArea()
        {
            RenderSettings.fog = false;
        }

        public void TeleportPlayer(Transform teleportLocation)
        {
            firstPersonController.enabled = false;
            characterController.enabled = false;

            Transform playerTransform = firstPersonController.gameObject.transform;
            playerTransform.position = teleportLocation.position;
            playerTransform.rotation = teleportLocation.rotation;

            firstPersonController.enabled = true;
            characterController.enabled = true;
        }
    }
}