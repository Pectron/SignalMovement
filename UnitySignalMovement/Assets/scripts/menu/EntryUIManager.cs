using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EntryUIManager : MonoBehaviour
{
    public Button startButton;
    public Toggle consentToggle;

    public Button moveToConsent;
    public TMP_InputField codeText;

    public GameObject codePanel;
    public GameObject consentPanel;

    void Start()
    {
        consentPanel.SetActive(false);
        codePanel.SetActive(true);
        moveToConsent.interactable = false;
        startButton.interactable = false;
    }

    public void CheckCode() 
    {
        moveToConsent.interactable = true;

        if (codeText.text == " ")
            startButton.interactable = false;
    }

    public void CheckConsent()
    {
        startButton.interactable = true;

        if (!consentToggle.isOn)
            startButton.interactable = false;
    }

    public void StartApp() 
    {
        SceneManager.LoadScene(1);
    }

    public void ExitApp() 
    {
        Application.Quit();
    }

    public void MoveToConsent() 
    {        
        User.SetId(codeText.text);
        FindObjectOfType<DataManager>().questionnaireAnswers.Add("a01_userCode", User.Id);

        foreach (KeyValuePair<string, string> kvp in FindObjectOfType<DataManager>().questionnaireAnswers)
            Debug.Log(kvp.Key + " " + kvp.Value);        
        
        codePanel.SetActive(false);
        consentPanel.SetActive(true);
    }



    // for condition
    // FindObjectOfType<DataManager>().questionnaireAnswers.Add("a02_condition", ***condition variable***);
}
