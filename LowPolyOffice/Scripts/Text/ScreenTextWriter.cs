using Office.Character.Dialog;
using Office.Character.Player;
using Office.Interaction;
using Office.Props;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Office.Text {
    public class ScreenTextWriter : MonoBehaviour {

        #region Singleton
        public static ScreenTextWriter instance;

        private void Awake() {
            if (instance) Debug.LogWarning("More than one instance of ItemDatabase found!");
            instance = this;
        }
        #endregion

        #region Subtitles
        [Header("Subtitles")]

        [Space(5f)]
        public TextMeshProUGUI textMesh;
        public Image backgroundImage;
        public CanvasGroup canvasGroup;

        [Space(5f)]
        [Tooltip("Speed to fadeIn subtitles panel")]
        public float fadeSpeed = 2f;

        [Tooltip("Delay before input unlocked after dialog is finished")]
        public float onDialogExitDelay = 0.2f;

        [Tooltip("Delay before input unlocked after enter a dialog")]
        public float onDialogEnterDelay = 0.5f;
        #endregion

        #region Label
        [Header("Label")]

        [Space(5f)]
        public RectTransform label;
        public CanvasGroup labelCanvasGroup;
        public TextMeshProUGUI labelText;

        [Space(5f)]
        [Tooltip("Speed to fadeIn label")]
        public float labelFadeSpeed = 2f;
        #endregion

        #region Public Fields
        // Is player in dialog?
        public bool inDialog { get; private set; } = false;

        // Represents if player clicked to go to next dialog line
        public bool nextLineTriggered { get; private set; } = false;

        #endregion

        #region Private Fields
        private DialogExecutor currentDialogExecutor;

        private DialogSet currentDialog;
        private int currentLineIndex = 0;

        private Coroutine currentSubtitlesRoutine;
        private Coroutine currentLabelRoutine;

        private bool isLableActive;
        private Interactable currentInteractable;
        #endregion

        private void Start() {
            PlayerInput.instance.onMouseEnterInteractable.AddListener(UpdateLabel);
        }

        public void WriteTextByUniqueName(string textUniqueName) {
            string text = TextDatabase.instance.GetText(textUniqueName);
            if (currentSubtitlesRoutine != null) StopCoroutine(currentSubtitlesRoutine);
            currentSubtitlesRoutine = StartCoroutine(ShowTextCoroutine(text));
        }

        public void StartDialog(DialogSet dialogSet, DialogExecutor dialogExecutor) {
            PlayerInput.instance.blockInput = true;
            PlayerInput.instance.SetBlockInputWithDelay(false, onDialogEnterDelay);
            PlayerInput.instance.onAnyKeyDownEvenet.AddListener(NextLine);
            PlayerInput.instance.blockRaycastInput = true;
            inDialog = true;
            currentDialog = dialogSet;
            currentLineIndex = 0;
            currentDialogExecutor = dialogExecutor;
            NextLine();
        }

        public void NextLine() {

            if (!inDialog) return;

            nextLineTriggered = true;

            if (currentLineIndex >= currentDialog.lines.Count) { 
                EndDialog();
                return;
            }

            if(currentDialog.lines[currentLineIndex].reactionCollection)
                currentDialog.lines[currentLineIndex].reactionCollection.React();

            WriteTextByUniqueName(currentDialog.lines[currentLineIndex].lineUniqueName);

            currentLineIndex++;
        }

        public void EndDialog() {
            inDialog = false;
            PlayerInput.instance.SetBlockRaycastInputWithDelay(false, onDialogExitDelay);
            PlayerInput.instance.onAnyKeyDownEvenet.RemoveListener(NextLine);
            currentDialogExecutor.EndDialog();
        }

        public void UpdateLabel(Interactable interactable) {

            label.position = Input.mousePosition;

            if (!interactable || !interactable.GetComponent<NameHolder>()) {
                if (!isLableActive) return;
                isLableActive = false;
                if (currentLabelRoutine != null) StopCoroutine(currentLabelRoutine);
                currentLabelRoutine = StartCoroutine(FadeLabel(0f));
                currentInteractable = null;
                return; 
            }

            if (currentInteractable && (interactable.GetHashCode() == currentInteractable.GetHashCode())) return;

            currentInteractable = interactable;

            if (currentLabelRoutine != null) StopCoroutine(currentLabelRoutine);
            currentLabelRoutine = StartCoroutine(ShowLabelCoroutine(interactable.GetComponent<NameHolder>().GetName()));
            isLableActive = true;
        }

        private IEnumerator Fade(float target) {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime * fadeSpeed);
                yield return null;
            }
        }

        private IEnumerator ShowTextCoroutine(string text) {

            backgroundImage.enabled = true;

            yield return Fade(0f);

            textMesh.text = text;

            yield return Fade(1f);

            nextLineTriggered = false;

            while (!nextLineTriggered) {
                yield return null;
            }

            nextLineTriggered = false;

            yield return Fade(0f);

            textMesh.text = "";

            backgroundImage.enabled = false;
        }

        private IEnumerator FadeLabel(float target) {

            while (!Mathf.Approximately(labelCanvasGroup.alpha, target)) {
                labelCanvasGroup.alpha = Mathf.MoveTowards(labelCanvasGroup.alpha, target, Time.deltaTime * labelFadeSpeed);
                yield return null;
            }
        }

        private IEnumerator ShowLabelCoroutine(string text) {
            yield return FadeLabel(0f);
            labelText.text = text;
            yield return FadeLabel(1f);
        }
    }
}
