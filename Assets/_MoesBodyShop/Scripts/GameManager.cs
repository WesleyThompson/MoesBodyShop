namespace MoesBodyShop
{
    using DigitalRuby.RainMaker;

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityStandardAssets.Characters.FirstPerson;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private MainMenuController mainMenu;
        [SerializeField] private FirstPersonController firstPersonController;

        private const string TwitterURL = "https://twitter.com/tooManyWes";

        private void Awake()
        {
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
    }
}