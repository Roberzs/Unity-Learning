using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class02MovingSphere : MonoBehaviour
{
    // SerializeField 序列化 表示该字段在编辑器中公开 Range 设置区间
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10;        // 最高速度

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10, maxAirAcceleration = 1;     // 最大加速度 空中最大加速度

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;      // 跳跃高度

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;        // 支持空中跳跃的最大次数

    int jumpPhase;      // 跳跃次数

    Vector3 velocity, desiredVelocity;   // 速度  想要达到的速度

    Rigidbody body;

    bool desiredJump;   // 是否跳跃
    bool onGround;      // 是否处于地面

    private void Awake()
    {
        // 获取刚体组件
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        //playerInput.Normalize();    // 向量归一化
        playerInput = Vector2.ClampMagnitude(playerInput, 1.0f);    // 约束输入向量

        //transform.position = new Vector3(playerInput.x, 0f, playerInput.y);     // 绝对位置的移动

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        // 当按下跳跃键 有可能不会调用下一帧的FixedUpdate 这时返回值将会变成false  所以 需要进行位运算防止此类情况
        desiredJump |= Input.GetButtonDown("Jump");
    }

    // 默认0.02秒调用一次
    private void FixedUpdate()
    {
        UpdateState();

        // 根据当前是否在地面上 获取目前应当遵守的加速度
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;    // 速率

        // 控制目标速度并将加速度应用于实际速度，直到它与期望的速度相匹配 
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (desiredJump)    // 检测是否需要跳跃
        {
            desiredJump = false;
            Jump();
        }

        // 将速度赋值给刚体组件的速度
        body.velocity = velocity;

        // 为了使onGround参数有效 在fixedUpdate事件执行完之后 先将 onGround 设置为 false 然后依靠碰撞事件去判断目前是否还在与地面接触
        onGround = false;
    }

    // 更新状态
    private void UpdateState()
    {
        velocity = body.velocity;   // 刚体的运动速度可能会受到碰撞等事件的影响 所以要先获取一下当前速度
        // 如果目前在地面上 将跳跃次数清空
        if (onGround)
        {
            jumpPhase = 0;
        }
    }

    private void Jump()
    { 

        if (onGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;

            // 跳跃力度 = √(-2 * 自身重量 * 期望高度)
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            // 限制向上的速度
            if (velocity.y > 0f)
            {
                // Mathf.Max 起修正作用 如果目前速度已经大于跳跃速度 相减将会变成负数 
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;
        }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        //onGround = true;
        EvaluateCollision(collision);
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    // 此处废弃原因 如果在OnCollisionEnter将onGround设为true OnCollisionExit将onGround设置为false - 有可能物体撞到了墙后再离开墙 onGround 会变成false 造成物体无法跳跃
    //    onGround = false;
    //}


    // 验证自身与所碰撞物体是否是在接触地面 (作用: 根据法线属性判断 如果撞到的是墙 则不可以跳跃)
    private void EvaluateCollision (Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            // 获取法线属性
            Vector3 normal = collision.GetContact(i).normal;
            // 如果Y值为1 则为垂直 
            onGround |= normal.y >= 0.9f;
        }
    }
}
