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

    public Transform leftPoint;
    public Transform rightPoint;
    public AnimationCurve moveCurve;
    private float moveTime = .5f;

    public UnityAction<TotemBlock> OnRelease;

    private InputAction releaseAction;

    void Start()
    {
        releaseAction = InputSystem.actions.FindAction("Release");
    }

    private void Update()
    {
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
        if (holdedBlock || true)
        {
            MoveHand();
        }
    }

    private void MoveHand()
    {
        moveTime += Time.deltaTime * speed;

        var y = moveCurve.Evaluate(moveTime);
        var pos = Vector3.Lerp(leftPoint.position, rightPoint.position, y);
        transform.position = pos;
    }

    public void ReleaseBlock()
    {
        if (!holdedBlock) return;
        OnRelease?.Invoke(holdedBlock);

        holdedBlock.EnablePhysics();
        holdedBlock.transform.SetParent(originalParent);
        originalParent = null;

        holdedBlock.EndEffect();
        holdedBlock.PlayFall();

        holdedBlock = null;
        speed = baseSpeed;
    }

    public void GrabBlock(TotemBlock block)
    {
        if (holdedBlock) return;

        block.DisablePhysics();
        originalParent = block.transform.parent;
        block.transform.SetParent(transform);

        block.StartEffect();
        block.PlaySelected();

        holdedBlock = block;
        speed = baseSpeed * block.handSpeed;
    }
}
