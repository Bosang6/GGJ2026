using UnityEngine;

public class BlockScript : MonoBehaviour
{

    public MaskInfo maskInfo;

    public GameObject wings;

    public AudioClip fall;
    public AudioClip impact;
    public AudioClip wingsSound;
    public AudioClip selectedSound;

    private bool hasWings = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartEffect();
    }

    void StartEffect()
    {
        Grab.Instance.moveSpeed = 1;
        switch (maskInfo.type)
        {
            case E_EffectType.Wobble:
                PostProcessingControls.Instance.TransitionWobbleEffect(3);
                break;
            case E_EffectType.PumpkinMask:
                PostProcessingControls.Instance.TransitionPumpkinMaskEffect(3);
                break;
            case E_EffectType.Venice:
                Grab.Instance.moveSpeed = 2;
                break;
        }
    }

    void EndEffect()
    {
        switch (maskInfo.type)
        {
            case E_EffectType.Wobble:
                PostProcessingControls.Instance.TransitionWobbleEffect(0);
                break;
            case E_EffectType.PumpkinMask:
                PostProcessingControls.Instance.TransitionPumpkinMaskEffect(0);
                break;
            case E_EffectType.Wresler:
                Shaking.Instance.start = true;
                break;

        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(enabled){
            int score = ScoreController.Instance.GetScore();

            if ((score + maskInfo.score) / 10 > score / 10)
            {
                Instantiate(wings, transform);
                hasWings = true;
            }
            
            PlayImpact();

            ScoreController.Instance.AddToScore(maskInfo.score);
            EndEffect();
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFall()
    {
        GetComponent<AudioSource>().clip = fall;
        GetComponent<AudioSource>().Play();
    }
    
    
    public void PlayImpact()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSources[0].clip = impact;
        if (hasWings)
        {
            audioSources[1].clip = wingsSound;
            audioSources[1].Play();
        }
        audioSources[0].Play();
    }
    
    public void PlaySelected()
    {
        if (!selectedSound) return;
        
        GetComponent<AudioSource>().clip = selectedSound;
        GetComponent<AudioSource>().Play();
    }
}