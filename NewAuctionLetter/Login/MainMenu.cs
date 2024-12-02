using NewAuctionLetter.LTV_Letter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewAuctionLetter.Login
{
    public partial class MainMenu : Form
    {
        GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
        AUD.auditSoapClient obj1 = new AUD.auditSoapClient();

        public MainMenu()
        {
            InitializeComponent();
        }

        private void lTVLetterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LTV_Letter_Form form = new LTV_Letter_Form();
            
            form.Show();
        }
    }
}
