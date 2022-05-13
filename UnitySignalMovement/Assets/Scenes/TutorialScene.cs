using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScene : MonoBehaviour
{

    [SerializeField]
    private GameObject spawnParedes, spawnLetras, spawnAlvos, spawnTudo;

    [SerializeField] 
    private GameObject[] botoes; //[0] é o butNext
    private int pressButtons;

    private controlTime contTime;
    public static float timeDesviar = 0.5f;


    //audios
    [SerializeField] AudioClip[] dialog;
    AudioSource aSWorker;
    private bool talking;
    private int phase;
    private int indexDialogo;

    [SerializeField]
    private AudioSource musicPlay;

    [SerializeField] int indexValorNumTutorial;
    public int indexNumTutorial;

    // Start is called before the first frame update
    void Start()
    {
        contTime = GameObject.Find("ControlTempo").GetComponent<controlTime>();
        aSWorker = GetComponent<AudioSource>();

        phase = 0;
        StartCoroutine(TalkPhase());
    }

    // Update is called once per frame
    void Update()
    {
        //Fases Acao em que existe tempo
        if(phase == 3 || phase == 5 || phase == 7)
        {
            if(indexNumTutorial <= 0)
            {
                spawnParedes.SetActive(false);
                spawnLetras.SetActive(false);
                spawnAlvos.SetActive(false);
                spawnTudo.SetActive(false);

                contTime.StopCount(true);
                musicPlay.Stop();
                NextPhase();
            }
            
            contTime.setTempo(timeDesviar);
        }
        else if (phase == 9)
        {
            if (contTime.getTempo() <= 0)
            {
                spawnParedes.SetActive(false);
                spawnLetras.SetActive(false);
                spawnAlvos.SetActive(false);
                spawnTudo.SetActive(false);

                contTime.StopCount(true);
                musicPlay.Stop();
                NextPhase();
            }
        }
        else
            ResetIndexValorTut(1);

    }

    IEnumerator TalkPhase()
    {
        switch (phase)
        {
            case 0: //ter atenção aos comandos, verificar se a mao dir esta certa - clicar no bot se tudo ok

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;

            case 1: //andar no espaço e ter cuidado com as bordas / ir clicar em botoes

                botoes[0].GetComponent<Animator>().SetTrigger("desaparece");
                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[1]);
                AppearButton(botoes[2]);
                AppearButton(botoes[3]);
                AppearButton(botoes[4]);
                talking = false;
                break;

            case 2: // preparar para desviar paredes

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;

            case 3: //SPAWN PAREDES

                ResetIndexValorTut(1);
                botoes[0].GetComponent<Animator>().SetTrigger("desaparece");
                //contTime.StopCount(false);
                spawnParedes.SetActive(true);
                musicPlay.Play();
                break;

            case 4: //preparar para spawn cubos

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;

            case 5: //SPAWN CUBOS

                ResetIndexValorTut(4);
                contTime.StopCount(false);
                botoes[0].GetComponent<Animator>().SetTrigger("desaparece");
                //contTime.StopCount(false);
                spawnLetras.SetActive(true);
                musicPlay.Play();
                break;

            case 6: //preparar para alvos spawn

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;

            case 7: //SPAWN ALVOS

                ResetIndexValorTut(2);
                contTime.StopCount(false);
                botoes[0].GetComponent<Animator>().SetTrigger("desaparece");
                //contTime.StopCount(false);
                spawnAlvos.SetActive(true);
                musicPlay.Play();
                break;

            case 8: //preparar para spawn tudo

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;

            case 9: //SPAWN TUDO

                ResetIndexValorTut(40);
                botoes[0].GetComponent<Animator>().SetTrigger("desaparece");
                contTime.StopCount(false);
                spawnTudo.SetActive(true);
                spawnParedes.SetActive(true);
                musicPlay.Play();
                break;

            case 10: //fim

                talk();
                yield return new WaitForSeconds(aSWorker.clip.length);
                AppearButton(botoes[0]);
                talking = false;
                break;
        }
    }

    private void talk()
    {
        talking = true;
        aSWorker.clip = dialog[indexDialogo];
        aSWorker.Play();
        indexDialogo++;
    }

    public void NextPhase()
    {
        if ((talking == false && contTime.GetStopCount() == true) || (talking == false && indexNumTutorial <= 0))
        {
            if (phase == 10)
                SceneManager.LoadScene("menu");
            else
            {
                phase++;
                StartCoroutine(TalkPhase());
            }
        }
    }

    public void countPressButton(GameObject but)
    {
        but.GetComponent<Animator>().SetTrigger("desaparece");
        pressButtons ++;

        if (pressButtons >= 4)
        {
            botoes[1].GetComponent<Animator>().SetTrigger("desaparece");
            botoes[2].GetComponent<Animator>().SetTrigger("desaparece");
            botoes[3].GetComponent<Animator>().SetTrigger("desaparece");
            botoes[4].GetComponent<Animator>().SetTrigger("desaparece");
            NextPhase();
            pressButtons = 0;
        }
    }

    private void AppearButton(GameObject but)
    {
        but.GetComponentInChildren<ButtonPress>().Appear();
        but.GetComponent<Animator>().SetTrigger("aparece");
        but.GetComponentInChildren<AudioSource>().Play();
    }


    public void ResetIndexValorTut(int multiplicar)
    {
        indexNumTutorial = indexValorNumTutorial * multiplicar;
    }
}
