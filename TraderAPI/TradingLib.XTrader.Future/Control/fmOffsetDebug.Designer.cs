﻿namespace TradingLib.XTrader.Future
{
    partial class fmOffsetDebug
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.posOffsetArgList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "key";
            // 
            // posOffsetArgList
            // 
            this.posOffsetArgList.FormattingEnabled = true;
            this.posOffsetArgList.ItemHeight = 12;
            this.posOffsetArgList.Location = new System.Drawing.Point(53, 13);
            this.posOffsetArgList.Name = "posOffsetArgList";
            this.posOffsetArgList.Size = new System.Drawing.Size(797, 88);
            this.posOffsetArgList.TabIndex = 1;
            // 
            // fmOffsetDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(862, 257);
            this.Controls.Add(this.posOffsetArgList);
            this.Controls.Add(this.label1);
            this.Name = "fmOffsetDebug";
            this.Text = "fmOffsetDebug";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox posOffsetArgList;
    }
}