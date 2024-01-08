using System.Collections.Generic;
using System.Linq;
using Majiang;
using UnityEngine;

public class MajiangShanView : MajiangPaiView
{
    public static readonly int PaiXCount = 17;
    public static readonly int WangpaiCount = 14;

    private int paishu;
    private List<string> baopai;

    public void Init()
    {
        CreatePaiList(PaiXCount * 2);
    }

    public void UpdatePaishu(int paishu, List<string> baopai)
    {
        lock (_lock)
        {
            if (paishu != this.paishu || !baopai.SafeSequenceEqual(this.baopai))
            {
                this._updateRequest = true;
                this.paishu = paishu;
                this.baopai = baopai;
            }
        }
    }

    public override void UpdatePai()
    {
        var paiSize = MajiangView.Instance.paiSize;
        int paiIndex = 0;
        for (int i = 0; i < paishu; i++)
        {
            var pai = GetOrCreatePai(paiIndex++);
            pai.SetState(MajiangPai.State.FaceDown);
            pai.SetPai("_");

            var xIndex = i / 2;
            var yIndex = i % 2;

            var x = (xIndex - PaiXCount / 2f) * paiSize.x;
            var y = yIndex * paiSize.y;

            pai.SetPosition(_boxPosition + new Vector3(x, y, 0));
        }

        if (baopai != null)
        {
            var wangpai = Enumerable.Repeat("_", WangpaiCount).ToList();
            for (int i = 0; i < baopai.Count; i++)
            {
                wangpai[i * 2 + 5] = baopai[i];
            }

            for (int i = 0; i < WangpaiCount; i++)
            {
                var p = wangpai[i];

                var pai = GetOrCreatePai(paiIndex++);
                var state = MajiangPai.GetState(p);
                pai.SetState(state);
                pai.SetPai(p);

                var xIndex = (PaiXCount * 2 - WangpaiCount + i) / 2;
                var yIndex = i % 2;

                var x = (xIndex - PaiXCount / 2f) * paiSize.x;
                var y = yIndex * paiSize.y;

                pai.SetPosition(_boxPosition + new Vector3(x, y, 0));
            }
        }

        HiddenRemainPai(paiIndex);
    }
}