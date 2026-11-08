using System.Collections.Generic;
using UnityEngine;

public class AlarmSystem : MonoBehaviour
{
    private const float StopVolumeValue = 0.001f;

    [SerializeField] private List<AlarmTrigger> _alarmTriggers;

    [SerializeField] private AudioSource _alarmSound;
    [SerializeField, Range(0f, 1f)] private float _maxVolume = 1f;

    [SerializeField] private float _volumeChangeSpeed = 0.5f;

    private int _intruderCount = 0;
    private float _targetVolume = 0f;

    private void Awake()
    {
        _alarmSound.loop = true;
        _alarmSound.volume = 0f;
        _alarmSound.playOnAwake = false;
    }

    private void OnEnable()
    {
        for (int i = 0; i < _alarmTriggers.Count; i++)
        {
            _alarmTriggers[i].IntruderDetected += AddIntruer;
            _alarmTriggers[i].IntruderExited += SubtractIntruder;
        }    
    }

    private void OnDisable()
    {
        for (int i = 0; i < _alarmTriggers.Count; i++)
        {
            _alarmTriggers[i].IntruderDetected -= AddIntruer;
            _alarmTriggers[i].IntruderExited -= SubtractIntruder;
        }
    }

    private void Update()
    {
        _alarmSound.volume = Mathf.MoveTowards(_alarmSound.volume, _targetVolume, _volumeChangeSpeed * Time.deltaTime);

        if (_intruderCount == 0 && _alarmSound.volume <= StopVolumeValue && _alarmSound.isPlaying)
        {
            _alarmSound.Stop();
        }
    }

    private void AddIntruer()
    {
        _intruderCount++;
        _targetVolume = _maxVolume;

        if (!_alarmSound.isPlaying)
        {
            _alarmSound.Play();
        }
    }

    private void SubtractIntruder()
    {
        _intruderCount = Mathf.Max(0, _intruderCount - 1);

        if (_intruderCount == 0)
        {
            _targetVolume = 0f;
        }
    }
}