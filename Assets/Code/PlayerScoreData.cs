namespace DoodleJump
{
    public class PlayerScoreData
    {
        public string Username { get; set; }
        public int Score { get; set; }

        public PlayerScoreData(string username, int score)
        {
            Username = username;
            Score = score;
        }
    }
}