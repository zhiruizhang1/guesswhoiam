using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameBoard : MonoBehaviour
{
    public FaceGenerator generator;
    public Text gameTitle;
    public GameObject facesBoard;
    public Image operationPanel;

    public Button randomButton;
    public Button confirmButton;

    public Button guessButton;
    public Button cancelGuessButton;
    public Button nextRoundButton;

    public Image TipPanel;
    public Image TipWindow;
    public Text TipText;
    public Button confirmFaceButton;

    private GameObject selectedFace = null;
    private int selectedFaceIndex = -1;
    private List<GameObject> faces;
    private List<GameObject> cards;
    private int playerNumber;
    private Vector3 utFaceSize;
    private Rect pxFaceRect;
    private bool isFlipping = false;
    private bool guessMode = false;

    public GameController gameController;
    public SoundCenter soundCenter;

    private void appropriateShowFaces()
    {
        const float BOARD_HEIGHT_IN_SCREEN = 0.88f;
        const float TITLE_HEIGHT_IN_REST = 0.35f;
        const float GAP_UT = 0.072f;

        GameProfile profile = GameSystem.Instance.Profiles[GameSystem.Instance.CurrentPlayer];

        // compute facesBoard width and height
        float pxBaseWidth = Screen.width;
        float pxBaseHeight = Screen.height * BOARD_HEIGHT_IN_SCREEN;
        var utBoardSize = Camera.main.ScreenToWorldPoint(new Vector3(pxBaseWidth, pxBaseHeight, 10));
        utBoardSize *= 2;
        var utScreenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
        utScreenSize *= 2;

        float faceHeight = (utBoardSize.y + GAP_UT) / 10 - GAP_UT;

        if (faceHeight * 5 + GAP_UT * 4 > utScreenSize.x)
        {
            faceHeight = utBoardSize.x / 5;
            utBoardSize.y = faceHeight * 10 + GAP_UT * 9;
        }
        else
        {
            utBoardSize.x = faceHeight * 5 + GAP_UT * 4;
        }

        float faceScale = faceHeight / utFaceSize.y;

        float x = 0.0f - utBoardSize.x / 2 + utFaceSize.x * faceScale / 2;
        float y = utBoardSize.y / 2 - utFaceSize.y / 2 + (utScreenSize.y - utBoardSize.y) / 2 - (utScreenSize.y - utBoardSize.y) * TITLE_HEIGHT_IN_REST;
        int count = 0;

        foreach (var face in faces)
        {
            face.transform.parent = facesBoard.transform;
            face.transform.localPosition = new Vector3(x, y, 0);
            face.transform.localScale = new Vector3(faceScale, faceScale, 1);
            face.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            face.SetActive(!profile.FlippedTable[count]);

            var card = this.cards[count];
            card.transform.parent = facesBoard.transform;
            card.transform.localPosition = new Vector3(x, y, 0);
            card.transform.localScale = new Vector3(faceScale, faceScale, 1);
            card.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            card.SetActive(profile.FlippedTable[count]);
            

            x += (utFaceSize.x * faceScale + GAP_UT);

            count++;
            if (count % 5 == 0)
            {
                y -= utFaceSize.y * faceScale + GAP_UT;
                x = 0.0f - utBoardSize.x / 2 + utFaceSize.x * faceScale / 2;
            }
        }

        var canvasScale = ((RectTransform)operationPanel.canvas.transform).localScale;

        //adjust operation panel
        if (GameSystem.Instance.Process == GameProcess.CHOOSE_FACE)
        {
            this.guessButton.image.enabled = false;
            this.guessButton.enabled = false;
            this.nextRoundButton.image.enabled = false;
            this.nextRoundButton.enabled = false;
            this.randomButton.image.enabled = true;
            this.randomButton.enabled = true;
            this.confirmButton.image.enabled = true;
            this.confirmButton.enabled = true;
        }
        else if (GameSystem.Instance.Process == GameProcess.GAMING)
        {
            this.randomButton.image.enabled = false;
            this.randomButton.enabled = false;
            this.confirmButton.image.enabled = false;
            this.confirmButton.enabled = false;
            this.guessButton.image.enabled = true;
            this.guessButton.enabled = true;
            this.nextRoundButton.image.enabled = true;
            this.nextRoundButton.enabled = true;

            if (null != this.selectedFace)
            {
                this.selectedFace.transform.parent = null;
                Destroy(selectedFace);
                this.selectedFace = null;
            }
            this.selectedFace = Instantiate(this.faces[profile.SelectedFaceIndex], this.transform);
            this.selectedFace.SetActive(true);
            this.selectedFaceIndex = profile.SelectedFaceIndex;
        }

        //operationPanel.rectTransform.offsetMin = new Vector2(Screen.width - pxOperationPanelWidth, 0);

        this.randomButton.image.rectTransform.offsetMin = new Vector2(GAP_UT * 100, GAP_UT * 100);
        this.confirmButton.image.rectTransform.offsetMin = new Vector2(
            this.confirmButton.image.rectTransform.offsetMin.x, GAP_UT * 100);
        this.confirmButton.image.rectTransform.offsetMax = new Vector2(0.0f - GAP_UT * 100, 
            this.confirmButton.image.rectTransform.offsetMax.y);

        if (null != this.selectedFace)
        {
            this.selectedFace.GetComponent<FaceTemplate>().Selector.SetActive(false);
            this.selectedFace.GetComponent<FaceTemplate>().Border.SetActive(false);
            var sfy = 0.0f - utScreenSize.y / 2 + GAP_UT * 2 + utFaceSize.y * faceScale / 2;
            var sfx = 0.0f - utScreenSize.x / 2 + GAP_UT * 2 + utFaceSize.x * faceScale / 2;
            this.selectedFace.transform.localPosition = new Vector3(sfx, sfy, 1);
        }

        // update confirm button status
        this.confirmButton.interactable = null != this.selectedFace;
    }

    private void removeSelectedFace()
    {
        this.selectedFaceIndex = -1;
        if (null != this.selectedFace)
        {
            this.selectedFace.transform.parent = null;
            Destroy(selectedFace);
            this.selectedFace = null;
        }
        this.selectFace(this.selectedFaceIndex);
    }

    private void Awake()
    {
        faces = generator.GetFaces();
        cards = generator.GetCards();

        var schema = faces[0].GetComponent<FaceTemplate>();
        var bodySprite = schema.Body.GetComponentInChildren<SpriteRenderer>();
        this.utFaceSize = bodySprite.bounds.size;
        this.pxFaceRect = bodySprite.sprite.rect;
    }

    private void OnEnable()
    {
        this.CloseTipPanel();
    }

    private void selectFace(int faceIndex)
    {
        this.faces.ForEach(f => f.GetComponent<FaceTemplate>().Selector.SetActive(false));

        if (faceIndex >= 0)
        {
            var face = this.faces[faceIndex];
            face.GetComponent<FaceTemplate>().Selector.SetActive(true);
            this.selectedFace = Instantiate(face, this.transform);
        }

        this.selectedFaceIndex = faceIndex;
        this.appropriateShowFaces();
    }

    private void flipFace(int faceIndex)
    {
        if (faceIndex >= 0)
        {
            this.isFlipping = true;

            var face = this.faces[faceIndex];
            var card = this.cards[faceIndex];
            float firstAngle = 0.0f;
            float secondAngle = -90.0f;
            float thirdAngle = -270.0f;
            float forthAngle = -360.0f;

            GameProfile profile = GameSystem.Instance.Profiles[GameSystem.Instance.CurrentPlayer];

            if (profile.FlippedTable[faceIndex])
            {
                var temp = card;
                card = face;
                face = temp;

                firstAngle = -360.0f;
                secondAngle = -270.0f;
                thirdAngle = -90.0f;
                forthAngle = 0.0f;
            }

            Sequence flipSequence = DOTween.Sequence();

            face.transform.eulerAngles = new Vector3(0.0f, firstAngle, 0.0f);
            flipSequence.Append(face.transform.DORotate(new Vector3(0, secondAngle, 0), 0.1f));
            flipSequence.AppendCallback(() =>
            {
                face.SetActive(false);
                card.SetActive(true);
                card.transform.eulerAngles = new Vector3(0.0f, thirdAngle, 0.0f);
            });
            flipSequence.Append(card.transform.DORotate(new Vector3(0, forthAngle, 0), 0.1f));
            flipSequence.AppendCallback(() =>
            {
                profile.FlippedTable[faceIndex] = !profile.FlippedTable[faceIndex];
                this.isFlipping = false;
            });

            flipSequence.Play();
            
        }
    }

    public int GetSelectedFaceIndex()
    {
        return this.selectedFaceIndex;
    }

    public void chooseRandomFace()
    {
        this.removeSelectedFace();
        var rand = new System.Random();
        var faceIndex = rand.Next(this.faces.Count);
        this.selectFace(faceIndex);
    }

    public void Enter()
    {
        this.gameObject.SetActive(true);
        this.guessButton.interactable = true;
        GamePlayers player = GameSystem.Instance.CurrentPlayer;
        GameProfile profile = GameSystem.Instance.Profiles[player];
        switch (GameSystem.Instance.Process)
        {
            case GameProcess.CHOOSE_FACE:
                gameTitle.text = string.Format("Player{0} Choose a Face.", (int)player);
                break;
            case GameProcess.GAMING:
                gameTitle.text = string.Format("Player{0} Game Round {1}", (int)player, profile.TotalRounds);
                break;
            default:
                break;
        }
        this.removeSelectedFace();
    }

    public void Exit()
    {
        this.gameObject.SetActive(false);
    }

    public void OpenTipPanel()
    {
        this.TipPanel.enabled = true;
        this.TipWindow.enabled = true;
        this.TipText.enabled = true;
        this.confirmFaceButton.enabled = true;
        this.confirmFaceButton.image.enabled = true;
    }

    public void CloseTipPanel()
    {
        this.confirmFaceButton.image.enabled = false;
        this.confirmFaceButton.enabled = false;
        this.TipText.enabled = false;
        this.TipWindow.enabled = false;
        this.TipPanel.enabled = false;
    }

    public void OnFaceClick(int faceIndex)
    {
        if (GameSystem.Instance.Process == GameProcess.CHOOSE_FACE)
        {
            this.removeSelectedFace();
            this.soundCenter.Confirm.Play();
            this.selectFace(faceIndex);
        }
        else if (GameSystem.Instance.Process == GameProcess.GAMING)
        {
            if (!isFlipping)
            {
                if (this.guessMode)
                {
                    GamePlayers anotherPlayer = GamePlayers.PLAYER1;
                    if (anotherPlayer == GameSystem.Instance.CurrentPlayer)
                    {
                        anotherPlayer = GamePlayers.PLAYER2;
                    }
                    GameProfile profile = GameSystem.Instance.Profiles[anotherPlayer];
                    if (faceIndex == profile.SelectedFaceIndex)
                    {
                        //Game end
                        this.soundCenter.Correct.Play();
                        this.gameController.EndGame();
                    }
                    else
                    {
                        this.soundCenter.Wrong.Play();
                        this.flipFace(faceIndex);
                        this.OnCancelGuessButtonClick();
                        this.guessButton.interactable = false;
                    }
                }
                else
                {
                    this.soundCenter.Confirm.Play();
                    this.flipFace(faceIndex);
                }
            }
        }
        
    }

    public void OnGuessButtonClick()
    {
        this.guessButton.image.enabled = false;
        this.guessButton.enabled = false;
        this.cancelGuessButton.image.enabled = true;
        this.cancelGuessButton.enabled = true;
        this.nextRoundButton.interactable = false;

        this.guessMode = true;
    }

    public void OnCancelGuessButtonClick()
    {
        this.cancelGuessButton.image.enabled = false;
        this.cancelGuessButton.enabled = false;
        this.guessButton.image.enabled = true;
        this.guessButton.enabled = true;
        this.nextRoundButton.interactable = true;

        this.guessMode = false;
    }

}
