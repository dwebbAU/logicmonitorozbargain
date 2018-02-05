using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using logicmonitorozbargain.Models;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;


namespace logicmonitorozbargain.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int resultCount = 3)
        {
            WebRequest request = WebRequest.Create("https://www.ozbargain.com.au/deals/feed");
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.  
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.  
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.  
            string responseFromServer = reader.ReadToEnd();
            // Display the content.  
            //Console.WriteLine (responseFromServer);  

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(responseFromServer);
            XmlNodeList elemList = xmlDoc.GetElementsByTagName("title");
            List<string> bargains = new List<string>();
            if(resultCount > elemList.Count){
                resultCount = elemList.Count - 1;
            }
            for (int i = 1; i < resultCount+1; i++)
            {
                bargains.Add((elemList[i].InnerXml).Replace("@","at"));
                Console.WriteLine(elemList[i].InnerXml);
            }

            string[] strBargains = bargains.ToArray();
            ViewBag.bargains = strBargains;

            // Clean up the streams and the response.  
            reader.Close();
            response.Close();

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
