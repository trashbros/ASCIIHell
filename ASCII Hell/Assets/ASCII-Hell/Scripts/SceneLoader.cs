using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;
        public Canvas loadingScreen;
        private bool loadScene = false;

        private Scene sceneLoaderScene;
        private Scene gameControllerScene;

        private int sceneLoadingCount = 0;
        private int sceneUnloadingCount = 0;

        [SerializeField]
        private int scene;
        [SerializeField]
        private int oldScene;
        [SerializeField]
        private TMPro.TextMeshProUGUI loadingText;

        // called zero
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
            //loadingScreen = this.GetComponentInChildren<Canvas>();
            //loadingScreen.gameObject.SetActive(false);

            //sceneLoaderScene = SceneManager.GetSceneByName("SceneHandler");

            //LoadNewScene("StartScreen");
        }

        // called first
        void OnEnable()
        {
            Debug.Log("OnEnable called");
            //SceneManager.sceneLoaded += OnSceneLoaded;
            //SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        // called second
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);

            loadScene = false;
            //loadingScreen.gameObject.SetActive(false);
        }

        void OnSceneUnloaded(Scene scene)
        {
            //loadingScreen.gameObject.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            //Debug.Log("Start");
        }


        // Update is called once per frame
        void Update()
        {
            // If the new scene has started loading...
            if (loadScene == true)
            {

                // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
                //loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

            }
        }

        // called when the game is terminated
        void OnDisable()
        {
            //Debug.Log("OnDisable");
            //SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void LoadNewScene(string sceneNew, LoadSceneMode options = LoadSceneMode.Additive)
        {
            //loadingScreen.gameObject.SetActive(true);
            loadScene = true;

            StartCoroutine(LoadScene(sceneNew,options));
        }

        public void UnloadOldScene(string sceneOld, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            if (SceneManager.GetSceneByName(sceneOld) != null)
            {
                //loadingScreen.gameObject.SetActive(true);
                loadScene = true;

                StartCoroutine(UnloadScene(sceneOld,options));
            }
        }

        private Scene[] GetLoadedScenes()
        {
            int countLoaded = SceneManager.sceneCount;
            Scene[] loadedScenes = new Scene[countLoaded];

            for (int i = 0; i < countLoaded; i++)
            {
                loadedScenes[i] = SceneManager.GetSceneAt(i);
            }

            return loadedScenes;
        }


        #region Coroutines
        // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to unload.
        IEnumerator UnloadScene(string scene, UnloadSceneOptions options = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects)
        {
            sceneUnloadingCount += 1;

            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            AsyncOperation async = SceneManager.UnloadSceneAsync(scene, options);

            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone)
            {
                yield return null;
            }

            //yield return new WaitForSeconds(3);

            sceneUnloadingCount -= 1;

            if (sceneLoadingCount == 0 && sceneUnloadingCount == 0)
            {
                //loadingScreen.gameObject.SetActive(false);
                loadScene = false;
            }
        }

        // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
        IEnumerator LoadScene(string scene, LoadSceneMode options = LoadSceneMode.Additive)
        {
            sceneLoadingCount += 1;

            // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
            AsyncOperation async = SceneManager.LoadSceneAsync(scene, options);

            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone)
            {
                yield return null;
            }

            //yield return new WaitForSeconds(3);

            sceneLoadingCount -= 1;
            if (sceneLoadingCount == 0 && sceneUnloadingCount == 0)
            {
                //loadingScreen.gameObject.SetActive(false);
                loadScene = false;
            }
        }
        #endregion

    }

