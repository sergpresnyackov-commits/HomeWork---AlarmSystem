using System;
using System.Collections.Generic;
using UnityEngine;

public class IntruderCounter : MonoBehaviour
{
    public event Action AlarmShouldTurnOn;
    public event Action AlarmShouldTurnOff;

    [SerializeField] private List<AlarmTrigger> _alarmTriggers;
    [SerializeField] private int _turnOnCount = 1;
    [SerializeField] private int _turnOffCount = 0;

    private int _intruderCount = 0;

    private void OnEnable()
    {
        for (int i = 0; i < _alarmTriggers.Count; i++)
        {
            _alarmTriggers[i].IntruderDetected += AddIntruder;
            _alarmTriggers[i].IntruderExited += SubtractIntruder;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _alarmTriggers.Count; i++)
        {
            _alarmTriggers[i].IntruderDetected -= AddIntruder;
            _alarmTriggers[i].IntruderExited -= SubtractIntruder;
        }
    }

    private void AddIntruder()
    {
        _intruderCount++;

        if (_intruderCount == _turnOnCount)
        {
            AlarmShouldTurnOn?.Invoke();
        }
    }

    private void SubtractIntruder()
    {
        _intruderCount = Mathf.Max(0, _intruderCount - 1);

        if (_intruderCount == _turnOffCount)
        {
            AlarmShouldTurnOff?.Invoke();
        }
    }
}