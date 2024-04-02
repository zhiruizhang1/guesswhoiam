using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceGenerator : MonoBehaviour
{
    private FaceFactory factory = null;
    private List<GameObject> faces = new List<GameObject>();
    private List<GameObject> cards = new List<GameObject>();

    public GameObject FaceTemplate;
    public GameObject gameCard;

    private void Awake()
    {
        factory = new FaceFactory();

        for (int i = 0; i < 50; i++)
        {
            Face face = factory.GenerateFace();
            var faceObject = generateFace(face);
            var template = faceObject.GetComponent<FaceTemplate>();
            template.faceId = i;
            this.faces.Add(faceObject);

            var card = Instantiate(this.gameCard, this.transform);
            var gameCard = card.GetComponent<GameCard>();
            gameCard.cardId = i;
            this.cards.Add(card);
        }
    }

    private Color getRandomSkinColor()
    {
        Color[] skinColors = new Color[]
        {
            new Color(255.0f / 255, 249.0f / 255, 240.0f / 255),
            new Color(255.0f / 255, 222.0f / 255, 166.0f / 255),
            new Color(171.0f / 255, 123.0f / 255, 41.0f / 255),
            new Color(48.0f / 255, 33.0f / 255, 10.0f / 255)
        };
        var rand = new System.Random();
        int colorIndex = rand.Next(skinColors.Length);
        return skinColors[colorIndex];
    }

    private Color getRandomHairColor()
    {
        Color[] hairColors = new Color[]
        {
            new Color(247.0f / 255.0f, 213.0f / 255.0f, 111.0f / 255.0f),
            new Color(189.0f / 255.0f, 74.0f / 255.0f, 51.0f / 255.0f),
            new Color(139.0f / 255.0f, 189.0f / 255, 25.0f / 255),
            new Color(0.0f / 255, 80.0f / 255, 255.0f / 255),
            new Color(20.0f / 255, 20.0f / 255, 20.0f / 255)
        };
        var rand = new System.Random();
        int colorIndex = rand.Next(hairColors.Length);
        return hairColors[colorIndex];
    }

    private Color getRandomEyesColor()
    {
        Color[] eyesColors = new Color[]
        {
            new Color(189.0f / 255.0f, 74.0f / 255.0f, 51.0f / 255.0f),
            new Color(139.0f / 255.0f, 189.0f / 255, 25.0f / 255),
            new Color(39.0f / 255, 164.0f / 255, 227.0f / 255),
            new Color(0, 0, 0)
        };
        var rand = new System.Random();
        int colorIndex = rand.Next(eyesColors.Length);
        return eyesColors[colorIndex];
    }

    private Color getRandomClothingColor()
    {
        var rand = new System.Random();
        return new Color((float)rand.Next(256) / 255.0f, (float)rand.Next(256) / 255.0f,
            (float)rand.Next(256) / 255.0f);
    }

    private void dyeSkinColor(FaceTemplate template, Color color)
    {
        List<Transform> layerTransforms = new List<Transform>();
        layerTransforms.AddRange(template.Body.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Face.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Nose.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Ears.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Mouth.GetComponentsInChildren<Transform>());
        foreach (Transform layerTransform in layerTransforms)
        {
            if (layerTransform != template.Body.transform &&
                layerTransform != template.Face.transform &&
                layerTransform != template.Nose.transform &&
                layerTransform != template.Ears.transform &&
                layerTransform != template.Mouth.transform)
            {
                SpriteRenderer sprite = layerTransform.gameObject.GetComponent<SpriteRenderer>();
                sprite.color = color;
            }
        }
    }

    private void dyeHairColor(FaceTemplate template, Color color)
    {
        //if (rand.Next(100) > 50)
        //{
        List<Transform> layerTransforms = new List<Transform>();
        layerTransforms.AddRange(template.RearHair1.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.RearHair2.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.FrontHair.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.EyeBrows.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Beard.GetComponentsInChildren<Transform>());
        foreach (Transform layerTransform in layerTransforms)
        {
            if (layerTransform != template.RearHair1.transform &&
                layerTransform != template.RearHair2.transform &&
                layerTransform != template.FrontHair.transform &&
                layerTransform != template.EyeBrows.transform &&
                layerTransform != template.Beard.transform)
            {
                SpriteRenderer sprite = layerTransform.gameObject.GetComponent<SpriteRenderer>();
                sprite.color = color;
            }
        }
        //}
    }

    private void dyeEyesColor(FaceTemplate template, Color color)
    {
        List<Transform> layerTransforms = new List<Transform>();
        layerTransforms.AddRange(template.Eyes.GetComponentsInChildren<Transform>());
        foreach (Transform layerTransform in layerTransforms)
        {
            if (layerTransform != template.Eyes.transform &&
                layerTransform.gameObject.name.Equals("c1"))
            {
                SpriteRenderer sprite = layerTransform.gameObject.GetComponent<SpriteRenderer>();
                sprite.color = color;
            }
        }
    }

    private void dyeClothingColor(FaceTemplate template, Color color)
    {
        List<Transform> layerTransforms = new List<Transform>();
        layerTransforms.AddRange(template.Clothing1.GetComponentsInChildren<Transform>());
        layerTransforms.AddRange(template.Clothing2.GetComponentsInChildren<Transform>());
        foreach (Transform layerTransform in layerTransforms)
        {
            if (layerTransform != template.Clothing1.transform &&
                layerTransform != template.Clothing2.transform)
            {
                SpriteRenderer sprite = layerTransform.gameObject.GetComponent<SpriteRenderer>();
                sprite.color = color;
            }
        }
    }

    private GameObject generateFace(Face face)
    {
        GameObject faceObject = null;

        var rand = new System.Random();
        faceObject = Instantiate(this.FaceTemplate, this.transform);
        Color skinColor = getRandomSkinColor();
        Color hairColor = getRandomHairColor();
        Color eyesColor = getRandomEyesColor();
        Color clothingColor = getRandomClothingColor();

        FaceTemplate template = faceObject.GetComponent<FaceTemplate>();

        setSpriteToPart(template.RearHair2, face.Type, face.RearHair2);
        setSpriteToPart(template.Body, face.Type, face.Body);
        setSpriteToPart(template.Face, face.Type, face.Transform);
        setSpriteToPart(template.RearHair1, face.Type, face.RearHair1);
        setSpriteToPart(template.Eyes, face.Type, face.Eyes);
        setSpriteToPart(template.Ears, face.Type, face.Ears);
        setSpriteToPart(template.EyeBrows, face.Type, face.EyeBrows);
        setSpriteToPart(template.Nose, face.Type, face.Nose);
        setSpriteToPart(template.Mouth, face.Type, face.Mouth);
        setSpriteToPart(template.Glasses, face.Type, face.Glasses, 30);
        setSpriteToPart(template.FrontHair, face.Type, face.FrontHair, 80);
        setSpriteToPart(template.Clothing2, face.Type, face.Clothing2);
        setSpriteToPart(template.Clothing1, face.Type, face.Clothing1, 20);
        setSpriteToPart(template.AccessoryA, face.Type, face.AccessoryA, 20);
        setSpriteToPart(template.AccessoryB, face.Type, face.AccessoryB, 20);

        if (face.Type.Equals("male"))
        {
            setSpriteToPart(template.Beard, face.Type, face.Beard, 40);
        }

        dyeSkinColor(template, skinColor);
        dyeHairColor(template, hairColor);
        dyeEyesColor(template, eyesColor);
        dyeClothingColor(template, clothingColor);

        return faceObject;
    }

    private void setSpriteToPart(GameObject part, string faceType, FaceLayer[] layers, int probability=100)
    {
        var rand = new System.Random();
        if (probability > rand.Next(100))
        {
            Dictionary<string, GameObject> partLayers = new Dictionary<string, GameObject>();
            foreach (Transform ts in part.GetComponentInChildren<Transform>())
            {
                partLayers[ts.gameObject.name] = ts.gameObject;
            }
            foreach (FaceLayer layer in layers)
            {
                SpriteRenderer partSprite = partLayers[layer.LayerName].GetComponent<SpriteRenderer>();
                Texture2D texture = Resources.Load<Texture2D>(string.Format("{0}/{1}", faceType, layer.FileName));
                partSprite.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public List<GameObject> GetFaces()
    {
        return this.faces;
    }

    public List<GameObject> GetCards()
    {
        return this.cards;
    }
}
