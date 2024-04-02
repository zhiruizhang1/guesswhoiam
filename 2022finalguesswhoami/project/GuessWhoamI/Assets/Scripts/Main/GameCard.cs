using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class GameCard : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public int cardId;
    public GameBoard gameBoard;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameBoard.OnFaceClick(this.cardId);
    }

}
