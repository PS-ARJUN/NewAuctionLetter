using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web.Services;
using System.Deployment.Application;
using System.Security.Cryptography;
using System.IO;

namespace NewAuctionLetter.Login
{
    public partial class Login : Form
    {
        GL_AUCTION.Auction_serviceSoapClient obj = new GL_AUCTION.Auction_serviceSoapClient();
        AUD.auditSoapClient obj1 = new AUD.auditSoapClient();
        WindowsHelper.Windows.DataStore WH = new WindowsHelper.Windows.DataStore(); 

        private string BranchID;
        private string BranchName;
        private string[] UsrID;
        private string FirmID;
        private string FirmName;
        private ValidateSession.Session objSession = new ValidateSession.Session();

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Loginfn();
        }

        private void Login_Load(object sender, EventArgs e)
        {

           /// BranchName = "A.O.VALAPAD";
             ///BranchID = "0";

            // string serverip = "10.0.0.111";
            // --------------------------------------------

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                if (ApplicationDeployment.CurrentDeployment.ActivationUri == null || string.IsNullOrEmpty(ApplicationDeployment.CurrentDeployment.ActivationUri.Query))
                {
                    MessageBox.Show("Access Restricted. Please open the application from portal(http://app.manappuram.net/Portal/Login.aspx)", "Letter Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    Application.Exit();
                }
                else
                {
                    string serverip = "app.manappuram.net";
                    // string serverip = "10.0.0.111";
                    string result = objSession.ValidateSession(ApplicationDeployment.CurrentDeployment.ActivationUri.Query, serverip);
                    int branchID = objSession.BranchID;
                    BranchID = branchID.ToString();
                    BranchName = objSession.BranchName;
                    UsrID = objSession.UserId.Split('!');
                    FirmID = objSession.FirmID.ToString();
                    FirmName = objSession.FirmName;
                    // WH.setData("branch_id", BranchID);
                    // WH.setData("branch_name", BranchName);
                    if (result != "Success")
                    {
                        MessageBox.Show(result);
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Access Restricted. Please open the application from portal(http://app.manappuram.net/Portal/Login.aspx)", "Letter Module", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                Application.Exit();
            }
        }

        public void Loginfn()
        {
            try
            {
                DataTable dtt = new DataTable();
                string[] empname;
                int pwd;

                if (string.IsNullOrWhiteSpace(txt_userid.Text))
                {
                    MessageBox.Show("Enter User ID", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_userid.Focus();
                    return;
                }
                if (txt_userid.Text.Length < 5)
                {
                    MessageBox.Show("Invalid User ID", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_userid.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(txt_passwd.Text))
                {
                    MessageBox.Show("Enter Password", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_passwd.Focus();
                    return;
                }
                int chk_cnt = obj1.checkpunch(int.Parse(BranchID), int.Parse(txt_userid.Text));
                if (chk_cnt == 0)
                {
                    MessageBox.Show("You are not Punched Today in this Branch", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_userid.Focus();
                    return;
                }
                DataTable DT2 = obj.Get_auth_user_cnt(int.Parse(txt_userid.Text), int.Parse(BranchID), 2).Tables[0];
                DataTable DT3 = obj.access_form(4030, int.Parse(txt_userid.Text)).Tables[0];

                if (txt_userid.Text == UsrID[0])
               // if (txt_userid.Text == "44250")
                {
                    dtt = obj1.get_usr_dtl(int.Parse(txt_userid.Text)).Tables[0];
                    if (dtt.Rows.Count > 0)
                    {
                        empname = dtt.Rows[0][0].ToString().Split('~');
                        WH.setData("UserID", txt_userid.Text);
                        WH.setData("UserName", empname[1]);
                        WH.setData("BranchID", BranchID);
                        WH.setData("BranchName", BranchName);
                        string decryptedValue = Decrypt(obj.login_check_VAPT(int.Parse(txt_userid.Text), txt_passwd.Text, int.Parse(BranchID)), "m@f1l992");
                        pwd = int.Parse(decryptedValue.Split('~')[0]);
                        if (pwd == 0)
                        {
                            MessageBox.Show("Check User ID or Password", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txt_userid.Focus();
                            return;
                        }
                        else
                        {
                            if (DT2.Rows[0][0].Equals(0)  && DT3.Rows[0][0].Equals(0))
                            {
                                MessageBox.Show("You Are Not Authorised to Access this Module", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            MainMenu form = new MainMenu();
                            form.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Check User ID or Password", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txt_userid.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Login Only Allowed with Portal User ID", "LOGIN", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txt_userid.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        public static string Decrypt(string encryptedText, string pass)
        {
            byte[] rgbIV = { 33, 67, 86, 135, 16, 253, 234, 28 };
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(pass);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write))
                {
                    byte[] inputByteArray = Convert.FromBase64String(encryptedText.Replace(" ", "+"));
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }



}
    

