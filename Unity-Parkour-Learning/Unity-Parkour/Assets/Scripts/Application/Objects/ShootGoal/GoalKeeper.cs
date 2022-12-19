using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeper : MonoBehaviour
{
    private Animation mAnimation;

    private void Awake()
    {
        mAnimation = GetComponentInChildren<Animation>();
    }


}
