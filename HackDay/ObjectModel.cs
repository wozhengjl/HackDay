using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace HackDay
{
    public class TagIdTotalCountEntry : TableEntity
    {
        /// <summary>
        /// Suppose Topology Table is pretty small, so it's not necessary to Partition
        /// </summary>
        public const string Key = "TagId";

        public TagIdTotalCountEntry()
        {
            this.PartitionKey = TagIdTotalCountEntry.Key;
        }

        public string Name { get; set; }

        public int Count { get; set; }

        public string Bolt { get; set; }
    }

    public class PageTotalCountEntry : TableEntity
    {
        /// <summary>
        /// Suppose Topology Table is pretty small, so it's not necessary to Partition
        /// </summary>
        public const string Key = "Page";

        public PageTotalCountEntry()
        {
            this.PartitionKey = PageTotalCountEntry.Key;
        }

        public string Name { get; set; }

        public int Count { get; set; }

        public string Bolt { get; set; }
    }
    /// <summary>
    /// ActorEntity
    /// </summary>
    public class ReportCompletnessEntry : TableEntity
    {
        /// <summary>
        /// Suppose Topology Table is pretty small, so it's not necessary to Partition
        /// </summary>
        public const string Key = "ReportName";

        public ReportCompletnessEntry()
        {
            this.PartitionKey = ReportCompletnessEntry.Key;
        }

        public string Name { get; set; }

        public int RequestCount { get; set; }

        public double TotalCompletness { get; set; }

        public double Completness { get; set; }

        public string Bolt { get; set; }
    }

    /// <summary>
    /// ActorEntity
    /// </summary>
    public class LocationEntry : TableEntity
    {
        /// <summary>
        /// Suppose Topology Table is pretty small, so it's not necessary to Partition
        /// </summary>
        public const string Key = "Location";

        public LocationEntry()
        {
            this.PartitionKey = LocationEntry.Key;
        }

        public string Name { get; set; }

        public int Count { get; set; }

        public string Bolt { get; set; }
    }

    /// <summary>
    /// ActorEntity
    /// </summary>
    public class ActorAssignment : TableEntity
    {
        /// <summary>
        /// Suppose Topology Table is pretty small, so it's not necessary to Partition
        /// </summary>
        public const string Key = "Actor";

        public ActorAssignment(Guid actorId)
        {
            this.PartitionKey = ActorAssignment.Key;
            this.RowKey = actorId.ToString();
            this.ETag = "*";
        }

        public ActorAssignment()
        {
            this.PartitionKey = ActorAssignment.Key;
        }

        public string State { get; set; }

        public string Name { get; set; }

        public string Topology { get; set; }

        public string InQueue { get; set; }

        public string OutQueues { get; set; }

        public string SchemaGroupingMode { get; set; }

        public string GroupingField { get; set; }

        public bool? IsSpout { get; set; }

        public string Operation { get; set; }

        public DateTime HeartBeat { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorStack { get; set; }
    }
}