// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NodeInfo.cs" company="Felandil IT">
//    Copyright (c) 2008 -2017 Felandil IT. All rights reserved.
//  </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Tangle.Net.Source.Entity
{
  /// <summary>
  /// The node info.
  /// </summary>
  public class NodeInfo
  {
    #region Public Properties

    /// <summary>
    /// Gets or sets the app name.
    /// </summary>
    public string AppName { get; set; }

    /// <summary>
    /// Gets or sets the app version.
    /// </summary>
    public string AppVersion { get; set; }

    /// <summary>
    /// Gets or sets the duration.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Gets or sets the jre available processors.
    /// </summary>
    public long JreAvailableProcessors { get; set; }

    /// <summary>
    /// Gets or sets the jre free memory.
    /// </summary>
    public long JreFreeMemory { get; set; }

    /// <summary>
    /// Gets or sets the jre max memory.
    /// </summary>
    public long JreMaxMemory { get; set; }

    /// <summary>
    /// Gets or sets the jre total memory.
    /// </summary>
    public long JreTotalMemory { get; set; }

    /// <summary>
    /// Gets or sets the latest milestone.
    /// </summary>
    public string LatestMilestone { get; set; }

    /// <summary>
    /// Gets or sets the latest milestone index.
    /// </summary>
    public int LatestMilestoneIndex { get; set; }

    /// <summary>
    /// Gets or sets the latest solid subtangle milestone.
    /// </summary>
    public string LatestSolidSubtangleMilestone { get; set; }

    /// <summary>
    /// Gets or sets the latest solid subtangle milestone index.
    /// </summary>
    public int LatestSolidSubtangleMilestoneIndex { get; set; }

    /// <summary>
    /// Gets or sets the neighbors.
    /// </summary>
    public int Neighbors { get; set; }

    /// <summary>
    /// Gets or sets the packets queue size.
    /// </summary>
    public int PacketsQueueSize { get; set; }

    /// <summary>
    /// Gets or sets the time.
    /// </summary>
    public long Time { get; set; }

    /// <summary>
    /// Gets or sets the tips.
    /// </summary>
    public int Tips { get; set; }

    /// <summary>
    /// Gets or sets the transactions to request.
    /// </summary>
    public int TransactionsToRequest { get; set; }

    #endregion
  }
}