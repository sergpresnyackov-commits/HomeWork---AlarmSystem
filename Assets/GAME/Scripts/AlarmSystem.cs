using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IntruderCounter))]
public class AlarmSystem : MonoBehaviour
{
    private const float StopVolumeValue = 0.001f;

    [SerializeField] private AudioSource _alarmSound;
    [SerializeField, Range(0f, 1f)] private float _maxVolume = 1f;
    [SerializeField] private float _volumeChangeSpeed = 0.5f;

    private Coroutine _volumeCoroutine;
    private IntruderCounter _intruderCounter;

    private void Awake()
    {
        _alarmSound.loop = true;
        _alarmSound.volume = 0f;
        _alarmSound.playOnAwake = false;

        if (TryGetComponent(out IntruderCounter intruderCounter))
        {
            _intruderCounter = intruderCounter;
        }
    }

    private void OnEnable()
    {
        _intruderCounter.AlarmShouldTurnOn += TurnOn;
        _intruderCounter.AlarmShouldTurnOff += TurnOff;
    }

    private void OnDisable()
    {
        _intruderCounter.AlarmShouldTurnOn -= TurnOn;
        _intruderCounter.AlarmShouldTurnOff -= TurnOff;
    }

    private void TurnOn()
    {
        RestartRoutine();

        if (!_alarmSound.isPlaying)
        {
            _alarmSound.Play();
        }

        _volumeCoroutine = StartCoroutine(ChangeVolume(_maxVolume));
    }

    private void TurnOff()
    {
        RestartRoutine();
        _volumeCoroutine = StartCoroutine(ChangeVolume(0f));
    }

    private void RestartRoutine()
    {
        if (_volumeCoroutine != null)
        {
            StopCoroutine(_volumeCoroutine);
            _volumeCoroutine = null;
        }
    }

    private IEnumerator ChangeVolume(float targetVolume)
    {
        while (!Mathf.Approximately(_alarmSound.volume, targetVolume))
        {
            _alarmSound.volume = Mathf.MoveTowards(_alarmSound.volume, targetVolume, _volumeChangeSpeed * Time.deltaTime);

            yield return null;
        }

        if (targetVolume == 0f && _alarmSound.volume <= StopVolumeValue)
        {
            _alarmSound.Stop();
        }

        _volumeCoroutine = null;
    }
}