using UnityEngine.SceneManagement;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameViews
{
    public static class Loader
    {

        public enum Scene
        {
            InitBootstrap,
            Menu,
            Game,
        }

        public static void Load(Scene scene)
        {
            SceneManager.LoadScene(scene.ToString());
        }
    }
}