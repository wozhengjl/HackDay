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
            IList<CountryItem> cnData = new List<CountryItem>()
            {
                new CountryItem() {DateHour = "0", Country = "AL", Count = 1000},
                new CountryItem() {DateHour = "0", Country = "AO", Count = 2000},
                new CountryItem() {DateHour = "0", Country = "AF", Count = 3000},
                new CountryItem() {DateHour = "0", Country = "DZ", Count = 4000},
                new CountryItem() {DateHour = "0", Country = "EE", Count = 2000},
                new CountryItem() {DateHour = "0", Country = "ZA", Count = 4000},
                new CountryItem() {DateHour = "0", Country = "DZ", Count = 6000},
            };
            return cnData;
        }

        public IList<CompletenessItem> GetDQData(string date, string page)
        {
            IList<CompletenessItem> dqData = new List<CompletenessItem>()
            {
                new CompletenessItem() {DateHour = "0", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "0", Page = "ConnectionByClientTypeDaily", Completeness = 0.991},
                new CompletenessItem() {DateHour = "2", Page = "MailboxUsage", Completeness = 0.999},
                new CompletenessItem() {DateHour = "2", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "4", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "4", Page = "ConnectionByClientTypeDaily", Completeness = 1}, 
                new CompletenessItem() {DateHour = "6", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "6", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "8", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "8", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "12", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "12", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "14", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "14", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "16", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "16", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "18", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "18", Page = "ConnectionByClientTypeDaily", Completeness = 1},
                new CompletenessItem() {DateHour = "20", Page = "MailboxUsage", Completeness = 1},
                new CompletenessItem() {DateHour = "20", Page = "ConnectionByClientTypeDaily", Completeness = 1},
            };
            return dqData;
        }

        public IList<TagIDItem> GetTagIDData(string date)
        {
            date = "12/12/2013";
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
            //for (int hour = 0; hour <= 23; hour++)
            //{
            //    for (int i = 0; i < tagIDs.Count(); i++)
            //    
            //        string rowKey = tagIDs[i] + "_" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_" + hour;
            //        // Create a retrieve operation that takes a customer entity.
            //        // 8553, 8555, 8661, 8819， 7261
            //        TableOperation retrieveOperation = TableOperation.Retrieve<TagIdTotalCountEntry>(TagIdTotalCountEntry.Key, rowKey);

            //        // Execute the retrieve operation.
            //        TableResult retrievedResult = table.Execute(retrieveOperation);
            //        if (retrievedResult.Result != null)
            //        {
            //            tagIDEntity = (TagIdTotalCountEntry)retrievedResult.Result;
            //            TagIDItem tagIDItem = new TagIDItem() { DateHour = hour.ToString(), TagID = tagIDs[i], Count = tagIDEntity.Count };
            //            tagIDData.Add(tagIDItem);
            //        }
            //    }
            //}

            return tagIDData;
        }

        public IList<PageItem> GetPageData(string date)
        {
            date = "12/12/2013";
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
            
            //PageTotalCountEntry pageEntity = null;

            //for (int hour = 0; hour <= 23; hour++)
            //{
            //    for (int i = 0; i < pages.Count(); i++)
            //    {
            //        string rowKey = pages[i] + "_" + parts[0] + "_" + parts[1] + "_" + parts[2] + "_" + hour;

            //        TableOperation retrieveOperation = TableOperation.Retrieve<PageTotalCountEntry>(PageTotalCountEntry.Key, rowKey);

            //        // Execute the retrieve operation.
            //        TableResult retrievedResult = table.Execute(retrieveOperation);
            //        if (retrievedResult.Result != null)
            //        {
            //            pageEntity = (PageTotalCountEntry)retrievedResult.Result;
            //            PageItem pageItem = new PageItem() { DateHour = hour.ToString(), Page = pages[i], Count = pageEntity.Count };
            //            pageData.Add(pageItem);
            //        }
            //    }
            //}
            return pageData; 
        }
    }
}
