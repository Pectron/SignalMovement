using UnityEngine;
using System.IO;
public static class DataModuleBase
{
    public static string GetPath(string userId)
    {
        DirectoryInfo dirInfo = Directory.GetParent(Application.dataPath);
        string filePath = dirInfo.FullName + $"//{GetProjectName()}_Recolha//{userId}";

        // Creates directory in the specified path unless it already exists
        Directory.CreateDirectory(filePath);

        return filePath;
    }
    private static string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
}
