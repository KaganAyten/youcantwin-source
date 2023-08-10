using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimerController : MonoBehaviour
{
    private float min, sec, hour;
    [SerializeField] private TMP_Text timerText; 
    void Start()
    {
        InvokeRepeating(nameof(Timer), 0, 1);
    }
    private void Timer()
    {
        sec++;
        if (sec >= 60)
        {
            min++;
            sec = 0;
        }
        if (min >= 60)
        {
            hour++;
            min = 0;
        }
        timerText.text = hour.ToString("00") + ":" +min.ToString("00") + ":" + sec.ToString("00");
    } 
      
}
