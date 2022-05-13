using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMode : MonoBehaviour
{
    private WebExporter webExporter;
    // Start is called before the first frame update
    void Start()
    {
        webExporter = (WebExporter) FindObjectOfType(typeof(WebExporter));
    }

    public void SetTestMode()
    {
        webExporter.SetOnTestMode();
    }
}
