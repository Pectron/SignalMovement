using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanParedes : MonoBehaviour
{
    [SerializeField]
    private GameObject[] paredes;

    [SerializeField]
    private float maxTempo, minTempo, decadencia;
    private float tempo;

    [SerializeField]
    private int   seedNumber;
    private int[] valoresRandom;
    private int   indexValor;

    // Start is called before the first frame update
    void Start()
    {
        Random.seed = seedNumber;
        valoresRandom = new int[100];
        int i = 0;
        while (i < valoresRandom.Length)
        {
            if (i <= 9)
                valoresRandom[i] = i;
            else
                valoresRandom[i] = (int)(Random.value * 10);

            i++;
        }

        tempo = maxTempo/10;
    }

    // Update is called once per frame
    void Update()
    {
        tempo -= Time.deltaTime;

        if(tempo <= 0)
        {
            Spawn();
            if(maxTempo > minTempo)
                maxTempo -= decadencia;

            tempo = maxTempo;
        }
    }


    void Spawn()
    {
        Instantiate(paredes[valoresRandom[indexValor]], new Vector3(paredes[valoresRandom[indexValor]].transform.position.x, paredes[valoresRandom[indexValor]].transform.position.y, transform.position.z), paredes[valoresRandom[indexValor]].transform.rotation);

        indexValor++;
        if (indexValor > valoresRandom.Length - 1)
            indexValor = 0;
    }
}
