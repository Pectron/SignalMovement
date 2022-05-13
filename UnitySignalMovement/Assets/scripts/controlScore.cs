using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class controlScore : MonoBehaviour
{
    private TextMeshProUGUI texto;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        texto = GetComponent<TextMeshProUGUI>();
    }


    public void DoScore(int add)
    {
        score += add;
        correctFormatScore();
    }

    private void correctFormatScore()
    {
        if(score < 0)
        {
            score = 0;
        }

        if(score < 10)
        {
            texto.text = "00000" + score.ToString();
        }
        else if (score < 100)
        {
            texto.text = "0000" + score.ToString();
        }
        else if (score < 1000)
        {
            texto.text = "000" + score.ToString();
        }
        else if (score < 10000)
        {
            texto.text = "00" + score.ToString();
        }
        else if (score < 100000)
        {
            texto.text = "0" + score.ToString();
        }
        else if (score < 1000000)
        {
            texto.text = "" + score.ToString();
        }
    }

    public void WrongColor()
    {
        texto.color = Color.red;
    }

    public void normalColor()
    {
        texto.color = new Color(1f, 1f, 1f);
    }
}
