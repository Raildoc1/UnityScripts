using Office.Character;
using Office.Character.Player;
using System.Collections;
using UnityEngine;

namespace Office.Interaction {
    [RequireComponent(typeof(Mover))]
    public class PlayerInteract : MonoBehaviour {

        private Coroutine currentAction; // stores current coroutine to be able to stop it if player clicked on other Interactable before current coroutine was finished
        private Interactable currentTarget; // represent current player's target

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

        // To reset target call Interact(null)
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

            // set player destination to targetTransform.position
            mover.MoveTo(targetTransform.position, isRunning);

            // wait while player is moving
            while (mover.isMoving) {
                yield return null;
            }

            // teleport player to targetTransform if player should match target
            if (interactable.matchTargetTransform) {
                transform.position = targetTransform.position;
                transform.forward = targetTransform.forward;
            }

            // interact than player close enough to target
            interactable.Interact();
        }

    }
}