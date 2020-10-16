using Office.Interaction;
using System.Collections.Generic;

namespace Office.Character.Dialog {
    [System.Serializable]
    public class DialogSet {
        public string uniqueName;
        public List<DialogLine> lines;

        public DialogSet(string uniqueName) {
            this.uniqueName = uniqueName;
            lines = new List<DialogLine>();
        }

        [System.Serializable]
        public class DialogLine {
            public string lineUniqueName;
            public ReactionCollection reactionCollection;
            public bool isPlayersLine;
            public DialogLine(string lineUniqueName, ReactionCollection reactionCollection, bool isPlayersLine) {
                this.lineUniqueName = lineUniqueName;
                this.reactionCollection = reactionCollection;
                this.isPlayersLine = isPlayersLine;
            }
        }
    }
}
