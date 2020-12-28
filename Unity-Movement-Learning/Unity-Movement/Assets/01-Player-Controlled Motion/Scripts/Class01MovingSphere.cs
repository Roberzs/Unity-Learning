using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class01MovingSphere : MonoBehaviour
{
    // SerializeField 序列化 表示该字段在编辑器中公开 Range 设置区间
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10;        // 最高速度

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10;     // 最大加速度

    [SerializeField]
    Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);    // 约束移动位置 （左下角位置以及大小）

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;        // 弹跳量

    Vector3 velocity;   // 速度


    private void Update()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");

        //playerInput.Normalize();    // 向量归一化
        playerInput = Vector2.ClampMagnitude(playerInput, 1.0f);    // 约束输入向量

        //transform.position = new Vector3(playerInput.x, 0f, playerInput.y);     // 绝对位置的移动
        
        Vector3 desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;     // 想要达到的速度
        float maxSpeedChange = maxAcceleration * Time.deltaTime;    // 速率

        // 控制目标速度并将加速度应用于实际速度，直到它与期望的速度相匹配 
        //if (velocity.x < desiredVelocity.x) 
        //{
        //    velocity.x = Mathf.Min(velocity.x + maxSpeedChange, desiredVelocity.x);
        //}
        //else if(velocity.x > desiredVelocity.x)
        //{
        //    velocity.x = Mathf.Max(velocity.x - maxSpeedChange, desiredVelocity.x);
        //}
        // MoveTowards(当前值, 目标值, 每次移动的最大长度)
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        //transform.localPosition += displacement;        // 相对位置的移动

        Vector3 newPosition = transform.localPosition + displacement;
        //if (!allowedArea.Contains(new Vector2(newPosition.x, newPosition.z)))     // 如果当前位置不在约束位置之中， 将其设置当前位置， 并在更新前取消位置
        //{
        //    // 但是 该方法虽然限制住了位置 当到达边缘时 会因为球体的速度（惯性）贴在边缘上。

        //    // Mathf.Clamp 限制一个值的大小
        //    newPosition.x = Mathf.Clamp(newPosition.x, allowedArea.xMin, allowedArea.xMax);
        //    newPosition.z = Mathf.Clamp(newPosition.z, allowedArea.yMin, allowedArea.yMax);
        //}

        // 如果移动到了边缘 将位置设置成边缘位置，而且将速度归零
        if (newPosition.x < allowedArea.xMin)   
        {
            newPosition.x = allowedArea.xMin;
            //velocity.x = 0f;      // 消除速度
            velocity.x = -velocity.x * bounciness;  // 反弹 *bounciness表示弹度
        }
        else if (newPosition.x > allowedArea.xMax)
        {
            newPosition.x = allowedArea.xMax;
            //velocity.x = 0f;
            velocity.x = -velocity.x * bounciness;
        }
        if (newPosition.z < allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            //velocity.z = 0f;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            newPosition.z = allowedArea.yMax;
            //velocity.z = 0f;
            velocity.z = -velocity.z * bounciness;
        }

        // 移动
        transform.localPosition = newPosition;
    }
}
