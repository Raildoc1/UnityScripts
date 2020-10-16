using Office.Text;

namespace Office.Interaction {
    public class TextReaction : Reaction {

        public string textUniqueName;

        protected override void ImmediateReaction() {
            ScreenTextWriter.instance.WriteTextByUniqueName(textUniqueName);
        }
    }
}
