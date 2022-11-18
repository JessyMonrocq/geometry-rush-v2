using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCredits : MonoBehaviour
{
    public GameObject XR;
    private float rotY = 180;
    public float rotOffset = 0.0001f;
    public float offsetY = 0;

    public Image cred;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        XR.transform.localRotation = Quaternion.Euler(0, rotY, 0);
        rotY += rotOffset;
        cred.transform.position = new Vector3(cred.transform.position.x, cred.transform.position.y + offsetY, cred.transform.position.z);
        offsetY = offsetY + 0.000005f;
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
