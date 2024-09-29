using System;
using UnityEngine;
using TMPro;

public class ClockUI : MonoBehaviour
{
    [Header("Arrows")]
    [SerializeField] private RectTransform _hourArrow;
    [SerializeField] private RectTransform _minArrow;
    [SerializeField] private RectTransform _secArrow;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI _hourText;
    [SerializeField] private TextMeshProUGUI _minText;
    [SerializeField] private TextMeshProUGUI _secText;

    [Header("Clock")]
    [SerializeField] private Clock _clock;

    private void Awake()
    {
        _clock.OnTimeChanged += UpdateTime;
    }

    public void UpdateTime(DateTime currentTime)
    {
        int hours = currentTime.Hour;
        int minutes = currentTime.Minute;
        int seconds = currentTime.Second;
        int milliseconds = currentTime.Millisecond;

        float hourAngle = (360f / 12f) * (hours % 12 + minutes / 60f + seconds / 60f / 60f);
        float minuteAngle = (360f / 60f) * (minutes + seconds / 60f + milliseconds / 60f / 1000f);
        float secondAngle = (360f / 60f) * (seconds + milliseconds / 1000f);

        _hourArrow.localRotation = Quaternion.Euler(0, 0, -hourAngle);
        _minArrow.localRotation = Quaternion.Euler(0, 0, -minuteAngle);
        _secArrow.localRotation = Quaternion.Euler(0, 0, -secondAngle);

        _hourText.SetText($"{hours:D2}");
        _minText.SetText($"{minutes:D2}");
        _secText.SetText($"{seconds:D2}");
    }
}
