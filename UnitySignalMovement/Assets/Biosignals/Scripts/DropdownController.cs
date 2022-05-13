using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropdownController : MonoBehaviour
{
    public GameObject dropDownValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHide()
    {
        if(dropDownValue.activeInHierarchy)
        {
            dropDownValue.SetActive(false);
        }
        else
        {
            dropDownValue.SetActive(true);
        }
    }
}
