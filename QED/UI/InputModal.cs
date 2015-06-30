using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using JCSLA;
namespace QED.UI
{
	/// <summary>
	/// Summary description for InputModal.
	/// </summary>
	public class InputModal : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Panel pan;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		public Hashtable AnswerTable = new Hashtable();
		const int LBL_WIDTH = 120; const int CONTROL_HIGHT = 16; const int TXT_WIDTH = 200;
		const int L_MARGIN = 1; const int HPAD = 1; const int VPAD = 10;
		int _currentY = 1;

		public InputModal(string prompt, params string[] questions)
		{
			InitializeComponent();
			int i = 0;
			if (prompt != "") {
				this.Text = prompt;
			}
			foreach (string askFor in questions) {
				Label lbl = new Label(); lbl.Text = askFor; lbl.Name = "lbl" + prompt;
				TextBox txt = new TextBox(); txt.Name = "txt" +  i++;
				this.AnswerTable.Add(askFor, txt);
				lbl.Size = new Size(LBL_WIDTH, CONTROL_HIGHT);
				txt.Size = new Size(TXT_WIDTH, CONTROL_HIGHT);
				this.AddToPanel(lbl, txt);
			}
		}
		public InputModal(string prompt) {
			InitializeComponent();
			if (prompt != "") {
				this.Text = prompt;
			}
		}
		public InputModal(string prompt, params ComboBox[] cbs) {
			InitializeComponent();
			if (prompt != "") this.Text = prompt;
			foreach (ComboBox cb in cbs) {
				cb.Sorted = true;
				Label lbl = new Label(); lbl.Text = cb.Name; lbl.Name = "lbl" + prompt;				
				this.AnswerTable.Add(cb.Name, cb);
				lbl.Size = new Size(LBL_WIDTH, CONTROL_HIGHT);
				cb.Size = new Size(TXT_WIDTH, CONTROL_HIGHT);
				this.AddToPanel(lbl, cb);
			}
		}
		public string Answer(string question){
			Control ctrl = (Control)this.AnswerTable[question];
			
			if (ctrl.GetType().ToString() == "System.Windows.Forms.TextBox"){
				return ((TextBox)this.AnswerTable[question]).Text.Trim();
			}else{ 
				if (((ComboBox)this.AnswerTable[question]).SelectedItem == null) throw new Exception("Combo box wasn't selected");
				return ((ComboBox)this.AnswerTable[question]).SelectedItem.ToString();
			}
		}
		public void AddToPanel(Label lbl, Control ctrl) {
			_currentY += (CONTROL_HIGHT + VPAD);
			lbl.Location = new Point(L_MARGIN, _currentY); 
			ctrl.Location = new Point(LBL_WIDTH + L_MARGIN + HPAD, _currentY); 
			this.pan.Controls.AddRange(new Control[]{lbl, ctrl});
		}
		public void AddToPanel(string lblText, Control ctrl) {
			Label lbl = new Label();
			lbl.Text = lblText;
			AddToPanel(lbl, ctrl);
		}
		public void AddToPanel(Control ctrl) {
			Label lbl = new Label(); lbl.Text = ctrl.Name; lbl.Name = "lbl" + ctrl.Name;
			this.AnswerTable.Add(ctrl.Name, ctrl);
			lbl.Size = new Size(LBL_WIDTH, CONTROL_HIGHT);
			this.AddToPanel(lbl, ctrl);
		}
		public void AddToPanel(Label lbl) {
			_currentY += (CONTROL_HIGHT + VPAD);
			lbl.Location = new Point(L_MARGIN, _currentY); 
			this.pan.Controls.Add(lbl);
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
			this.pan = new System.Windows.Forms.Panel();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// pan
			// 
			this.pan.AutoScroll = true;
			this.pan.Location = new System.Drawing.Point(8, 0);
			this.pan.Name = "pan";
			this.pan.Size = new System.Drawing.Size(352, 152);
			this.pan.TabIndex = 0;
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(288, 160);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "&Cancel";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(200, 160);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "&OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// InputModal
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(376, 197);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.pan);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "InputModal";
			this.ShowInTaskbar = false;
			this.Text = "InputModal";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e) {
		
		}


	}
}
