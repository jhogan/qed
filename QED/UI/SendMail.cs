using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using JCSLA;
using QED.DataValidation;
namespace QED.UI
{
	/// <summary>
	/// Summary description for SendMail.
	/// </summary>
	public class SendMail : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFrom;
		private System.Windows.Forms.TextBox txtBody;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtSubject;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtTo;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		string n = System.Environment.NewLine;
		string _smtp = "";
		const bool DEBUG = false;
		bool _force = false;
		public SendMail(string from, string to, string subject, string body, string smtp)
		{
			InitializeComponent();
			this.txtFrom.Text = from; this.txtTo.Text = to; this.txtSubject.Text = subject; this.txtBody.Text = body;
			_smtp = smtp;
		}
		public SendMail(string from, string to, string subject, string body, string smtp, bool force) :this(from,  to, subject, body, smtp){
			_force = force;
			if (force){
				this.ControlBox = false;
			}
		}
		private bool IsValid{
			get{
				return (this.BrokenRules.Count == 0);
			}
		}
		private BrokenRules BrokenRules{
			get{
				BrokenRules ret = new BrokenRules();
				EmailValidator ev = new EmailValidator(this.txtFrom.Text);
				ret.Assert("From address is not valid", !ev.IsValid);
				ev = new EmailValidator(this.txtTo.Text);
				ret.Assert("To address is not valid", !ev.IsValid);
				ret.Assert("Subject is empty", txtSubject.Text.Trim() == "");
				ret.Assert("Body is empty", txtBody.Text.Trim() == "");
				ret.Assert("SMTP server is empty" , _smtp.Trim() == "");
				return ret;
			}
		}
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
			this.btnSend = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFrom = new System.Windows.Forms.TextBox();
			this.txtBody = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtSubject = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtTo = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(456, 360);
			this.btnSend.Name = "btnSend";
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "&Send";
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 23);
			this.label1.TabIndex = 1;
			this.label1.Text = "From";
			// 
			// txtFrom
			// 
			this.txtFrom.Location = new System.Drawing.Point(168, 16);
			this.txtFrom.Name = "txtFrom";
			this.txtFrom.Size = new System.Drawing.Size(360, 20);
			this.txtFrom.TabIndex = 2;
			this.txtFrom.Text = "";
			// 
			// txtBody
			// 
			this.txtBody.Location = new System.Drawing.Point(16, 144);
			this.txtBody.Multiline = true;
			this.txtBody.Name = "txtBody";
			this.txtBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtBody.Size = new System.Drawing.Size(512, 200);
			this.txtBody.TabIndex = 4;
			this.txtBody.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(144, 23);
			this.label2.TabIndex = 3;
			this.label2.Text = "Body";
			// 
			// txtSubject
			// 
			this.txtSubject.Location = new System.Drawing.Point(168, 80);
			this.txtSubject.Name = "txtSubject";
			this.txtSubject.Size = new System.Drawing.Size(360, 20);
			this.txtSubject.TabIndex = 6;
			this.txtSubject.Text = "";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 23);
			this.label3.TabIndex = 5;
			this.label3.Text = "Subject";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(144, 23);
			this.label4.TabIndex = 7;
			this.label4.Text = "To";
			// 
			// txtTo
			// 
			this.txtTo.Location = new System.Drawing.Point(168, 48);
			this.txtTo.Name = "txtTo";
			this.txtTo.Size = new System.Drawing.Size(360, 20);
			this.txtTo.TabIndex = 8;
			this.txtTo.Text = "";
			// 
			// SendMail
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(544, 389);
			this.Controls.Add(this.txtTo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtSubject);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtBody);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtFrom);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSend);
			this.Name = "SendMail";
			this.Text = "SendMail";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSend_Click(object sender, System.EventArgs e) {
			if (this.IsValid){
				if (UI.AskYesNoDefNo(this, "Confirm Send") == DialogResult.Yes){
					System.Web.Mail.SmtpMail.SmtpServer = _smtp;
					if (DEBUG){
						string body = txtBody.Text + n + "DEBUG. Intended for " + this.txtTo.Text;
						System.Web.Mail.SmtpMail.Send(this.txtFrom.Text, "jhogan@mail.com", txtSubject.Text, body);
					}else{
						System.Web.Mail.SmtpMail.Send(this.txtFrom.Text, txtTo.Text, txtSubject.Text, txtBody.Text);
					}
					this.Close();
				}
			}else{
				MessageBox.Show(this, "This email is not configured correctly: " + n + this.BrokenRules.ToString(), "QED");
			}
		}
	}
}
