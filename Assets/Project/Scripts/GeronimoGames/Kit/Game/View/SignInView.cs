using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class SignInView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button loginButton;

        [SerializeField]
        private Button createButton;

        [SerializeField]
        private Button forgotButton;

        public event Action OnLogin;
        public event Action OnCreate;
        public event Action OnForgot;

        protected override void Start()
        {
            base.Start();

            loginButton.onClick.AddListener(() => { OnLogin?.Invoke(); });
            createButton.onClick.AddListener(() => { OnCreate?.Invoke(); });
            forgotButton.onClick.AddListener(() => { OnForgot?.Invoke(); });
        }
    }
}