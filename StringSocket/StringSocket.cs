using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace CustomNetworking
{
    /// <summary>
    /// Delegate used for send callback
    /// </summary>
    public delegate void SendCallback(Exception e, object payload);

    /// <summary>
    /// Delegate used for receive callback
    /// </summary>
    public delegate void ReceiveCallback(String s, Exception e, object payload);

    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchonization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// If we have an open Socket s, we can create a StringSocket by doing
    /// 
    ///    StringSocket ss = new StringSocket(s, new UTF8Encoding());
    /// 
    /// We can write a string to the StringSocket by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameters to the callback are a
    /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
    /// the Exception that caused the send attempt to fail.
    /// 
    /// We can read a string from the StringSocket by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
    /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
    /// it is the requested string (with the newline removed).  If the Exception is non-null, 
    /// it is the Exception that caused the send attempt to fail.

    /// </summary>
    public class StringSocket
    {

        // The socket through which we communicate with the remote client
        private Socket socket;

        // Text that has been received from the client but not yet dealt with
        private string incoming = "";

        // Buffer that will contain incoming bytes and characters
        private byte[] bytesFromSocket = new byte[1024];

        // For synchronizing send and recieve
        private readonly object sendSync = new object();
        private readonly object receiveSync = new object();

        // Bytes that we are actively trying to send, along with the
        private byte[] bytesToSocket = new byte[0];

        //Queues to manage callbacks and what we are sending.
        private ConcurrentQueue<ReceiveRequest> receiveQueue = new ConcurrentQueue<ReceiveRequest>();
        private ConcurrentQueue<SendRequest> sendQueue = new ConcurrentQueue<SendRequest>();

        //Queue to keep track of completed lines
        private ConcurrentQueue<string> lineQueue = new ConcurrentQueue<string>();

        //Counters to keep track of buffer indicies 
        private int sendCount = 0;
        private int receiveCount = 0;

        //To hold the encoding type
        private Encoding encoding;


        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// StringSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public StringSocket(Socket s, Encoding e)
        {
            socket = s;         //Underlying socket
            encoding = e;       //Passed in Encoding type
        }

        //////////// Methods for sending ////////////////////////

        
        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket, or failed in the 
        /// attempt, it invokes the callback.  The parameters to the callback are a
        /// (possibly null) Exception and the payload.  If the Exception is non-null, it is
        /// the Exception that caused the send attempt to fail. 
        /// 
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="callback"></param>
        /// <param name="payload"></param>
        public void BeginSend(String s, SendCallback callback, object payload)
        {
            //Place the Callback on the Queue.
            sendQueue.Enqueue(new SendRequest(s, callback, payload));

            // For the first sendRequest start sending. Otherwise sending is in progress.
            if (sendQueue.Count == 1)
                SendBytes();
        }

        /// <summary>
        /// Attempts to send the entire outgoing string.
        /// This method should not be called unless sendSync has been acquired.
        /// </summary>
        private void SendBytes()
        {
            if (sendQueue.Count > 0)
            {
                //Endcodes the string into bytes to be sent over the socket.
                SendRequest sr;
                sendQueue.TryPeek(out sr);
                bytesToSocket = encoding.GetBytes(sr.line);

                //Begin sending byte array from input string
                socket.BeginSend(bytesToSocket, 0, bytesToSocket.Length, SocketFlags.None, MessageSent, null);
            }
        }


        /// <summary>
        /// Called when a message has been successfully sent
        /// </summary>
        /// <param name="sendResult"></param>
        private void MessageSent(IAsyncResult sendResult)
        {
            //Get the index of the last byte that was sent.
            sendCount = socket.EndSend(sendResult);        

            //Check if we sent everything.
            if (sendCount == bytesToSocket.Length)
            {
                //Call back send request.
                SendRequest sr;
                sendQueue.TryDequeue(out sr);
                ThreadPool.QueueUserWorkItem(x => sr.callback(null, sr.payload));

                SendBytes();        //Process any unfinished send requests
            }

            //Otherwise start sending remaing bytes.
            else
                socket.BeginSend(bytesToSocket, sendCount, bytesToSocket.Length - sendCount, SocketFlags.None, MessageSent, null);
        }


        ///////////// Methods for receiving //////////////////////


        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        ///     
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, or
        /// failed in the attempt, it invokes the callback.  The parameters to the callback are
        /// a (possibly null) string, a (possibly null) Exception, and the payload.  Either the
        /// string or the Exception will be non-null, but nor both.  If the string is non-null, 
        /// it is the requested string (with the newline removed).  If the Exception is non-null, 
        /// it is the Exception that caused the send attempt to fail.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="payload"></param>
        public void BeginReceive(ReceiveCallback callback, object payload)
        {
            //Place the recieve request on the queue.
            receiveQueue.Enqueue(new ReceiveRequest(callback, payload));

            // For the first request start receving bytes otherwise we are already in the process.
            if (receiveQueue.Count == 1)
                ReceiveBytes();
        }

        /// <summary>
        /// Attempts to receive the incoming string.
        /// </summary>
        private void ReceiveBytes()
        {
                // Process callbacks for completed lines ending in a newline.
                while (lineQueue.Count > 0 && receiveQueue.Count > 0)
                {
                    //Get the first completed line.
                    string tempLine;
                    lineQueue.TryDequeue(out tempLine);

                    //Callback the receive request for that line.
                    ReceiveRequest rr; 
                    receiveQueue.TryDequeue(out rr);
                    ThreadPool.QueueUserWorkItem(x => rr.callback(tempLine, null, rr.payload));
                }

                // Process remaining receive request.
                if (receiveQueue.Count > 0)                    
                    socket.BeginReceive(bytesFromSocket, 0, bytesFromSocket.Length, SocketFlags.None, MessageReceived, null);
        }

        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        /// <param name="receiveResult"></param>
        private void MessageReceived(IAsyncResult receiveResult)
        {
            //Set the receive byte array index to the last recieved byte.
            receiveCount = socket.EndReceive(receiveResult);        

            //Get the string starting from index 0 until the last recieved byte.
            incoming += encoding.GetString(bytesFromSocket, 0, receiveCount);

            //Initialize to start of string.
            int begin = 0;

            //Initialize to first index of the new line char,
            //Otherwise if no new line char exist initializes to -1.
            int end = incoming.IndexOf('\n', begin);

            //Process completed lines determined by the new line char.
            while (end >= 0)
            {
                //Enqueue the full line that ends in a new line.
                lineQueue.Enqueue(incoming.Substring(begin, end - begin));

                //Set begin to the next char after new line.
                begin = end + 1;

                //Get index of next new line char.
                end = incoming.IndexOf('\n', begin);
            }

            //Remove completed lines.
            incoming = incoming.Substring(begin);

            //Process remaining receive requests.
            ReceiveBytes();

        }
    }
}