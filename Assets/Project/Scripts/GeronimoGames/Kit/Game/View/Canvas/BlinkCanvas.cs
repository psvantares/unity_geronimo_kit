using UnityEngine;

namespace GeronimoGames.Kit.Game.Canvas
{
    public class BlinkCanvas : MonoBehaviour
    {
        [Header("CANVAS")]
        [SerializeField]
        private CanvasGroup canvasGroup;

        private float alphaMin = 0.25f;
        private float alphaMax = 1.0f;
        private float timer;

        private const float SPEED = 1.0f;

        private void Update()
        {
            canvasGroup.alpha = Mathf.Lerp(alphaMin, alphaMax, timer);

            timer += SPEED * Time.deltaTime;

            if (!(timer > 1.0f))
            {
                return;
            }

            (alphaMax, alphaMin) = (alphaMin, alphaMax);
            timer = 0.0f;
        }
    }
}