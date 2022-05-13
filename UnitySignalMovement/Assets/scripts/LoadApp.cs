using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadApp : MonoBehaviour
{
    public void StartApp() 
    {
        SceneManager.LoadScene(2);
    }
}
