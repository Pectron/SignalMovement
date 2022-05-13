using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebriefingUIManager_noQuest : MonoBehaviour
{
    int page;

    [HideInInspector]
    public bool isLastPage;

    public List<GameObject> pages = new List<GameObject>();
    public TMP_Text pageNumber;
    public Button previousPage, nextPage;
    public Button finishButton;
    private bool hasAgreed;
    public GameObject waitPanel;
    public GameObject finishPanel;

    private void OnEnable()
    {
        pageNumber.gameObject.SetActive(false);
        page = 0;
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == 0)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
        CheckPage();
        finishButton.gameObject.SetActive(false);

        StartCoroutine(FinishApp());
    }

    public void TurnPage(bool _b)
    {
        if (_b)
        {
            pages[page].SetActive(false);
            page++;
            pages[page].SetActive(true);
        }
        else
        {
            pages[page].SetActive(false);
            page--;
            pages[page].SetActive(true);
        }
        CheckPage();
    }

    void CheckPage()
    {
        pageNumber.text = (page + 1).ToString() + "/" + pages.Count.ToString();

        if (pages.Count > 1)
        {
            if (page == 0)
            {
                previousPage.gameObject.SetActive(false);
                nextPage.gameObject.SetActive(true);
                isLastPage = false;
            }
            else if (page == (pages.Count - 1))
            {
                nextPage.gameObject.SetActive(false);
                previousPage.gameObject.SetActive(true);
                isLastPage = true;
            }
            else
            {
                previousPage.gameObject.SetActive(true);
                nextPage.gameObject.SetActive(true);
                isLastPage = false;
            }
        }
        else 
        {
            previousPage.gameObject.SetActive(false);
            nextPage.gameObject.SetActive(false);
        }
            
    }

    void FinishPanel()
    {
        waitPanel.SetActive(false);
        finishPanel.SetActive(true);
    }

    IEnumerator FinishApp()
    {
        waitPanel.SetActive(true);
        FindObjectOfType<QuestionnaireUIManager>().SendQuestions();
        yield return new WaitForSeconds(2f);
        waitPanel.SetActive(false);
        finishPanel.SetActive(true);
    }

}
