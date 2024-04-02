using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameBoard gameBoard;
    public GameRound gameRound;

    private void Awake()
    {
        GameSystem.Instance.Process = GameProcess.INIT;
        GameSystem.Instance.CurrentPlayer = GamePlayers.PLAYER1;
        GameSystem.Instance.Profiles.Add(GamePlayers.PLAYER1, GameProfile.CreateNewProfile());
        GameSystem.Instance.Profiles.Add(GamePlayers.PLAYER2, GameProfile.CreateNewProfile());

        GameSystem.Instance.Process = GameProcess.CHOOSE_FACE;
        NewRound(false);
    }

    private void exchangePlayer()
    {
        if (GameSystem.Instance.CurrentPlayer == GamePlayers.PLAYER1)
        {
            GameSystem.Instance.CurrentPlayer = GamePlayers.PLAYER2;
        }
        else
        {
            GameSystem.Instance.CurrentPlayer = GamePlayers.PLAYER1;
        }
    }

    public void NewRound(bool playerExchange = false)
    {
        if (GameSystem.Instance.Process == GameProcess.CHOOSE_FACE)
        {
            GameSystem.Instance.Profiles[GameSystem.Instance.CurrentPlayer].SelectedFaceIndex =
                this.gameBoard.GetSelectedFaceIndex();
            if (GameSystem.Instance.CurrentPlayer == GamePlayers.PLAYER2)
            {
                GameSystem.Instance.Process = GameProcess.GAMING;
            }
        }
        

        if (playerExchange)
        {
            this.exchangePlayer();
        }

        this.gameBoard.Exit();
        this.gameRound.Enter();
    }

    public void StartGame(bool playerExchange = false)
    {
        if (playerExchange)
        {
            this.exchangePlayer();
        }
        if (GameSystem.Instance.Process == GameProcess.GAMING)
        {
            GameProfile profile = GameSystem.Instance.Profiles[GameSystem.Instance.CurrentPlayer];
            profile.TotalRounds++;
        }
        this.gameRound.Exit();
        this.gameBoard.Enter();
    }

    public void EndGame()
    {
        GameSystem.Instance.Process = GameProcess.GAME_END;
        this.NewRound(false);
    }

    public void ExitGame()
    {
        GameSystem.Instance.Destroy();
        SceneManager.LoadScene("SplashScene");
    }
}
