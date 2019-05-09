namespace Tangle.Net.Zmq
{
  public static class MessageType
  {
    public const string All = "";

    public const string ConfirmedTransaction = "sn";

    public const string IriCacheInformation = "hmr";

    public const string LatestMilestoneIndex = "lmi";

    public const string LatestSolidSubtangleMilestone = "lmsi";

    public const string LatestSolidSubtangleMilestoneTransactionHash = "lmhs";

    public const string NeighborDnsConfirmations = "dnscc";

    public const string NeighborDnsValidations = "dnscv";

    public const string RequestQueueTransactionRemoval = "rtl";

    public const string TipTransactionRequester = "rstat";

    public const string Transactions = "tx";

    public const string TransactionsTraversed = "mctn";

    public const string TransactionTrytes = "tx_trytes";

    public const string UpdateToNeighborsIp = "dnscu";
  }
}