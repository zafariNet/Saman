using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Text.RegularExpressions;

namespace VoipConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            connectToAst();
        }
        private static void connectToAst()
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.16.110"), 5038);
            clientSocket.Connect(serverEndPoint);
            clientSocket.Send(Encoding.ASCII.GetBytes("Action: Login\r\nUsername: hossein\r\nSecret: 123456\r\nActionID: 1\r\n\r\n"));
            int bytesRead = 0;
            int QueueSummaryCompleteCount = 0;
            do
            {
                byte[] buffer = new byte[2048];
                bytesRead = clientSocket.Receive(buffer);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                if (true||response.IndexOf("QueueSummaryComplete") >= 0){
                    QueueSummaryCompleteCount++;
                }
                if (QueueSummaryCompleteCount >= 100 || response.IndexOf("ActionID: 1") >= 0)
                {
                    QueueSummaryCompleteCount = 0;
                    clientSocket.Send(Encoding.ASCII.GetBytes("Action: QueueSummary\r\n"));
                    clientSocket.Send(Encoding.ASCII.GetBytes("ActionID: 5\r\n\r\n"));
                }
                if(response.IndexOf("ActionID: 5")>=0)
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"e:/ami.txt", true))
                {
                    file.WriteLine(response);
                }
                if (response.IndexOf("Queue") >= 0)
                    Console.WriteLine(response);
            } while (bytesRead != 0);
        }

        public static void proccessResponse(string response)
        {
            //IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext("AMIHub");
            //Dictionary<string, string> mapping = new Dictionary<string, string>();
            string[] extens;
            //foreach (KeyValuePair<string, string> pair in mapping)
            //{
            string pair = "7779";
            extens = pair.Split(',');
            //extens = pair.Key.Split(',');
            foreach (string exten in extens)
            {
                if ((response.IndexOf("'Event':'Newstate'") >= 0 || response.IndexOf("'Event':'Newexten'") >= 0 || response.IndexOf("'Event':'Hangup'") >= 0) &&
                    (response.IndexOf("'ConnectedLineNum':'" + exten + "'") >= 0 || response.IndexOf("'CallerIDNum':'" + exten + "'") >= 0 ||
                    response.IndexOf("'Extension':'" + exten + "'") >= 0)
                    )
                {
                    //hubContext.Clients.Client(pair.Value).expandRightPanel(response);
                    // hubContext.Clients.All.expandRightPanel(response);
                    Console.WriteLine(response);
                }
            }
            //}
        }
        private static void process(string str)
        {
            string[] data = { };
            string[] lines = Regex.Split(str, "\r\n\r\n");
            foreach (string line in lines)
            {
                if (line == "")
                    continue;
                string s1 = "";
                string kama = "";
                string[] propertis = Regex.Split(line, "\r\n");
                foreach (string prop in propertis)
                {
                    if (prop == "")
                        continue;
                    int i = prop.IndexOf(":");
                    if (i > 0)
                    {
                        s1 += kama;
                        s1 += "'" + prop.Substring(0, i).Trim() + "'" + ":'" + prop.Substring(i + 1).Trim() + "'";
                        kama = ",";
                    }
                }
                if (s1 == "")
                    continue;
                proccessResponse("{" + s1 + "}");
            }
        }
    }
}
