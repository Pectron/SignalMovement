using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneEventMarker : MonoBehaviour
{
    [Tooltip("Name of the event, if none is given it will name it as the current active scene name")]
    public string EventID;
    [SerializeField] private bool markOnStart;

    private void Start()
    {
        if (markOnStart) 
        {
            MarkEvent();
        }
    }

    public void MarkEvent()
    {
        if (string.IsNullOrEmpty(EventID))
        {
            EventID = SceneManager.GetActiveScene().name;
        }

        PhysiologySignalsManager.Instance?.Event(EventID);
    }
}
