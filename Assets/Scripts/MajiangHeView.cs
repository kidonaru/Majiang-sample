using UnityEngine;

public class MajiangHeView : MajiangPaiView
{
    public static readonly int PaiXCount = 6;
    public static readonly int PaiZCount = 3;

    private Majiang.He he = new Majiang.He();

    public void Init()
    {
        CreatePaiList(24);
    }

    public void UpdateHe(Majiang.He he)
    {
        lock (_lock)
        {
            if (he.Equals(this.he))
            {
                this._updateRequest = true;
                this.he = (Majiang.He) he.Clone();
            }
        }
    }

    public override void UpdatePai()
    {
        var paiSize = MajiangView.Instance.paiSize;
        var paiIndex = 0;
        var x = - PaiXCount / 2f * paiSize.x;
        var z = PaiZCount / 2f * paiSize.z;

        foreach (var p in he._pai)
        {
            if (MajiangPai.IsFulou(p)) continue;

            var pai = GetOrCreatePai(paiIndex++);
            var state = MajiangPai.GetState(p);

            pai.SetState(state);
            pai.SetPai(p);

            var width = pai.GetWidth();
            x += width / 2;
            pai.SetPosition(_boxPosition + new Vector3(x, 0, z));
            x += width / 2;

            // 次の行へ (3列目はそのまま延長)
            if (paiIndex < PaiXCount * PaiZCount && paiIndex % PaiXCount == 0)
            {
                x = - PaiXCount / 2f * paiSize.x;
                z -= paiSize.z;
            }
        }

        HiddenRemainPai(paiIndex);
    }
}