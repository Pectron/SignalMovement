using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DemographicsUIManager_PM : MonoBehaviour
{
    [Header("Pages")]
    public List<GameObject> pages = new List<GameObject>();
    int page;

    [Header("Age")]
    public TMP_InputField ageInput;

    [Header("Gender")]
    public Button maleButton;
    public Button femaleButton;
    public Button otherGenderButton;
    public TMP_InputField genderOtherInput;


    [Header("Nationality")]
    public Button portugueseButton;
    public Button otherNationalityButton;
    public TMP_InputField nationalityOtherInput;


    [Header("Education")]
    public List<Button> educationButtons = new List<Button>();

    [Header("Civil State")]
    public List<Button> civilButtons = new List<Button>();
    
    [Header("Job")]
    public List<Button> jobButtons = new List<Button>();
    
    /*[Header("PC")]
    public List<Button> pcButtons = new List<Button>();
    */
    [Header("Games")]
    public List<Button> gamesButtons = new List<Button>();
    
    [Header("VR")]
    public List<Button> vrButtons = new List<Button>();
    
    [Header("Answers")]
    private string age;
    private string gender;
    private string nationality;
    private string education;
    private string civil;
    private string job;
    //private string pcXP;
    private string gamesXP;
    private string vrXP;
    string question;
    public SortedDictionary<string, string> demographicsAnswers = new SortedDictionary<string, string>();
    string[] questions = {"q01_age", "q02_gender", "q03_nat", "q04_educ", "q05_civil", "q06_job", "q07_gamesXP", "q08_vrXP" };
    public TMP_Text pageNumber;
    public Button previousPage, nextPage;

    public int totalQuestions;

    private void Start()
    {
        
        if (genderOtherInput.text == "")
            gender = null;

        if (nationalityOtherInput.text == "")
            nationality = null;

        CheckPage();
    }
    private void OnEnable()
    {
        page = 0;
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == 0)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
        totalQuestions = questions.Length;
        FindObjectOfType<QuestionnaireUIManager>().continueButton.GetComponent<Button>().interactable = false;

        CheckForCompletion();
        CheckPage();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            foreach (KeyValuePair<string, string> kvp in demographicsAnswers)
                Debug.Log(kvp.Key + " " + kvp.Value);
        }
    }
    public void SetAge()
    {
        question = "q01_age";
        if (ageInput.text != "")
            age = ageInput.text;
        else
            age = null;

        AddAnswer(question, age);
        CheckForCompletion();
    }


    public void ChooseGender(string _st)
    {
        question = "q02_gender";
        maleButton.interactable = true;
        femaleButton.interactable = true;
        otherGenderButton.interactable = true;
        genderOtherInput.interactable = false;
        genderOtherInput.text = "";

        if (_st == "male")
            maleButton.interactable = false;
        else if (_st == "female")
            femaleButton.interactable = false;
        else
        {
            otherGenderButton.interactable = false;
            genderOtherInput.interactable = true;
        }

        if (_st != "other")
            gender = _st;
        else
            gender = null;

        AddAnswer(question, gender);
        CheckForCompletion();
    }

    public void OtherGenderChoice()
    {
        question = "q02_gender";
        gender = genderOtherInput.text;

        if (genderOtherInput.text == "")
            gender = null;
        else
            AddAnswer(question, gender);

        CheckForCompletion();
    }

    public void ChooseNationality(string _st)
    {
        question = "q03_nat";
        portugueseButton.interactable = true;
        otherNationalityButton.interactable = true;
        nationalityOtherInput.interactable = false;
        nationalityOtherInput.text = "";
        if (_st == "other")
        {
            otherNationalityButton.interactable = false;
            nationalityOtherInput.interactable = true;
        }
        else
            portugueseButton.interactable = false;

        if (_st != "other")
            nationality = _st;
        else
            nationality = null;

        AddAnswer(question, nationality);
        CheckForCompletion();
    }

    public void OtherNationalityChoice()
    {
        question = "q03_nat";
        nationality = nationalityOtherInput.text;

        if (nationalityOtherInput.text == "")
            nationality = null;
        else
            AddAnswer(question, nationality);
        CheckForCompletion();
    }

    public void ChooseEducation(int _i)
    {
        question = "q04_educ";
        foreach (Button _but in educationButtons)
        {
            _but.interactable = true;
        }

        educationButtons[_i - 1].interactable = false;

        education = _i.ToString();

        AddAnswer(question, education);
        CheckForCompletion();
    }

    public void ChooseCivil(int _i)
    {
        question = "q05_civil";
        foreach (Button _but in civilButtons)
        {
            _but.interactable = true;
        }

        civilButtons[_i - 1].interactable = false;

        civil = _i.ToString();

        AddAnswer(question, civil);
        CheckForCompletion();
    }
    
    public void ChooseJob(int _i)
    {
        question = "q06_job";
        foreach (Button _but in jobButtons)
        {
            _but.interactable = true;
        }

        jobButtons[_i - 1].interactable = false;

        job = _i.ToString();

        AddAnswer(question, job);
        CheckForCompletion();
    }

