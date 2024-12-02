using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewAuctionLetter.LTV_Letter
{
    public partial class LTV_Letter_Form : Form
    {
        public LTV_Letter_Form()
        {
            InitializeComponent();
        }

        private void LTV_Letter_Form_Load(object sender, EventArgs e)
        {
            GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
            AUD.auditSoapClient obj1 = new AUD.auditSoapClient();

            //GL_AUCTION.Auction_serviceSoap obj = new GL_AUCTION.Auction_serviceSoap();
            DataTable dt = new DataTable();
            string indata = "";
            dt = obj.Auction_procedure_select(indata, 328).Tables[0];
            this.ltr_type.DataSource = dt;
            this.ltr_type.DisplayMember = dt.Columns[1].ColumnName;
            this.ltr_type.ValueMember = dt.Columns[0].ColumnName;
        }

        private void ltr_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
            AUD.auditSoapClient obj1 = new AUD.auditSoapClient();
            DataTable dt = new DataTable();
            string indata = "";
            dt = obj.Auction_procedure_select(indata, 329).Tables[0];
            this.cmb_Language.DataSource = dt;
            this.cmb_Language.DisplayMember = dt.Columns[1].ColumnName;
            this.cmb_Language.ValueMember = dt.Columns[0].ColumnName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.ltr_type.SelectedIndex == 0)
            {
                MessageBox.Show("Select Letter Type!");
                return;
            }
            if (this.cmb_Language.SelectedIndex == 0)
            {
                MessageBox.Show("Select Language!");
                return;
            }
            string type = this.ltr_type.SelectedValue.ToString();
            LTV_Letter_Print form = new LTV_Letter_Print();
            form.lang = this.cmb_Language.Text;
            form.lt_type = type;
            form.Show();
        }
    }
}
