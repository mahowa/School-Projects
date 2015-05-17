using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

namespace CustomNetworking
{
    
   
    /// <summary>
    /// Class used to storing instances of reqeusts when BeginReceive is called
    /// </summary>
    public class ReceiveRequest
    {
        // The delagate which will be called on the when the receive socket is finished
        public ReceiveCallback callback { get; set; }

        // Holds any generic object, which can be used at the users discretion 
        public object payload { get; set; }

        /// <summary>
        /// Simple constructor for creating new ReceiveRequest objects.
        /// </summary>
        /// <param name="_callback"></param>
        /// <param name="_payload"></param>
        public ReceiveRequest (ReceiveCallback _callback, object _payload)
        {
            callback = _callback;
            payload = _payload;
        }
    }


    /// <summary>
    /// Class used for storing instances of requests when BeginSend is called.
    /// </summary>
    public class SendRequest
    {
        // The string which is being sent over the socket.
        public string line {get; set;}

        //The delagate which will be called when the socket is finished sending
        public SendCallback  callback {get; set;}

        // Holds any generic object, which can be used at the users discretion 
        public object payload {get; set;}

        /// <summary>
        /// Simple constructor for creating new SendRequest objects.
        /// </summary>
        /// <param name="_line"></param>
        /// <param name="_callback"></param>
        /// <param name="_payload"></param>
        public SendRequest( string _line, SendCallback _callback, object _payload)
        {
            line = _line;
            callback = _callback;
            payload = _payload;
        }



    }
}
