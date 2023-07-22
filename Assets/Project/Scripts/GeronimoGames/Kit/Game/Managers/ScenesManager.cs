using System.Collections;
using GeronimoGames.Kit.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GeronimoGames.Kit.Game
{
    public class ScenesManager : Singleton<ScenesManager>
    {
        public void LoadScene(string nameScene)
        {
            StartCoroutine(LoadSceneAsync(nameScene));
        }

        public void UnloadScene(string nameScene)
        {
            StartCoroutine(UnloadSceneAsync(nameScene));
        }

        private static IEnumerator LoadSceneAsync(string nameScene)
        {
            yield return new WaitForSeconds(0.5f);
            var async = SceneManager.LoadSceneAsync(nameScene, LoadSceneMode.Single);

            while (!async.isDone)
            {
                yield return null;
            }

            var scn = SceneManager.GetSceneByName(nameScene);
            SceneManager.SetActiveScene(scn);
        }

        private static IEnumerator UnloadSceneAsync(string nameScene)
        {
            var async = SceneManager.UnloadSceneAsync(nameScene);

            yield return new WaitForSeconds(0.5f);

            while (true)
            {
                if (async is { isDone: true })
                {
                    var asyncUnload = Resources.UnloadUnusedAssets();
                    yield return asyncUnload;

                    yield break;
                }

                yield return null;
            }
        }
    }
}