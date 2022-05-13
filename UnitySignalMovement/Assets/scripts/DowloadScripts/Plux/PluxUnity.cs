using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Timers;
using System.Runtime.InteropServices;

namespace Plux
{

    [Flags]
    public enum BluetoothDomains
    {
        BTH = 1,
        BLE = 2
    }

    public enum ReadResolution
    {
        Low,
        High
    }

    [Flags]
    public enum Channels
    {
        CH1 = 1,
        CH2 = 2,
        CH3 = 4,
        CH4 = 8,
        CH5 = 16,
        CH6 = 32,
        CH7 = 64,
        CH8 = 128
    }

    public class PluxUnity : MonoBehaviour, IPhysiologyDevice
    {
        /// <summary>
        /// Flags enum for bluetooth domains to connect to, Classic Bluetooth and LOW Energy Bluetooth
        /// </summary>
        [SerializeField] private BluetoothDomains _bluetoothDomains;

        /// <summary>
        /// Resolution at which the device is to be read 
        /// </summary>
        [SerializeField, Tooltip("Which resolution the PLUX device is to be read, if the device doesn't have resolution options the default value will be picked")]
        private ReadResolution _resolution;

        [SerializeField, Tooltip("Channels to read from")]
        private Channels _readChannels;
        /// <summary>
        /// Sampling rate of the device, this is parsed to a correct rate if the input is strange
        /// </summary>
        [SerializeField] private int _samplingRate;

        /// <summary>
        /// Tells the manager to search for a connectable device on start
        /// instead of waiting for the user to call a function to connect
        /// </summary>
        [SerializeField, Tooltip("Should try and connect on start?")]
        private bool _connectOnStart;

        /// <summary>
        /// Connected device name in Inspector
        /// </summary>
        [Header("Debug")]
        [SerializeField, Tooltip("This will show the name of the connected device once there is one")]
        private string _connectedDeviceName;

        // Private fields
        private List<string> _availableDevices;
        private List<int> _activeChannels;
        private string _selectedDevice;
        // Read resolution of the device
        private int _deviceResolution;
        private int _lifeTimeSamples;
        private bool _updatePlot;
        private bool _lastPlotState;
        private PluxCSVRecorder _csvRecorder;

        #region properties
        public PluxDeviceManager PluxDevManager { get; private set; }
        public bool Connected { get; private set; }
        public bool AcquiringData { get; private set; }
        public bool Recording { get; private set; }
        /// <summary>
        /// Value from 0 - 100 with the battery of the connected device, defaults to -1
        /// This value is updated when a new device is connected
        /// </summary>
        public int DeviceBatteryLevel { get; private set; }
        /// <summary>
        /// Read Resolution of the active device, returns -1 if there is no device connected
        /// </summary>
        public int DeviceReadResolution
        {
            get
            {
                if (Connected)
                    return _deviceResolution;
                else
                    return -1;
            }
        }

        public string DeviceName => "Plux";

        public bool DeviceIsReady { get; private set; }
        #endregion

        #region UnityMessages
        private void Awake()
        {
            DeviceBatteryLevel = -1;
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            // Create new device
            PluxDevManager = new PluxDeviceManager(ScanResults, ConnectionDone);

            System.Timers.Timer pluxPlotResetTimer = new System.Timers.Timer();

            // Handles plux memory reset at marked intervals
            pluxPlotResetTimer.Elapsed += new ElapsedEventHandler(MemoryRebootFlag);
            pluxPlotResetTimer.Interval = 1000;
            pluxPlotResetTimer.Enabled = true;
            pluxPlotResetTimer.AutoReset = true;


            StartCoroutine(ConnectWhenAvailable());
        }

        private void Update()
        {
            try
            {
            
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Debug.Log("Exception in the Update method: " + exception.StackTrace);
            }
            catch (ExternalException exc)
            {
                Debug.Log("ExternalException in the Update() callback:\n" + exc.Message + "\n" + exc.StackTrace);

                // Stop Acquisition in a secure way.
                
            }
            catch (Exception exc)
            {
                Debug.Log("Unidentified Exception inside Update() callback:\n" + exc.Message + "\n" + exc.StackTrace);
            }
        }

        /// <summary>
        /// Removes any connected device so we avoid errors when exiting the application
        /// </summary>
        void OnApplicationQuit()
        {
            // Disconnect from device.
            PluxDevManager.DisconnectPluxDev();
            Debug.Log("Application ending after " + Time.time + " seconds");
        }
        #endregion

