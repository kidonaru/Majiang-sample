using System;
using System.Collections.Generic;
using UnityEngine;

public class MajiangBoardView : MonoBehaviour
{
    [SerializeField]
    private List<MajiangPlayerArea> playerAreaList;

    public void Init()
    {
        for (var l = 0; l < 4; l++)
        {
            var area = playerAreaList[l];
            area.Init(l);
        }
    }

    public void UpdatePaishu(Majiang.IBoard board)
    {
        var paishu = board != null ? board.shan.paishu : MajiangShanView.PaiXCount * 2 * 4;
        var zhuangfeng = board?.zhuangfeng ?? 0;
        var baopai = board?.shan.baopai;

        for (var l = 0; l < 4; l++)
        {
            var area = playerAreaList[l]; // 牌山はどこでもいい
            var areaPaishuMax = MajiangShanView.PaiXCount * 2;
            var ll = (l + 4 - zhuangfeng) % 4;
            var areaPaishu = paishu - ll * areaPaishuMax;
            areaPaishu = Math.Max(Math.Min(areaPaishu, areaPaishuMax), 0);
            area.UpdatePaishu(areaPaishu, ll == 3 ? baopai : null);
        }
    }

    public void UpdateShoupai(Majiang.IBoard board, Majiang.Hule hule = null)
    {
        for (var l = 0; l < 4; l++)
        {
            var id = board.player_id[l];
            var area = playerAreaList[id];
            var isHule = hule?.l == l;
            area.UpdateShoupai(board.shoupai[l], isHule);
        }
    }

    public void UpdateHe(Majiang.IBoard board)
    {
        for (var l = 0; l < 4; l++)
        {
            var id = board.player_id[l];
            var area = playerAreaList[id];
            area.UpdateHe(board.he[l]);
        }
    }
}