using System;
// using UnityEditor.Rendering.Universal;
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
        SetPumpkinMaskEffect(0);
        SetWobbleEffect(0);
    }

    private void Awake()
    {
        instance = this;
    }

    private float currentWobble = 0, intendedWobble = 0, transitionWobbleSpeed = 0;

    private float currentPumpkinMask = 0, intendedPumpkinMask = 0, transitionPumpkinMaskSpeed = 0;

    // Update is called once per frame
    void Update()
    {
        if (transitionWobbleSpeed != 0)
        {
            int sign = Math.Sign(intendedWobble-currentWobble);
            currentWobble += sign * transitionWobbleSpeed * Time.deltaTime;
            if (Math.Sign(intendedWobble - currentWobble) != sign)
            {
                currentWobble = intendedWobble;
                transitionWobbleSpeed = 0;
            }
            SetWobbleEffect(currentWobble);
        }
        if (transitionPumpkinMaskSpeed != 0)
        {
            int sign = Math.Sign(intendedPumpkinMask-currentPumpkinMask);
            currentPumpkinMask += sign * transitionPumpkinMaskSpeed * Time.deltaTime;
            if (Math.Sign(intendedPumpkinMask -currentPumpkinMask) != sign)
            {
                currentPumpkinMask = intendedPumpkinMask;
                transitionPumpkinMaskSpeed = 0;
            }
            SetPumpkinMaskEffect(currentPumpkinMask);
        }
    }

    public void TransitionWobbleEffect(float intensity, float transitionTime = 1f)
    {
        intendedWobble = intensity;
        transitionWobbleSpeed = Math.Abs(intendedWobble-currentWobble) / transitionTime;
    }


    private void SetWobbleEffect(float intensity)
    {
        rendererFeatureWobble.SetActive(intensity != 0);
        rendererFeatureWobble.passMaterial.SetFloat("_Speed", 3.1f);
        rendererFeatureWobble.passMaterial.SetFloat("_Intensity", intensity * 0.005f);
    }

    public void TransitionPumpkinMaskEffect(float intensity, float transitionTime = 1f)
    {
        intendedPumpkinMask = intensity;
        transitionPumpkinMaskSpeed = Math.Abs(intendedPumpkinMask-currentPumpkinMask) / transitionTime;
    }

    void SetPumpkinMaskEffect(float intensity)
    {
        rendererFeaturePumpkinMask.SetActive(intensity!=0);
        rendererFeaturePumpkinMask.passMaterial.SetFloat("_Intensity", 1f-(float) Math.Pow(.1f,intensity));
    }




}