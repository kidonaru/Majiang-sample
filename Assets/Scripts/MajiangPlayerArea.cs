using System.Collections.Generic;
using UnityEngine;

public class MajiangPlayerArea : MonoBehaviour
{
    [SerializeField]
    public MajiangShanView shanView;

    [SerializeField]
    public MajiangShoupaiView shoupaiView;

    [SerializeField]
    public MajiangHeView heView;

    public void Init(int l)
    {
        shanView.Init();
        shoupaiView.Init();
        heView.Init();
    }

    public void UpdatePaishu(int paishu, List<string> baopai)
    {
        shanView.UpdatePaishu(paishu, baopai);
    }

    public void UpdateShoupai(Majiang.Shoupai shoupai, bool isHule)
    {
        shoupaiView.UpdateShoupai(shoupai, isHule);
    }

    public void UpdateHe(Majiang.He he)
    {
        heView.UpdateHe(he);
    }
}