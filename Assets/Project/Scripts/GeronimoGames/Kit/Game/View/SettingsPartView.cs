using System;
using UnityEngine;

namespace GeronimoGames.Kit.Game
{
    public class SettingsPartView : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField]
        private SettingButtonView[] settingsButton;

        public event Action<SettingsType> OnClick;

        private void Start()
        {
            foreach (var button in settingsButton)
            {
                button.OnClick += ButtonOnClick;
            }
        }

        public void Initialize(string paramName, SettingsType type)
        {
            foreach (var button in settingsButton)
            {
                if (button.Type == type)
                {
                    button.SetText(paramName);
                }
            }
        }

        private void ButtonOnClick(SettingsType type)
        {
            OnClick?.Invoke(type);
        }
    }
}