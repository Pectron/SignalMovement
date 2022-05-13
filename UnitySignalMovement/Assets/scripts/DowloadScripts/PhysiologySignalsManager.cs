using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LSL;
using Assets.LSL4Unity.Scripts;

public class PhysiologySignalsManager : MonoBehaviour
{
    public static PhysiologySignalsManager Instance { get; private set; }

    IPhysiologyDevice[] devices;
    IPhysiologyDevice activeShowing;
    [SerializeField] private string[] loadedDevices;
    [Tooltip("Called when all devices are ready and baseline is recorded")]
    public UnityEngine.Events.UnityEvent devicesReady;
    [SerializeField] private string sceneToLoadOnDevicesReady;
    [SerializeField] private GameObject startLabrecorderCanvas;
    [SerializeField] private GameObject startOpenSignalsCanvas;
    [SerializeField] private GameObject recordingBaseLineCanvas;
    [SerializeField] private float baselineTimer;


    private bool labrecorderStarted = false;
    private LSLMarkerStream markerStream;

    public float AcquisitionTime { get; private set; }
    public bool Recording { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        devices = GetComponentsInChildren<IPhysiologyDevice>();
        markerStream = GetComponent<LSLMarkerStream>();
    }

    private void Start()
    {
        
        if (!DevicesAvailable())
        {
            //Debug.LogError("No Physiology devices were found");
            StartCoroutine(NoDeviceRun());
            //return;
        }
        
        StartCoroutine(TestDevices());
    }

    private void Update()
    {
        if (Recording)
        {
            AcquisitionTime += Time.unscaledDeltaTime;
        }
    }

    public void StartAcquisition()
    {
        if (!DevicesAvailable()) return;

        DoByDevice((x) => x.StartAcquisition(x.DeviceName + "_Data"));
        AcquisitionTime = 0;
        Recording = true;
        recordingStart?.Invoke();
        // For device, start acquisition
    }

    public void StopAcquisition()
    {
        if (!DevicesAvailable() && !Recording) return;

        NewMarker("Acquisition End");
        //DoByDevice((x) => x.WriteEvent("Acquisition End"));
        DoByDevice((x) => x.StopAcquisition());
        Recording = false;
        recordingEnd?.Invoke();
    }

    public void Event(string id)
    {
        NewMarker(id);
        //DoByDevice((x) => x.WriteEvent(id));
    }

    private void OnApplicationQuit()
    {
        StopAcquisition();
    }

    private IEnumerator TestDevices()
    {
        foreach (IPhysiologyDevice device in devices)
        {
            activeShowing = device;
            Debug.Log($"{activeShowing.DeviceName}: Awaiting confirmation");

            // Do per device something, open close
            yield return new WaitUntil(() => device.DeviceIsReady);
            Debug.Log($"{activeShowing.DeviceName}: Device is Ready");
        }
        //Pop up for start lab recorder

        yield return new WaitUntil(() => labrecorderStarted);

        // Pop up for start base line recording...

        // Wait until base line recording is done
        yield return BaseLine();

        // Devices tested and base lline recorded!
        devicesReady?.Invoke();

        // Insert a header telling the base line has ended
        Debug.Log($"Base line ended at: {AcquisitionTime}");

        NewMarker("Base Line Ended");
        //DoByDevice((x) => x.WriteEvent("Base line ended"));

        if (!string.IsNullOrEmpty(sceneToLoadOnDevicesReady))
        {
            SceneManager.LoadScene(sceneToLoadOnDevicesReady);
        }
    }



    private IEnumerator NoDeviceRun()
    {
        //Pop up for start lab recorder

        yield return new WaitUntil(() => labrecorderStarted);

        // Pop up for start base line recording...

        // Wait until base line recording is done
        yield return BaseLine();

        // Devices tested and base lline recorded!
        devicesReady?.Invoke();

        // Insert a header telling the base line has ended
        Debug.Log($"Base line ended at: {AcquisitionTime}");

        NewMarker("Base Line Ended");
        //DoByDevice((x) => x.WriteEvent("Base line ended"));

        if (!string.IsNullOrEmpty(sceneToLoadOnDevicesReady))
        {
            SceneManager.LoadScene(sceneToLoadOnDevicesReady);
        }
    }



    public void StartOpenSignals()
    {
        startOpenSignalsCanvas.SetActive(false);
    }

    public void LabrecorderStart()
    {
        labrecorderStarted = true;
        startLabrecorderCanvas.SetActive(false);
    }

    public void ShowLabRecorderCanvas()
    {
        DoByDevice((x) => x.StartLSL());
        startLabrecorderCanvas.SetActive(true);
    }

    public void ShowOpensignalsCanvas()
    {
        startOpenSignalsCanvas.SetActive(true);
    }

    private IEnumerator BaseLine()
    {
        DoByDevice((x) => x.StartAcquisition(x.DeviceName + "_Data"));
        NewMarker("Base line start");
        //DoByDevice((x) => x.WriteEvent("Base line start"));


        AcquisitionTime = 0;
        Recording = true;

        yield return new WaitForSecondsRealtime(baselineTimer);

        recordingBaseLineCanvas.SetActive(false);
    }

    private bool DevicesAvailable()
    {
        return devices != null && (devices.Length > 0);
    }

    public void DoByDevice(System.Action<IPhysiologyDevice> action)
    {
        if (action == null) return;

        for (int i = 0; i < devices.Length; i++)
        {
            action.Invoke(devices[i]);
        }
    }

    public static event System.Action recordingStart;
    public static event System.Action recordingEnd;

    private void OnValidate()
    {
        devices = GetComponentsInChildren<IPhysiologyDevice>();
        loadedDevices = new string[devices.Length];
        for (int i = 0; i < loadedDevices.Length; i++)
        {
            loadedDevices[i] = devices[i].DeviceName;
        }
    }

    public void NewMarker(string markerName)
    {
        markerStream.Write(markerName);
    }
}
