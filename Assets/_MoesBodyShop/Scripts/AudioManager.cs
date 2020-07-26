namespace MoesBodyShop
{
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [SerializeField] private AudioMixer audioMixer;

        private const string MasterVolumeParam = "MasterVolume";
        private const string SFXVolumeParam = "SFXVolume";
        private const string RainVolumeParam = "RainVolume";
        private const string UIVolumeParam = "UIVolume";
        private const float FullVolume = 0f;
        private const float ZeroVolume = -80f;

        private void Awake()
        {
            instance = GetComponent<AudioManager>();
        }

        public void SetSFXVolume(bool isOn)
        {
            if (isOn)
            {
                SetSFXVolume(FullVolume);
            }
            else
            {
                SetSFXVolume(ZeroVolume);
            }
        }

        public void SetSFXVolume(float value)
        {
            audioMixer.SetFloat(SFXVolumeParam, value);
        }

        public void SetUIVolume(bool isOn)
        {
            if (isOn)
            {
                SetUIVolume(FullVolume);
            }
            else
            {
                SetUIVolume(ZeroVolume);
            }
        }

        public void SetUIVolume(float value)
        {
            audioMixer.SetFloat(UIVolumeParam, value);
        }
    }
}