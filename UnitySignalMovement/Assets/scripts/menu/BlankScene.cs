using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlankScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadQuest() 
    {
        SceneManager.LoadScene(3);
    }
}
