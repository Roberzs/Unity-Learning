using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitItemController : Controller
{
    public override void Execute(object data)
    {
        ItemArgs args = (ItemArgs)data;
        PlayerMove player = GetView<PlayerMove>();
        UIBoard board = GetView<UIBoard>();
        GameModel gameModel = GetModel<GameModel>();
        
        switch (args.kind)
        {
            case ItemKind.ItemInvincible:
                player.HitInvincible();
                board.HitInvincible();
                gameModel.InvincibleCnt -= args.hitCount;
                break;
            case ItemKind.ItemMagnet:
                player.HitMagnet();
                board.HitMagnet();
                gameModel.MagnetCnt -= args.hitCount;
                break;
            case ItemKind.ItemMultiply:
                player.HitMultiply();
                board.HitMultiply();
                gameModel.MultiplyCnt -= args.hitCount;
                break;
            default:
                break;
        }

        board.UpdateUI();
    }

    
}
