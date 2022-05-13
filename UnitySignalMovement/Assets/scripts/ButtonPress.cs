using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPress : MonoBehaviour
{

    private Animator animPress;

    private PhysiologySignalsManager _bioGadget;

    bool tutorial;

    [SerializeField] private UnityEvent pressBut;

    [SerializeField] private bool apenasUmClick;
    private int clicou;

    // Start is called before the first frame update
    void Start()
    {
        var bio = GameObject.Find("Biosignals");
        if (bio != null)
        {
            _bioGadget = bio.GetComponent<PhysiologySignalsManager>();
        }

        animPress = gameObject.GetComponent<Animator>();
        clicou = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.name == "RightHand Controller" || other.gameObject.name == "LeftHand Controller") && clicou == 0)
        {
            animPress.SetTrigger("press");

            OnpressBut();

            if (_bioGadget != null)
            {
                //clicou mandar sinal
                _bioGadget.NewMarker("Clicou Butao Parede");
            }
            
            if(apenasUmClick)
                clicou++;
        }
    }

    public virtual void OnpressBut()
    {
        if (pressBut != null) pressBut.Invoke();
    }

    public void Appear()
    {
        clicou = 0;
    }
}
