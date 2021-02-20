using UnityEngine;
using System.Collections;

public class CampOnClick : MonoBehaviour
{
    private ICamp mCamp;
    public ICamp Camp { set { mCamp = value; } }

    private void OnMouseUpAsButton()
    {
        GameFacade.Instance.ShowCampInfo(mCamp);
    }
}

