/**
 * Stay Awake for Windows
 * Copyright (c) 2017 Paul Scott
 * paul.scott.kildare@live.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Stay_Awake {
	public partial class Form1 : Form {
		[FlagsAttribute]
		public enum EXECUTION_STATE : uint {
			ES_AWAYMODE_REQUIRED = 0x00000040,
			ES_CONTINUOUS = 0x80000000,
			ES_DISPLAY_REQUIRED = 0x00000002,
			ES_SYSTEM_REQUIRED = 0x00000001
			// Legacy flag, should not be used.
			// ES_USER_PRESENT = 0x00000004
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern uint SetThreadExecutionState(EXECUTION_STATE esFlags);

		public Form1() {
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e) {
			timer1.Enabled = true;
			label1.BackColor = Color.LimeGreen;
			label1.Text = "ON";
		}

		private void timer1_Tick(object sender, EventArgs e) {
			SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
		}

		private void button2_Click(object sender, EventArgs e) {
			timer1.Enabled = false;
			SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
			label1.BackColor = Color.DarkRed;
			label1.Text = "OFF";
		}

		private void Form1_Resize(object sender, EventArgs e) {
			if (FormWindowState.Minimized == this.WindowState) {
				notifyIcon1.Visible = true;
				notifyIcon1.ShowBalloonTip(500);
				//this.Hide();
				this.ShowInTaskbar = false;
			}

			else if (FormWindowState.Normal == this.WindowState) {
				notifyIcon1.Visible = false;
			}
		}

		private void notifyIcon1_MouseClick(object sender, MouseEventArgs e) {
			this.WindowState = FormWindowState.Normal;
			this.ShowInTaskbar = true;
			notifyIcon1.Visible = false;
		}
	}
}