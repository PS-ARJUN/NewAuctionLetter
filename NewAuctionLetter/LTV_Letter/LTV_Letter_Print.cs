using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace NewAuctionLetter.LTV_Letter
{
    public partial class LTV_Letter_Print : Form
    {
        public string lang { get; set; }
        public string lt_type { get; set; }

        string cust = "";
        public LTV_Letter_Print()
        {
            InitializeComponent();
        }

        private void LTV_Letter_Print_Load(object sender, EventArgs e)
        {
            string language = lang;
            string type = lt_type;
            //string inlang = "";
            //this.reportViewer1.RefreshReport();
            //this.reportViewer1.LocalReport.DataSources.Clear();

            GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
            AUD.auditSoapClient obj1 = new AUD.auditSoapClient();
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();

        string indata = language + "^" + type;

            //dt = obj.Auction_procedure_select(indata, 330).Tables[0];
            dt = obj.Auction_procedure_select(indata, 330).Tables[0];


         
             this.reportViewer1.LocalReport.ReportPath = "LTV_One_Telugu.rdlc";

            //this.reportViewer1.LocalReport.ReportEmbeddedResource = "NewAuctionLetter.LTV_One_Telugu.rdlc";

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.RefreshReport();
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dt));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet3", dt2));
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet2", dt1));
            reportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(SetSubDataSource);
            // Refresh the report
            reportViewer1.RefreshReport();

        }

        public void SetSubDataSource(object sender, SubreportProcessingEventArgs e)
        {
            var val = e.Parameters["cust_id"].Values[0];
            string language = lang;

            DataTable dt1 = new System.Data.DataTable();
            GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
            dt1 = obj.Auction_procedure_select(val, 331).Tables[0];
            ReportDataSource dts = new ReportDataSource("SubSet", dt1);
            e.DataSources.Add(dts);
           

        }
    }
}
