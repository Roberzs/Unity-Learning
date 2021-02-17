/****************************************************
    文件：CharacterBuilderDirector.cs
    作者：zhyStay
    邮箱: zhy18125@163.com
    日期：2021/2/10 23:41:20
    功能：Nothing
*****************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBuilderDirector
{
    public static ICharacter Construct(ICharacterBuilder builder)
    {
        builder.AddCharacterAttr();
        builder.AddGameObject();
        builder.AddWeapon();
        return builder.GetResult();
    }
}
