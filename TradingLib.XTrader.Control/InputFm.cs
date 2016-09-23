using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public partial class InputFm : Form
{
    public InputFm()
    {
        InitializeComponent();
    }

    private void InputFm_Shown(object sender, EventArgs e)
    {
        name.Focus();
    }

    private void def_ValueChanged(object sender, EventArgs e)
    {
        cur.Value = def.Value;
    }
}
