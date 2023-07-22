using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class SwitchToggle : MonoBehaviour
    {
        [Header("IMAGES")]
        [SerializeField]
        private Image backgroundImage;

        [SerializeField]
        private Image offImage;

        [SerializeField]
        private Image onImage;

        private Toggle tglSwitch;
        private Color color;

        private void Start()
        {
            tglSwitch = GetComponent<Toggle>();
            color = backgroundImage.color;

            if (tglSwitch != null)
            {
                if (tglSwitch.isOn)
                {
                    backgroundImage.color = Color.green;
                }

                tglSwitch.onValueChanged.AddListener((value) =>
                {
                    if (value)
                    {
                        backgroundImage.color = Color.green;
                        offImage.gameObject.SetActive(false);
                        onImage.gameObject.SetActive(true);
                    }
                    else
                    {
                        backgroundImage.color = color;
                        onImage.gameObject.SetActive(false);
                        offImage.gameObject.SetActive(true);
                    }
                });
            }
        }
    }
}