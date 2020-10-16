using System.Collections.Generic;
using UnityEngine;

namespace Office.Props {
    [RequireComponent(typeof(Collider))]
    public class StepSound : MonoBehaviour {

        public GameObject audioSource;
        public List<AudioClip> clips;

        public float delay;

        private float timer;

        private bool stepTriggerred = true;

        private void Start() {
            timer = delay;
        }

        private void Update() {
            if (stepTriggerred) timer -= Time.deltaTime;
            if (timer < 0f) stepTriggerred = false;
        }

        private void OnTriggerEnter(Collider other) {

            if (stepTriggerred) return;

            if (other.gameObject.tag.Equals("Ground")) {
                //Instantiate(audioSource, transform.position, transform.rotation);
                //var audio = audioSource.GetComponent<AudioSource>();


                var audio = Instantiate(audioSource, transform.position, transform.rotation).GetComponent<AudioSource>();

                audio.clip = clips[Random.Range(0, clips.Count)];

                audio.Play();

                timer = delay;
                stepTriggerred = true;
            }
        }
    }
}

