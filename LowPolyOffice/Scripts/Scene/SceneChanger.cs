using Office.Character.Player;
using Office.Props;
using Office.Save;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Office.SceneManagement {
    public class SceneChanger : MonoBehaviour {

        public event Action BeforeSceneUnload;
        public event Action AfterSceneLoad;

        public readonly string defaultScene = "SCENE_ROOM_A";
        public string initialStartingPositionName = "Door";

        public SaveData playerSaveData;

        public FadeInOut fader;

        private IEnumerator Start() {
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