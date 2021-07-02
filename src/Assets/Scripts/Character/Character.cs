﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Character : MonoBehaviour
{
    /// <summary>
    /// 玩家角色动作管理器名
    /// </summary>
    internal const string AniName_CharState = "PlayerCharState";

    float speed = 20.0f;   //移动速度

    float jumpForce = 20000.0f;   //跳跃力

    Animator CharAnimator => gameObject.GetComponent<Animator>();

    Transform CharTransform => gameObject.GetComponent<Transform>();

    private void Awake()
    {
        Initialization();
        AddEventHandler();
    }

    private void Initialization()
    {
        AniState = PlayerCharAnimationState.Stand;

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    bool isJumping = false;

    private void Update()
    {
        var rg = this.GetComponent<Rigidbody2D>();
        Vector2 vec = rg.velocity;
        var vecX = Input.GetAxis("Horizontal");
        var vecY = Input.GetAxis("Vertical");
        var sc = CharTransform.localScale;
        float scaleX = 0;
        float scaleY = sc.y;
        var speedY = rg.velocity.y;
        if (Input.GetKeyDown(KeyCode.C) && isJumping == false)
        {
            rg.velocity = new Vector2(rg.velocity.x, jumpForce * Time.deltaTime);
            CharAnimator.SetInteger(AniName_CharState, 2);
            AniState = PlayerCharAnimationState.Jump;
            isJumping = true;
            StartCoroutine(WaitForJumpEnd());
        }
        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.RightArrow) && isJumping == false)
        {
            rg.velocity = new Vector2(rg.velocity.x, jumpForce * Time.deltaTime);
            CharAnimator.SetInteger(AniName_CharState, 2);
            AniState = PlayerCharAnimationState.Jump;
            isJumping = true;
            StartCoroutine(WaitForJumpEnd());
        }
        if (Input.GetKeyDown(KeyCode.C) && Input.GetKey(KeyCode.LeftArrow) && isJumping == false)
        {
            rg.velocity = new Vector2(rg.velocity.x, jumpForce * Time.deltaTime);
            CharAnimator.SetInteger(AniName_CharState, 2);
            AniState = PlayerCharAnimationState.Jump;
            isJumping = true;
            StartCoroutine(WaitForJumpEnd());
        }
    }

    IEnumerator WaitForJumpEnd()
    {
        var rg = this.GetComponent<Rigidbody2D>();
        yield return new WaitUntil(() => Math.Abs(rg.velocity.y) < 0.05f);
        isJumping = false;
        AniState = PlayerCharAnimationState.Stand;
        CharAnimator.SetInteger(AniName_CharState, 0);
        Debug.Log("解除跳跃");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var rg = this.GetComponent<Rigidbody2D>();
        var vecX = Input.GetAxis("Horizontal");
        var vecY = Input.GetAxis("Vertical");
        var sc = CharTransform.localScale;
        float scaleX = 0;
        float scaleY = sc.y;
        var speedY = rg.velocity.y;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rg.velocity = new Vector2(vecX * speed, rg.velocity.y);
            scaleX = sc.x > 0 ? -sc.x : sc.x;
            CharTransform.localScale = new Vector2(scaleX, scaleY);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rg.velocity = new Vector2(vecX * speed, rg.velocity.y);
            scaleX = Math.Abs(sc.x);
            CharTransform.localScale = new Vector2(scaleX, scaleY);
        }
        if (vecX < -0.01f && Math.Abs(vecY) < 0.01f && isJumping == false)
        {
            AniState = PlayerCharAnimationState.WalkLeft;
            CharAnimator.SetInteger(AniName_CharState, 1);
        }
        if (vecX > 0.01f && Math.Abs(vecY) < 0.01f && isJumping == false)
        {
            AniState = PlayerCharAnimationState.WalkRight;
            CharAnimator.SetInteger(AniName_CharState, 1);
        }
        if (Math.Abs(vecX) < 0.01f && Math.Abs(speedY) < 0.01f)
        {
            CharAnimator.SetInteger(AniName_CharState, 0);
            AniState = PlayerCharAnimationState.Stand;
        }
    }

    private void AddEventHandler()
    {

    }

    /// <summary>
    /// 杀死自己
    /// </summary>
    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    PlayerCharAnimationState AniState { get; set; }

    /// <summary>
    /// 角色朝向
    /// </summary>
    PlayerCharFace Face
    {
        get
        {
            if (CharTransform.localScale.x > 0)
                return PlayerCharFace.Right;
            else
                return PlayerCharFace.Left;
        }
    }
}

/// <summary>
/// 玩家角色动作状态
/// </summary>
enum PlayerCharAnimationState
{ 
    Stand,
    WalkLeft,
    WalkRight,
    Jump,
    Attack,
}

/// <summary>
/// 角色朝向
/// </summary>
enum PlayerCharFace
{ 
    Left,
    Right,
}
