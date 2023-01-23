using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SMIS.Models.ViewModel
{
    //DataContract for Serializing Data - required to serve in JSON format
    [DataContract]
    public class DataPoint2
    {
        public DataPoint2(string label, dynamic y)
        {
            this.Label = label;
            this.Y = y;
        }

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";

        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public dynamic Y = null;
    }
}