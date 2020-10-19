using Office.Character.Player;
using Office.Props;
using Office.Save;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Office.SceneManagement {
    public class SceneChanger : MonoBehaviour {

        #region Events
        public event Action BeforeSceneUnload; // Called than change scene before current scene is unloaded
        public event Action AfterSceneLoad; // Called than change scene after new scene is loaded
        #endregion

        [Tooltip("Scene to load on start game if no scenes loaded")]
        public readonly string defaultScene = "SCENE_ROOM_A";

        [Tooltip("Name of starting point to set player on than game startes.")]
        public string initialStartingPositionName = "Door";

        [Tooltip("SaveData to store player position and rotation")]
        public SaveData playerSaveData;

        private FadeInOut fader;

        private IEnumerator Start() {

            fader = FindObjectOfType<FadeInOut>();

            PlayerInput.instance.blockInput = true;
            fader.Fade(1f, true);

            playerSaveData.Save(PlayerMover.playerStartingPositionKey, initialStartingPositionName);

            if (SceneManager.sceneCount == 1) {
                yield return StartCoroutine(LoadSceneAndSetActive(defaultScene));
            } else {
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));
            }

            fader.Fade(0f);
            PlayerInput.instance.blockInput = false;
        }

        public void FadeAndLoadScene(string sceneName) {
            StartCoroutine(FadeAndSwitchScenes(sceneName));
        }

        private IEnumerator LoadSceneAndSetActive(string sceneName) {

            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            Scene newlyLoadScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

            SceneManager.SetActiveScene(newlyLoadScene);
        }

        private IEnumerator FadeAndSwitchScenes(string sceneName) {
            PlayerInput.instance.blockInput = true;

            yield return fader.FadeRoutine(1f);

            BeforeSceneUnload?.Invoke();

            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

            yield return fader.FadeRoutine(0f);

            AfterSceneLoad?.Invoke();

            PlayerInput.instance.blockInput = false;
        }

    }
}