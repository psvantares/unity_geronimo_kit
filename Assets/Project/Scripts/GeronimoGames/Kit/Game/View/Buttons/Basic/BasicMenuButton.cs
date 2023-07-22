using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public sealed class BasicMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private Image lineImage;

        private float widthButton;
        private float heightButton;
        private float widthLine;
        private float heightLine;

        private const float TIME_ANIM = 0.1f;

        private float velocity;
        private bool enter;
        private bool exit;

        public event Action OnClick;

        private void Start()
        {
            widthButton = GetComponent<RectTransform>().rect.width;
            heightButton = GetComponent<RectTransform>().rect.height;
            widthLine = lineImage.GetComponent<RectTransform>().rect.width;
            heightLine = lineImage.GetComponent<RectTransform>().rect.height;
        }

        private void Update()
        {
            if (enter)
            {
                GoAnim();
            }

            if (exit)
            {
                BackAnim();
            }
        }

        private void OnEnable()
        {
            enter = false;
            exit = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBGL
            enter = true;
            exit = false;
#endif
        }

        public void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_WEBGL
            enter = false;
            exit = true;
#endif
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            enter = true;
            exit = false;

            StartCoroutine(OnClickAsync());
        }

        public void InitialState()
        {
            enter = false;
            exit = true;
        }

        private void GoAnim()
        {
            var targetLine = lineImage.GetComponent<RectTransform>().rect.width;
            var width = Mathf.SmoothDamp(targetLine, widthButton, ref velocity, TIME_ANIM);

            lineImage.rectTransform.sizeDelta = new Vector2(width, heightButton);
        }

        private void BackAnim()
        {
            var targetLine = lineImage.GetComponent<RectTransform>().rect.width;
            var width = Mathf.SmoothDamp(targetLine, widthLine, ref velocity, TIME_ANIM);

            lineImage.rectTransform.sizeDelta = new Vector2(width, heightLine);
        }

        private IEnumerator OnClickAsync()
        {
            yield return new WaitForSeconds(TIME_ANIM + 0.12f);
            OnClick?.Invoke();
        }
    }
}