using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;



using System.Collections.Generic;

namespace VESHTER
{
    public class HttpRequestState
    {
        // This class stores the State of the request.
        const int BUFFER_SIZE = 1024;
        public StringBuilder RequestData;
        public byte[] BufferRead;
        public HttpWebRequest Request;
        public HttpWebResponse Response;
        public Stream StreamResponse;
        public HttpRequestState()
        {
            BufferRead = new byte[BUFFER_SIZE];
            RequestData = new StringBuilder("");
            Request = null;
            StreamResponse = null;
        }
    }

    class HttpHelper
    {
        //public static ManualResetEvent allDone = new ManualResetEvent(false);
        const int BUFFER_SIZE = 1024;
        const int DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout

        // Abort the request if the timer fires.
        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }

        public HttpHelper(string url)
        {

            try
            {
                // Create a HttpWebrequest object to the desired URL. 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

                myHttpWebRequest.Method = "POST";
                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

                

                /**
                  * If you are behind a firewall and you do not have your browser proxy setup
                  * you need to use the following proxy creation code.

                    // Create a proxy object.
                    WebProxy myProxy = new WebProxy();

                    // Associate a new Uri object to the _wProxy object, using the proxy address
                    // selected by the user.
                    myProxy.Address = new Uri("http://myproxy");


                    // Finally, initialize the Web request object proxy property with the _wProxy
                    // object.
                    myHttpWebRequest.Proxy=myProxy;
                  ***/

                // Create an instance of the RequestState and assign the previous myHttpWebRequest
                // object to its request field.  
                HttpRequestState myRequestState = new HttpRequestState();
                myRequestState.Request = myHttpWebRequest;
                

                // Start the asynchronous request.
                IAsyncResult result =
                  (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), myRequestState);

                // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
                //ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myHttpWebRequest, DefaultTimeout, true);

                // The response came in the allowed time. The work processing will happen in the 
                // callback function.
                //allDone.WaitOne();

                // Release the HttpWebResponse resource.
                //myRequestState.response.Close();
            }
            catch (WebException ex)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", ex.Message);
                Console.WriteLine("\nStatus:{0}", ex.Status);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", ex.Message);
                throw ex;
            }
        }
        private static void ResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                HttpRequestState myRequestState = (HttpRequestState)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.Request;
                myRequestState.Response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.Response.GetResponseStream();
                myRequestState.StreamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                return;
            }
            catch (WebException ex)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", ex.Message);
                Console.WriteLine("\nStatus:{0}", ex.Status);
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nRespCallback Exception raised!");
                Console.WriteLine("\nMessage:{0}", ex.Message);
                throw ex;
            }
            //allDone.Set();
        }
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {

                HttpRequestState myRequestState = (HttpRequestState)asyncResult.AsyncState;
                Stream responseStream = myRequestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    myRequestState.RequestData.Append(Encoding.Unicode.GetString(myRequestState.BufferRead, 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(myRequestState.BufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
                    return;
                }
                else
                {
                    Console.WriteLine("\nThe contents of the Html page are : ");
                    if (myRequestState.RequestData.Length > 1)
                    {
                        string stringContent;
                        stringContent = myRequestState.RequestData.ToString();
                        System.Windows.MessageBox.Show(stringContent);
                    }
                    Console.WriteLine("Press any key to continue..........");
                    Console.ReadLine();

                    responseStream.Close();
                }

            }
            catch (WebException e)
            {
                Console.WriteLine("\nReadCallBack Exception raised!");
                Console.WriteLine("\nMessage:{0}", e.Message);
                Console.WriteLine("\nStatus:{0}", e.Status);
            }
            //allDone.Set();

        }

    }
}