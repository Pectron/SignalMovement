using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialControl : MonoBehaviour
{
    [SerializeField] Sequencia seq;

    [SerializeField] controlTime contTime;

    [SerializeField] GameObject tut1, tut2, tut3;
    Transform tutorialObject;

    [SerializeField] AudioClip[] dialog;
    AudioSource aSWorker;
    private bool talking;

    // Start is called before the first frame update
    void Awake()
    {
        aSWorker = GetComponent<AudioSource>();
        contTime.StopCount(true);
        spawnTutorial();
        StartCoroutine(TutorialPhase());
    }

    IEnumerator TutorialPhase()
    {
        talking = true;
        if(aSWorker.clip != null)
        {
            aSWorker.Play();
            yield return new WaitForSeconds(aSWorker.clip.length);
        }
        talking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialObject != null)
        {
            if (tutorialObject.childCount == 0)
            {
                Destroy(tutorialObject.parent.gameObject);
                if (talking == true)
                    spawnTutorial();
                else
                    acabarTutorial();
            }
        }
    }

    void spawnTutorial()
    {
        if (tut1 != null)
        {
            aSWorker.clip = dialog[0];
            tutorialObject = Instantiate(tut1).transform.GetChild(0);

            seq.ListaAlvosCubos.Add(tutorialObject.GetChild(0).gameObject);
            seq.ListaAlvosCubos.Add(tutorialObject.GetChild(1).gameObject);
        }
        else if (tut2 != null)
        {
            aSWorker.clip = dialog[1];
            tutorialObject = Instantiate(tut2).transform.GetChild(0);
        }
        else if (tut3 != null)
        {
            aSWorker.clip = dialog[2];
            tutorialObject = Instantiate(tut3, transform.GetChild(0)).transform.GetChild(0);
        }
    }

    void acabarTutorial()
    {
        contTime.StopCount(false);
        Destroy(this.gameObject);
    }
}
