using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRound : MonoBehaviour
{
    public Text roundTitle;
    public Text roundTip;
    public Text player1Label;
    public Text player2Label;
    public Button startButton;
    public Button replayButton;

    public FaceGenerator faceGenerator;

    public void Enter()
    {
        GamePlayers player = GameSystem.Instance.CurrentPlayer;
        this.gameObject.SetActive(true);
        if (GameSystem.Instance.Process == GameProcess.GAME_END)
        {
            this.roundTitle.text = "Game End";
            this.roundTip.text = string.Format("Winner is Player{0}! Congratulation!",
                (int)player);
            player1Label.enabled = true;
            player2Label.enabled = true;
            startButton.image.enabled = false;
            startButton.enabled = false;
            replayButton.image.enabled = true;
            replayButton.enabled = true;

            var faces = this.faceGenerator.GetFaces();

            var canvasScale = player1Label.canvas.transform.localScale;
            float x2 = Screen.width / 2.0f / 100.0f -
                player1Label.rectTransform.offsetMin.x * canvasScale.x / 100.0f +
                1.44f / 2.0f - 240.0f * canvasScale.x / 100.0f;
            float x1 = 0 - x2;
            float y = -1.0f;

            var player1Profile = GameSystem.Instance.Profiles[GamePlayers.PLAYER1];
            var player2Profile = GameSystem.Instance.Profiles[GamePlayers.PLAYER2];
            var player1Face = faces[player1Profile.SelectedFaceIndex];
            var player2Face = faces[player2Profile.SelectedFaceIndex];
            var face1 = Instantiate(player1Face, this.transform);
            var face2 = Instantiate(player2Face, this.transform);
            face1.SetActive(true);
            face2.SetActive(true);
            face1.transform.localPosition = new Vector3(x1, y, 0);
            face2.transform.localPosition = new Vector3(x2, y, 0);
        }
        else
        {
            this.roundTitle.text = string.Format("Player{0} Round",
                (int)player);
        }
    }

    public void Exit()
    {
        this.gameObject.SetActive(false);
    }
}
