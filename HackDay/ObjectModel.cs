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
}