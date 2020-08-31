namespace MoesBodyShop
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Events;

    public class GeneratorController : MonoBehaviour
    {
        [SerializeField] private AudioSource generatorSound;
        [SerializeField] private AudioSource pumpSound;
        [SerializeField] private Animator generatorAnimator;
        [SerializeField] private MeshRenderer[] lights;
        [SerializeField] private Material offMat;
        [SerializeField] private Material onMat;

        public UnityEvent OnActivate;
        public UnityEvent OnPump;

        private const byte MaxPumpsRequired = 3;
        private const string PumpParam = "Pump";
        private const float PumpAnimDuration = 1f;

        private byte _pumpCount = 0;
        private bool _isActivated = false;
        private bool _isPumping = false;

        public void ResetGenerator()
        {
            _pumpCount = 0;
            _isActivated = false;
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].material = offMat;
            }
            generatorSound.Stop();

            StopAllCoroutines();
        }

        public void PumpGenerator()
        {
            if (!_isActivated && !_isPumping)
            {
                if (OnPump != null)
                {
                    OnPump.Invoke();
                }
                pumpSound.Play();
                StartCoroutine(PumpRoutine(PumpAnimDuration, _pumpCount));
                generatorAnimator.SetTrigger(PumpParam);
                _pumpCount++;
                if (_pumpCount >= MaxPumpsRequired)
                {
                    ActivateGenerator();
                }
            }
        }

        public void ActivateGenerator()
        {
            generatorSound.Play();
            if (OnActivate != null)
            {
                OnActivate.Invoke();
            }
        }

        private IEnumerator PumpRoutine(float animDuration, byte lightIndex)
        {
            _isPumping = true;

            yield return new WaitForSeconds(animDuration);
            lights[lightIndex].material = onMat;

            _isPumping = false;
        }
    }
}