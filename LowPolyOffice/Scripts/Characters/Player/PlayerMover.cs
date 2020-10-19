using Office.Save;
using UnityEngine;

namespace Office.Character.Player {
    public class PlayerMover : Mover {

        [Tooltip("SaveData to store player position and rotation")]
        public SaveData playerSaveData;

        [Tooltip("Name of initial starting point")]
        public static readonly string playerStartingPositionKey = "PlayerStartingPosition";

        protected override void Start() {

            base.Start();

            string startingPositionName = "";

            // Trying to Load players position and set Player to it
            playerSaveData.Load(playerStartingPositionKey, ref startingPositionName);

            Transform initTransform = StartingPosition.FindPositionName(startingPositionName);

            if (initTransform) {
                transform.position = initTransform.position;
                transform.rotation = initTransform.rotation;
            }

            // Init destination position to make Player stand still on start 
            destinationPosition = transform.position;
        }
    }
}
