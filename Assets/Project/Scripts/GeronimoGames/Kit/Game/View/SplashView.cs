using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class SplashView : BasicView
    {
        [Header("BUTTONS")]
        [SerializeField]
        private Button tapButton;

        [Header("TEXTS")]
        [SerializeField]
        private Text tapText;

        [Header("IMAGES")]
        [SerializeField]
        private Image iconImage;

        [SerializeField]
        private Image[] linesImage;

        private int countLine;

        protected override void Start()
        {
            base.Start();

            tapButton.onClick.AddListener(() =>
            {
                tapText.gameObject.SetActive(false);
                iconImage.gameObject.SetActive(false);
                StartCoroutine(AnimStartAsync());
            });

            SetActive(true);
        }

        private IEnumerator AnimStartAsync()
        {
            foreach (var line in linesImage)
            {
                yield return new WaitForSeconds(0.05f);
                StartCoroutine(AnimAsync(line));
            }
        }

        private IEnumerator AnimAsync(Image line)
        {
            while (Math.Abs(line.fillAmount - 1) > 0.0001)
            {
                yield return new WaitForSeconds(0.01f * Time.deltaTime);
                line.fillAmount += 0.075f;
            }

            yield return new WaitForSeconds(0.50f);
            line.fillOrigin = 1;

            while (Math.Abs(line.fillAmount) > 0.0001)
            {
                yield return new WaitForSeconds(0.01f * Time.deltaTime);
                line.fillAmount -= 0.075f;
            }

            countLine++;

            if (countLine == linesImage.Length)
            {
                LoadScene();
            }
        }

        private static void LoadScene()
        {
            ScenesManager.Instance.LoadScene("Main");
        }
    }
}