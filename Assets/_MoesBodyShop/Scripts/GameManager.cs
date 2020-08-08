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
        [SerializeField] private FirstPersonController firstPersonController;
        [SerializeField] private Text interactText;
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

            firstPersonController.SetCursorLock(false);
        }

        void Start()
        {
            //Can't change audio during awake :(
            AudioManager.instance.SetSFXVolume(false);
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

            //Adjust rain
            rain.FollowCamera = false;

            //Move player
            firstPersonController.gameObject.transform.position = undergroundSpawnLocation.position;
        }
    }
}