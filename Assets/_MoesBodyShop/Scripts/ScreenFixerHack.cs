namespace MoesBodyShop
{
    using UnityEngine;

    public class ScreenFixerHack : MonoBehaviour
    {
        [SerializeField] private GameObject screenCamera;

        private void Awake()
        {
            screenCamera.SetActive(false);
            screenCamera.SetActive(true);
        }
    }
}