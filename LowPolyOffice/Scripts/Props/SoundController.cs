using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Office.Props {
    public class SoundController : MonoBehaviour {

        public List<AudioSourceEntity> audios;

        private Animator animator;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        private void FixedUpdate() {
            foreach (var a in audios) {
                if (animator.GetBool(a.animatorBoolName)) a.audioSource.enabled = true;
                else a.audioSource.enabled = false;
            }
        }

        [System.Serializable]
        public class AudioSourceEntity {
            public AudioSource audioSource;
            public string animatorBoolName;
        }
    }
}
