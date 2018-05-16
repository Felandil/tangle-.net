namespace Tangle.Net.Repository.Responses
{
    using System.Collections.Generic;

    public class WereAddressesSpentFromResponse
    {
        public int Duration { get; set; }
        public List<bool> States { get; set; }
    }
}