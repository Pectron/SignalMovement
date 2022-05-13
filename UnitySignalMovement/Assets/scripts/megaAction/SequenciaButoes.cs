using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenciaButoes : MonoBehaviour
{
    //Valores random
    [SerializeField]
    private int   seedValor;
    private int[] valoresRandom;
    private int   indexRandom;

    private GameObject[] Botoes;
    private GameObject butaoDestacado;

    [SerializeField]
    private Material materialNormal, materialDestacado;

    //PONTOS
    [SerializeField]
    private controlScore score;

    //SONS
    [SerializeField]
    private AudioSource clickSound, perdeuSound, timeSound;

    //TIMER
    [SerializeField]
    private Timer TimerBar;
    [SerializeField]
    private float maxTime, minTime, descontarTime;
    private float time;
    [SerializeField]
    private controlTime contTime;

    //Errar
    [SerializeField]
    private float tempoInativo;
    [SerializeField]
    private Light luz;

    private AudioSource _botaoAudioSource;


    //Questionario
    [SerializeField]
    private GameObject askGameObject;
    private bool answered;

    // Start is called before the first frame update
    void Start()
    {
        //TEMPO
        Botoes = GameObject.FindGameObjectsWithTag("button");
        TimerBar.SetMaxTime(maxTime);
        TimerBar.SetTime(maxTime);
        time = maxTime;

        Random.seed = seedValor;
        valoresRandom = new int[1000];
        int i = 0;
        while (i < valoresRandom.Length)
        {
            valoresRandom[i] = Random.Range(0, 35);
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(answered)
        {
            time -= Time.deltaTime;

            //AUDIO BUTAO DESTACADO
            _botaoAudioSource = butaoDestacado.transform.parent.transform.GetChild(2).GetComponent<AudioSource>();
            _botaoAudioSource.pitch = -(1.0f / 7) * time + 2.4f; //10 = maxtime inicio
            _botaoAudioSource.volume = -(1.0f / 7) * time + 2.4f;

            if (time < 0)
            {
                tempoInativo = 1;
                perdeuBotao();
            }

            TimerBar.SetTime(time);

            controlLight();
        }
    }


    public void checkButClick(GameObject butaoClick)
    {
        if(butaoClick == butaoDestacado)
        {
            butaoDestacado.transform.GetChild(0).GetComponent<Renderer>().material = materialNormal;
            butaoDestacado.transform.parent.GetChild(2).GetComponent<AudioSource>().Stop();

            clickSound.volume = Random.Range(0.8f, 1.2f);
            clickSound.pitch  = Random.Range(1.5f, 2f);
            clickSound.Play();

            score.DoScore(Random.Range(5, 10));

            descontarTempo();
            DestacarButao();
        }
    }

    private void perdeuBotao()
    {
        butaoDestacado.transform.GetChild(0).GetComponent<Renderer>().material = materialNormal;
        _botaoAudioSource.Stop();

        perdeuSound.volume = Random.Range(0.8f, 1.2f);
        perdeuSound.pitch  = Random.Range(0.8f, 1.3f);
        perdeuSound.Play();

        score.DoScore(Random.Range(-5, -10));

        descontarTempo();
        DestacarButao();
    }

    private void DestacarButao()
    {
        int num = valoresRandom[indexRandom];

        butaoDestacado = Botoes[num];
        butaoDestacado.transform.GetChild(0).GetComponent<Renderer>().material = materialDestacado;

        clickSound.volume = Random.Range(0.8f, 1.2f);
        clickSound.pitch  = Random.Range(0.5f, 1f);
        clickSound.Play();

        _botaoAudioSource = butaoDestacado.transform.parent.transform.GetChild(2).GetComponent<AudioSource>();
        _botaoAudioSource.Play();
        time = maxTime;
        _botaoAudioSource.pitch = 1;
        _botaoAudioSource.volume = 1;

        indexRandom++;
        if (indexRandom > valoresRandom.Length - 1)
            indexRandom = 0;
    }

    private void descontarTempo()
    {
        if (maxTime > minTime)
            maxTime -= descontarTime;
        else
            maxTime = minTime;

        TimerBar.SetMaxTime(maxTime);
    }


    public void wasAnswered(float height)
    {
        transform.position = new Vector3(transform.position.x, height - 2.06f , transform.position.z);
        answered = true;
        askGameObject.SetActive(false);
        contTime.StopCount(false);

        DestacarButao();
    }

    private void controlLight()
    {
        //LUZ
        if (luz != null)
        {
            if (tempoInativo > 0)
            {
                tempoInativo -= Time.deltaTime;
                luz.color = Color.red;
                score.WrongColor();
            }
            else
            {
                luz.color = new Color(1, 1f, 1f);
                score.normalColor();
            }
        }
    }
}
