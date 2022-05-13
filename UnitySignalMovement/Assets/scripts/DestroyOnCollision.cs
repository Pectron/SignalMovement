using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    [SerializeField]
    private string tagName;

    [SerializeField]
    private int ponto1 = 5, ponto2 = 11;

    [SerializeField]
    private bool destruirTudo = true;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == tagName)
        {
            if(gameObject.name == "Alvo")
            {
                GameObject.Find("RightHand Controller").GetComponent<Gun>().Hit();
                GameObject.Find("Pontos").GetComponent<controlScore>().DoScore(Random.Range(ponto1, ponto2));

                if(GameObject.Find("TutorialManage")?.GetComponent<TutorialScene>())
                    GameObject.Find("TutorialManage").GetComponent<TutorialScene>().indexNumTutorial -= 1;
            }
                
            if(destruirTudo)
                Destroy(collision.gameObject);

            Destroy(this.gameObject);
        }
    }
}
