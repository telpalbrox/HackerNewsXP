using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;

namespace HackerNews
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private ReceivePostsDelegate receivePostsDelegate;
		private IList items = null;
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
			// JsonParser.tests();
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
			this.items = items;
			for (int i = 0; i < items.Count; i++) 
			{
				HNItem item = (HNItem) items[i];
				LinkLabel itemLabel = new System.Windows.Forms.LinkLabel();
				itemLabel.Text = item.Title;
				itemLabel.Dock = System.Windows.Forms.DockStyle.Top;
				itemLabel.Click += new System.EventHandler(this.onClickItem);
				itemLinksPanel.Controls.Add(itemLabel);
			}
		}

		private void onClickItem(object sender, System.EventArgs args)
		{
			LinkLabel label = (LinkLabel) sender;
			HNItem item = getItemByTitle(label.Text);
			if (item == null)
			{
				return;
			}

			HNItemForm itemForm  = new HNItemForm(item);
			itemForm.Show();
		}

		private HNItem getItemByTitle(String title) 
		{
			if (items == null)
			{
				return null;
			}

			for (int i = 0; i < items.Count; i++)
			{
				HNItem item = (HNItem) items[i];
				if (item.Title.Equals(title))
				{
					return item;
				}
			}

			return null;
		}
	}
}
