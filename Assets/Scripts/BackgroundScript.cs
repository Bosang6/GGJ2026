using System;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{

    private Transform cameraTransform;
    private SpriteRenderer sr;
    
    public float colorChangeSpeed = 0f;
    public float colorOffset = 0f;

    public bool isFront;


    public List<Sprite> backgrounds = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraTransform.position.y >= transform.position.y)
            transform.Translate(10*Vector3.up);
        
        float h = colorOffset + colorChangeSpeed * cameraTransform.position.y;

        Sprite sprite;
        Debug.Log(backgrounds.Count + ":" + (h+1)%backgrounds.Count);
    
        if (isFront)
            sprite = backgrounds[Mathf.FloorToInt((h+1)%backgrounds.Count)];
        else 
            sprite = backgrounds[Mathf.FloorToInt(h%backgrounds.Count)];
        sr.sprite = sprite;
        Color c = Color.white;
        if (isFront)
            c.a = h%1f;
        sr.color = c;
    }
}