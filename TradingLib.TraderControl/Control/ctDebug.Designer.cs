﻿namespace TradingLib.TraderControl
{
    partial class ctDebug
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._msg = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // _msg
            // 
            this._msg.BackColor = System.Drawing.SystemColors.ControlText;
            this._msg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._msg.Dock = System.Windows.Forms.DockStyle.Fill;
            this._msg.ForeColor = System.Drawing.Color.Lime;
            this._msg.FormattingEnabled = true;
            this._msg.HorizontalScrollbar = true;
            this._msg.ItemHeight = 12;
            this._msg.Location = new System.Drawing.Point(0, 0);
            this._msg.Margin = new System.Windows.Forms.Padding(2);
            this._msg.Name = "_msg";
            this._msg.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this._msg.Size = new System.Drawing.Size(612, 232);
            this._msg.TabIndex = 0;
            // 
            // DebugControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._msg);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DebugControl";
            this.Size = new System.Drawing.Size(612, 232);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox _msg;
    }
}
