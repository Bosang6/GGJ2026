using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerHand : MonoBehaviour
{
    public TotemBlock holdedBlock { get; private set; }
    private Transform originalParent;

    public float baseSpeed = 0.75f;
    public float speed = 0f;

    public float releaseDelay = .5f;
    private float release_cd = 0;

    public Transform hand;
    public Transform leftPoint;
    public Transform rightPoint;
    public AnimationCurve moveCurve;
    public Animator lowerAnim;
    private float moveTime = .5f;

    public UnityAction<TotemBlock> OnRelease;

    private InputAction releaseAction;

    void Start()
    {
        releaseAction = InputSystem.actions.FindAction("Release");
        GameManager.Instance.OnGameOver.AddListener(OnGameover);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver) return;

        if (release_cd > 0)
        {
            release_cd -= Time.deltaTime;
        }
        else if (releaseAction.WasPressedThisFrame() && holdedBlock)
        {
            release_cd = releaseDelay;
            ReleaseBlock();
        }
    }

    void FixedUpdate()
    {
        if (holdedBlock)
        {
            MoveHand();
        }
    }

    private void MoveHand()
    {
        moveTime += Time.fixedDeltaTime * speed;

        var y = moveCurve.Evaluate(moveTime);
        var pos = Vector3.Lerp(leftPoint.localPosition, rightPoint.localPosition, y);
        hand.transform.localPosition = pos;
    }

    public void ReleaseBlock()
    {
        if (!holdedBlock) return;

        if (GameManager.Instance.cheatsEnabled) {
            var top = GameManager.Instance.Totem.totemBlocks.LastOrDefault() ?? GameManager.Instance.Totem.baseBlock;
            var pos = holdedBlock.transform.position;
            pos.x = top.transform.position.x;
            holdedBlock.transform.position = pos;
        }

        holdedBlock.EnablePhysics();
        holdedBlock.transform.SetParent(originalParent);
        originalParent = null;

        holdedBlock.EndEffect();
        holdedBlock.PlayFall();

        holdedBlock = null;
        speed = baseSpeed;

        OnRelease?.Invoke(holdedBlock);

        lowerAnim.SetBool("HasBlock", false);
    }

    public void GrabBlock(TotemBlock block)
    {
        if (holdedBlock) return;

        block.DisablePhysics();
        originalParent = block.transform.parent;
        block.transform.SetParent(hand);
        block.transform.localPosition = Vector3.zero;

        block.StartEffect();
        block.PlaySelected();

        holdedBlock = block;
        speed = baseSpeed * block.handSpeed;

        lowerAnim.SetBool("HasBlock", true);
    }

    void OnGameover()
    {
        lowerAnim.SetBool("IsGameover", true);
        if (holdedBlock)
        {
            holdedBlock.EndEffect();
            //Destroy(holdedBlock.gameObject);
            //holdedBlock = null;
        }
    }
}
