using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    [Header ("State Buttons")]
    [SerializeField] private Button _start;
    [SerializeField] private Button _stop;
    [SerializeField] private Button _sync;
    [SerializeField] private Toggle _switchSyncToggle;

    [Header("Slider")]
    [SerializeField] private Slider _updateIntervalSlider;
    [SerializeField] private TextMeshProUGUI _sliderText;

    [Header("Manual Input Elements")]
    [SerializeField] private Toggle _switchInputToggle;

    [Header("InputFields")]
    [SerializeField] private Button _applyKeyboardButton;
    [SerializeField] private GameObject _inputPanel;
    [SerializeField] private TMP_InputField _hoursInput;
    [SerializeField] private TMP_InputField _minutesInput;
    [SerializeField] private TMP_InputField _secondsInput;

    [Header("Watch Crown")]
    [SerializeField] private GameObject _watchCrownPanel;
    [SerializeField] private Button _applyWatchCrownButton;

    [Header ("Arrows")]
    [SerializeField] private RectTransform _hourTransform;
    [SerializeField] private RectTransform _minTransform;
    [SerializeField] private RectTransform _secTransform;

    [Header("Clock Components")]
    [SerializeField] private Clock _clock;
    [SerializeField] private ClockUI _clockUI;
    [SerializeField] private WatchCrown _watchCrown;

    private void Awake()
    {
        _start.onClick.AddListener(StartClock);
        _stop.onClick.AddListener(StopClock);
        _sync.onClick.AddListener(SyncClock);
        _switchSyncToggle.onValueChanged.AddListener(SwitchSync);
        _applyKeyboardButton.onClick.AddListener(ApplyKeyboardButtonClick);
        _switchInputToggle.onValueChanged.AddListener(OnToggleChanged);
        _updateIntervalSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _applyWatchCrownButton.onClick.AddListener(ApplyWatchCrownButtonClick);

        SetManualInputActive(false);
    }

    private void StartClock() 
    {
        _clock.ClockActive = true;
    }

    private void StopClock()
    {
        _clock.ClockActive = false;
    }

    private void SyncClock()
    {
        _clock.SetTimeFromServer();
    }

    private void SwitchSync(bool isOn)
    {
        _clock.SyncActive = isOn;
    }

    private void OnToggleChanged(bool isOn)
    {
        SetManualInputActive(isOn);
        
        _clock.ClockActive = !isOn;
    }

    private void SetManualInputActive(bool isActive)
    {
        _inputPanel.SetActive(isActive);
        _watchCrownPanel.SetActive(isActive);
    }

    private void ApplyKeyboardButtonClick()
    {
        if (int.TryParse(_hoursInput.text, out int hours) &&
            int.TryParse(_minutesInput.text, out int minutes) &&
            int.TryParse(_secondsInput.text, out int seconds))
        {
            if (hours >= 0 && hours <= 23 &&
                minutes >= 0 && minutes <= 59 &&
                seconds >= 0 && seconds <= 59)
            {
                _clock.SetTime(hours, minutes, seconds);
            }
            else
            {
                Debug.LogError("Input values are out of range. Hours must be between 0 and 23, minutes and seconds between 0 and 59.");
            }
        }
        else
        {
            Debug.LogError("Incorrect data in input fields. Please enter two-digit numbers.");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        _clock.SetUpdateInterval((int)value);
        _sliderText.SetText($"{(int)value}");
    }

    private void ApplyWatchCrownButtonClick() 
    {
        _clock.GetTimeFromAngles(_hourTransform, _minTransform, _secTransform);
    }
}
