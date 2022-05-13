using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class menuControl : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tempoValor;

    int timeGameplay = 3;

    private PhysiologySignalsManager _bioGadget;

    private void Awake()
    {
        var bio = GameObject.Find("Biosignals");
        if (bio != null)
        {
            _bioGadget = bio.GetComponent<PhysiologySignalsManager>();
            _bioGadget.NewMarker("Entrou no Menu Gameplay");
        }
    }

    public void CarregarPassive()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("Entrou no Modo Passive");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Passive");
    }

    public void CarregarMedium()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("Entrou no Modo Medium");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Medium");
    }

    public void CarregarAction()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("Entrou no Modo Action");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Action");
    }

    public void CarregarMega()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("Entrou no Modo Mega");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("MegaAction");
    }

    public void CarregarPraticar()
    {
        SceneManager.LoadScene("Pratique");
    }

    public void Exit()
    {
        _bioGadget.NewMarker("Acabou Gameplay");
        SceneManager.LoadScene("Questionnaire_PM");
    }
}