        /// <summary>
        /// Callback that receives a list of PLUX devices found during bluetooth scan
        /// </summary>
        /// <param name="devices">List of device names found</param>
        public void ScanResults(List<string> devices)
        {
            // Store devices
            _availableDevices = new List<string>();

            Debug.Log("Number of Devices detected: " + _availableDevices.Count);

            if (devices.Count != 0)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    if (!devices[i].Contains("BTH") && !devices[i].Contains("BLE")) continue;

                    Debug.Log("Device ----> " + devices[i] + " Added");
                    _availableDevices.Add(devices[i]);
                }
            }

            if (_availableDevices.Count == 0)
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Callback when a device is connected
        /// </summary>
        public void ConnectionDone()
        {
            if (!Connected)
            {
                Connected = true;
                onBluetoothConnected?.Invoke();

                Debug.Log("Connectiion with " + _selectedDevice + "Success!");
                Debug.Log("Connected product ID: " + PluxDevManager.GetProductIdUnity());

                string devType = PluxDevManager.GetDeviceTypeUnity();

                // Set resolution of the device
                if (devType == "MuscleBan BE Plux")
                {
                    _deviceResolution = _resolution == ReadResolution.High ? 16 : 8;
                }
                else if (devType == "BITalino")
                {
                    // Can only read in 1 resolution 
                    _deviceResolution = 10;
                }
                else if (devType == "biosignalsplux" || devType == "BioPlux")
                {
                    if (devType == "biosignalsplux")
                    {
                        _deviceResolution = _resolution == ReadResolution.High ? 16 : 8;
                    }
                    else
                    {
                        _deviceResolution = _resolution == ReadResolution.High ? 12 : 8;
                    }
                }
                else if (devType == "OpenBANPlux")
                {
                    _deviceResolution = _resolution == ReadResolution.High ? 16 : 8;
                }
                else if (PluxDevManager.GetProductIdUnity() != 542) // Device not supported!
                {
                    throw new NotSupportedException();
                }

                _connectedDeviceName = devType;
                DeviceBatteryLevel = -1;

                if (devType != "BioPlux") // BioPlux doesnt have battery
                {
                    DeviceBatteryLevel = PluxDevManager.GetBatteryUnity();
                }
            }
        }

        /// <summary>
        /// Searches for available bluetooths using  `PluxDeviceManager.GetDetectableDevicesUnity()`
        /// </summary>
        public void ScanForDevices()
        {
            try
            {
                List<string> domains = new List<string>();
                if (_bluetoothDomains.HasFlag(BluetoothDomains.BTH))
                {
                    domains.Add("BTH");
                }
                if (_bluetoothDomains.HasFlag(BluetoothDomains.BLE))
                {
                    domains.Add("BLE");
                }

                // This will call ScanResults callback when its done
                PluxDevManager.GetDetectableDevicesUnity(domains);
            }
            catch (Exception e)
            {
                Debug.Log("Error Scanning for bluetooth devices: " + e.Message);
            }
        }

        /// <summary>
        /// Connects to an available device
        /// </summary>
        public void Connect()
        {
            try
            {
                if (!Connected)
                {
                    // Connect to the first available
                    _selectedDevice = _availableDevices[0];
                    // Connect device
                    PluxDevManager.PluxDev(_selectedDevice);
                }
                else if (Connected)
                {
                    // Disconect device
                    try
                    {
                        // Disconnect device.
                        PluxDevManager.DisconnectPluxDev();
                    }
                    catch (Exception exception)
                    {
                        Debug.Log("Trying to disconnect from an unconnected device...");
                    }

                    Connected = false;
                    DeviceBatteryLevel = -1;
                    Reboot();
                }
            }
            catch (Exception e)
            {
                // Print information about the exception.
                Debug.Log(e);
            }
        }

