using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class02MovingSphere : MonoBehaviour
{
    // SerializeField 序列化 表示该字段在编辑器中公开 Range 设置区间
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10;        // 最高速度

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10;     // 最大加速度

    Vector3 velocity, desiredVelocity;   // 速度  想要达到的速度

    Rigidbody body;

    bool desiredJump;   // 是否跳跃

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
        velocity = body.velocity;   // 刚体的运动速度可能会受到碰撞等事件的影响 所以要先获取一下当前速度
        float maxSpeedChange = maxAcceleration * Time.deltaTime;    // 速率

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
    }

    private void Jump()
    {
        velocity.y += 5f;
    }

}
