using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    Transform hoursPivot, minutesPivot, secondsPivot;

    const float hoursToDegrees = 30f, minutesToDegrees = 6f, secondsToDegrees = 6f;
    private void Update()
    {
        var time = DateTime.Now.TimeOfDay;

        hoursPivot.localRotation = Quaternion.Euler(hoursToDegrees * (float)time.TotalHours, 0, 0);
        minutesPivot.localRotation = Quaternion.Euler(minutesToDegrees * (float)time.TotalMinutes, 0, 0);
        secondsPivot.localRotation = Quaternion.Euler(secondsToDegrees * (float)time.TotalSeconds, 0, 0);

    }
}