        public void StartAcquisition(string fileName = null)
        {
            try
            {
                int nOfChannels = 0;
                bool[] channels = new bool[] {
                    _readChannels.HasFlag(Channels.CH1),
                    _readChannels.HasFlag(Channels.CH2),
                    _readChannels.HasFlag(Channels.CH3),
                    _readChannels.HasFlag(Channels.CH4),
                    _readChannels.HasFlag(Channels.CH5),
                    _readChannels.HasFlag(Channels.CH6),
                    _readChannels.HasFlag(Channels.CH7),
                    _readChannels.HasFlag(Channels.CH8)};

                for (int i = 0; i < 8; i++)
                {
                    if (channels[i])
                    {
                        // Add active channel to the List
                        _activeChannels.Add(i + 1);
                        nOfChannels++;
                    }
                }

                _samplingRate = ValidateSamplingRate(_samplingRate);
                _lifeTimeSamples = 0;

                if (_activeChannels.Count != 0)
                {
                    AcquiringData = true;
                    startAcquisitionCallback?.Invoke();

                    if (fileName != null)
                    {
                        _csvRecorder.CreateFile(fileName);
                        Recording = true;
                    }

                    if (PluxDevManager.GetDeviceTypeUnity() != "MuscleBAN BsE Plux")
                    {
                        PluxDevManager.StartAcquisitionUnity(_samplingRate, _activeChannels, _deviceResolution);
                    }
                    else
                    {
                        // Definition of the frequency divisor (subsampling ratio).
                        int freqDivisor = 10;
                        PluxDevManager.StartAcquisitionMuscleBanUnity(_samplingRate, _activeChannels, _deviceResolution,
                            freqDivisor);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Exception: " + e.Message + "\n" + e.StackTrace);
            }
        }

        public void StopAcquisition() 
        {
            Stop();    
        }

        private void Stop(int forceStop = 0) 
        {

            if (Recording) 
            {
                if (AcquiringData) 
                {
                    endAcquisitionCallback?.Invoke();
                    _csvRecorder.EndRecording();
                }
            }

            bool typeOfStop = PluxDevManager.StopAcquisitionUnity(forceStop);
            
            Recording = false;
            AcquiringData = false;

            if (typeOfStop || forceStop == -1) 
            {
                Debug.LogWarning("Something went wrong, safe force stop was done");
            }
        }

        private int ValidateSamplingRate(int samplingRate)
        {
            if (samplingRate != 0)
            {
                if (samplingRate / 10000 >= 1 || samplingRate > 4000)
                {
                    Debug.Log("Sampling rate set to a default value of 1000");
                    samplingRate = 1000;
                }
            }
            else
            {
                samplingRate = 1000;
            }

            if (samplingRate > 100 && _selectedDevice.Contains("BLE") && NumberOfActiveChannels() > 1) 
            {
                samplingRate = 100;
            }
            
            return samplingRate;
        }

        public int NumberOfActiveChannels()
        {
            int nOfChannels = 0;
            bool[] channels = new bool[] 
            {
                _readChannels.HasFlag(Channels.CH1),
                _readChannels.HasFlag(Channels.CH2),
                _readChannels.HasFlag(Channels.CH3),
                _readChannels.HasFlag(Channels.CH4),
                _readChannels.HasFlag(Channels.CH5),
                _readChannels.HasFlag(Channels.CH6),
                _readChannels.HasFlag(Channels.CH7),
                _readChannels.HasFlag(Channels.CH8)
            };

            for (int i = 0; i < 8; i++)
            {
                if (channels[i])
                {
                    nOfChannels++;
                }
            }

            return nOfChannels;
        }

        /// <summary>
        /// Called every 1 second by plot timer initialized at Start()
        /// </summary>
        /// <param name="source"> source obj</param>
        /// <param name="e"></param>
        private void MemoryRebootFlag(object source, ElapsedEventArgs e) 
        {
            _updatePlot = true;
        }

        public void Reboot()
        {
            _activeChannels = new List<int>();
        }

        /// <summary>
        /// Waits until a device is ready to be connected after ScanDevices() is called, and conencts to it
        /// </summary>
        /// <returns>Unity Coroutine</returns>
        private IEnumerator ConnectWhenAvailable()
        {
            yield return new WaitUntil(() => _availableDevices != null && _availableDevices.Count > 0);
            Connect();
        }

        public void WriteEvent(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called in button event to confirm that the values are ok<br></br>
        /// If it has a canvas group disable alpha and raycast blocking.
        /// </summary>
        public void ConfirmDeviceState()
        {
            DeviceIsReady = true;

            if (AcquiringData)
            {
                Stop();
            }
        }

        public void StartLSL()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called every memory reboot with each data plot read from the plux device
        /// </summary>
        public event Action<int[][], int> dataSample;

        /// <summary>
        /// Called when the acquisition as ended
        /// </summary>
        public event Action endAcquisitionCallback;

        /// <summary>
        /// Called whe the acquisition starts
        /// </summary>
        public event Action startAcquisitionCallback;
        public event Action onBluetoothConnected;
    }
}