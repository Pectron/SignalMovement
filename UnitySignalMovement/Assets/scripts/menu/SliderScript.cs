using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderScript : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI sliderText;

    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((v) =>
        {
            sliderText.text = v.ToString();
        });

        sliderText.text = "2";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
