using UnityEngine;

namespace Office.Character {
    [RequireComponent(typeof(Animator))]
    public class HeadLookAt : MonoBehaviour {

        [Tooltip("Turns on/off HeadLookAt")]
        public bool isLookingAt = false;

        [Tooltip("Transform to look at (Set it to null to turn off HeadLookAt)")]
        public Transform lookObj = null;

        [Tooltip("HeadLookAt weight")]
        public float lookWeight = 2f;

        [Tooltip("HeadLookAt offset")]
        public Vector3 offset;

        private Animator animator;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK() {

            if (!animator) return;

            if (isLookingAt) {
                if (lookObj != null) {
                    animator.SetLookAtWeight(lookWeight);
                    animator.SetLookAtPosition(lookObj.position + offset);
                }
            } else {
                animator.SetLookAtWeight(0);
            }
        }
    }
}