/*    public void PCXP(int _i)
    {
        question = "q07_PC-XP";
        foreach (Button _but in pcButtons)
        {
            _but.interactable = true;
        }

        pcButtons[_i - 1].interactable = false;

        pcXP = _i.ToString();

        AddAnswer(question, pcXP);
        CheckForCompletion();
    }*/
    public void GamesXP(int _i)
    {
        question = "q07_gamesXP";
        foreach (Button _but in gamesButtons)
        {
            _but.interactable = true;
        }

        gamesButtons[_i - 1].interactable = false;

        gamesXP = _i.ToString();

        AddAnswer(question, gamesXP);
        CheckForCompletion();
    }

    public void VRXP(int _i)
    {
        question = "q08_vrXP";
        foreach (Button _but in vrButtons)
        {
            _but.interactable = true;
        }

        vrButtons[_i - 1].interactable = false;

        vrXP = _i.ToString();

        AddAnswer(question, vrXP);
        CheckForCompletion();
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
    void AddAnswer(string _question, string _answer) 
    {
        if (demographicsAnswers.ContainsKey(_question))
            demographicsAnswers.Remove(_question);

        demographicsAnswers.Add(_question, _answer);

        if (FindObjectOfType<DataManager>().questionnaireAnswers.ContainsKey(_question))
            FindObjectOfType<DataManager>().questionnaireAnswers.Remove(_question);

        FindObjectOfType<DataManager>().questionnaireAnswers.Add(_question, _answer);

    }
    public void CheckForCompletion() 
    {
        if (FindObjectOfType<QuestionnaireUIManager>().canSkipAnswers)
        {
            foreach (string _st in questions)
            {
                if (!demographicsAnswers.ContainsKey(_st))
                {
                    demographicsAnswers.Add(_st, FindObjectOfType<QuestionnaireUIManager>().empty);
                }
                if (!FindObjectOfType<DataManager>().questionnaireAnswers.ContainsKey(_st))
                    FindObjectOfType<DataManager>().questionnaireAnswers.Add(_st, FindObjectOfType<QuestionnaireUIManager>().empty);
            }
        }
        else
        {
            if (demographicsAnswers.Count < totalQuestions)
                FindObjectOfType<QuestionnaireUIManager>().continueButton.GetComponent<Button>().interactable = false;
            else
                FindObjectOfType<QuestionnaireUIManager>().continueButton.GetComponent<Button>().interactable = true;
        }
    }

    public void ClearSelection()
    {
        ageInput.text = "";

        maleButton.interactable = true;
        femaleButton.interactable = true;
        otherGenderButton.interactable = true; ;
        genderOtherInput.text = "";

        portugueseButton.interactable = true; ;
        otherNationalityButton.interactable = true; ;
        nationalityOtherInput.text = "";


        foreach (Button _btn in educationButtons)
        {
            _btn.interactable = true;
        }
        foreach (Button _btn in civilButtons)
        {
            _btn.interactable = true;
        }
        foreach (Button _btn in jobButtons)
        {
            _btn.interactable = true;
        }
        /*foreach (Button _btn in pcButtons)
        {
            _btn.interactable = true;
        }*/
        foreach (Button _btn in gamesButtons)
        {
            _btn.interactable = true;
        }
        foreach (Button _btn in vrButtons)
        {
            _btn.interactable = true;
        }
        
        ClearAnswers();

        CheckForCompletion();
    }

    void CheckPage()
    {
        pageNumber.text = (page + 1).ToString() + "/" + pages.Count.ToString();

        if (page == 0)
        {
            previousPage.gameObject.SetActive(false);

            if (pages.Count > 1)
                nextPage.gameObject.SetActive(true);
            else
                nextPage.gameObject.SetActive(false);
        }
        else if (page == (pages.Count - 1))
        {
            nextPage.gameObject.SetActive(false);
            previousPage.gameObject.SetActive(true);
            if (FindObjectOfType<QuestionnaireUIManager>().canSkipAnswers)
            {
                if (FindObjectOfType<QuestionnaireUIManager>().continueButton.GetComponent<Button>().interactable == false)
                    FindObjectOfType<QuestionnaireUIManager>().continueButton.GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            previousPage.gameObject.SetActive(true);
            nextPage.gameObject.SetActive(true);
        }
    }

    public void ClearAnswers()
    {
        demographicsAnswers.Clear();
    }
}