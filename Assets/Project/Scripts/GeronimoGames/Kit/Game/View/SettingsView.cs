using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class SettingsView : BasicViewNavigation
    {
        [Header("TOGGLES")]
        [SerializeField]
        private Toggle appToggle;

        [SerializeField]
        private Toggle cameraToggle;

        [SerializeField]
        private Toggle networkToggle;

        [SerializeField]
        private Toggle otherToggle;

        [Header("GAME OBJECTS")]
        [SerializeField]
        private GameObject appGo;

        [SerializeField]
        private GameObject cameraGo;

        [SerializeField]
        private GameObject networkGo;

        [SerializeField]
        private GameObject otherGo;

        [SerializeField]
        private GameObject containerGo;

        [Header("PART")]
        [SerializeField]
        private SettingsPartView settingsPartView;

        [Header("PREFABS")]
        [SerializeField]
        private GameObject paramButtonPrefab;

        private List<ParamButtonView> lstParamButton;

        protected override void Start()
        {
            base.Start();

            lstParamButton = new List<ParamButtonView>();

            appToggle.onValueChanged.AddListener((value) =>
            {
                if (!value)
                {
                    return;
                }

                DisableParams();

                appGo.SetActive(true);
                cameraGo.SetActive(false);
                networkGo.SetActive(false);
                otherGo.SetActive(false);
            });

            cameraToggle.onValueChanged.AddListener((value) =>
            {
                if (!value)
                {
                    return;
                }

                DisableParams();

                cameraGo.SetActive(true);
                appGo.SetActive(false);
                networkGo.SetActive(false);
                otherGo.SetActive(false);
            });

            networkToggle.onValueChanged.AddListener((value) =>
            {
                if (!value)
                {
                    return;
                }

                DisableParams();

                networkGo.SetActive(true);
                cameraGo.SetActive(false);
                appGo.SetActive(false);
                otherGo.SetActive(false);
            });

            otherToggle.onValueChanged.AddListener((value) =>
            {
                if (!value)
                {
                    return;
                }

                DisableParams();

                otherGo.SetActive(true);
                cameraGo.SetActive(false);
                networkGo.SetActive(false);
                appGo.SetActive(false);
            });

            Init();
        }

        private void Init()
        {
            settingsPartView.OnClick += SettingsPartViewOnClick;
        }

        public void DisableParams()
        {
            containerGo.SetActive(false);
            DestroyElements();
        }

        private void SettingsPartViewOnClick(SettingsType type)
        {
            DestroyElements();

            var data = LocalCore.GetDataFromJsonFromPath<ParamModel>(type.ToString(), "Settings").ToList();

            containerGo.SetActive(true);
            InitParams(data, type);
        }

        private void InitParams(IEnumerable<ParamModel> data, SettingsType type)
        {
            foreach (var model in data)
            {
                var param = Instantiate(paramButtonPrefab, containerGo.transform, false);
                param.transform.localScale = Vector3.one;
                param.transform.SetAsFirstSibling();

                var button = param.GetComponent<ParamButtonView>();
                button.Initialize(model, type);

                button.OnClick += ButtonOnClick;

                lstParamButton.Add(button);
            }
        }

        private void ButtonOnClick(string paramName, int paramId, SettingsType type)
        {
            containerGo.SetActive(false);
            settingsPartView.Initialize(paramName, type);
        }

        private void DestroyElements()
        {
            if (lstParamButton == null)
            {
                return;
            }

            foreach (var element in lstParamButton)
            {
                Destroy(element.gameObject);
            }

            lstParamButton.Clear();
        }
    }
}