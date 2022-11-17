using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject XR;
    private float rotY = 180;
    public float rotOffset = 0.05f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        XR.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        rotY += rotOffset;
    }

    public void Play()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
