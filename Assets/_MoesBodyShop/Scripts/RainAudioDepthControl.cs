namespace MoesBodyShop
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RainAudioDepthControl : MonoBehaviour
    {
        [SerializeField] private float verticalOffset;

        private AudioManager _audioManager;

        private void Start()
        {
            _audioManager = AudioManager.instance;
        }

        private void Update()
        {
            if(transform.position.y < verticalOffset)
            {
                _audioManager.SetRainVolume(transform.position.y + verticalOffset);
            }
        }
    }
}