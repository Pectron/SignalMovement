using UnityEngine;
using System.IO;

public static class CSVManager
{
    private static string reportDirectoryName = "Reports/";
    private static string reportFileName = "AllData.csv";
    private static string reportSeparator = ",";
    private static string[] reportHeaders = GameObject.FindObjectOfType<AddAllData>().headers.ToArray();

    #region Interactions

    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();
        using (StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += strings[i];
            }
            sw.WriteLine(finalString);
        }
    }

    public static void CreateReport()
    {
        VerifyDirectory();
        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                {
                    finalString += reportSeparator;
                }
                finalString += reportHeaders[i];
            }
            sw.WriteLine(finalString);
        }
            
    }

    #endregion

    #region Operations

    static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    static void VerifyFile()
    {
        string file = GetFilePath();
        if (!File.Exists(file))
        {
            CreateReport();
        }
    }

    #endregion

    #region Queries

#if UNITY_ANDROID
    static string GetDirectoryPath()
    {
        string result = "";
        result = Application.persistentDataPath + "/" + reportDirectoryName;
        return result;
    }

#elif UNITY_STANDALONE_WIN
 
    static string GetDirectoryPath()
    {
        string result ="";
        result = Application.dataPath + "/" + reportDirectoryName;
        return result;
    }
#elif UNITY_WEBGL

    static string GetDirectoryPath()
    {
        string result = "";
        result = Application.dataPath + "/" + reportDirectoryName;
        return result;
    }
#endif

    public static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }

#endregion
}
