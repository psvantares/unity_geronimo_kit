using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public enum SettingsType
    {
        FontSize = 0,
        FontCustom = 1,
        Language = 2,
        Theme = 3,
    }

    public class SettingButtonView : MonoBehaviour
    {
        [Header("TEXTS")]
        [SerializeField]
        private Text settingText;

        [Header("SETTINGS")]
        [SerializeField]
        private SettingsType settingsType;

        private Button settingButton;

        public event Action<SettingsType> OnClick;

        public SettingsType Type
        {
            get => settingsType;
            set => settingsType = value;
        }

        private void Start()
        {
            settingButton = GetComponent<Button>();

            if (settingButton != null)
            {
                settingButton.onClick.AddListener(() => { OnClick?.Invoke(Type); });
            }
        }

        public void SetText(string paramName)
        {
            if (settingText != null)
            {
                settingText.text = paramName;
            }
        }
    }
}