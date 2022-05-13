using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestionnaireUIManager : MonoBehaviour
{
    int page;

    public GameObject previousButton;
    public GameObject continueButton;
    public List<GameObject> quizPages = new List<GameObject>();

    public bool canSkipAnswers;
    

    public string empty;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        for (int i = 0; i < quizPages.Count; i++)
        {
            if (i == 0)
                quizPages[i].SetActive(true);
            else
                quizPages[i].SetActive(false);
        }
        page = 0;
        previousButton.SetActive(false);
    }

    public void NextSection()
    {
        quizPages[page].SetActive(false);
        page++;
        quizPages[page].SetActive(true);

        if (page + 1 == quizPages.Count)
            continueButton.SetActive(false);
        else
            continueButton.SetActive(true);

        previousButton.SetActive(true);
    }

    public void PreviousSection()
    {
        quizPages[page].SetActive(false);
        page--;
        quizPages[page].SetActive(true);

        if (page == 0)
            previousButton.SetActive(false);
        else
            previousButton.SetActive(true);

        if (page + 1 < quizPages.Count)
            continueButton.SetActive(true);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("quitting app");
    }

    public void SendQuestions()
    {
        SortedDictionary<string, string> questAnswers = FindObjectOfType<DataManager>().questionnaireAnswers;

        foreach (KeyValuePair<string, string> kvp in questAnswers)
            Debug.Log(kvp.Key + " - " + kvp.Value);

        GetComponent<AddAllData>().AddData();

        /*QuestionnaireRecorder questRec = new QuestionnaireRecorder();
        questRec.RecordData(questAnswers);*/

    }

    public void ClearAnswers() 
    {
        FindObjectOfType<DataManager>().questionnaireAnswers.Clear();        
        Debug.Log(FindObjectOfType<DataManager>().questionnaireAnswers.Count);
        Debug.Log("DB cleared");
    }

    public void LoadBlank() 
    {
        SceneManager.LoadScene(2);
    }
}
