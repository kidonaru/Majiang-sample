using System.Collections.Generic;
using System.Text.RegularExpressions;
using Majiang;
using UnityEngine;

public class MajiangShoupaiView : MajiangPaiView
{
    private Majiang.Shoupai shoupai = new Majiang.Shoupai();
    private string shoupaiStr;
    private string zimo;
    private List<string> bingpai = new List<string>();
    private List<List<string>> fulou = new List<List<string>>();
    private bool isHule;

    public void Init()
    {
        CreatePaiList(20);
    }

    public void UpdateShoupai(Majiang.Shoupai shoupai, bool isHule)
    {
        lock (_lock)
        {
            if (shoupai.Equals(this.shoupai) && isHule == this.isHule) {
                return;
            }

            this._updateRequest = true;
            this.shoupai = (Majiang.Shoupai) shoupai.Clone();
            this.isHule = isHule;
            this.zimo = shoupai._zimo;
            this.bingpai = this.shoupai._bingpai.display_pai();
            this.fulou = shoupai._fulou.ConvertAll(ConvertFulouToList);

            // ツモ牌を手牌から除外
            if (!this.zimo.IsNullOrEmpty())
            {
                var index = this.bingpai.FindIndex(x => x == zimo);
                if (index >= 0)
                {
                    this.bingpai.RemoveAt(index);
                }
            }
        }
    }

    public List<string> ConvertFulouToList(string mianzi)
    {
        List<string> ret = new List<string>();
        char s = mianzi[0];

        Match match = Regex.Match(mianzi.Substring(1), @"\d[\+\=\-_]?");

        while (match.Success)
        {
            ret.Add(s.ToString() + match.Value);
            match = match.NextMatch();
        }

        return ret;
    }

    public override void UpdatePai()
    {
        var paiSize = MajiangView.Instance.paiSize;
        var paiIndex = 0;
        var xMax = Mathf.Abs(box.bounds.max.x); // FIXME

        var state = isHule ? MajiangPai.State.FaceUp : MajiangPai.State.Standing;

        var x = -(bingpai.Count / 2f) * paiSize.x;
        foreach (var p in bingpai)
        {
            var pai = GetOrCreatePai(paiIndex++);
            pai.SetState(state);
            pai.SetPai(p);

            x += paiSize.x;
            pai.SetPosition(_boxPosition + new Vector3(x, 0, 0));
        }

        // ツモ牌
        if (!zimo.IsNullOrEmpty()) {
            var pai = GetOrCreatePai(paiIndex++);
            pai.SetState(state);
            pai.SetPai(zimo);

            x += paiSize.x;
            pai.SetPosition(_boxPosition + new Vector3(x, 0, 0));
        }

        x = xMax - _boxPosition.x;
        foreach (var list in fulou)
        {
            foreach (var p in list.Reversed())
            {
                var pai = GetOrCreatePai(paiIndex++);
                state = MajiangPai.GetState(p);

                pai.SetState(state);
                pai.SetPai(p);

                var width = pai.GetWidth();
                x -= width / 2;
                pai.SetPosition(_boxPosition + new Vector3(x, 0, 0));
                x -= width / 2;
            }
        }

        HiddenRemainPai(paiIndex);
    }
}