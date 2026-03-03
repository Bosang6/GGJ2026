using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public int dynamicBlockCount = 6;
    public int dynamicWithDampingBlockCount = 2;

    public float adjustmentFactor = 0.22f;
    public float minAdjustment = 0.18f;
    public float damping = 0.1f;

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
        baseBlock.totem = this;
    }

    public void AddBlock(TotemBlock block)
    {
        var top = totemBlocks.LastOrDefault();

        block.transform.SetParent(transform);
        block.totem = this;
        block.indexInTotem = totemBlocks.Count;
        totemBlocks.Add(block);

        var newStatic = GetStaticBlocks().LastOrDefault();
        if (newStatic)
        {
            var slow = newStatic.AddComponent<SlowBlock>();
            slow.damping = 2f * damping;
            var rb = newStatic.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.angularVelocity /= 4f;
            var angle = top.transform.eulerAngles.z;
            angle = (angle > 180f) ? angle - 360f : angle;
            if (angle > 0f)
            {
                rb.angularVelocity -= Mathf.Max(minAdjustment, adjustmentFactor * angle);
            }
            else if (angle < 0f)
            {
                rb.angularVelocity += Mathf.Max(minAdjustment, adjustmentFactor * -angle);
            }
        }

        var newDamping = GetDynamicWithDampingBlocks().LastOrDefault();
        if (newDamping)
        {
            var rb = newDamping.GetComponent<Rigidbody2D>();
            rb.angularDamping += damping;
            rb.linearDamping += damping;
        }

    }

    public void RemoveBlock(TotemBlock block)
    {
        if (block.indexInTotem == -1) return;

        totemBlocks[block.indexInTotem] = null;
        block.totem = null;
        block.indexInTotem = -1;

        while (totemBlocks.Count > 0 && totemBlocks.Last() == null)
        {
            totemBlocks.RemoveAt(totemBlocks.Count - 1);

            var newDynamic = GetDynamicBlocks().FirstOrDefault();
            if (newDynamic)
            {
                var rb = newDynamic.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.angularDamping -= damping;
                rb.linearDamping -= damping;
            }

            var newDamping = GetDynamicWithDampingBlocks().FirstOrDefault();
            if (newDamping)
            {
                var slow = newDamping.GetComponent<SlowBlock>();
                Destroy(slow);
                var rb = newDamping.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    void FixedUpdate()
    {
        Crick();
    }

    private int GetDynamicBlocksCount() => Mathf.Min(totemBlocks.Count, dynamicBlockCount);
    private int GetDynamicWithDampingBlocksCount() => Mathf.Max(0, GetDynamicBlocksCount() - (dynamicBlockCount - dynamicWithDampingBlockCount));
    private int GetStaticBlocksCount() => Mathf.Max(0, totemBlocks.Count - dynamicBlockCount);
    public IEnumerable<TotemBlock> GetDynamicBlocks() => totemBlocks.Skip(GetStaticBlocksCount());
    public IEnumerable<TotemBlock> GetDynamicWithDampingBlocks() => GetDynamicBlocks().Take(GetDynamicWithDampingBlocksCount());
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
