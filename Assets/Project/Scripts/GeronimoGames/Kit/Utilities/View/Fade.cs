using System.Collections;
using UnityEngine;

namespace GeronimoGames.Kit.Utilities
{
    public static class Fade
    {
        public static IEnumerator FadeIn(CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }

        public static IEnumerator FadeOut(CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;

            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }
    }
}