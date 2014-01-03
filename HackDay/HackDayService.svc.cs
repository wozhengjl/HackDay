using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace HackDay
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HackDayService : IService
    {
        private string mDate = "12/12/2013";
        private static Microsoft.WindowsAzure.Storage.CloudStorageAccount GetStorageAccount()
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = null;

            // Retrieve the storage account from the connection string.
            storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(ConfigurationManager.AppSettings["Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"]);
            return storageAccount;
        }

        private CloudTable GetTable(string name)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = GetStorageAccount();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(name);

            table.CreateIfNotExists();

            return table;
        }

        public static CloudQueue GetQueue(string name)
        {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = GetStorageAccount();

            CloudQueueClient client = storageAccount.CreateCloudQueueClient();

            CloudQueue queue = client.GetQueueReference(name);

            queue.CreateIfNotExists();

            return queue;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public IList<string> GetTenantsData(string date)
        {
            var queueClient = GetQueue("latestaccess");

            var messages = queueClient.GetMessages(32);

            if (messages.Count() == 0)
            {
                return new List<string>();
            }
            else
            {
                return messages.Select(c => TenantAccessFormatter(c.AsString)).ToList();
            }
        }

        private string TenantAccessFormatter(string str)
        {
            var parts = str.Split(new string[] { "___" }, StringSplitOptions.None);
            return string.Format("Tenant {0} is accessing {1} Report ... \n", parts[0], parts[1]);
        }

        public IList<CountryItem> GetCountryData(string date)
        {
            date = this.mDate;
            IList<CountryItem> cnData = new List<CountryItem>();

            CloudTable table = GetTable("Location");
            var parts = date.Split(new char[] { '/' });

            string rowKeyPrefix = parts[0] + "_" + parts[1] + "_" + parts[2] + "_";

            string filter1 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyPrefix);
            string filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKeyPrefix + "ZZZ");
            string filter3 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, LocationEntry.Key);
            string filter = TableQuery.CombineFilters(filter1, TableOperators.And, filter2);
            filter = TableQuery.CombineFilters(filter3, TableOperators.And, filter);

            TableQuery<LocationEntry> query = new TableQuery<LocationEntry>().Where(filter);

            foreach (LocationEntry locationEntity in table.ExecuteQuery(query))
            {
                string location = locationEntity.RowKey.Substring(rowKeyPrefix.Length);
                CountryItem locationItem = new CountryItem() { DateHour = "0", Country = location, Count = locationEntity.Count };
                cnData.Add(locationItem);
            }

            return cnData;
        }

        public IList<CompletenessItem> GetDQData(string date, string page)
        {
            date = this.mDate;
            IList<CompletenessItem> dqData = new List<CompletenessItem>();
            List<string> reports = new List<string> { "MailboxUsage", "ConnectionbyClientTypeDaily" };
            CloudTable table = GetTable("reportCompletness");
            var parts = date.Split(new char[] { '/' });
            for (int i = 0; i < reports.Count(); i++)
            {
                string rowKeyPrefix = reports[i] + "_" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_";

                string filter1 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyPrefix + "0");
                string filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKeyPrefix + "9");
                string filter3 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, ReportCompletnessEntry.Key);
                string filter = TableQuery.CombineFilters(filter1, TableOperators.And, filter2);
                filter = TableQuery.CombineFilters(filter3, TableOperators.And, filter);

                TableQuery<ReportCompletnessEntry> query = new TableQuery<ReportCompletnessEntry>().Where(filter);

                foreach (ReportCompletnessEntry compltEntity in table.ExecuteQuery(query))
                {
                    string hour = compltEntity.RowKey.Substring(rowKeyPrefix.Length);
                    CompletenessItem compltItem = new CompletenessItem() { DateHour = hour, Page = reports[i], Completeness = compltEntity.Completness };
                    dqData.Add(compltItem);
                }
            }
            return dqData;
        }

        public IList<TagIDItem> GetTagIDData(string date)
        {
            date = this.mDate;
            List<string> tagIDs = new List<string>{"8553", "8555", "8661", "8819", "7261"};

            IList<TagIDItem> tagIDData = new List<TagIDItem>();

            CloudTable table = GetTable("tagId");
            var parts = date.Split(new char[] { '/' });
            for (int i = 0; i < tagIDs.Count(); i++)
            {
                string rowKeyPrefix = tagIDs[i] + "_" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_";
               
                string filter1 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyPrefix + "0");
                string filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKeyPrefix + "9");
                string filter3 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, TagIdTotalCountEntry.Key);
                string filter = TableQuery.CombineFilters(filter1, TableOperators.And, filter2);
                filter = TableQuery.CombineFilters(filter3, TableOperators.And, filter);
                
                TableQuery<TagIdTotalCountEntry> query = new TableQuery<TagIdTotalCountEntry>().Where(filter);
                
                foreach (TagIdTotalCountEntry tagIDEntity in table.ExecuteQuery(query))
                {
                    string hour = tagIDEntity.RowKey.Substring(rowKeyPrefix.Length);
                    TagIDItem tagIDItem = new TagIDItem() { DateHour = hour, TagID = tagIDs[i], Count = tagIDEntity.Count };
                    tagIDData.Add(tagIDItem);
                }
            }
      
            return tagIDData;
        }

        public IList<PageItem> GetPageData(string date)
        {
            date = this.mDate;
            IList<PageItem> pageData = new List<PageItem>();
            List<string> pages = new List<string> { "MailboxUsage", "ConnectionByClientType" };
            CloudTable table = GetTable("page");
            var parts = date.Split(new char[] { '/' });

            for (int i = 0; i < pages.Count(); i++)
            {
                string rowKeyPrefix = pages[i] + "_" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_";

                string filter1 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, rowKeyPrefix + "0");
                string filter2 = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, rowKeyPrefix + "9");
                string filter3 = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PageTotalCountEntry.Key);
                string filter = TableQuery.CombineFilters(filter1, TableOperators.And, filter2);
                filter = TableQuery.CombineFilters(filter3, TableOperators.And, filter);

                TableQuery<PageTotalCountEntry> query = new TableQuery<PageTotalCountEntry>().Where(filter);

                foreach (PageTotalCountEntry pageEntity in table.ExecuteQuery(query))
                {
                    string hour = pageEntity.RowKey.Substring(rowKeyPrefix.Length);
                    PageItem pageItem = new PageItem() { DateHour = hour, Page = pages[i], Count = pageEntity.Count };
                    pageData.Add(pageItem);
                }
            }

            return pageData; 
        }
    }
}
