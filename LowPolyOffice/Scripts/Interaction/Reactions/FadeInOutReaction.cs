// Fade Out -> Show text for N seconds -> Fade In

using Office.Props;

namespace Office.Interaction {
    public class FadeInOutReaction : Reaction {

        public string text = "New Text";
        public float secondsLength = 1f;

        private FadeInOut fader;

        protected override void SpecificInit() {
            fader = FindObjectOfType<FadeInOut>();
        }

        protected override void ImmediateReaction() {
            fader.FadeShowText(text, secondsLength);
        }
    }
}
