/****************************************************
	文件：Item.cs
	作者：zhystay
	邮箱：zhy18125@163.com
	日期：#CreateTime#
	功能：Nothing
*****************************************************/

using UnityEngine;

public class Item : MonoBehaviour
{
    public string SelfPath;
    [SerializeField] private float rotateSpeed = 10f;


    protected virtual void Awake()
    {

    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {
        transform.localEulerAngles = Vector3.zero;
    }

    public virtual void HitPlayer(Transform player)
    {
        // 回收
        Destroy(gameObject);
    }

    public void PlayEffect(string effectName, Vector3 pos)
    {
        GameObject itemEff = GameRoot.Instance.factoryManager.GetGameObjectResource(effectName);
        itemEff.transform.position = pos;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagDefine.Player))
        {
            HitPlayer(other.transform);
        }
    }

    private void Update()
    {
        transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f);
    }
}
