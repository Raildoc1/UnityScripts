﻿using Office.Character.Player;
using UnityEngine;

namespace Office.Interaction {
    public class Interactable : MonoBehaviour {

        [Tooltip("Will player match target position and rotation.")]
        public bool matchTargetTransform = true;

        public ConditionCollection[] conditionCollections = new ConditionCollection[0];
        public ReactionCollection defaultReactionCollection;

        public void Interact() {

            PlayerInput.instance.blockInput = true;

            for (int i = 0; i < conditionCollections.Length; i++) {
                if (conditionCollections[i].CheckAndReact()) return;
            }

            defaultReactionCollection.React();
        }
    }
}