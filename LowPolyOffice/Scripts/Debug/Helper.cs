using UnityEngine;
using TMPro;
using Office.Interaction;

public class Helper : MonoBehaviour {

    public TextMeshProUGUI text;

    private float _fps = 0;
    private float _deltaTime = 0;
    private int _frameCounter = 0;
    private float _minFps = 9999;
    private float _maxFps = 0;
    private float _avgFps = 0;

    private int _minutes = 0;
    private float _seconds = 0;

    public int hours { get; private set; } = 0;
    public int minutes { 
        get { 
            return _minutes; 
        }
        private set {
            if (value >= 60) { hours++; }
            _minutes = value % 60;
        } 
    }

    public float seconds {
        get {
            return _seconds;
        }
        private set {
            if (value >= 60f) { minutes++; }
            _seconds = value % 60f;
        }
    }

    private void Start() {
        AllConditions.instance.Reset();
    }
    /*

    private void OnEnable() {
        Debug.Log("OnEnable()");
    }

    private void Awake() {
        Debug.Log("Awake()");
    }

    public void PrintDebugLog(string s) {
        Debug.Log(s);
    }
    */
    public void Update() {
        string debugText = "";
        _frameCounter++;
        seconds += Time.deltaTime;
        _deltaTime += Time.deltaTime;
        _fps = 1 / Time.deltaTime;

        debugText += $"FPS: {_fps}\n";

        if (_frameCounter % 120 == 0) {
            _avgFps = 120 / _deltaTime;
            _deltaTime = 0;
        }

        if (_frameCounter % 1200 == 0) {
            _maxFps = 0;
            _minFps = 9999;
        }

        debugText += $"AVG FPS: {_avgFps}\n";

        if (_fps > _maxFps) _maxFps = _fps;
        if (_fps < _minFps) _minFps = _fps;

        debugText += $"MIN FPS: {_minFps}\n";
        debugText += $"MAX FPS: {_maxFps}\n";

        debugText += $"TIME: {hours}:{minutes}:{seconds}";

        text.text = debugText;
    }
}
