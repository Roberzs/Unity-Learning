using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class02MovingSphere02 : MonoBehaviour
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

    [SerializeField, Range(0, 90)]
    float maxGroundAngle = 25f;     // 最大地面角度

    float minGroundDotProduct;      // 角度点乘

    int jumpPhase;      // 跳跃次数

    Vector3 velocity, desiredVelocity;   // 速度  想要达到的速度

    Vector3 contactNormal;  // 碰撞体的法线信息

    Rigidbody body;

    bool desiredJump;   // 是否跳跃
    bool onGround;      // 是否处于地面

    private void Awake()
    {
        // 获取刚体组件
        body = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void OnValidate()
    {
        // 倾斜角度的余弦值 因为maxGroundAngle是角度 所以 * Mathf.Deg2Rad 进行转换
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
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

        AdjustVelocity();

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
        else
        {
            // 如果当前离开地面 将法线信息调整成Vector3.up 以便于空气跳时可以继续按原方向跳跃
            contactNormal = Vector3.up;
        }
    }

    private void Jump()
    { 

        if (onGround || jumpPhase < maxAirJumps) {
            jumpPhase += 1;

            // 跳跃力度 = √(-2 * 自身重量 * 期望高度)
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);

            float alignedSpeed = Vector3.Dot(velocity, contactNormal);

            // 限制向上的速度
            if (alignedSpeed > 0f)
            {
                // Mathf.Max 起修正作用 如果目前速度已经大于跳跃速度 相减将会变成负数 
                jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
            }
            //velocity.y += jumpSpeed;
            // 根据法线信息跳跃
            velocity += contactNormal * jumpSpeed;
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
            if (normal.y >= minGroundDotProduct)
            {
                onGround = true;
                contactNormal = normal;
            }
        }
    }

    // 返回在平面上的移动速度
    private Vector3 ProjectOnContactPlane (Vector3 vector)
    {
        // Vector3.Dot: 返回进行Dot计算的两个向量之间的夹角的余弦值(Cos弧度角). 能进行Dot计算的前提是两个向量首先要变成单位向量
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }

    // 调整速度
    private void AdjustVelocity()
    {
        // 对向量归一化 求得 x z 轴的正确方向
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        // 将当前速度投影到两个向量上 求得相对速度 ↓
        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        // 根据当前是否在地面上 获取目前应当遵守的加速度
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;    // 速率
        // 控制目标速度并将加速度应用于实际速度，直到它与期望的速度相匹配 
        float newX = Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }
}
