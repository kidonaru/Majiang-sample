using System.Collections.Generic;
using Majiang;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MajiangController : MonoBehaviour
{
    public static MajiangController Instance;

    public enum GameState
    {
        None,
        Init,
        Playing,
        Pause,
        End,
    }

    [SerializeField]
    public MajiangView view;

    [SerializeField]
    public Majiang.Rule rule;

    [SerializeField]
    public int speed = 1;

    [SerializeField]
    public int wait = 1000;

    [SerializeField]
    public Button startButton;

    [SerializeField]
    public Button pauseButton;

    [SerializeField]
    public Button resumeButton;

    public Majiang.Game game;
    public Majiang.Player player;
    public GameState state = GameState.Init;

    private void Awake()
    {
        Instance = this;

        startButton.onClick.AddListener(OnGameStart);
        pauseButton.onClick.AddListener(OnGamePause);
        resumeButton.onClick.AddListener(OnGameResume);
    }

    private void Start()
    {
        view.Init();

        UpdateGameState(GameState.Init);
    }

    private void OnDestroy()
    {
        game.stop();
        game = null;
        Instance = null;
    }

    private void OnGameStart()
    {
        if (state == GameState.Init || state == GameState.End)
        {
            var players = new List<Majiang.Player>{ null, null, null, null };
            for (int i = 0; i < 4; i++)
            {
                players[i] = new Majiang.AI.Player();
            }

            player = players[0];

            game = new Majiang.Game(players, OnGameEnd, rule);
            game.player = new List<string> { "自家", "下家", "対面", "上家" };
            game.speed = speed;
            game.wait = wait;
            game.view = view;

            game.kaiju();

            UpdateGameState(GameState.Playing);
        }
    }

    private void OnGamePause()
    {
        if (state == GameState.Playing)
        {
            game.stop();
            UpdateGameState(GameState.Pause);
        }
    }

    private void OnGameResume()
    {
        if (state == GameState.Pause)
        {
            game.start();
            UpdateGameState(GameState.Playing);
        }
    }

    private void OnGameEnd(Paipu paipu)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (state == GameState.Playing)
            {
                UpdateGameState(GameState.End);
            }
        });
    }

    private void UpdateGameState(GameState state)
    {
        this.state = state;

        switch (state)
        {
            case GameState.Init:
                startButton.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                view.FadeIn();
                break;
            case GameState.Playing:
                startButton.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(true);
                resumeButton.gameObject.SetActive(false);
                view.FadeIn();
                break;
            case GameState.Pause:
                startButton.gameObject.SetActive(false);
                pauseButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(true);
                view.FadeOut();
                break;
            case GameState.End:
                startButton.gameObject.SetActive(true);
                pauseButton.gameObject.SetActive(false);
                resumeButton.gameObject.SetActive(false);
                view.FadeOut();
                break;
        }
    }
}