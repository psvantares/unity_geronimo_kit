using System;
using UnityEngine;

namespace GeronimoGames.Kit.Game
{
    public class MenuView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private BasicMenuButton homeButton;

        [SerializeField]
        private BasicMenuButton profileButton;

        [SerializeField]
        private BasicMenuButton aboutUsButton;

        [SerializeField]
        private BasicMenuButton portfolioButton;

        [SerializeField]
        private BasicMenuButton settingsButton;

        [SerializeField]
        private BasicMenuButton logoutButton;

        public event Action OnHome;
        public event Action OnProfile;
        public event Action OnAboutUs;
        public event Action OnPortfolio;
        public event Action OnSettings;
        public event Action OnLogout;

        protected override void Start()
        {
            base.Start();

            homeButton.OnClick += HomeButtonOnClick;
            profileButton.OnClick += ProfileButtonOnClick;
            aboutUsButton.OnClick += AboutUsButtonOnClick;
            portfolioButton.OnClick += PortfolioButtonOnClick;
            settingsButton.OnClick += SettingsButtonOnClick;
            logoutButton.OnClick += LogoutButtonOnClick;
        }

        private void HomeButtonOnClick()
        {
            OnHome?.Invoke();
        }

        private void ProfileButtonOnClick()
        {
            OnProfile?.Invoke();
        }

        private void AboutUsButtonOnClick()
        {
            OnAboutUs?.Invoke();
        }

        private void PortfolioButtonOnClick()
        {
            OnPortfolio?.Invoke();
        }

        private void SettingsButtonOnClick()
        {
            OnSettings?.Invoke();
        }

        private void LogoutButtonOnClick()
        {
            OnLogout?.Invoke();
        }
    }
}