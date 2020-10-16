using UnityEngine;

namespace Office.Interaction {
    public class BehaviourReaction : DelayedReaction {
        public Behaviour behaviour;     // The Behaviour to be turned on or off.
        public bool enabledState;       // The state the Behaviour will be in after the Reaction.


        protected override void ImmediateReaction() {
            behaviour.enabled = enabledState;
        }
    }
}