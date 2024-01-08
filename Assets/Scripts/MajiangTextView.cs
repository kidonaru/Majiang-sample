using UnityEngine;
using UnityEngine.UI;

public abstract class MajiangTextView : MonoBehaviour
{
    [SerializeField]
    protected Text text;

    protected readonly object _lock = new object();

    private string _info;
    private bool _infoUpdated = false;

    public string info
    {
        get => _info;
        set
        {
            if (_info != value)
            {
                _info = value;
                _infoUpdated = true;
            }
        }
    }

    void Update()
    {
        lock (_lock)
        {
            if (_infoUpdated)
            {
                text.text = _info;
                _infoUpdated = false;
            }
        }
    }
}