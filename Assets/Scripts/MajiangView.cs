using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MajiangView : MonoBehaviour, Majiang.View
{
    public static MajiangView Instance;

    [SerializeField]
    public MajiangBoardView boardView;

    [SerializeField]
    public MajiangScoreView scoreView;

    [SerializeField]
    public MajiangInfoView infoView;

    [SerializeField]
    public MajiangHuleView huleView;

    [SerializeField]
    public MajiangPai pai;

    [SerializeField]
    public Image fadeImage;

    /// <summary>
    /// 牌のサイズ
    /// </summary>
    public Vector3 paiSize;

    private readonly object _lock = new object();

    /// <summary>
    /// 牌を生成する
    /// </summary>
    /// <param name="p"></param>
    /// <param name="parentTransform"></param>
    /// <returns></returns>
    public MajiangPai CreatePai(string p, MajiangPai.State state, Transform parentTransform)
    {
        var newPai = Instantiate(pai, parentTransform);
        newPai.Init(p, state);
        return newPai;
    }

    public void Init()
    {
        boardView.Init();
        boardView.UpdatePaishu(null);

        scoreView.gameObject.SetActive(true);
        infoView.gameObject.SetActive(true);
        huleView.gameObject.SetActive(true);

        scoreView.UpdateInfo(null);
        infoView.UpdateInfo(null);
        huleView.UpdateInfo(null, null);
    }

    public void FadeOut(float duration = 0.5f)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.CrossFadeAlpha(1, duration, false);
        });
    }

    public void FadeIn(float duration = 0.5f)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            fadeImage.CrossFadeAlpha(0, duration, false);
        });
    }

    private void Awake()
    {
        Instance = this;

        // 牌のサイズを取得
        pai.gameObject.SetActive(true);
        paiSize = pai.boxCollider.size;
        pai.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void UpdateViewParam(
        Majiang.Message paipuLog = null,
        Majiang.Paipu paipu = null,
        bool redrawRequest = false)
    {
        lock (_lock)
        {
            var player = MajiangController.Instance.game;
            var board = player.model;

            if (redrawRequest)
            {
                boardView.UpdatePaishu(board);
                boardView.UpdateShoupai(board);
                boardView.UpdateHe(board);
                FadeIn();
            }

            switch (paipuLog?.type)
            {
                case Majiang.MessageType.Zimo:
                    boardView.UpdatePaishu(board);
                    boardView.UpdateShoupai(board);
                    break;
                case Majiang.MessageType.Dapai:
                    boardView.UpdateShoupai(board);
                    boardView.UpdateHe(board);
                    break;
                case Majiang.MessageType.Hule:
                case Majiang.MessageType.Pingju:
                    boardView.UpdateShoupai(board, paipuLog.hule);
                    boardView.UpdateHe(board);
                    FadeOut();
                    break;
                case Majiang.MessageType.Fulou:
                    boardView.UpdateShoupai(board);
                    boardView.UpdateHe(board);
                    break;
            }

            scoreView.UpdateInfo(board);
            infoView.UpdateInfo(board);
            huleView.UpdateInfo(board, paipuLog?.hule);

            if (paipuLog?.pingju != null)
            {
                huleView.UpdateInfoOnPingju(paipuLog.pingju);
            }

            if (paipu != null)
            {
                huleView.UpdateInfoOnJieju(paipu);
                FadeOut();
            }
        }
    }

    //////// Majiang.View implementation ////////
    // マルチスレッドで動作するので注意

    public void kaiju()
    {
        Debug.Log("開局");
    }

    public void update(Majiang.Message paipuLog = null)
    {
        Debug.Log($"update: {paipuLog}");

        if (paipuLog == null)
        {
            return;
        }

        UpdateViewParam(paipuLog: paipuLog);
    }

    public void redraw()
    {
        Debug.Log($"redraw:");
        UpdateViewParam(redrawRequest: true);
    }

    public void summary(Majiang.Paipu paipu = null)
    {
        lock (_lock)
        {
            var newPaipu = (Majiang.Paipu) paipu.Clone();
            newPaipu.log = null;
            UpdateViewParam(paipu: newPaipu);
            Debug.Log($"終局: {newPaipu}");
        }
    }

    public void say(string type, int lunban)
    {
        Debug.Log($"{type}: {lunban}");
    }
}