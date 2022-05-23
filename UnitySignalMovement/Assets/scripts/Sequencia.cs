using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sequencia : MonoBehaviour
{
    [SerializeField] private float nivel;
    [SerializeField] private bool  tutorial;
    private TutorialScene tutorialScene;

    //BASES MOVIMENTO
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float maxSpeed = 25;
    [SerializeField]
    private float speedInicial = 10;
    [SerializeField]
    private float incremento = 7;

    private Rigidbody rb;


    public List<GameObject> ListaAlvosCubos = new List<GameObject>();

    //Array de sequencias Letras e Alvos
    [SerializeField]
    private GameObject[] sequenciasLetras, sequenciasAlvos;
   
    [SerializeField]
    private float PosInicialZ, minPosZ;


    //Tempo
    [SerializeField]
    private float tempoInativo; //quando errar
    private float time = 0;
    private controlTime contTime;

    [SerializeField]
    private Light luz;

    [SerializeField]
    private controlScore score;


    //RANDOM Seed
    [SerializeField]
    private int SeedSpawn = 42;
    [SerializeField]
    private int SeedSpawnLetters = 42;
    [SerializeField]
    private int SeedSpawnAlvos = 42;

    private int[] valoresRandom, valoresRandomLetter, valoresRandomAlvos;
    private int indexValor = 0, indexValorLetter = 0, indexValorAlvos = 0;
    private bool inStart = true;


    //SOUND
    [SerializeField]
    private AudioSource BackgSound;
    [SerializeField]
    private AudioSource BassSound;
    [SerializeField]
    private AudioSource WrongSound;


    //Explosion Effect
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private GameObject explosionEffectRed;



    // Start is called before the first frame update
    void Start()
    {
        valoresRandom       = SeedRandom(SeedSpawn);
        valoresRandomLetter = SeedRandom(SeedSpawnLetters);
        valoresRandomAlvos  = SeedRandom(SeedSpawnAlvos);

        rb       = transform.GetChild(0).GetComponent<Rigidbody>();
        contTime = GameObject.Find("ControlTempo").GetComponent<controlTime>();

        if (tutorial)
            tutorialScene = GameObject.Find("TutorialManage").GetComponent<TutorialScene>();
    }


    // Update is called once per frame
    void Update()
    {
        //Se o tempo n�o tiver parado
        if(contTime.GetStopCount() == false)
        {
            VerificarCubosVazios();
            LimitZLoose();

            if (tutorial == false)
            {
                MoveSequencia();
            }

            controlLight();
        }
    }
    
    void resetCube() //Destroi tudo e sequencia volta � posicao inicial
    {
        //letras
        if (transform.GetChild(0).childCount > 0)
            Destroy(transform.GetChild(0).GetChild(0).gameObject);

        transform.GetChild(0).position = new Vector3(0, 0, 60);

        //alvos
        if (transform.GetChild(1).childCount > 0)
            Destroy(transform.GetChild(1).GetChild(0).gameObject);

        spawnSequence();
    }

    
    void spawnSequence() //De forma "aleatoria", escolhe uma sequencia e spawna
    {
        int numModo   = valoresRandom[indexValor];
        int numLetter = valoresRandomLetter[indexValorLetter];
        int numAlvo   = valoresRandomAlvos[indexValorAlvos];

        if (nivel == 1)
        {
            SpawnLetterCube(numLetter);
        }
        else if (nivel == 2)
        {
            SpawnAlvo(numAlvo);
        }
        else if (nivel == 3)
        {
            if(numModo == 0 || numModo == 3)
                SpawnLetterCube(numLetter);

            else if (numModo == 1 || numModo == 4 || numModo == 6 || numModo == 8 || numModo == 9)
                SpawnAlvo(numAlvo);

            else if (numModo == 2 || numModo == 5 || numModo == 7)
            {
                SpawnAlvo(numAlvo);
                SpawnLetterCube(numLetter);
            }
        }

        //obter os cubos e alvos spawnados
        ListaAlvosCubos = new List<GameObject>();
        SearchAlvosCubes();

        indexValor++;
        if(indexValor > valoresRandom.Length - 1)
               indexValor = 0;
    }

    void SpawnLetterCube(int num)
    {
        if (num == 6)
            num = 0;
        else if (num == 7)
            num = 2;
        else if (num == 8)
            num = 3;
        else if (num == 9)
            num = 5;

        GameObject sequencia = sequenciasLetras[num];
        Instantiate(sequencia, transform.position, Quaternion.identity, this.transform.GetChild(0));

        indexValorLetter++;
        if (indexValorLetter > valoresRandomLetter.Length - 1)
            indexValorLetter = 0;
    }

    void SpawnAlvo(int num)
    {
        GameObject sequencia = sequenciasAlvos[num];
        Instantiate(sequencia, sequenciasAlvos[num].transform.position, sequenciasAlvos[num].transform.rotation, this.transform.GetChild(1));

        indexValorAlvos++;
        if (indexValorAlvos > valoresRandomAlvos.Length - 1)
            indexValorAlvos = 0;
    }

    void SearchAlvosCubes() //Procura os cubos e alvos coloca-os numa lista
    {
        GameObject[] cubos = GameObject.FindGameObjectsWithTag("Cube");

        foreach (GameObject cube in cubos)
            ListaAlvosCubos.Add(cube);
    }

    public void verifyLetterCorrectCube(string letra) //fun��o chamada quando existe um click, e verifica se a letra de algum cubo corresponde ao botao clicado
    {

        bool letraCerta = false;

        if (tempoInativo > 0)
            return;

        if (ListaAlvosCubos.Count > 0)
        {
            foreach (GameObject cube in ListaAlvosCubos)
            {
                if (cube != null)
                {
                    if (cube.GetComponent<LetterCube>() != null)
                    {
                        letraCerta = cube.GetComponent<LetterCube>().checkRightLetter(letra);

                        if (letraCerta)
                        {
                            setIndexOnTutorial();

                            if (letra == "R")
                            {
                                Instantiate(explosionEffect, cube.transform.position, cube.transform.rotation);
                                BassSound.pitch = Random.Range(0.4f, 1f);
                            }
                            else if (letra == "L")
                            {
                                Instantiate(explosionEffectRed, cube.transform.position, cube.transform.rotation);
                                BassSound.pitch = Random.Range(1.4f, 2f);
                            }


                            score.DoScore(Random.Range(2, 4));
                            BassSound.volume = Random.Range(1.2f, 2f);
                            BassSound.Play();

                            ListaAlvosCubos.Remove(cube);
                            Destroy(cube);
                            break;
                        }
                    }
                }
            }
        }

        if(letraCerta == false && nivel != 0)
        {
            teclaErrada(1, -1, -3);
        }
    }

    private void VerificarCubosVazios()
    {
        bool allVazios = true;

        for(int i = ListaAlvosCubos.Count - 1; i >= 0; i--)
        {
            if (ListaAlvosCubos[i] != null)
            {
                allVazios = false;
            }
        }

        if (allVazios)
            ListaAlvosCubos = new List<GameObject>();
    }

    private void LimitZLoose()
    {
        Vector3 posChild0 = transform.GetChild(0).position;

        if (posChild0.z < minPosZ || ListaAlvosCubos.Count <= 0)
        {
            if (posChild0.z < minPosZ)
            {
                teclaErrada(0.5f, -4, -7);
            }
            resetCube();
        }
    }

    private void MoveSequencia()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -movementSpeed);

        //MOVIMENTO
        if (indexValor < 7 && inStart == true)
        {
            movementSpeed = 3.5f + 0.4f * indexValor;
            BackgSound.pitch = 1;
        }
        else
        {
            inStart = false;
            time += Time.deltaTime;
            movementSpeed = speedInicial + (incremento * time / 60);

            if (movementSpeed < 12)
                BackgSound.pitch = 1.065f;
            else if (movementSpeed < 18)
                BackgSound.pitch = 1.13f;
            else if (movementSpeed < 22)
                BackgSound.pitch = 1.2f;
            else
                BackgSound.pitch = 1.25f;
        }

        if (movementSpeed > maxSpeed)
            movementSpeed = maxSpeed;
    }
    
    public void teclaErrada(float time, int valor1, int valor2)
    {
        tempoInativo = time;
        score.DoScore(Random.Range(valor1, valor2));

        WrongSound.volume = Random.Range(0.05f, 0.12f);
        WrongSound.pitch = Random.Range(1.5f, 1.6f);
        WrongSound.Play();
    }

    private void controlLight()
    {
        //LUZ
        if(luz != null)
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

    public float GetSpeed() => movementSpeed;

    private int[] SeedRandom(int seedValor)
    {
        Random.seed = seedValor;
        int[] valores = new int[100];
        int i = 0;
        while (i < valores.Length)
        {
            if (i <= 8)
                valores[i] = i;
            else
                valores[i] = (int)(Random.value * 10);

            i++;
        }

        return valores;
    }

    private void setIndexOnTutorial()
    {
        if (tutorial)
            tutorialScene.indexNumTutorial -= 1;
    }
}

