using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameSystem
{
    private GameSystem() { }
    private static GameSystem instance = null;
    public static GameSystem Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameSystem();
                instance.initialize();
            }
            return instance;
        }
    }
    private void initialize()
    {
        this.Process = GameProcess.INIT;
        this.Profiles = new Dictionary<GamePlayers, GameProfile>();
    }
    public GameProcess Process { get; set; }
    public GamePlayers CurrentPlayer { get; set; }
    public Dictionary<GamePlayers, GameProfile> Profiles { get; set; }
    public void Destroy()
    {
        GameSystem.instance = null;
    }
}
