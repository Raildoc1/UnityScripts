using System.Collections.Generic;
using UnityEngine;

namespace Office.Character.Player {
    public class StartingPosition : MonoBehaviour {

        public static List<StartingPosition> allPositions = new List<StartingPosition>();

        public string positionName;

        private void OnEnable() {
            allPositions.Add(this);
        }

        private void OnDisable() {
            allPositions.Remove(this);
        }

        public static Transform FindPositionName(string name) {

            foreach (var pos in allPositions) {
                if (pos.positionName.Equals(name)) return pos.transform;
            }

            return null;
        }

    }
}