using UnityEngine;

namespace GeronimoGames.Kit.Game
{
    public class MainController : MonoBehaviour
    {
        [Header("VIEWS")]
        [SerializeField]
        private LogoView logoView;

        [SerializeField]
        private MenuView menuView;

        [SerializeField]
        private SignInView signInView;

        [SerializeField]
        private SignUpView signUpView;

        [SerializeField]
        private RecoveryView recoveryView;

        [SerializeField]
        private TermsView termsView;

        [SerializeField]
        private WalkthroughView walkThroughView;

        [SerializeField]
        private ProfileView profileView;

        [SerializeField]
        private AboutUsView aboutUsView;

        [SerializeField]
        private HomeView homeView;

        [SerializeField]
        private PortfolioView portfolioView;

        [SerializeField]
        private SettingsView settingsView;

        private void Start()
        {
            signUpView.OnSignUp += SignUpViewOnSignUp;
            signUpView.OnAlreadyAccount += SignUpViewOnAlreadyAccount;

            signInView.OnLogin += SignInViewOnLogin;
            signInView.OnCreate += SignInViewOnCreate;
            signInView.OnForgot += SignInViewOnForgot;

            recoveryView.OnSend += RecoveryViewOnSend;
            recoveryView.OnAlreadyAccount += RecoveryViewOnAlreadyAccount;

            walkThroughView.OnClick += WalkThroughViewOnClick;

            menuView.OnHome += MenuViewOnHome;
            menuView.OnProfile += MenuViewOnProfile;
            menuView.OnAboutUs += MenuViewOnAboutUs;
            menuView.OnPortfolio += MenuViewOnPortfolio;
            menuView.OnSettings += MenuViewOnSettings;
            menuView.OnLogout += MenuViewOnLogout;

            profileView.OnBack += ProfileViewOnBack;
            aboutUsView.OnBack += AboutUsViewOnBack;
            homeView.OnBack += HomeViewOnBack;
            portfolioView.OnBack += PortfolioViewOnBack;
            settingsView.OnBack += SettingsViewOnBack;

            Init();
        }

        private void OnDisable()
        {
            signUpView.OnSignUp -= SignUpViewOnSignUp;
            signUpView.OnAlreadyAccount -= SignUpViewOnAlreadyAccount;

            signInView.OnLogin -= SignInViewOnLogin;
            signInView.OnCreate -= SignInViewOnCreate;
            signInView.OnForgot -= SignInViewOnForgot;

            recoveryView.OnSend -= RecoveryViewOnSend;
            recoveryView.OnAlreadyAccount -= RecoveryViewOnAlreadyAccount;

            walkThroughView.OnClick -= WalkThroughViewOnClick;

            menuView.OnHome -= MenuViewOnHome;
            menuView.OnProfile -= MenuViewOnProfile;
            menuView.OnAboutUs -= MenuViewOnAboutUs;
            menuView.OnPortfolio -= MenuViewOnPortfolio;
            menuView.OnSettings -= MenuViewOnSettings;
            menuView.OnLogout -= MenuViewOnLogout;

            profileView.OnBack -= ProfileViewOnBack;
            aboutUsView.OnBack -= AboutUsViewOnBack;
            homeView.OnBack -= HomeViewOnBack;
            portfolioView.OnBack -= PortfolioViewOnBack;
            settingsView.OnBack -= SettingsViewOnBack;
        }

        private void Init()
        {
            walkThroughView.SetActive(true);
        }

        private void WalkThroughViewOnClick()
        {
            walkThroughView.SetActive(false);

            logoView.SetActive(true);
            termsView.SetActive(true);
            signUpView.SetActive(true);
        }

        private void SignUpViewOnSignUp()
        {
        }

        private void SignUpViewOnAlreadyAccount()
        {
            signUpView.SetActive(false);

            signInView.SetActive(true);
        }

        private void SignInViewOnLogin()
        {
            logoView.SetActive(false);
            termsView.SetActive(false);
            signInView.SetActive(false);

            menuView.SetActive(true);
        }

        private void SignInViewOnCreate()
        {
            signInView.SetActive(false);

            signUpView.SetActive(true);
        }

        private void SignInViewOnForgot()
        {
            signInView.SetActive(false);

            recoveryView.SetActive(true);
        }

        private void RecoveryViewOnSend()
        {
        }

        private void RecoveryViewOnAlreadyAccount()
        {
            recoveryView.SetActive(false);

            signInView.SetActive(true);
        }

        private void ProfileViewOnBack()
        {
            profileView.SetActive(false);

            menuView.SetActive(true);
        }

        private void MenuViewOnHome()
        {
            menuView.SetActive(false);

            homeView.SetActive(true);
        }

        private void MenuViewOnProfile()
        {
            menuView.SetActive(false);

            profileView.SetActive(true);
        }

        private void MenuViewOnAboutUs()
        {
            menuView.SetActive(false);

            aboutUsView.SetActive(true);
        }

        private void MenuViewOnPortfolio()
        {
            menuView.SetActive(false);

            portfolioView.SetActive(true);
        }

        private void MenuViewOnSettings()
        {
            menuView.SetActive(false);

            settingsView.DisableParams();
            settingsView.SetActive(true);
        }

        private void AboutUsViewOnBack()
        {
            aboutUsView.SetActive(false);

            menuView.SetActive(true);
        }

        private void HomeViewOnBack()
        {
            homeView.SetActive(false);

            menuView.SetActive(true);
        }

        private void PortfolioViewOnBack()
        {
            portfolioView.SetActive(false);

            menuView.SetActive(true);
        }

        private void SettingsViewOnBack()
        {
            settingsView.SetActive(false);

            menuView.SetActive(true);
        }

        private void MenuViewOnLogout()
        {
            menuView.SetActive(false);

            walkThroughView.SetActive(true);
        }
    }
}