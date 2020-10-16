using Office.Character.Player;
using Office.Save;
using Office.SceneManagement;

namespace Office.Interaction {
    public class SceneReaction : Reaction {
        public string sceneName;
        public string startingPointInLoadedScene;
        public SaveData playerSaveData;

        private SceneChanger sceneController;

        protected override void SpecificInit() {
            sceneController = FindObjectOfType<SceneChanger>();
        }


        protected override void ImmediateReaction() {
            playerSaveData.Save(PlayerMover.playerStartingPositionKey, startingPointInLoadedScene);
            sceneController.FadeAndLoadScene(sceneName);
        }
    }
}