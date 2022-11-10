using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyFloatEvent : UnityEvent<float>
{
}

[RequireComponent(typeof(AudioSource))]

public class AudioSync : MonoBehaviour
{
    [SerializeField] float energy = 0f;
    AudioSource currentMusic;
    float[] audioBuffer;
    [SerializeField] int nbSamples = 512;
    [SerializeField] GameObject energyVisualizer = null;
    [SerializeField] float intensityMultiplier = 10f;
    [SerializeField] float energyThreshold = 0.3f;
    [SerializeField] UnityEvent energyAboveThEvents;
    [SerializeField] UnityEvent energyBelowThEvents;

    [SerializeField] MyFloatEvent energyFloat;


    // Start is called before the first frame update
    void Start()
    {
        currentMusic = GetComponent<AudioSource>();
        audioBuffer = new float[nbSamples];
    }

    // Update is called once per frame
    void Update()
    {
        ComputeEnergy();
       // VisualizeEnergy();
        ResolveEnergyEvents();
    }

    private void ResolveEnergyEvents()
    {
        if (energy > energyThreshold)
        {
            energyAboveThEvents.Invoke();
        }
        else
        {
            energyBelowThEvents.Invoke();
        }

        energyFloat.Invoke(energy);
    }

    private void VisualizeEnergy()
    {
        energyVisualizer.transform.localScale = new Vector3(energy * intensityMultiplier, energy * intensityMultiplier, energy * intensityMultiplier);
        
    }

    private void ComputeEnergy()
    {
        currentMusic.GetOutputData(audioBuffer, 0);
        energy = 0f;
        for (int i = 0; i < audioBuffer.Length; i++)
        {
            energy += audioBuffer[i] * audioBuffer[i];
        }
        energy /= audioBuffer.Length;
        energy = Mathf.Sqrt(energy);
    }
}
