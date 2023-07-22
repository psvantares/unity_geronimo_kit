using GeronimoGames.Kit.Utilities;
using UnityEngine;

namespace GeronimoGames.Kit.Game
{
    public class BasicView : MonoBehaviour
    {
        [Header("ROOT")]
        [SerializeField]
        private GameObject root;

        private CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = root.AddComponent<CanvasGroup>();
            canvasGroup.alpha = 0.0f;
        }

        protected virtual void Start()
        {
        }

        public void SetActive(bool value)
        {
            root.SetActive(value);

            StopAllCoroutines();

            StartCoroutine(value
                ? Fade.FadeIn(canvasGroup, 1.0f, 0.5f)
                : Fade.FadeOut(canvasGroup, 0.0f, 0.5f));
        }
    }
}