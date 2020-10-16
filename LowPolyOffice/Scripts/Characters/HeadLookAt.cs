using UnityEngine;
using System;
using System.Collections;

namespace Office.Character {
    [RequireComponent(typeof(Animator))]
    public class HeadLookAt : MonoBehaviour {

        protected Animator animator;
        public bool ikActive = false;
        public Transform lookObj = null;
        public float lookWeight = 2f;
        public Vector3 offset;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        private void OnAnimatorIK() {

            if (!animator) return;

            if (ikActive) {
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