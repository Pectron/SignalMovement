using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class controlTime : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro tempoValor;
    private float tempo;

    [SerializeField]
    private bool stopCount, inTutorial;

    // Start is called before the first frame update
    void Start()
    {
        if (!inTutorial)
            tempo = PlayerPrefs.GetInt("tempo") * 60;
        else
            tempo = TutorialScene.timeDesviar * 60;

        if (tempo <= 0)
            tempo = 1 * 60;

        tempoValor.text = tempo.ToString("0.00");
        controlTextTempo();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopCount)
        {
            tempo -= Time.deltaTime;
            controlTextTempo();

            if (tempo <= 0)
                jogoAcabou();
        }
    }


    private void controlTextTempo()
    {
        float minutes = Mathf.FloorToInt(tempo / 60);
        float seconds = Mathf.FloorToInt(tempo % 60);

        tempoValor.text = minutes.ToString() + ":" + seconds.ToString();
        if (seconds < 10)
        {
            tempoValor.text = minutes.ToString() + ":0" + seconds.ToString();
        }
    }

    public void jogoAcabou()
    {
        if(!inTutorial)
            SceneManager.LoadScene("menu");
    }

    public void menu()
    {
        SceneManager.LoadScene("menu");
    }

    public float getTempo()
    {
        return tempo;
    }

    public void setTempo(float valor)
    {
        tempo = valor * 60;
        controlTextTempo();
    }

    public void StopCount(bool valor)
    {
        stopCount = valor;

        if(inTutorial && valor)
        {
            tempo = TutorialScene.timeDesviar * 60;
            controlTextTempo();
        }  
    }

    public bool GetStopCount() => stopCount;
}
