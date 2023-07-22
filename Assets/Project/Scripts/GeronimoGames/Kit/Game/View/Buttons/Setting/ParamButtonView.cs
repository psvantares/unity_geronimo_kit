using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class ParamButtonView : MonoBehaviour
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button paramButton;

        [Header("TEXTS")]
        [SerializeField]
        private Text paramText;

        [Header("SETTINGS")]
        [SerializeField]
        private SettingsType settingsType;

        private string paramName;
        private int paramId;

        public event Action<string, int, SettingsType> OnClick;

        private void Start()
        {
            paramButton.onClick.AddListener(() => { OnClick?.Invoke(paramName, paramId, settingsType); });
        }

        public void Initialize(ParamModel model, SettingsType settingsType)
        {
            this.settingsType = settingsType;

            paramName = model.paramName;
            paramId = model.paramId;

            paramText.text = paramName;
        }
    }
}