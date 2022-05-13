using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAllData : MonoBehaviour
{
    [HideInInspector]
    public List<string> headers = new List<string>();
    [HideInInspector] 
    public List<string> allData = new List<string>();
    
    public void AddData()
    {
        /*foreach (KeyValuePair<string, string> kvp in FindObjectOfType<DataManager>().experienceAnswers)
        {
            headers.Add(kvp.Key);
            allData.Add(kvp.Value);
        }*/

        foreach (KeyValuePair<string, string> kvp in FindObjectOfType<DataManager>().questionnaireAnswers)
        {
            headers.Add(kvp.Key);
            allData.Add(kvp.Value);
        }

        CSVManager.AppendToReport(allData.ToArray());
    }
}
