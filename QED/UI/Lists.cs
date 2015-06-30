using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace QED
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmLists : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox lstList;
		private System.Windows.Forms.TreeView tvwList;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmLists()
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
			this.lstList = new System.Windows.Forms.ListBox();
			this.tvwList = new System.Windows.Forms.TreeView();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lstList
			// 
			this.lstList.Location = new System.Drawing.Point(352, 48);
			this.lstList.MultiColumn = true;
			this.lstList.Name = "lstList";
			this.lstList.Size = new System.Drawing.Size(528, 537);
			this.lstList.TabIndex = 0;
			// 
			// tvwList
			// 
			this.tvwList.ImageIndex = -1;
			this.tvwList.Location = new System.Drawing.Point(8, 48);
			this.tvwList.Name = "tvwList";
			this.tvwList.SelectedImageIndex = -1;
			this.tvwList.Size = new System.Drawing.Size(336, 536);
			this.tvwList.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(208, 23);
			this.label1.TabIndex = 2;
			this.label1.Text = "List Categories";
			// 
			// frmLists
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(888, 597);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tvwList);
			this.Controls.Add(this.lstList);
			this.Name = "frmLists";
			this.Text = "Lists";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
