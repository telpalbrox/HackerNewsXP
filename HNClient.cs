using System;
using System.Collections;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;

namespace HackerNews
{
	public class HNClient 
	{
		private static String URL = "http://cors-anywhere.herokuapp.com/https://hnrss.org/frontpage";

		public IList GetTopItems()
		{
			IList hnItems = new ArrayList();
			WebRequest wReq = WebRequest.Create(HNClient.URL);
			wReq.Headers.Add("X-Requested-With", "Some shity Windows XP app that doesn't support https");
			WebResponse response = wReq.GetResponse();
			Stream stream = response.GetResponseStream();
			TextReader reader = new StreamReader(stream);
			String responseText = reader.ReadToEnd();
			XmlDocument document = new XmlDocument();
			document.LoadXml(responseText);
			XmlNodeList items = document.GetElementsByTagName("item");
			for (int i = 0; i < items.Count; i++) 
			{
				XmlNode item = items.Item(i);
				String title = item.FirstChild.InnerText;
				String description = item.ChildNodes.Item(1).InnerText;
				String url = item.ChildNodes.Item(3).InnerText;
				String user = item.ChildNodes.Item(4).InnerText;
				hnItems.Add(new HNItem(title, description, url, user));
				
			}
			response.Close();
			return hnItems;
		}
	}
}
