using System.Collections;
using TMPro;
using UnityEngine;

namespace Office.Props {
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOut : MonoBehaviour {

        public TextMeshProUGUI textMeshProUGUI;

        [Range(0f, 5f)]
        [SerializeField]
        private float delayTime = 1f;

        [Range(0f, 5f)]
        [SerializeField]
        private float timeToFade = 2f;

        private CanvasGroup _blackScreenCanvas;
        private CanvasGroup blackScreenCanvas {
            get { 
                if (!_blackScreenCanvas) _blackScreenCanvas = GetComponent<CanvasGroup>();
                return _blackScreenCanvas;
            }
        }

        private Coroutine fadeRoutine; // stores current routine to turn it off on new coroutine starts before current is finished

        public void Fade(float targetAlpha, bool immediate = false) {
            if (immediate) {
                blackScreenCanvas.alpha = targetAlpha;
            } else {
                if (fadeRoutine != null) StopCoroutine(fadeRoutine);
                fadeRoutine = StartCoroutine(FadeRoutine(targetAlpha));
            }
        }

        public IEnumerator FadeRoutine(float targetAlpha) {

            yield return new WaitForSeconds(delayTime);

            while (!Mathf.Approximately(targetAlpha, blackScreenCanvas.alpha)) {
                blackScreenCanvas.alpha = Mathf.MoveTowards(blackScreenCanvas.alpha, targetAlpha, Time.deltaTime / timeToFade);
                yield return null;
            }

        }

        public void FadeShowText(string text, float secondsLength) {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeShowTextRoutine(text, secondsLength));
        }

        public IEnumerator FadeShowTextRoutine(string text, float secondsLength) {

            yield return FadeRoutine(0f);

            textMeshProUGUI.text = text;
            textMeshProUGUI.enabled = true;

            yield return FadeRoutine(1f);

            yield return new WaitForSeconds(secondsLength);

            yield return FadeRoutine(0f);

            textMeshProUGUI.enabled = false;

        }
    }
}
