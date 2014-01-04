using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace HackDay
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "")]
    public interface IService
    {
        [OperationContract]
        [WebGet]
        string GetData(int value);

        [OperationContract]
        [WebGet]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        [WebGet]
        IList<CompletenessItem> GetDQData(string date, string page);

        [OperationContract]
        [WebGet]
        IList<TagIDItem> GetTagIDData(string date);

        [OperationContract]
        [WebGet]
        IList<PageItem> GetPageData(string date);

        [OperationContract]
        [WebGet]
        IList<string> GetTenantsData(string date);

        [OperationContract]
        [WebGet]
        IList<CountryItem> GetCountryData(string date);

        [OperationContract]
        [WebGet]
        IList<ActorItem> GetActorData();
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }

    [DataContract]
    public class CompletenessItem
    {
        [DataMember]
        public string DateHour
        {
            get;
            set;
        }

        [DataMember]
        public string Page
        {
            get;
            set;
        }

        [DataMember]
        public double Completeness
        {
            get;
            set;
        }
    }

    [DataContract]
    public class TagIDItem
    {
        [DataMember]
        public string DateHour
        {
            get;
            set;
        }

        [DataMember]
        public string TagID
        {
            get;
            set;
        }

        [DataMember]
        public int Count
        {
            get;
            set;
        }
    }

    [DataContract]
    public class PageItem
    {
        [DataMember]
        public string DateHour
        {
            get;
            set;
        }

        [DataMember]
        public string Page
        {
            get;
            set;
        }

        [DataMember]
        public int Count
        {
            get;
            set;
        }
    }

    [DataContract]
    public class CountryItem
    {
        [DataMember]
        public string DateHour
        {
            get;
            set;
        }

        [DataMember]
        public string Country
        {
            get;
            set;
        }

        [DataMember]
        public int Count
        {
            get;
            set;
        }
    }

    public enum ActorState
    {
        /// <summary>
        /// New Actor Thread just created by a Service Role.
        /// It happens when a Service Role is just rolled up or
        /// Previous thread was killed by some reasons
        /// </summary>
        NewBorn = 0, 

        /// <summary>
        /// Actor is taking Queue item assigned to it.
        /// $NOTE: after Actor is assigned, it's never ending until error out
        /// </summary>
        Working = 1,

        /// <summary>
        /// Any unexpected Exceptions causes thread abort
        /// </summary>
        Error = 2,

        /// <summary>
        /// No heatheat more than 30s
        /// </summary>
        TimeOut = 3,

        /// <summary>
        /// Default Sate
        /// </summary>
        Unknown = 4,
    }

    [DataContract]
    public class ActorItem
    {
        [DataMember]
        public string TimeStamp
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public ActorState State
        {
            get;
            set;
        }

        [DataMember]
        public bool? IsSpout
        { 
            get; 
            set; 
        }

        [DataMember]
        public string Key
        {
            get;
            set;
        }
    }
}
