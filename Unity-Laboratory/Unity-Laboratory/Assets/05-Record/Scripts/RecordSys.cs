using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordSys : MonoBehaviour
{
    private Dictionary<int, RecordData> recordDataDic = new Dictionary<int, RecordData>();

    private float timer;

    public static RecordSys Instance { get; private set; }

    private void Start()
    {
        Instance = this;
        timer = Time.realtimeSinceStartup;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            if (Time.realtimeSinceStartup - timer >= 0.01f)
            {
                // µ¹·Å
                foreach (var id in recordDataDic.Keys)
                {
                    if (recordDataDic[id].itemObj != null)
                        recordDataDic[id].Record();
                }
                timer = Time.realtimeSinceStartup;
            }
        }
        else
        {
            if (Time.realtimeSinceStartup - timer >= 0.02f)
            {
                // Â¼ÖÆ
                foreach (Transform item in transform)
                {
                    int id = item.GetInstanceID();
                    if (!recordDataDic.ContainsKey(id))
                    {
                        recordDataDic.Add(id, new RecordData());
                    }
                    recordDataDic[id].AddRecordItem(item.GetComponent<Rigidbody>());
                }
                timer = Time.realtimeSinceStartup;
            }
            
        }

    }
}

public class RecordItem
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;

    public RecordItem(Rigidbody rigidbody)
    {
        position = rigidbody.transform.position;
        rotation = rigidbody.transform.eulerAngles;
        velocity = rigidbody.velocity;
    }
}

public class RecordData
{
    public Stack<RecordItem> recordItems = new Stack<RecordItem>();

    public Rigidbody itemObj;

    public int Count => recordItems.Count;

    public void AddRecordItem(Rigidbody rigidbody)
    {
        itemObj = rigidbody;
        recordItems.Push(new RecordItem(rigidbody));
    }

    public void Record()
    {
        if (Count > 0)
        {
            RecordItem recordItem = recordItems.Pop();
            itemObj.transform.position = recordItem.position;
            itemObj.transform.eulerAngles = recordItem.rotation;
            itemObj.velocity = recordItem.velocity;
        }
        else
        {
            if(itemObj.name == "Sphere(Clone)")
            {
                GameObject.Destroy(itemObj.gameObject);
            }
        }
    }
}


