using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Face
{
    // male or female
    public string Type { get; set; }
    public FaceLayer[] Body { get; set; }
    public FaceLayer[] RearHair1 { get; set; }
    public FaceLayer[] RearHair2 { get; set; }
    public FaceLayer[] Transform { get; set; }
    public FaceLayer[] FrontHair { get; set; }
    public FaceLayer[] Eyes { get; set; }
    public FaceLayer[] Ears { get; set; }
    public FaceLayer[] Beard { get; set; }
    public FaceLayer[] EyeBrows { get; set; }
    public FaceLayer[] Nose { get; set; }
    public FaceLayer[] Mouth { get; set; }
    public FaceLayer[] Clothing1 { get; set; }
    public FaceLayer[] Clothing2 { get; set; }
    public FaceLayer[] Glasses { get; set; }
    public FaceLayer[] AccessoryA { get; set; }
    public FaceLayer[] AccessoryB { get; set; }
}

public class FaceLayer
{
    public string LayerName { get; set; }
    public string FileName { get; set; }

    public FaceLayer() { }
    public FaceLayer(string layerName, string fileName)
    {
        LayerName = layerName;
        FileName = fileName;
    }
}
