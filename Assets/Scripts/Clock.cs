using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

public class Clock : MonoBehaviour
{
    public bool ClockActive = true;
    public bool SyncActive = true;

    private float _updateInterval = 1f;
    private DateTime _currentTime;
    private long _elapsedTime = 0;
    private Coroutine _timeFromServerCoroutine;

    private const string _url = "https://yandex.com/time/sync.json";

    public Action<DateTime> OnTimeChanged;

    [Serializable]
    public class TimeData
    {
        public long time;
    }

    private void Start()
    {
        _timeFromServerCoroutine = StartCoroutine(UpdateTimeFromServer());
    }

    private void FixedUpdate()
    {
        if (ClockActive)
        {
            _currentTime = _currentTime.AddMilliseconds(Time.fixedDeltaTime * 1000);
            UpdateTimeValues();

            _elapsedTime += (long)(Time.fixedDeltaTime * 1000);

            if (_elapsedTime >= _updateInterval * 60 * 1000)
            {
                _elapsedTime = 0;

                if (SyncActive)
                {
                    SetTimeFromServer();
                }
            }
        }
    }

    private IEnumerator UpdateTimeFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(_url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Ошибка при подключении: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                TimeData timeData = JsonUtility.FromJson<TimeData>(jsonResponse);

                _currentTime = DateTimeOffset.FromUnixTimeMilliseconds(timeData.time).UtcDateTime.ToLocalTime();

                Debug.Log("Time updated from server");

                UpdateTimeValues();
            }
        }

        _timeFromServerCoroutine = null;
    }

    private void UpdateTimeValues()
    {
        OnTimeChanged?.Invoke(_currentTime);
    }

    public void SetTime(int hours, int minutes, int seconds)
    {
        _currentTime = new DateTime(_currentTime.Year, _currentTime.Month, _currentTime.Day, hours, minutes, seconds);
        UpdateTimeValues();
    }

    public void SetUpdateInterval(int interval)
    {
        _updateInterval = interval;
    }

    public void SetTimeFromServer()
    {
        if (_timeFromServerCoroutine == null)
        {
            StartCoroutine(UpdateTimeFromServer());
        }
    }

    public void GetTimeFromAngles(RectTransform _hourArrow, RectTransform _minArrow, RectTransform _secArrow)
    {
        float hourAngle = (-_hourArrow.localRotation.eulerAngles.z + 360f) % 360f;
        float minuteAngle = (-_minArrow.localRotation.eulerAngles.z + 360f) % 360f;
        float secondAngle = (-_secArrow.localRotation.eulerAngles.z + 360f) % 360f;

        int hours = ((int)(hourAngle / (360f / 12f))) % 12;
        int minutes = ((int)(minuteAngle / (360f / 60f))) % 60;
        //int seconds = ((int)(secondAngle / (360f / 60f))) % 60;

        DateTime now = DateTime.Now;
        DateTime calculatedTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, 0); //optional: seconds can be taken into account

        _currentTime = calculatedTime;
        UpdateTimeValues();
    }
}
