using System;
using UnityEngine;
using TMPro;


//Code inspired by - https://www.youtube.com/watch?v=DH2ZxwRBwwg
public class Timer : MonoBehaviour
{
    private TMP_Text _timerText;
    enum TimerType {Countdown, Stopwatch}
    [SerializeField] private TimerType timerType;

    [SerializeField] private float timeToDisplay = 0f;

    [SerializeField] private float timeToStart = 0f;

    private bool _isRunning;

    private void Awake()
    {
        _timerText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        EventManager.TimerStart += EventManager_TimerStart;
        EventManager.TimerStop += EventManager_TimerStop;
        EventManager.TimerUpdate += EventManager_TimerUpdate;
        EventManager.TimerReset += EventManager_TimerReset;
    }

    private void OnDisable()
    {
        EventManager.TimerStart -= EventManager_TimerStart;
        EventManager.TimerStop -= EventManager_TimerStop;
        EventManager.TimerUpdate -= EventManager_TimerUpdate;
        EventManager.TimerReset -= EventManager_TimerReset;
    }

    private void EventManager_TimerUpdate(float value) => timeToDisplay = value;

    private void EventManager_TimerStart() => _isRunning = true;
 
    private void EventManager_TimerStop() => _isRunning = false;

    private void EventManager_TimerReset() => timeToDisplay = timeToStart;


    private void Update() // this displays the time elapsed during the game.
    {
        if (!_isRunning) return;

        if (timerType == TimerType.Countdown && timeToDisplay < 0.0f) return;

        timeToDisplay += timerType == TimerType.Countdown ? -Time.deltaTime : Time.deltaTime;

        TimeSpan timeSpan = TimeSpan.FromSeconds(timeToDisplay);
        _timerText.text = "Time : " + timeSpan.ToString(@"mm\:ss\:ff");
    }
}
