using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Xyz.MomsSpaghettiCode.CrossWorlds.GameLogic.Network
{
    public class SceneTransitionHandler : NetworkBehaviour
    {
        public static SceneTransitionHandler sceneTransitionHandler { get; internal set; }

        [SerializeField] public string DefaultMainMenu = "Menu";

        public delegate void ClientLoadedSceneDelegateHandler(ulong clientId);
        public event ClientLoadedSceneDelegateHandler OnClientLoadedScene;
        
        // public delegate void SceneStateChangedDelegateHandler(SceneStates newState)
        [HideInInspector]
        public delegate void SceneStateChangedDelegateHandler(SceneStates newState);
        [HideInInspector]
        public event SceneStateChangedDelegateHandler OnSceneStateChanged;

        
        private int m_numberOfClientsLoaded;

        public enum SceneStates
        {
            Init,
            Menu,
            Lobby,
            Play,
        }

        private SceneStates m_SceneState;

        private void Awake()
        {
            if (sceneTransitionHandler != this && sceneTransitionHandler != null)
            {
                GameObject.Destroy(sceneTransitionHandler.gameObject);
            }

            sceneTransitionHandler = this;
            SetSceneState(SceneStates.Init);
        }

        private void SetSceneState(SceneStates sceneState)
        {
            m_SceneState = sceneState;
            if (OnSceneStateChanged != null)
            {
                OnSceneStateChanged.Invoke(m_SceneState);
            }
        }

        public SceneStates GetCurrentSceneState()
        {
            return m_SceneState;
        }

        private void Start()
        {
            if (m_SceneState == SceneStates.Init)
            {
                SceneManager.LoadScene(DefaultMainMenu);
            }
        }

        public void SwitchScene(string sceneName)
        {
            if (NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
                NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            //We are only interested by Client Loaded Scene events
            if (sceneEvent.SceneEventType != SceneEventType.LoadComplete) return;

            m_numberOfClientsLoaded += 1;
            OnClientLoadedScene?.Invoke(sceneEvent.ClientId);
        }
        
        
        public bool AllClientsAreLoaded()
        {
            return m_numberOfClientsLoaded == NetworkManager.Singleton.ConnectedClients.Count;
        }

        /// <summary>
        /// ExitAndLoadStartMenu
        /// This should be invoked upon a user exiting a multiplayer game session.
        /// </summary>
        public void ExitAndLoadStartMenu()
        {
            OnClientLoadedScene = null;
            SetSceneState(SceneStates.Menu);
            SceneManager.LoadScene(1);
        }
    }
}