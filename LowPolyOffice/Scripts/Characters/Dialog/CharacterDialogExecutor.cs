using Office.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Office.Character.Dialog {
    public class CharacterDialogExecutor : DialogExecutor {

        private string animatorBoolName = "";

        private HeadLookAt headLookAt {
            get {
                return GetComponent<HeadLookAt>();
            }
        }

        public override void StartDialog(string dialogUniqueName, string animatorBoolName = "") {
            if (headLookAt) headLookAt.lookObj = GameObject.FindGameObjectWithTag("Player").transform;
            this.animatorBoolName = animatorBoolName;
            foreach (var dialogSet in dialogSets) {
                if (dialogSet.uniqueName.Equals(dialogUniqueName)) {
                    ScreenTextWriter.instance.StartDialog(dialogSet, this);
                    break;
                }
            }
        }

        public override void EndDialog() {
            if (headLookAt) headLookAt.lookObj = null;
            if (!animatorBoolName.Equals("")) {
                GetComponent<Animator>().SetBool(animatorBoolName, false);
            }
        }
    }
}
