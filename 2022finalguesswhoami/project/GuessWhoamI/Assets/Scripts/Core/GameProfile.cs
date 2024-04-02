using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameProfile
{
    private GameProfile()
    {
    }
    public int SelectedFaceIndex { get; set; }
    public bool[] FlippedTable { get; set; }
    public int GuessTimes { get; set; }
    public int TotalRounds { get; set; }

    public static GameProfile CreateNewProfile(int selectedFaceIndex = -1)
    {
        GameProfile profile = new GameProfile();

        profile.SelectedFaceIndex = selectedFaceIndex;
        profile.FlippedTable = new bool[50];

        return profile;
    }
}
