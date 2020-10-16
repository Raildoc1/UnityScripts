using Office.Character.Dialog;
using Office.Character.Player;
using UnityEngine;

namespace Office.Interaction {
    public class TriggerDialogReaction : DelayedReaction {

        public bool hasAnimator = false;
        public DialogExecutor dialogExecutor;
        public string dialogUniqueName;
        public string animatorBoolName;

        protected override void ImmediateReaction() {
            PlayerInput.instance.blockRaycastInput = true;
            if (hasAnimator) dialogExecutor.GetComponent<Animator>().SetBool(animatorBoolName, true);
            dialogExecutor.StartDialog(dialogUniqueName, animatorBoolName);
        }
    }
}
