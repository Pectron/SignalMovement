using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Plux
{
    [RequireComponent(typeof(PluxUnityInterface))]
    public class PluxCSVRecorder : MonoBehaviour
    {
        private static string SAVE_PATH;
        private static readonly string Extension = ".csv";
        private const string FOLDER = "Plux";

        [Header("This is just to debug the value during runtime")]
        [SerializeField] private string savePath;
        private string dir;
        private PluxUnityInterface pluxInterface;
        /// <summary>
        /// Used to write the samples and main data
        /// </summary>
        private Stream _stream;
        /// <summary>
        /// Used to write event headers with timestamps
        /// </summary>
        private Stream _eventStream;
        /// <summary>
        /// Data writer for the main data stream
        /// </summary>
        private StreamWriter _dataWriter;
        /// <summary>
        /// Event writer for the event stream
        /// </summary>
        private StreamWriter _eventWriter;

        private void Awake()
        {
            if (SAVE_PATH == null)
            {
                SAVE_PATH = Application.dataPath + "/../";
            }

            dir = SAVE_PATH;
            dir += "/Plux";
            savePath = dir;

            Debug.Log(dir);

            Directory.CreateDirectory(savePath);

            pluxInterface = GetComponent<PluxUnityInterface>();
        }

        public void CreateFile(string fileName)
        {
            if (fileName == null)
                return;

            string path = DataModuleBase.GetPath(User.Id);
            path = Path.Combine(path, FOLDER);

            Directory.CreateDirectory(path);

            _stream = File.Open(Path.Combine(path, fileName + Extension), FileMode.OpenOrCreate);
            _dataWriter = new StreamWriter(_stream);

            // Events
            _eventStream = File.Open(Path.Combine(path, "Plux_Events" + Extension), FileMode.OpenOrCreate);
            _eventWriter = new StreamWriter(_eventStream);
            
            // Main data header
            Header(_dataWriter);

            // Events file header
            _eventWriter.WriteLine("Event ID, Timestamp(s), Real time(HH:mm:ss:fff)");
        }

        
        public void AddSample(int[][] data, int totalSamples)
        {
            if (data == null || data.Length <= 0) return;
            ParseData(data, totalSamples, _dataWriter);
        } 
        

        /// <summary>
        /// Writes an event inside the data file using an ID and the real time of when the event was created
        /// </summary>
        /// <param name="id"> Name id of the event</param>
        /// <param name="time"> Time of the event</param>
        public void WriteEvent(string id, float appSeconds)
        {
            if (_eventWriter != null) 
            {
                _eventWriter.WriteLine($"{id},{appSeconds},{DateTime.Now.ToString(("HH:mm:ss:fff"))}");
            }
        }

        public void EndRecording()
        {
            // Get Data from plux device
            // ParseData(data, _writer);

            _dataWriter.Close();
            _eventWriter.Close();
        }

        private void ParseData(int[][] data, int totalSamples, StreamWriter stream)
        {
            if (data == null) return;

            int startSample = totalSamples - data.Length;

            for (int i = 0; i < data.Length; i++)
            {
                // sample number
                string info = string.Format("{0},", startSample + i);

                for (int j = 0; j < data[i].Length; j++)
                {
                    // Channel --- Value
                    info += string.Format("{0},{1},", j, data[i][j]);
                }

                stream.WriteLine(info);
            }
        }

        private void Header(StreamWriter writer)
        {
            // Get device information
            writer.WriteLine($"Device,{pluxInterface.PluxDevManager.GetDeviceTypeUnity()}");
            writer.WriteLine($"ID,{ pluxInterface.PluxDevManager.GetProductIdUnity()}");
            writer.WriteLine($"Sampling Rate,{pluxInterface.SamplingRate}");
            writer.WriteLine($"Resolution,{pluxInterface.Resolution}");
            writer.WriteLine("");

            int channelCount = pluxInterface.ActiveChannels.Count;

            // Dynamic size header
            string header = $"Samples,";
            for (int i = 0; i < channelCount; i++)
            {
                header += string.Format($"CH{pluxInterface.ActiveChannels[i]},Value,");
            }

            writer.WriteLine(header);
        }

        private void OnApplicationQuit()
        {
            // Close the write stream on app quit just so we dont get any unexpected file crashes
            if (_dataWriter != null)
            {
                _dataWriter.Close();
            }

            if (_eventWriter != null) 
            {
                _eventWriter.Close();
            }
        }
    }
}