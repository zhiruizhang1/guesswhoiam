using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class FaceTemplate : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public int faceId;
    public GameObject Body;
    public GameObject Face;
    public GameObject RearHair1;
    public GameObject RearHair2;
    public GameObject FrontHair;
    public GameObject Eyes;
    public GameObject EyeBrows;
    public GameObject Ears;
    public GameObject Nose;
    public GameObject Mouth;
    public GameObject Glasses;
    public GameObject AccessoryA;
    public GameObject AccessoryB;
    public GameObject Clothing1;
    public GameObject Clothing2;
    public GameObject Beard;

    public GameObject Selector;
    public GameObject Border;

    public GameBoard gameBoard;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.gameBoard.OnFaceClick(this.faceId);
    }

}
