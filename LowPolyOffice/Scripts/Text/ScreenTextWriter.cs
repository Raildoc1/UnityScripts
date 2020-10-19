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

        public TextMeshProUGUI textMesh;
        public Image backgroundImage;
        public CanvasGroup canvasGroup;
        public float fadeSpeed = 2f;
        public float onDialogExitDelay = 0.2f;
        public float onDialogEnterDelay = 0.5f;
        public RectTransform label;
        public CanvasGroup labelCanvasGroup;
        public TextMeshProUGUI labelText;
        public float labelFadeSpeed = 2f;
        public Interactable currentInteractable;

        public bool inDialog { get; private set; } = false;
        public bool nextLineTriggered { get; private set; } = false;

        private int currentLineIndex = 0;
        private DialogSet currentDialog;
        private Coroutine currentRoutine;
        private Coroutine currentLabelRoutine;
        private DialogExecutor currentDialogExecutor;
        private bool isLableActive;

        private void Start() {
            PlayerInput.instance.onMouseEnterInteractable.AddListener(UpdateLabel);
        }

        public void WriteTextByUniqueName(string textUniqueName) {
            string text = TextDatabase.instance.GetText(textUniqueName);
            if (currentRoutine != null) StopCoroutine(currentRoutine);
            currentRoutine = StartCoroutine(ShowTextCoroutine(text));
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
