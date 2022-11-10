using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScale : MonoBehaviour
{
    public float intensity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScale(float f)
    {
        this.transform.localScale = new Vector3(f * intensity, f * intensity, f * intensity);
    }
}
