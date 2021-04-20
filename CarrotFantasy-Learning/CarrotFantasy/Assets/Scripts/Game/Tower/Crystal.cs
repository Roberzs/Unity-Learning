using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 水晶塔
/// </summary>
public class Crystal : TowerPersonalProperty {

    private float distance;
    private float bullectWidth;
    private float bullectLength;
    private AudioSource audioSource;
	
	protected override void Start () {
        base.Start();
        bulletGo = GameController.Instance.GetGameObjectResource("Tower/ID"+tower.towerID.ToString()+"/Bullect/"+towerLevel.ToString());
        bulletGo.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = GameController.Instance.GetAudioClip("NormalMordel/Tower/Attack/" + tower.towerID.ToString());
    }

    private void OnEnable()
    {
        if (animator==null)
        {
            return;
        }
        bulletGo = GameController.Instance.GetGameObjectResource("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString());
        bulletGo.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update () {
       
        if (GameController.Instance.isPause||targetTrans==null || GameController.Instance.gameOver)
        {
            if (targetTrans==null)
            {
                bulletGo.SetActive(false);
            }
            return;
        }
        Attack();       
    }

    protected override void Attack()
    {
        if (targetTrans==null)
        {
            return;
        }
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        animator.Play("Attack");
        if (targetTrans.gameObject.tag=="Item")
        {
            distance = Vector3.Distance(transform.position,targetTrans.position+new Vector3(0,0,3));
        }
        else
        {
            distance = Vector3.Distance(transform.position, targetTrans.position);
        }
        bullectWidth = 3 / distance;
        bullectLength = distance / 2;
        if (bullectWidth<=0.5f)
        {
            bullectWidth = 0.5f;
        }
        else if (bullectWidth>=1)
        {
            bullectWidth = 1;
        }
        bulletGo.transform.position = new Vector3((targetTrans.position.x+transform.position.x)/2, (targetTrans.position.y + transform.position.y) / 2, 0);
        bulletGo.transform.localScale = new Vector3(1,bullectWidth,bullectLength);
        bulletGo.SetActive(true);
        bulletGo.GetComponent<Bullet>().targetTrans = targetTrans;
    }

    protected override void DestoryTower()
    {
        bulletGo.SetActive(false);
        GameController.Instance.PushGameObjectToFactory("Tower/ID" + tower.towerID.ToString() + "/Bullect/" + towerLevel.ToString(), bulletGo);
        bulletGo = null;
        base.DestoryTower();
    }
}
