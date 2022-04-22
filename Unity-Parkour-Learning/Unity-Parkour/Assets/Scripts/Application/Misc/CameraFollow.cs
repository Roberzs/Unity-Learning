/****************************************************
	文件：CameraFollow.cs
	作者：Zhangying
	邮箱：zhy18125@gmail.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	Transform m_Player;
	Vector3 m_Offest;
	float m_Speed = 20;
    private void Awake()
    {
		m_Player = GameObject.FindGameObjectWithTag("Player").transform;
		m_Offest = transform.position - m_Player.position;
    }

    private void LateUpdate()
    {
		transform.position = Vector3.Lerp(transform.position, m_Player.transform.position + m_Offest, m_Speed);
        
    }
}
