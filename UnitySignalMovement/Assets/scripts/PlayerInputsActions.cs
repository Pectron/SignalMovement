using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerInputsActions : MonoBehaviour
{
    [SerializeField]
    private Sequencia sequencia, seq2, seq3, seq4;

    [SerializeField]
    private ActionBasedController controllerRight, controllerLeft;

    [SerializeField]
    public Gun gunRight, gunLeft;

    private PhysiologySignalsManager _bioGadget;

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
        bool isPressed = controllerRight.activateAction.action.ReadValue<bool>();
        controllerRight.activateAction.action.performed += ActionRight_performed;

        bool isPressed2 = controllerLeft.activateAction.action.ReadValue<bool>();
        controllerLeft.activateAction.action.performed += ActionLeft_performed;

        sequencia = GameObject.Find("Canvas_Sequencia").GetComponent<Sequencia>();

    }


    private void ActionRight_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gunRight.Fire();
    }

    private void ActionLeft_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        gunLeft.Fire();
    }

    public void OnR_Button()
    {
        Debug.Log("clicou TrackPad Right");
        sequencia.verifyLetterCorrectCube("R");
        if (seq2?.gameObject.active != false)
            seq2?.verifyLetterCorrectCube("R");
        if (seq3?.gameObject.active != false)
            seq3?.verifyLetterCorrectCube("R");
        if (seq4?.gameObject.active != false)
            seq4?.verifyLetterCorrectCube("R");

        //clicou mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("AC_LETTERBUTTONRIGHT");
    }

    public void OnL_Button()
    {
        Debug.Log("clicou TrackPad Left");
        sequencia.verifyLetterCorrectCube("L");
        if (seq2?.gameObject.active != false)
            seq2?.verifyLetterCorrectCube("L");
        if (seq3?.gameObject.active != false)
            seq3?.verifyLetterCorrectCube("L");
        if (seq4?.gameObject.active != false)
            seq4?.verifyLetterCorrectCube("L");

        //clicou mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("AC_LETTERBUTTONLEFT");
    }

    public void OnX_Button()
    {
        Debug.Log("clicou TrackPad Left");
        sequencia.verifyLetterCorrectCube("L");
        if(seq2?.gameObject.active != false)
            seq2?.verifyLetterCorrectCube("L");
        if (seq3?.gameObject.active != false)
            seq3?.verifyLetterCorrectCube("L");
        if (seq4?.gameObject.active != false)
            seq4?.verifyLetterCorrectCube("L");

        //clicou mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("AC_LETTERBUTTONLEFT");
    }

    public void OnA_Button()
    {
        Debug.Log("clicou TrackPad Right");
        sequencia.verifyLetterCorrectCube("R");
        if (seq2?.gameObject.active != false)
            seq2?.verifyLetterCorrectCube("R");
        if (seq3?.gameObject.active != false)
            seq3?.verifyLetterCorrectCube("R");
        if (seq4?.gameObject.active != false)
            seq4?.verifyLetterCorrectCube("R");

        //clicou mandar sinal
        if (_bioGadget != null)
            _bioGadget.NewMarker("AC_LETTERBUTTONRIGHT");
    }

}
