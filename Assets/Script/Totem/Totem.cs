using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public int dynamicBlockCount = 4;

    public int crickFramesThreshold = 20;
    private int crickFramesCount = 0;
    
    public float crickAngleThreshold = 0.021f;
    private Vector3? lastAngleVector = null;

    public List<TotemBlock> totemBlocks = new();
    public TotemBlock baseBlock;

    private AudioSource fallingTowerAudio;

    void Awake()
    {
        fallingTowerAudio = GetComponent<AudioSource>();
    }

    void Start()
    {
        AddBlock(baseBlock);
    }

    void FixedUpdate()
    {
        Crick();
    }

    public void AddBlock(TotemBlock block)
    {
        block.totem = this;
        block.indexInTotem = totemBlocks.Count;
        totemBlocks.Add(block);

        var newStatic = GetStaticBlocks().LastOrDefault();
        if (newStatic)
        {
            var rb = newStatic.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocityX /= 2f;
            rb.linearVelocityY = 0f;
            rb.angularVelocity /= 2f;
        }
    }

    private int GetStaticBlocksCount() => Mathf.Max(0, totemBlocks.Count - dynamicBlockCount);
    public IEnumerable<TotemBlock> GetDynamicBlocks() => totemBlocks.Skip(GetStaticBlocksCount());
    public IEnumerable<TotemBlock> GetStaticBlocks() => totemBlocks.Take(GetStaticBlocksCount());
    public IEnumerable<TotemBlock> GetTop(int n) => totemBlocks.Skip(Mathf.Max(0, totemBlocks.Count - n));


    private void Crick()
    {
        var topBlock = GetDynamicBlocks().LastOrDefault();
        var bottomBlock = GetDynamicBlocks().FirstOrDefault();
        if (!bottomBlock || !topBlock) return;

        var angleVector = topBlock.transform.position - bottomBlock.transform.position;
        
        if (lastAngleVector != null)
        {
            float angleDiff = Mathf.Abs(Vector3.Angle(angleVector, lastAngleVector.Value));
            if (angleDiff >= crickAngleThreshold)
            {
                crickFramesCount += 1;
                if (crickFramesCount >= crickFramesThreshold)
                {
                    fallingTowerAudio.Play();
                    crickFramesCount = 0;
                }
            }
            else
            {
                crickFramesCount = 0;
            }
        }
        lastAngleVector = angleVector;
    }
}
