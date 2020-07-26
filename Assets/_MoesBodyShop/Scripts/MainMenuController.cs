namespace MoesBodyShop
{
    using UnityEngine;

    public class MainMenuController : MonoBehaviour
    {
        public void CloseMenu()
        {
            this.gameObject.SetActive(false);
        }
    }
}