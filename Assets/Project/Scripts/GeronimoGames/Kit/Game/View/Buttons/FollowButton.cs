using UnityEngine;
using UnityEngine.UI;

namespace GeronimoGames.Kit.Game
{
    public class FollowButton : MonoBehaviour
    {
        [Header("SETTINGS")]
        [SerializeField]
        private string url;

        private void Start()
        {
            var button = gameObject.GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => { Application.OpenURL(url); });
            }
        }
    }
}