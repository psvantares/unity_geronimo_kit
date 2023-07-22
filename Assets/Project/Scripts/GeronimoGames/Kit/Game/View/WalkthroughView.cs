using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class WalkthroughView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button getStartedButton;

        public event Action OnClick;

        protected override void Start()
        {
            base.Start();

            getStartedButton.onClick.AddListener(() => { OnClick?.Invoke(); });
        }
    }
}