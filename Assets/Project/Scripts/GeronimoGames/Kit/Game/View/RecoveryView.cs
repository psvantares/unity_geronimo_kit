using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class RecoveryView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button sendButton;

        [SerializeField]
        private Button alreadyAccountButton;

        public event Action OnSend;
        public event Action OnAlreadyAccount;

        protected override void Start()
        {
            base.Start();

            sendButton.onClick.AddListener(() => { OnSend?.Invoke(); });
            alreadyAccountButton.onClick.AddListener(() => { OnAlreadyAccount?.Invoke(); });
        }
    }
}