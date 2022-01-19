using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    public static GuidePanel Instance = null;


    public GameObject fingerObj;
    public Image leftRoleSprite, rightRoleSprite, dialogBgSprite;
    public Text dialogText;
    public CircleGuidanceController circleMask;
    public RectGuidanceController rectMask;
    public GameObject maskContainer;
    public GameObject bg;
    //public Transform clickAreaTrans,bg;
    //public BoxCollider clickAreaBox;
    //public GameObject arrow,leftDialog,rightDialog;
    //public GuideDialogTextShow dialogTextShow;
    public bool isInLoading = false;
    private int curGuideId = 0;
    private GameObject curCloneGameObj;
    public RectTransform cloneContainer;
    public RectTransform clickArea;
    public GameObject arrow;

    public RectTransform Target;

    private void Awake()
    {
        Instance = this;

        
    }

    private void Start()
    {
        rectMask.SetTarget(Target);
        //maskContainer.SetActive(true);
        circleMask.gameObject.SetActive(false);
        rectMask.gameObject.SetActive(true);
    }
    //public void SetDataById(int id)
    //{
    //    if (curCloneGameObj != null)
    //    {
    //        Destroy(curCloneGameObj);
    //    }
    //    curGuideId = id;
    //    GuideInfo guideInfo = GuideEvent.Instance.GetGuideInfo(id);
    //    PlayerData.Instance.playerData.guideInfo.guideId = curGuideId;
    //    bool isShowBG = GuideTextData.Instance.GetIsShowBG(curGuideId);
    //    bg.gameObject.SetActive(isShowBG);
    //    if (!string.IsNullOrEmpty(guideInfo.guideText))
    //    {
    //        dialogText.gameObject.SetActive(true);
    //        dialogText.text = guideInfo.guideText;
    //        dialogBgSprite.gameObject.SetActive(true);
    //        if (guideInfo.dialogDir != DialogDir.Mid)
    //        {
    //            RectTransform.Edge edge = (RectTransform.Edge)guideInfo.dialogDir;
    //            int size = 0;
    //            switch (edge)
    //            {
    //                case RectTransform.Edge.Bottom:
    //                case RectTransform.Edge.Top:
    //                    size = Screen.height / 3;
    //                    break;
    //                case RectTransform.Edge.Left:
    //                case RectTransform.Edge.Right:
    //                    size = Screen.width / 3;
    //                    break;
    //            }
    //            dialogBgSprite.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(edge, 0, size);
    //        }
    //        else
    //        {
    //            dialogBgSprite.GetComponent<RectTransform>().localPosition = Vector3.zero;
    //            dialogBgSprite.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width / 3, Screen.height / 3);

    //        }
    //    }
    //    else
    //    {
    //        dialogBgSprite.gameObject.SetActive(false);
    //    }

    //    Sprite roleSprite = guideInfo.roleSprite;
    //    if (roleSprite != null)
    //    {

    //        leftRoleSprite.sprite = roleSprite;
    //        rightRoleSprite.sprite = roleSprite;
    //        leftRoleSprite.gameObject.SetActive(true);
    //        rightRoleSprite.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        leftRoleSprite.gameObject.SetActive(false);
    //        rightRoleSprite.gameObject.SetActive(false);
    //    }
    //    //50的偏差为手指动画的位移
    //    Vector3 fingerPos = guideInfo.trans == null ? Vector3.zero : new Vector3(guideInfo.trans.position.x + 50, guideInfo.trans.position.y - 50, guideInfo.trans.position.z);
    //    if (fingerPos != Vector3.zero)
    //    {
    //        fingerObj.SetActive(true);
    //        fingerObj.transform.position = fingerPos;
    //    }
    //    else
    //    {
    //        fingerObj.gameObject.SetActive(false);
    //    }

    //    if (guideInfo.showType == ShowType.Clone)
    //    {
    //        if (guideInfo.trans.gameObject != null)
    //        {
    //            curCloneGameObj = CloneGameObject(guideInfo);
    //            curCloneGameObj.SetActive(true);
    //        }
    //        maskContainer.gameObject.SetActive(false);
    //        clickArea.gameObject.SetActive(true);
    //        clickArea.sizeDelta = curCloneGameObj.GetComponent<RectTransform>().sizeDelta;
    //        clickArea.position = curCloneGameObj.transform.position;
    //    }
    //    else
    //    {
    //        clickArea.gameObject.SetActive(false);
    //        if (guideInfo.maskType == MaskType.Circle)
    //        {
    //            circleMask.SetTarget(guideInfo.trans);
    //            maskContainer.SetActive(true);
    //            circleMask.gameObject.SetActive(true);
    //            rectMask.gameObject.SetActive(false);
    //        }
    //        else if (guideInfo.maskType == MaskType.Rect)
    //        {
    //            rectMask.SetTarget(guideInfo.trans);
    //            maskContainer.SetActive(true);
    //            circleMask.gameObject.SetActive(false);
    //            rectMask.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            maskContainer.SetActive(false);
    //            clickArea.gameObject.SetActive(true);
    //            clickArea.sizeDelta = new Vector2(3000, 3000);//默认全屏
    //            clickArea.localPosition = Vector3.zero;
    //        }
    //    }

    //    if (IsInvoking("DelayCallEvent"))
    //    {
    //        CancelInvoke("DelayCallEvent");
    //    }
    //    Invoke("DelayCallEvent", 0.1f);
    //}

    //public GameObject CloneGameObject(GuideInfo info)
    //{
    //    GameObject cloneObj = GameObject.Instantiate(info.trans.gameObject);

    //    //如果克隆的物体是灰色的，改成正常颜色
    //    //ChangeToGrayMaterial[] changeToGrayMaterials = cloneObj.GetComponentsInChildren<ChangeToGrayMaterial>();
    //    //if(changeToGrayMaterials.Length!=0){
    //    //    foreach (ChangeToGrayMaterial gray in changeToGrayMaterials)
    //    //    {
    //    //        gray.SetGray(false);
    //    //    }
    //    //}
    //    cloneObj.transform.SetParent(cloneContainer);
    //    cloneObj.transform.position = info.trans.position;
    //    cloneObj.transform.localScale = info.trans.localScale;
    //    cloneObj.transform.eulerAngles = info.trans.eulerAngles;
    //    //关闭原物体的事件
    //    Button[] buttons = cloneObj.GetComponentsInChildren<Button>();
    //    foreach (var btn in buttons)
    //    {
    //        Destroy(btn);
    //    }
    //    return cloneObj;
    //}

    //private void DelayCallEvent()
    //{
    //    string strEvent = GuideTextData.Instance.GetEvent(curGuideId);
    //    if (!string.IsNullOrEmpty(strEvent))
    //    {

    //        GuideEvent.Instance.gameObject.SendMessage(strEvent);
    //    }
    //}

    //public void ShowGuideById(int id)
    //{
    //    SetDataById(id);
    //    UIManager.Instance.ShowPanel(PanelType.GuidePanel);
    //}


    //public void ClickNext()
    //{
    //    Debug.Log("next");
    //    string strClick = GuideTextData.Instance.GetClickAction(curGuideId);
    //    int nextId = GuideTextData.Instance.GetNextID(curGuideId);
    //    if (nextId != 0)
    //    {

    //        if (!string.IsNullOrEmpty(strClick))
    //        {
    //            GuideEvent.Instance.gameObject.SendMessage(strClick);
    //        }
    //        SetDataById(nextId);

    //    }
    //    else
    //    {

    //        if (isInLoading)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //        else
    //        {
    //            UIManager.Instance.HideGuidePanel();
    //        }

    //        if (!string.IsNullOrEmpty(strClick))
    //        {
    //            GuideEvent.Instance.gameObject.SendMessage(strClick);
    //        }
    //    }
    //}
    //public void SetArrowState(bool active)
    //{
    //    arrow.SetActive(active);
    //}
}
