using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace HackerNews
{
	/// <summary>
	/// Summary description for HNItemForm.
	/// </summary>
	public class HNItemForm : System.Windows.Forms.Form
	{
		private HNItem item;
		private System.Windows.Forms.Label itemLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public HNItemForm(HNItem item)
		{
			this.item = item;
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.itemLabel.Text = item.Title;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.itemLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// itemLabel
			// 
			this.itemLabel.Location = new System.Drawing.Point(16, 8);
			this.itemLabel.Name = "itemLabel";
			this.itemLabel.TabIndex = 0;
			// 
			// HNItemForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.itemLabel});
			this.Name = "HNItemForm";
			this.Text = "HNItemForm";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
