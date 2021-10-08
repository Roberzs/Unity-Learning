/****************************************************
    文件：Crow.cs
    作者：zhyStay
    邮箱：zhy18125@163.com
    日期：#CreateTime#
    功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{
    public Vector3 sumForce = Vector3.zero;             // 综合力

    public int separationWeight = 1;                    // 分离力权重
    public float separationDistance = 3.0f;             // 分离检测距离
    public List<GameObject> seprationNeighbors = new List<GameObject>();
    public Vector3 separationForce = Vector3.zero;      // 分离力

    public int alignmentWeight = 1;                    // 队列力权重
    public float alignmentDistance = 6.0f;             // 队列检测距离
    public List<GameObject> alignmentNeighbors = new List<GameObject>();
    public Vector3 alignmentForce = Vector3.zero;       // 队列力

    public int cohesionWeight = 1;                      // 聚集力权重
    public Vector3 cohesionForce = Vector3.zero;        // 聚集力

    public float checkInterval = 0.2f;
    public float mass = 1.0f;
    public Vector3 velocity = Vector3.zero;
    private Vector3 startVelocity = Vector3.zero;

    private void Start()
    {
        startVelocity = velocity;
        InvokeRepeating(nameof(CalcForce), 0.0f, checkInterval);
        Invoke(nameof(PlayAnim), Random.Range(0.0f, 1.5f));
    }

    private void PlayAnim()
    {
        GetComponent<Animation>().Play();
    }

    private void CalcForce()
    {
        sumForce = Vector3.zero;
        separationForce = Vector3.zero;
        alignmentForce = Vector3.zero;
        cohesionForce = Vector3.zero;
        // 分离力
        seprationNeighbors.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (var collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.CompareTag("Crow"))
            {
                seprationNeighbors.Add(collider.gameObject);
            }
        }
        // 与附近乌鸦距离过近 添加分离力 分离方向 = 自身坐标 - 附近乌鸦坐标 分离力 = 分离方向 / 分离距离
        foreach (var neighbor in seprationNeighbors)
        {
            Vector3 dir = transform.position - neighbor.transform.position;
            separationForce += dir.normalized / dir.magnitude;
        }
        if (seprationNeighbors.Count != 0)
        {
            separationForce *= separationWeight;
            sumForce += separationForce;
        }
        // 队列力
        alignmentNeighbors.Clear();
        colliders = Physics.OverlapSphere(transform.position, alignmentDistance);
        foreach (var collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.CompareTag(transform.tag))
            {
                alignmentNeighbors.Add(collider.gameObject);
            }
        }

        Vector3 avgDir = Vector3.zero;
        foreach (var neighbor in alignmentNeighbors)
        {
            avgDir += neighbor.transform.forward;
        }
        if (alignmentNeighbors.Count > 0)
        {
            avgDir /= alignmentNeighbors.Count;
            alignmentForce = avgDir + transform.forward;
            alignmentForce *= alignmentWeight;
            sumForce += alignmentForce;
        }
        // 聚集力
        if (seprationNeighbors.Count == 0 && alignmentNeighbors.Count > 0)
        {
            Vector3 center = Vector3.zero;
            foreach (var neighbor in alignmentNeighbors)
            {
                center += neighbor.transform.position;
            }
            center /= alignmentNeighbors.Count;
            Vector3 dirToCenter = center - transform.position;
            cohesionForce += dirToCenter;
            cohesionForce *= cohesionWeight;
            sumForce += cohesionForce;
        }

        // 保持恒定飞行速度的力
        Vector3 engineForce = startVelocity - velocity;
        sumForce += engineForce;
    }

    private void Update()
    {
        Vector3 a = sumForce / mass;
        velocity += a * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
}
