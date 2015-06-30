using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using QED.Business;
namespace QED.UI
{
	/// <summary>
	/// Summary description for Time.
	/// </summary>
	public class Time : System.Windows.Forms.Form {
		private System.Windows.Forms.Label lblTimeFor;
		private System.Windows.Forms.ListView lv;
		private System.Windows.Forms.MenuItem mnuItemTimeAdd;
		private System.Windows.Forms.MenuItem mnuItemTimeDelete;
		private System.Windows.Forms.ColumnHeader colMin;
		private System.Windows.Forms.ColumnHeader colDate;
		private System.Windows.Forms.ColumnHeader colComment;
		private System.Windows.Forms.ColumnHeader colUser;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		Effort _eff; Rollout _roll;
		Times _times;
		private System.Windows.Forms.ContextMenu mnuTime;

		Business.Time _time;
		public Time(Effort eff) {
			InitializeComponent();
			_eff = eff;
			this.lblTimeFor.Text = "Time data for effort: " + _eff.ConventionalId;
			_times = _eff.Times;
			this.UpdateLv();
		}
		public Time(Rollout roll) {
			InitializeComponent();
			_roll = roll;
			this.lblTimeFor.Text = "Time data for rollout: " + _roll.Id.ToString() + " Client: " + _roll.Client.Name + " Scheduled Date: " + _roll.ScheduledDate.ToShortDateString();
			_times = roll.Times;
			this.UpdateLv();
		}
		private void UpdateLv(){
			ListViewItem lvi;
			lv.Items.Clear();
			foreach (Business.Time time in _times){
				lvi = new ListViewItem(new string[]{time.Minutes.ToString(), time.Date.ToShortDateString(), time.User, time.Text});
				lvi.Tag = time;
				lv.Items.Add(lvi);
			}

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
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
		private void InitializeComponent() {
			this.lblTimeFor = new System.Windows.Forms.Label();
			this.lv = new System.Windows.Forms.ListView();
			this.colMin = new System.Windows.Forms.ColumnHeader();
			this.colDate = new System.Windows.Forms.ColumnHeader();
			this.colUser = new System.Windows.Forms.ColumnHeader();
			this.colComment = new System.Windows.Forms.ColumnHeader();
			this.mnuTime = new System.Windows.Forms.ContextMenu();
			this.mnuItemTimeAdd = new System.Windows.Forms.MenuItem();
			this.mnuItemTimeDelete = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// lblTimeFor
			// 
			this.lblTimeFor.Location = new System.Drawing.Point(8, 8);
			this.lblTimeFor.Name = "lblTimeFor";
			this.lblTimeFor.Size = new System.Drawing.Size(712, 16);
			this.lblTimeFor.TabIndex = 2;
			// 
			// lv
			// 
			this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																				 this.colMin,
																				 this.colDate,
																				 this.colUser,
																				 this.colComment});
			this.lv.FullRowSelect = true;
			this.lv.GridLines = true;
			this.lv.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lv.Location = new System.Drawing.Point(8, 32);
			this.lv.Name = "lv";
			this.lv.Size = new System.Drawing.Size(720, 328);
			this.lv.TabIndex = 3;
			this.lv.View = System.Windows.Forms.View.Details;
			this.lv.DoubleClick += new System.EventHandler(this.lv_DoubleClick);
			this.lv.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lv_MouseUp);
			// 
			// colMin
			// 
			this.colMin.Text = "Minutes";
			this.colMin.Width = 70;
			// 
			// colDate
			// 
			this.colDate.Text = "Date";
			this.colDate.Width = 95;
			// 
			// colUser
			// 
			this.colUser.Text = "User";
			this.colUser.Width = 141;
			// 
			// colComment
			// 
			this.colComment.Text = "Comment";
			this.colComment.Width = 513;
			// 
			// mnuTime
			// 
			this.mnuTime.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.mnuItemTimeAdd,
																					this.mnuItemTimeDelete});
			// 
			// mnuItemTimeAdd
			// 
			this.mnuItemTimeAdd.Index = 0;
			this.mnuItemTimeAdd.Text = "&Add";
			this.mnuItemTimeAdd.Click += new System.EventHandler(this.mnuItemTimeAdd_Click);
			// 
			// mnuItemTimeDelete
			// 
			this.mnuItemTimeDelete.Index = 1;
			this.mnuItemTimeDelete.Text = "&Delete";
			this.mnuItemTimeDelete.Click += new System.EventHandler(this.mnuItemTimeDelete_Click);
			// 
			// Time
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(760, 373);
			this.Controls.Add(this.lv);
			this.Controls.Add(this.lblTimeFor);
			this.Name = "Time";
			this.Text = "Time";
			this.ResumeLayout(false);

		}
		#endregion

		private void lv_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (e.Button == MouseButtons.Right){
				if (lv.SelectedItems != null){
					mnuItemTimeAdd.Enabled = true;
					mnuItemTimeDelete.Enabled = true;
				}else{
					mnuItemTimeAdd.Enabled = true;
					mnuItemTimeDelete.Enabled = false;
				}
				mnuTime.Show(lv, new Point(e.Y, e.Y));
			}
		}

		private void mnuItemTimeAdd_Click(object sender, System.EventArgs e) {
			try{
				InputModal im = new InputModal("", "Minutes", "Date", "User", "Comments");
				((TextBox)im.AnswerTable["User"]).Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;
				if (im.ShowDialog(this) == DialogResult.OK){
					_time = new Business.Time();
					_time.Minutes = Int32.Parse(im.Answer("Minutes")); 
					_time.Date = DateTime.Parse(im.Answer("Date").Trim());
					_time.User = im.Answer("User"); 
					_time.Text = im.Answer("Comments"); 
					if (_eff !=null){
						_eff.Times.Add(_time);
						_time.Effort = _eff;
					}else{
						_roll.Times.Add(_time);
						_time.Rollout = _roll;
					}
					
					if (_time.IsValid){
						_time.Update();
						ListViewItem lvi = new ListViewItem(new string[]{_time.Minutes.ToString(), _time.Date.ToShortDateString(), _time.User, _time.Text});
						lvi.Tag = _time;
						lv.Items.Add(lvi);
					}else{
						MessageBox.Show(this, _time.BrokenRules.ToString(), "QED");
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
		private void mnuItemTimeDelete_Click(object sender, System.EventArgs e) {
			if (lv.SelectedItems[0] != null){
				Business.Time _time = (Business.Time) lv.SelectedItems[0].Tag;
				if (UI.AskYesNoDefNo(this, "Confirm Delete") == DialogResult.Yes){
					_time.Delete();
					lv.Items.Remove(lv.SelectedItems[0]);
				}
			}
		}
		private void lv_DoubleClick(object sender, System.EventArgs e) {
			try{
				if (lv.SelectedItems[0] != null){
					Business.Time _time = (Business.Time) lv.SelectedItems[0].Tag;
					InputModal im = new InputModal("", "Minutes", "Date", "User", "Comments");
					((TextBox)im.AnswerTable["Minutes"]).Text = _time.Minutes.ToString();
					((TextBox)im.AnswerTable["Date"]).Text = _time.Date.ToShortDateString();
					((TextBox)im.AnswerTable["User"]).Text = _time.User;
					((TextBox)im.AnswerTable["Comments"]).Text = _time.Text;
					if (im.ShowDialog(this) == DialogResult.OK){
						_time.Minutes = Int32.Parse(((TextBox)im.AnswerTable["Minutes"]).Text);
						_time.Date = DateTime.Parse(((TextBox)im.AnswerTable["Date"]).Text); 
						_time.User = ((TextBox)im.AnswerTable["User"]).Text; 
						_time.Text = ((TextBox)im.AnswerTable["Comments"]).Text;
						if (_time.IsValid){
							_time.Update();
							this.UpdateLv();
						}else{
							MessageBox.Show(this, _time.BrokenRules.ToString(), "QED");
						}
					}
				}
			}
			catch(Exception ex){
				MessageBox.Show(this, ex.Message, "QED");
			}
		}
	}
}
