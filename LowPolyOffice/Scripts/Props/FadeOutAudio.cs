using System.Collections;
using UnityEngine;

namespace Office.Props {
    [RequireComponent(typeof(AudioSource))]
    public class FadeOutAudio : MonoBehaviour {

        public float destroyTime;
        [Range(0f, 1f)]
        public float fadeOutTimeProportion = 0.1f;

        private AudioSource audioSource;

        void Start() {
            audioSource = GetComponent<AudioSource>();
            StartCoroutine(FadeRoutine());
        }

        private IEnumerator FadeRoutine() {
            yield return new WaitForSeconds(destroyTime * (1 - fadeOutTimeProportion));
            float initVolume = audioSource.volume;
            while (!Mathf.Approximately(audioSource.volume, 0f)) {
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, (Time.deltaTime  * initVolume) / fadeOutTimeProportion);
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}

