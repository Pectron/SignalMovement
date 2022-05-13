/// <summary> 
/// Defines a Physiology device, which can be read from and the data recorded
/// </summary>
public interface IPhysiologyDevice
{
    string DeviceName { get; }
    /// <summary>
    /// A bool marking if this device is ready to start acquiring data<br></br>
    /// Ex.: Set to true after a signals test was made so it is known
    /// that everything is working for the Practicioner
    /// </summary>
    bool DeviceIsReady { get; }

    /// <summary>
    /// Starts the device acquisition
    /// </summary>
    /// <param name="fileName">
    /// File name to send the data to until `StopAcquisition()` is called<br></br>
    /// If fileName is null then this call should not result in any saved data.
    /// </param>
    void StartAcquisition(string fileName);

    /// <summary>
    /// Stops the device and closes any pending data files.
    /// </summary>
    void StopAcquisition();

    void WriteEvent(string id);

    void StartLSL();


}
