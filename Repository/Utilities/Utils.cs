using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Repository.Utils
{
    /// <summary>
    /// To know if the request was successfull or not
    /// </summary>
    public enum ResultType { Success, Failure };

    class ResultTypeClass
    {
        ResultType _resultType;
        public ResultTypeClass(ResultType resultType)
        {
            this._resultType = resultType;
        }

        override public string ToString()
        {
            return _resultType.ToString();
        }
    }


    /// <summary>
    /// Describes the result of a Database operation
    /// </summary>
    public class RequestResult
    {        
        public ResultType RequestResultType { get; set; }
        public string RequestResultMessage { get; set; }        

        public RequestResult(ResultType ResultType, string RequestResultMessage)
        {
            this.RequestResultType = ResultType;
            this.RequestResultMessage = RequestResultMessage;
        }

        public object ToJson()
        {
            var requestResult = new
            {
                RequestResultType = this.RequestResultType.ToString(),
                RequestResultMessage = this.RequestResultMessage
            };

            string json = JsonConvert.SerializeObject(requestResult);

            return JsonConvert.DeserializeAnonymousType(json, requestResult);
        }

        
    }

    
}