using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class BasicViewNavigation : BasicView
    {
        [Header("NAVIGATION")]
        [SerializeField]
        private Button backButton;

        public event Action OnBack;

        protected override void Start()
        {
            base.Start();

            backButton.onClick.AddListener(() => { OnBack?.Invoke(); });
        }
    }
}