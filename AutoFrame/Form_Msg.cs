using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFrame
{
    public partial class Form_Msg : Form
    {

        public string Message { get; set; }

        public string Caption { get; set; }

        public Form_Msg()
        {
            InitializeComponent();
        }

        private void Form_Msg_Load(object sender, EventArgs e)
        {
            this.Text = Caption;
            this.label_Msg.Text = Message;
        }

        public static DialogResult ShowMessage(string text,string caption)
        {
            Form_Msg frm = new Form_Msg();

            frm.Message = text;
            frm.Caption = caption;

            return frm.ShowDialog();
        }
    }
}
