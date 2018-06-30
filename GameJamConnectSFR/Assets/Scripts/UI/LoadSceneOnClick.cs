using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class LoadSceneOnClick : MonoBehaviour
    {
        public int SceneIndex;
        public LoadSceneMode LoadSceneMode;
        public bool AsyncLoad;

        public void LoadScene()
        {
            if (AsyncLoad)
            {
                SceneManager.LoadSceneAsync(SceneIndex, LoadSceneMode);
            }
            else
            {
                SceneManager.LoadScene(SceneIndex, LoadSceneMode);
            }
        }
    }
}
