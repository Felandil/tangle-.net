namespace Tangle.Net.Repository.Responses
{
    public class CheckConsistencyResponse
    {
        public int Duration { get; set; }
        public bool State { get; set; }
        public string Info { get; set; }
    }
}