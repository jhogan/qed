using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace QED.UI
{
	/// <summary>
	/// Summary description for CurrentTimerManager.
	/// </summary>
	public class CurrentTimerManager : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		frmMain _mainForm;
		const int LBL_WIDTH = 120; const int CONTROL_HIGHT = 24; const int BTN_WIDTH = 104;
		const int L_MARGIN = 1; const int HPAD = 1; const int VPAD = 10;
		const int R_MARGIN = 20;
		int _currentY = 1;
		Business.Times _times;
		public CurrentTimerManager(Business.Times times, frmMain mainForm)
		{
			InitializeComponent();
			_times = times;
			_mainForm = mainForm;
			if (times.Count == 0){
				Label lbl = new Label();
				lbl.Text = "No timers are currently running.";
				lbl.Location = new Point(L_MARGIN, _currentY); 
				lbl.Size = new Size(250, CONTROL_HIGHT);
				this.panel1.Controls.Add(lbl);
			}else{
				foreach (Business.Time time in times){
					AddTimeToPanel(time);
				}
			}
		}
		private void AddTimeToPanel(Business.Time time){
			string ctrlPostFixName = Guid.NewGuid().ToString();
			Label lbl = new Label(); lbl.Name = "lbl" + ctrlPostFixName;

			Button btn = new Button(); btn.Name = "btn" + ctrlPostFixName;
			btn.Image = _mainForm.btnTestTimer.Image;
			btn.ImageAlign = ContentAlignment.MiddleLeft;
			btn.Click += new System.EventHandler(btn_Click);

			lbl.Location = new Point(L_MARGIN, _currentY); 
			btn.Location = new Point(this.panel1.Width - BTN_WIDTH - R_MARGIN , _currentY); 

			lbl.Size = new Size(this.panel1.Width - BTN_WIDTH - R_MARGIN - HPAD, CONTROL_HIGHT);
			btn.Size = new Size(BTN_WIDTH, CONTROL_HIGHT);

			if (time.ForEffort){
				lbl.Text = "Effort: " + time.Effort.ConventionalId;
			}else{
				lbl.Text = "Rollout: " +  time.Rollout.ToString();
			}
			btn.Tag = time;
			time.OnMinuteChange += new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
			_mainForm.UpdateTimerButton(time, btn);
			this.panel1.Controls.AddRange(new Control[]{lbl, btn});
			_currentY += (CONTROL_HIGHT + VPAD);
		}
		private void Time_OnMinuteChange(Business.Time time, EventArgs e){
			foreach(Control ctrl in this.panel1.Controls){
				if (ctrl is Button){
					Button btn = (Button) ctrl;
					if (btn.Tag == time){
						_mainForm.UpdateTimerButton(time, btn);
					}
				}
			}
		}
		private void btn_Click(object sender, EventArgs e){
			Button btn = (Button)sender;
			Business.Time time = (Business.Time) btn.Tag;
			bool cancel = false;
			if (time.IsTimerRunning){
				time.StopTimer();
				_mainForm.SubmitTime(time, ref cancel);
				if (!cancel){
					_mainForm.UnhookTime(time);
					_times.Remove(time);
				}else{
					time.StartTimer();
				}
			}else{
				if (time.ForEffort)
					time = new Business.Time(time.Effort);
				else
					time = new Business.Time(time.Rollout);
				_times.Add(time);
				btn.Tag = time;
				time.StartTimer();
				time.OnMinuteChange += new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
				_mainForm.UnhookTime(time);
			}
			_mainForm.UpdateTimerButton(time, btn);
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Location = new System.Drawing.Point(16, 16);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(448, 208);
			this.panel1.TabIndex = 0;
			// 
			// CurrentTimerManager
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(480, 237);
			this.Controls.Add(this.panel1);
			this.Name = "CurrentTimerManager";
			this.Text = "Current Timer Manager";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.CurrentTimerManager_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		private void CurrentTimerManager_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			foreach(Business.Time time in _times){
				time.OnMinuteChange -= new Business.Time.OnMinuteChangeHandler(Time_OnMinuteChange);
			}
		}


	}
}
