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
}