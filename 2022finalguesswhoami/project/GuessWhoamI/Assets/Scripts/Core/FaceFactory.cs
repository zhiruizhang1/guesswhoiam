using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FaceFactory
{
    private JObject jFaceTypes;

    public FaceFactory()
    {
        loadFaceParts();
    }

    private void loadFaceParts()
    {
        TextAsset faceParts = Resources.Load<TextAsset>("facepart.json");
        jFaceTypes = JObject.Parse(faceParts.text);
    }

    private FaceLayer[] getRandomPartFrom(string faceType, string partName)
    {
        FaceLayer[] layers;

        var rand = new System.Random(DateTime.Now.Millisecond);
        var jFaceType = jFaceTypes.Value<JObject>(faceType);
        var jFacePart = jFaceType.Value<JObject>(partName);
        int partIndex = rand.Next(jFacePart.Count);
        var prop = jFacePart.Properties().ToArray()[partIndex];
        var jLayers = jFacePart.Value<JObject>(prop.Name);
        layers = new FaceLayer[jLayers.Count];
        int layerIndex = 0;
        foreach (JProperty layerProp in jLayers.Properties())
        {
            layers[layerIndex] = new FaceLayer(layerProp.Name, layerProp.Value.ToString());
            layerIndex++;
        }

        return layers;
    }

    public Face GenerateFace()
    {
        // initialize random generator
        var rand = new System.Random(DateTime.Now.Millisecond);

        // create new face
        Face face = new Face();

        // face type: male or female
        int typeIndex = rand.Next(2);
        var faceTypes = new string[] { "male", "female" };
        face.Type = faceTypes[typeIndex];

        face.RearHair2 = getRandomPartFrom(face.Type, "RearHair2");
        face.Body = getRandomPartFrom(face.Type, "Body");
        face.Transform = getRandomPartFrom(face.Type, "Face");
        face.RearHair1 = getRandomPartFrom(face.Type, "RearHair1");
        face.Eyes = getRandomPartFrom(face.Type, "Eyes");
        face.Ears = getRandomPartFrom(face.Type, "Ears");
        face.EyeBrows = getRandomPartFrom(face.Type, "Eyebrows");
        face.Nose = getRandomPartFrom(face.Type, "Nose");
        face.Mouth = getRandomPartFrom(face.Type, "Mouth");
        face.Glasses = getRandomPartFrom(face.Type, "Glasses");
        face.FrontHair = getRandomPartFrom(face.Type, "FrontHair");
        face.Clothing1 = getRandomPartFrom(face.Type, "Clothing1");
        face.Clothing2 = getRandomPartFrom(face.Type, "Clothing2");
        face.AccessoryA = getRandomPartFrom(face.Type, "AccA");
        face.AccessoryB = getRandomPartFrom(face.Type, "AccB");
        if (face.Type.Equals("male"))
        {
            face.Beard = getRandomPartFrom(face.Type, "Beard");
        }

        return face;
    }
}

