using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class QuestionnaireRecorder
{
    private const string EXTENSION = ".csv";
    private string filepath;
    StreamWriter file;

    public void RecordData(/*SortedDictionary<string, string> experience, */SortedDictionary<string, string> questionnaireAnswers)
    {
        string fileName;

        filepath = DataModuleBase.GetPath(User.Id);

        Directory.CreateDirectory(filepath);
        
        fileName = $"Dissonance_Questionnaire_{User.Id}";

        file = new StreamWriter(Path.Combine(filepath, fileName + EXTENSION), true, Encoding.UTF8);

        AddData(/*experience, */questionnaireAnswers);

        file.Close();
    }

    private void AddData(/*SortedDictionary<string, string> experience, */SortedDictionary<string, string> questionnaireAnswers)
    {
        /*file.WriteLine("Experience");
        file.WriteLine();

        foreach (KeyValuePair<string, string> kPair in experience)
        {
            file.Write(kPair.Key + ",");
        }

        file.WriteLine();

        foreach (KeyValuePair<string, string> kPair in experience)
        {
            file.Write(kPair.Value + ",");
        }

        file.WriteLine();
        file.WriteLine();*/

        file.WriteLine("Questionnaire");
        file.WriteLine();

        foreach (KeyValuePair<string, string> kPair in questionnaireAnswers)
        {
            file.Write(kPair.Key + ",");
        }

        file.WriteLine();

        foreach (KeyValuePair<string, string> kPair in questionnaireAnswers)
        {
            file.Write(kPair.Value + ",");
        }
    }
}
