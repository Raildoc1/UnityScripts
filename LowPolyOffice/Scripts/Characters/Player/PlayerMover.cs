using Office.Save;
using UnityEngine;

namespace Office.Character.Player {
    public class PlayerMover : Mover {

        public SaveData playerSaveData;

        public static readonly string playerStartingPositionKey = "PlayerStartingPosition";

        protected override void Start() {
            base.Start();
            string startingPositionName = "";
            playerSaveData.Load(playerStartingPositionKey, ref startingPositionName);
            Transform initTransform = StartingPosition.FindPositionName(startingPositionName);
            if (initTransform) {
                transform.position = initTransform.position;
                transform.rotation = initTransform.rotation;
            }
            destinationPosition = transform.position;
        }
    }
}
