using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class SignUpView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button signUpButton;

        [SerializeField]
        private Button alreadyAccountButton;

        public event Action OnAlreadyAccount;
        public event Action OnSignUp;

        protected override void Start()
        {
            base.Start();

            alreadyAccountButton.onClick.AddListener(() => { OnAlreadyAccount?.Invoke(); });
            signUpButton.onClick.AddListener(() => { OnSignUp?.Invoke(); });
        }
    }
}