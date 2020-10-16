using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Office.Props {
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOut : MonoBehaviour {

        public bool isFading { get; private set; } = false;

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

        public void Fade(float targetAlpha, bool immediate = false) {
            if (immediate) {
                blackScreenCanvas.alpha = targetAlpha;
            } else {
                StartCoroutine(FadeRoutine(targetAlpha));
            }
        }

        public IEnumerator FadeRoutine(float targetAlpha) {

            while (isFading) {
                yield return null;
            } 

            isFading = true;

            yield return new WaitForSeconds(delayTime);

            while (!Mathf.Approximately(targetAlpha, blackScreenCanvas.alpha)) {
                blackScreenCanvas.alpha = Mathf.MoveTowards(blackScreenCanvas.alpha, targetAlpha, Time.deltaTime / timeToFade);
                yield return null;
            }

            isFading = false;

        }

    }
}
