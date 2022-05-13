using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag;
    [SerializeField] private string markerInfo;
    [SerializeField] private bool disableAfter;
    private PhysiologySignalsManager phyManager;
    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindObjectOfType<PhysiologySignalsManager>())
        {
            phyManager = GameObject.FindObjectOfType<PhysiologySignalsManager>();
        }
        else
        {
            Debug.Log("ERROR: PhysiologySignalsManager not found!, disabling Markers");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            if(phyManager)
            {
                phyManager.NewMarker(markerInfo);
                if(disableAfter)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
