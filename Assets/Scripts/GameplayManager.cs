using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayManager : MonoBehaviour
{
    public GameObject player;
    public GameObject layers;
    public GameObject pipes;

    public static int currentLayerNum = 0;

    public enum GAME_STATE { MENU, PLAY, PAUSE, DEAD, RESTART, NEUTRAL };
    public GAME_STATE gameState = GAME_STATE.MENU;

    public static UnityEvent menu = new UnityEvent();
    public static UnityEvent play = new UnityEvent();
    public static UnityEvent pause = new UnityEvent();
    public static UnityEvent dead = new UnityEvent();
    public static UnityEvent restart = new UnityEvent();

    public static UnityEvent layerComplete = new UnityEvent();

    public static UnityEvent finishedLevelTransition = new UnityEvent();

    public bool completeLayer;

    public void StartGame()
    {
        player.SetActive(true);
        layers.SetActive(true);
        pipes.SetActive(true);
        pipes.GetComponent<Pipes>().SetRandomPipes();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (completeLayer)
        {
            completeLayer = false;

            layerComplete.Invoke();
        }

        switch (gameState)
        {
            case GAME_STATE.MENU:
                {
                    menu.Invoke();

                    gameState = GAME_STATE.NEUTRAL;

                    break;
                }
            case GAME_STATE.PLAY:
                {
                    StartGame();

                    play.Invoke();

                    gameState = GAME_STATE.NEUTRAL;

                    break;
                }
            case GAME_STATE.PAUSE:
                {
                    pause.Invoke();

                    gameState = GAME_STATE.NEUTRAL;

                    break;
                }
            case GAME_STATE.DEAD:
                {
                    dead.Invoke();

                    gameState = GAME_STATE.NEUTRAL;

                    break;
                }
            case GAME_STATE.RESTART:
                {
                    restart.Invoke();

                    gameState = GAME_STATE.NEUTRAL;

                    break;
                }
            case GAME_STATE.NEUTRAL:
                break;
            default:
                break;
        }
    }
}
