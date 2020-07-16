using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.IO;
using System.Xml;
using System.Threading;

namespace WindowsApplication1
{
	public class HNItem 
	{
		public String Title;
		public String Description;
		public String Url;
		public String User;

		public HNItem(String title, String description, String url, String user) 
		{
			Title = title;
			Description = description;
			Url = url;
			User = user;
		}
	}

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

	class ItemsEvent : EventArgs 
	{
		public IList items;

		ItemsEvent(IList items)
		{
			this.items = items;
		}
	}


	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private EventHandler onNetworkFinish;
		private ReceivePostsDelegate receivePostsDelegate;
		private System.Windows.Forms.Panel itemLinksPanel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.itemLinksPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// itemLinksPanel
			// 
			this.itemLinksPanel.AutoScroll = true;
			this.itemLinksPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.itemLinksPanel.Name = "itemLinksPanel";
			this.itemLinksPanel.Size = new System.Drawing.Size(456, 302);
			this.itemLinksPanel.TabIndex = 4;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(456, 302);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.itemLinksPanel});
			this.Name = "Form1";
			this.Text = "Hacker News";
			this.Load += new System.EventHandler(this.onLoad);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void sendRequest()
		{
			Thread networkThread = new Thread(new ThreadStart(GetTopItems));
			networkThread.Start();
		}

		private void GetTopItems()
		{
			HNClient client = new HNClient();
			IList items = client.GetTopItems();
			BeginInvoke(receivePostsDelegate, new object[] { items });
		}

		private void onLoad(object sender, System.EventArgs e)
		{
			receivePostsDelegate = new ReceivePostsDelegate(onReceivePosts);
			sendRequest();
		}

		private delegate void ReceivePostsDelegate(IList items);

		private void onReceivePosts(IList items)
		{
			Console.WriteLine(items);
			for (int i = 0; i < items.Count; i++) 
			{
				HNItem item = (HNItem) items[i];
				LinkLabel itemLabel = new System.Windows.Forms.LinkLabel();
				itemLabel.Text = item.Title;
				itemLabel.Dock = System.Windows.Forms.DockStyle.Top;
				itemLinksPanel.Controls.Add(itemLabel);
			}
		}
	}
}
