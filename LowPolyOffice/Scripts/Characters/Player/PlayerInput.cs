using Office.Interaction;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Office.Character.Player {
    public class PlayerInput : MonoBehaviour {

        #region Singleton
        public static PlayerInput instance;

        private void Awake() {
            if (instance) Debug.LogWarning("More than one instance of PlayerInput found!");
            onInteractableClickEvent = new UnityEvent<Interactable, bool>();
            onGroundClickEvent = new UnityEvent<Vector3, bool>();
            onAnyKeyDownEvenet = new UnityEvent();
            instance = this;
        }
        #endregion

        #region Keys

        [Header("Keys")]

        [Tooltip("Main key, used for walk to position and interact")]
        public KeyCode InteractKey = KeyCode.Mouse0;

        [Tooltip("Used for run to position and run to interactable")]
        public KeyCode RunToPositionKey = KeyCode.Mouse1;

        #endregion

        #region Settings

        private const float MaxRayDistance = 180f;

        [Header("Settings")]
        [Tooltip("Raycast layers")]
        public LayerMask raycastLayerMask;

        #endregion

        #region Block Input
        public bool blockInput { get; set; } = true; // Blocks all input
        public bool blockRaycastInput { get; set; } = false; // Blocks walk to position and interaction
        #endregion

        #region Private Fields
        private Coroutine blockInputRoutine;
        private Coroutine blockRaycastInputRoutine;
        #endregion

        #region Events
        public UnityEvent<Vector3, bool> onGroundClickEvent;
        public UnityEvent<Interactable, bool> onInteractableClickEvent;
        public UnityEvent<Interactable> onMouseOverInteractable;
        public UnityEvent onAnyKeyDownEvenet;
        #endregion

        #region Debug
        [Header("Debug")]

        [Tooltip("Debug mode")]
        public bool debugMode = false;

        [Tooltip("This object will be teleported to raycast hit position")]
        public GameObject debugPivotObj;

        #endregion

        private void Update() {

            if (blockInput) return;

            if (Input.anyKeyDown) onAnyKeyDownEvenet.Invoke();

            if (blockRaycastInput) return;

            if (Input.GetKey(InteractKey) || Input.GetKey(RunToPositionKey)) {

                Vector3 point;
                Interactable interactable;

                bool isRunning = Input.GetKey(RunToPositionKey);

                FindTarget(out point, out interactable);

                onInteractableClickEvent.Invoke(interactable, isRunning);

                if (!interactable) onGroundClickEvent.Invoke(point, isRunning);
            }
        }

        private void FixedUpdate() {

            if (blockInput || blockRaycastInput) {
                onMouseOverInteractable.Invoke(null);
                return; 
            }
            
            onMouseOverInteractable.Invoke(MousePositionRaycast(out _));
        }

        private void FindTarget(out Vector3 point, out Interactable interactable) {
            interactable = MousePositionRaycast(out point);
        }

        private Interactable MousePositionRaycast(out Vector3 point) {

            RaycastHit[] hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), MaxRayDistance, raycastLayerMask);

            bool pointSet = false;
            Vector3 p = Vector3.zero;

            foreach (var hit in hits) {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable) {
                    point = hit.point;
                    return interactable;
                } else if (hit.transform.tag.Equals("Ground")) {
                    p = hit.point;
                    pointSet = true;
                }
            }

            if (!pointSet && (hits.Length > 0)) point = hits[0].point;
            else point = p;

            return null;
        }

        public void SetBlockRaycastInputWithDelay(bool value, float delay) {
            if (blockRaycastInputRoutine != null) StopCoroutine(blockRaycastInputRoutine);
            blockRaycastInputRoutine = StartCoroutine(BlockRaycastInputWithDelayRoutine(value, delay));
        }
        
        public void SetBlockInputWithDelay(bool value, float delay) {
            if (blockInputRoutine != null) StopCoroutine(blockInputRoutine);
            blockInputRoutine = StartCoroutine(BlockInputWithDelayRoutine(value, delay));
        }

        private IEnumerator BlockRaycastInputWithDelayRoutine(bool value, float delay) {
            yield return new WaitForSeconds(delay);
            blockRaycastInput = value;
        }
        
        private IEnumerator BlockInputWithDelayRoutine(bool value, float delay) {
            yield return new WaitForSeconds(delay);
            blockInput = value;
        }
    }
}
