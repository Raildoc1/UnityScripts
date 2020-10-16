using UnityEngine;

namespace Office.Interaction {
    public class AudioReaction : Reaction {
        public GameObject audioSource;     // The AudioSource to play the clip.
        public AudioClip audioClip;         // The AudioClip to be played.
        public Transform audioSourceTransform;
        public float delay;                 // How long after React is called before the clip plays.


        protected override void ImmediateReaction() {

            var audioGameObject = Instantiate(audioSource, audioSourceTransform.position, audioSourceTransform.rotation);

            DontDestroyOnLoad(audioGameObject);

            var audio = audioGameObject.GetComponent<AudioSource>();

            audio.clip = audioClip;
            audio.PlayDelayed(delay);
        }
    }
}