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
            _bioGadget.NewMarker("GM_START_MENU");
        }
    }

    public void CarregarPassive()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_PASSIVE");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Passive");
    }

    public void CarregarMedium()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_MEDIUM");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Medium");
    }

    public void CarregarAction()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_ACTION");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("Action");
    }

    public void CarregarMega()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_MEGA");
        PlayerPrefs.SetInt("tempo", timeGameplay);
        SceneManager.LoadScene("MegaAction");
    }

    public void CarregarPraticar()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_PRATIQUE");

        SceneManager.LoadScene("Pratique");
    }

    public void Exit()
    {
        if (_bioGadget != null)
            _bioGadget.NewMarker("GM_START_EXIT");
        SceneManager.LoadScene("EndLabRecorder");
    }
}
