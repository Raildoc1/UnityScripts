using Office.Character;
using Office.Character.Player;
using System.Collections;
using UnityEngine;

namespace Office.Interaction {
    [RequireComponent(typeof(Mover))]
    public class PlayerInteract : MonoBehaviour {

        private Coroutine currentAction;
        private Interactable currentTarget;
        private Mover mover;


        private void Start() {
            mover = GetComponent<Mover>();
        }

        private void OnEnable() {
            PlayerInput.instance.onInteractableClickEvent.AddListener(Interact);
        }

        private void OnDisable() {
            PlayerInput.instance.onInteractableClickEvent.RemoveListener(Interact);
        }

        public void Interact(Interactable interactable, bool isRunning = false) {
            if (currentTarget == interactable) return;
            currentTarget = interactable;
            if (currentAction != null) StopCoroutine(currentAction);
            if (interactable) currentAction = StartCoroutine(InteractionRoutine(interactable, isRunning));
        }

        private IEnumerator InteractionRoutine(Interactable interactable, bool isRunning = false) {

            Transform targetTransform = interactable.transform.Find("TargetTransform");

            if (!targetTransform) {
                Debug.Log("TargetTransform not found!");
                targetTransform = interactable.transform; 
            }

            mover.MoveTo(targetTransform.position, isRunning);

            while (mover.isMoving) {
                yield return null;
            }

            transform.position = targetTransform.position;
            transform.forward = targetTransform.forward;

            interactable.Interact();
        }

    }
}