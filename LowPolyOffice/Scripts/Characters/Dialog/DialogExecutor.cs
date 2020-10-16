using Office.Character.Player;
using Office.Text;
using System.Collections.Generic;
using UnityEngine;

namespace Office.Character.Dialog {
    public class DialogExecutor : MonoBehaviour {

        public List<DialogSet> dialogSets = new List<DialogSet>();

        public virtual void StartDialog(string dialogUniqueName, string animatorBoolName = "") {
            foreach (var dialogSet in dialogSets) {
                if (dialogSet.uniqueName.Equals(dialogUniqueName)) {
                    ScreenTextWriter.instance.StartDialog(dialogSet, this);
                    break;
                }
            }
        }

        public virtual void EndDialog() { }
    }
}
