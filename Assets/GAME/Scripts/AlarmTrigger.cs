using System;
using UnityEngine;

public class AlarmTrigger : MonoBehaviour
{
    public event Action IntruderDetected;
    public event Action IntruderExited;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<AlarmTarget>(out _))
        {
            IntruderDetected?.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<AlarmTarget>(out _))
        {
            IntruderExited?.Invoke();
        }
    }
}