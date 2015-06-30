using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace QED.UI
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		public System.Windows.Forms.TextBox txtAbout;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public About()
		{
			InitializeComponent();
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
			this.txtAbout = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// txtAbout
			// 
			this.txtAbout.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
			this.txtAbout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.txtAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtAbout.Location = new System.Drawing.Point(8, 16);
			this.txtAbout.Multiline = true;
			this.txtAbout.Name = "txtAbout";
			this.txtAbout.Size = new System.Drawing.Size(280, 136);
			this.txtAbout.TabIndex = 0;
			this.txtAbout.Text = "";
			// 
			// About
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 157);
			this.Controls.Add(this.txtAbout);
			this.Name = "About";
			this.Text = "About QED";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
