using System.Collections.Generic;
using UnityEngine;

public abstract class MajiangPaiView : MonoBehaviour
{
    [SerializeField]
    protected BoxCollider box;

    [SerializeField]
    protected Transform paiParent;

    protected readonly object _lock = new object();
    protected bool _updateRequest = false;
    protected List<MajiangPai> _paiList = new List<MajiangPai>();
    protected Vector3 _boxPosition;

    private void Awake()
    {
        _boxPosition = box.transform.localPosition;
    }

    void Update()
    {
        if (!_updateRequest) return;

        lock (_lock)
        {
            _updateRequest = false;
            UpdatePai();
        }
    }

    public abstract void UpdatePai();

    private void OnDestroy()
    {
        DestroyAllPai();
    }

    public void DestroyAllPai()
    {
        foreach (var pai in _paiList)
        {
            Destroy(pai.gameObject);
        }
        _paiList.Clear();
    }

    public void CreatePaiList(int paiCount)
    {
        var diff = paiCount - _paiList.Count;
        if (diff > 0)
        {
            for (int i = 0; i < diff; i++)
            {
                CreatePai();
            }
        }
    }

    public MajiangPai GetOrCreatePai(int index)
    {
        CreatePaiList(index + 1);
        return _paiList[index];
    }

    /// <summary>
    /// 残りの牌を非表示にする
    /// </summary>
    /// <param name="index"></param>
    public void HiddenRemainPai(int index)
    {
        for (int i = index; i < _paiList.Count; i++)
        {
            _paiList[i].gameObject.SetActive(false);
        }
    }

    public void CreatePai()
    {
        var pai = MajiangView.Instance.CreatePai("_", MajiangPai.State.None, paiParent);
        _paiList.Add(pai);
    }
}