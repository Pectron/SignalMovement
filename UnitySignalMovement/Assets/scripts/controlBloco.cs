using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlBloco : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float minPosZ;
    private Rigidbody rb;

    private controlTime time;
    private bool passouParede;

    private PhysiologySignalsManager _bioGadget;


    private TutorialScene tutorialScene;



    private void Awake()
    {
        var bio = GameObject.Find("Biosignals");
        if (bio != null)
        {
            _bioGadget = bio.GetComponent<PhysiologySignalsManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb   = GetComponent<Rigidbody>();
        time = GameObject.Find("ControlTempo").GetComponent<controlTime>();
        tutorialScene = GameObject.Find("TutorialManage")?.GetComponent<TutorialScene>();

        minPosZ = -20f;
    }

    // Update is called once per frame
    void Update()
    {
        //MOVIMENTO
        movementSpeed = (1/time.getTempo() + 2) * 1.2f + 3;
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -movementSpeed);


        if (transform.position.z < minPosZ)
            Destroy(this.gameObject);

        GameObject player = GameObject.Find("Main Camera");
        if(transform.position.z < player.transform.position.z - 2 && passouParede == false)
        {
            passouParede = true;

            //passou mandar sinal
            if (_bioGadget != null)
                _bioGadget.NewMarker("AC_DODGEREDWALL");
            //

            Debug.Log("desvouuu");
            if (tutorialScene != null)
                tutorialScene.indexNumTutorial -= 1;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "controller")
            colidiu(-5, -10);

        if(col.gameObject.name == "Main Camera")
            colidiu(-10,-20);
    }

    void colidiu(int ponto1, int ponto2)
    {
        //colidiu mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("AC:COLLIDREDWALL");

        if (tutorialScene != null)
            tutorialScene.ResetIndexValorTut(1);

        GameObject.Find("Canvas_Sequencia").GetComponent<Sequencia>().teclaErrada(0.3f, ponto1, ponto2);
        Destroy(this.gameObject);
    }
}
