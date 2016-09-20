using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

    public partial class  MyEdit : TextBox
    {
        public MyEdit()
        {
           
        }

        public event EventHandler MyKeyUp=null;
        public event EventHandler MyKeyDown=null;   

        public override bool PreProcessMessage(ref   Message msg)
        {
            if (msg.Msg == 0x100)//WM_KEYDOWN
            {
                if (((Keys)msg.WParam.ToInt32()) == Keys.Up)
                {
                    if (MyKeyUp != null)
                    {
                        MyKeyUp(this,new EventArgs());
                    }

                    return true;
                }
                if (((Keys)msg.WParam.ToInt32()) == Keys.Down)
                {
                    if (MyKeyDown != null)
                    {
                        MyKeyDown(this, new EventArgs());
                    }
                    return true;
                }
            }
            return base.PreProcessMessage(ref  msg);
        }
    }

