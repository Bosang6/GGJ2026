using System.Linq;
using UnityEngine;

public class TotemBlock : MonoBehaviour
{
    public enum E_BlockType
    {
        Normal,
        Scary,
        Wresler,
        Pumpkin,
    }

    public GameObject wings_pref;

    public int score = 1;
    public float handSpeed = 1;
    public E_BlockType blockType = E_BlockType.Normal;

    public AudioClip fall;
    public AudioClip impact;
    public AudioClip wingsSound;
    public AudioClip selectedSound;

    public Totem totem;
    public int indexInTotem = -1;
    private bool hasWings = false;

    public void StartEffect()
    {
        switch (blockType)
        {
            case E_BlockType.Scary:
                PostProcessingControls.Instance.TransitionWobbleEffect(3);
                break;
            case E_BlockType.Pumpkin:
                PostProcessingControls.Instance.TransitionPumpkinMaskEffect(3);
                break;
        }
    }

    public void EndEffect()
    {
        switch (blockType)
        {
            case E_BlockType.Scary:
                PostProcessingControls.Instance.TransitionWobbleEffect(0);
                break;
            case E_BlockType.Pumpkin:
                PostProcessingControls.Instance.TransitionPumpkinMaskEffect(0);
                break;
        }
    }

    public void OnTotemHit()
    {
        switch (blockType)
        {
            case E_BlockType.Wresler:
                Shaking.Instance.start = true;
                break;
        }
    }

    public void EnablePhysics()
    {
        GetComponent<Rigidbody2D>().simulated = true;
    }

    public void DisablePhysics()
    {
        GetComponent<Rigidbody2D>().simulated = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (totem) return;

        var block = collision.GetComponent<TotemBlock>();
        if (block && block.totem){
            block.totem.AddBlock(this);

            int gameScore = GameManager.Instance.Score;

            if ((gameScore + score) / 10 > gameScore / 10)
            {
                Instantiate(wings_pref, transform);
                hasWings = true;
            }
            
            GameManager.Instance.Score += score;
            
            PlayImpact();
            OnTotemHit();
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

    private void OnDestroy()
    {
        if (totem)
        {
            totem.totemBlocks[indexInTotem] = null;
        }
    }
}