using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public Transform startPointTrans;
    public GameObject bulletObj;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Biu
            GameObject bullet = Instantiate(bulletObj, RecordSys.Instance.transform);
            bullet.transform.position = startPointTrans.position;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 1) * 35f;
        }
    }
}
