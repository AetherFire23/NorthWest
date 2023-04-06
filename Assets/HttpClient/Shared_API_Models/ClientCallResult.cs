//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.HttpClient.Shared_API_Models
//{
//    public class ClientCallResult
//    {
//        // Successes do not concern http client but client-related actions instead.
//        // Therefore all ClientCallResult are successful HTTP requests, but not necessarily successful from the point of view of the client.
//        // For example, if a client tries to add a new friend but it already exists.

//        public bool IsSuccessful { get; set; }
//        public string Message { get; set; }
//        public object Content { get; set; }

//        public static ClientCallResult Success => new ClientCallResult()
//        {
//            IsSuccessful = true,
//            Message = "Success"
//        };

//        public static ClientCallResult Failure => new ClientCallResult()
//        {
//            IsSuccessful = false,
//            Message = "Failed"
//        };
//    }
//}
