
using System;
using System.Net;
using System.IO;
using System.Reflection;

using System.Threading;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.RegularExpressions;






namespace BeliefRefinary
{
    class Program
    {

        static void Main(string[] args)
        {

    
                CreateLListener();


        }

   
    public static void CreateLListener(){

        Thread.CurrentThread.Priority = ThreadPriority.Highest;
        Thread.CurrentThread.Name = "WaitForWebRequests";

        HttpListener listener = new HttpListener();
            string WebHost = "http://localhost:80/";
        listener.Prefixes.Add(WebHost);
        listener.Start();
            Console.WriteLine("HTTP interface started "+WebHost);
        while(true){

            //ThreadPool.QueueUserWorkItem(Process, listener.GetContext());  
            Thread t = new Thread(Process); 
            t.Start(listener.GetContext()); 
        
        }
        
    }

    public static void Process(object o){
            Console.WriteLine("HTTP Request");
        Thread.CurrentThread.Name = "Webcall Renderer";
       Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
        var context = o as HttpListenerContext;
      
        HttpListenerRequest request = context.Request;

        HttpListenerResponse response = context.Response;

        string path = context.Request.Url.PathAndQuery;

        byte[] buffer = Encoding.ASCII.GetBytes("IF YOU SEE THIS MESSAGE IN YOUR BROWSER ITS WORKING");

            response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
 
        try{
            
            output.Write(buffer, 0, buffer.Length);
        
        }catch(System.Net.HttpListenerException e){ Console.WriteLine("http output error " + e.ToString()); }
            
            output.Close();

        }
    }
}


