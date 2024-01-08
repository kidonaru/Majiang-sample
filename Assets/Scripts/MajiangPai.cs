using UnityEngine;

public class MajiangPai : MonoBehaviour
{
    /// <summary>
    /// 牌の表示状態
    /// </summary>
    public enum State
    {
        None, // 非表示
        FaceUp, // 表向きの状態
        FaceDown, // 裏向きの状態
        FaceUpLeft, // 左回転の状態
        FaceUpRight, // 右回転の状態
        Standing, // 立っている状態
    } 

    [SerializeField]
    private string p;

    [SerializeField]
    private State state;

    [SerializeField]
    private Transform baseTransform;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    public BoxCollider boxCollider;

    private Vector3 _localPosition;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="s"></param>
    public void Init(string p, State state)
    {
        SetPai(p);
        SetState(state);
    }

    public void SetPai(string p)
    {
        if (p == "_")
        {
            p = "z5"; // 裏向きの牌は z5 とする
        }

        if (this.p == p) return;
        this.p = p;

        var filename = p.Substring(0, 2);
        if (filename == "_") filename = "z5"; // 裏向きの牌は z5 とする
        var sprite = Resources.Load<Sprite>($"Image/{filename}");
        spriteRenderer.sprite = sprite;
    }

    public void SetPosition(Vector3 position)
    {
        if (_localPosition != position)
        {
            transform.localPosition = position;
            _localPosition = position;
        }
    }

    public void SetState(State state)
    {
        gameObject.SetActive(state != State.None);

        if (state == this.state)
        {
            return;
        }
        this.state = state;

        if (state == State.Standing)
        {
            baseTransform.localPosition = new Vector3(0, 0.03f, 0);
        }
        else
        {
            baseTransform.localPosition = Vector3.zero;
        }

        switch (state)
        {
            case State.FaceUp:
                baseTransform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case State.FaceDown:
                baseTransform.localRotation = Quaternion.Euler(0, 0, 180);
                break;
            case State.FaceUpLeft:
                baseTransform.localRotation = Quaternion.Euler(0, -90, 0);
                break;
            case State.FaceUpRight:
                baseTransform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case State.Standing:
                baseTransform.localRotation = Quaternion.Euler(-90, 0, 0);
                break;
        }
    }

    public float GetWidth()
    {
        var paiSize = MajiangView.Instance.paiSize;
        if (state == State.FaceUpLeft || state == State.FaceUpRight)
        {
            return paiSize.z;
        }
        return paiSize.x;
    }

    public static bool IsFulou(string p)
    {
        return p.Length >= 3 && (p[2] == '+' || p[2] == '=' || p[2] == '-');
    }

    public static State GetState(string p)
    {
        if (p == "_")
        {
            return State.FaceDown;
        }

        if (p.Length >= 3 && p[2] == '*') return State.FaceUpLeft;
        if (IsFulou(p)) return State.FaceUpRight;

        return State.FaceUp;
    }
}