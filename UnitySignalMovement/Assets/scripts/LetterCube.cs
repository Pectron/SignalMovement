using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterCube : MonoBehaviour
{

    private enum Letra
    {
        R,
        L,
    }

    private Letra letra;

    public  int seedNumber;
    private int[] valoresRandom;
    private int indexValor = 0;

    Sequencia sequencia;

    [SerializeField]
    private Material MaterialRed;

    // Start is called before the first frame update
    void Start()
    {
        Random.seed = seedNumber;
        valoresRandom = new int[100];
        int i = 0;
        while (i < valoresRandom.Length)
        {
            valoresRandom[i] = (int)(Random.value * 10);
            i++;
        }

        sequencia = GameObject.Find("Canvas_Sequencia").GetComponent<Sequencia>();
        indexValor = System.Convert.ToInt32(sequencia.GetSpeed());

        GetRandomLetra();
        transform.GetChild(4).GetComponent<TextMesh>().text = letra.ToString();
    }

    void GetRandomLetra()
    {
        int num = valoresRandom[indexValor];

        if (num == 0 || num == 4 || num == 8 || num == 1 || num == 5)
        {
            letra = Letra.R;
        }
        else if (num == 2 || num == 6 || num == 3 || num == 7 || num == 9)
        {
            letra = Letra.L;

            //mudar cor bloco
            transform.GetChild(0).GetComponent<MeshRenderer>().material = MaterialRed;
            transform.GetChild(1).GetComponent<MeshRenderer>().material = MaterialRed;
            transform.GetChild(2).GetComponent<MeshRenderer>().material = MaterialRed;
            transform.GetChild(3).GetComponent<MeshRenderer>().material = MaterialRed;
        }

        indexValor++;
        if (indexValor > valoresRandom.Length - 1)
            indexValor = 0;
    }

    public bool checkRightLetter(string letter)
    {
        if (letter == letra.ToString())
            return true;

        return false;
    }
}
