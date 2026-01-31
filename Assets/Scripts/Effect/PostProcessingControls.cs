using System;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering.Universal;


public class PostProcessingControls : MonoBehaviour
{
    private static PostProcessingControls instance;
    public static PostProcessingControls Instance => instance;
    
    public FullScreenPassRendererFeature rendererFeatureWobble;
    public FullScreenPassRendererFeature rendererFeaturePumpkinMask;
    
    // public Material materialPumkinMask,materialWobble;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetWobbleLevel(0);
        SetPumpkinMaskLevel(0);
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetWobbleLevel(int level)
    {
        if (level >= 5)
            level = 5;    
        SetWobbleEffect(3.1f,level*0.005f);
    }

    void SetWobbleEffect(float speed, float intensity)
    {
        rendererFeatureWobble.SetActive(intensity != 0);
        rendererFeatureWobble.passMaterial.SetFloat("_Speed", speed);
        rendererFeatureWobble.passMaterial.SetFloat("_Intensity", intensity);
    }

    public void SetPumpkinMaskLevel(int level)
    {
        switch (level)
        {
            case 0:
                SetPumpkinMaskEffect(0f);
                break;
            case 1:
                SetPumpkinMaskEffect(.9f);
                break;
            case 2:
                SetPumpkinMaskEffect(.99f);
                break;
            case 3:
                SetPumpkinMaskEffect(.999f);
                break;
            default:
                SetPumpkinMaskEffect(1f);
                break;
        }
    }
    void SetPumpkinMaskEffect(float intensity)
    {
        rendererFeaturePumpkinMask.SetActive(intensity!=0);
        rendererFeaturePumpkinMask.passMaterial.SetFloat("_Intensity", intensity);
    }




}