using System.Collections.Generic;

namespace BallRun3D
{
    [System.Serializable]
    public class PlayerSaveData
    {
        public int levelID;
        public int ballID;
        public int coins;

        public bool isMusicOn;
        public bool isSfxOn;

        public List<int> ballsUnlocked = new List<int>();
   
    }
}