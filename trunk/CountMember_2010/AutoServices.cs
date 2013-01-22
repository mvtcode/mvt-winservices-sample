using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using MailMessage = System.Net.Mail.MailMessage;
using System.Data;
using System.Xml;

namespace CountMember
{
    partial class AutoServices : ServiceBase
    {
        private int iIntervalMinutes;
        private Timer t;
        private DateTime dateOld;// = DateTime.Now;
        //private bool bNew;
        public AutoServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer ReTime = new Timer(5 * 60000);
            if (StartService())
            {
                ReTime.Stop();
                ReTime.Dispose();
                WriteLog("Service Started");
            }
            else
            {
                ReTime.Elapsed += ReFreshRunServices;
                ReTime.AutoReset = true;
                ReTime.Enabled = true;
                ReTime.Start();
            }
        }

        private void ReFreshRunServices(object sender, ElapsedEventArgs e)
        {
            WriteLog("Service ReStart");
            if (StartService())
            {
                ((Timer)sender).Stop();
                ((Timer)sender).Dispose();
                WriteLog("Service Started");
            }
            else
            {
                WriteLog("chờ sau 5 phút Service sẽ khởi động lại.");
            }
        }

        private bool StartService()
        {
            try
            {
                WriteLog("Service Starting...");

                GetSetting();

                t = new Timer(iIntervalMinutes * 60000);
                t.Elapsed += TimerRuning;
                t.AutoReset = true;
                t.Enabled = true;
                t.Start();
                return true;
            }
            catch (Exception ex)
            {
                WriteLog("Lỗi Start Service: " + ex.Message);
                return false;
            }
        }

        protected override void OnStop()
        {
            try
            {
                WriteLog("Service Stoping...");
                t.Stop();
                t.Dispose();
            }
            catch (Exception ex)
            {
                WriteLog("Lỗi Stop Service: " + ex.Message);
            }
        }

        private bool GetSetting()
        {
            //load file setting
            iIntervalMinutes = 1;
            //bNew = true;
            return true;
        }

        private void TimerRuning(object sender, ElapsedEventArgs e)
        {
            //working...
            WriteLog("Server Runing...");

            if (dateOld==null)
            {
                sendemail();
                dateOld = DateTime.Now;
                //bNew = false;
            }
            else
            {
                if (dateOld.Day != DateTime.Now.Day)
                {
                    t.Stop();
                    sendemail();
                    t.Start();
                }
            }
        }

        private void WriteLog(string s)
        {
            //if(!Directory.Exists(Application.StartupPath + "\\log"))
            //{
            //    Directory.CreateDirectory(Application.StartupPath + "\\log");
            //}
            string sLogFile = Application.StartupPath + "\\log\\logfile.txt";
            TextWriter file = new StreamWriter(sLogFile, true);
            file.WriteLine("-> " + DateTime.Now + " : " + s);
            file.Close();
        }

        private void sendemail()
        {
            try
            {
                LichVanNien.Service1SoapClient services = new LichVanNien.Service1SoapClient();
                System.Xml.Linq.XElement obj = services.lichvansu(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                System.Xml.Linq.XNode calenday = obj.FirstNode;//id
                calenday = calenday.NextNode;//day
                calenday = calenday.NextNode;//lunaday
                string sContent = string.Format("Ngày: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//lunamonth
                sContent += string.Format("Tháng: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Năm: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Hoàng đạo: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Ngũ hành: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Sao: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Nên làm: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Không nên làm: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Xuất Hành: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Giờ tốt: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Tuổi xung: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("Tiết: {0}--", ((System.Xml.Linq.XElement) calenday).Value);// +System.Environment.NewLine;
                sContent += "(Send From AutoServices)";

                var smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("macvantan@gmail.com", "******");
                smtp.EnableSsl = true;
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("macvantan@gmail.com");
                //mailMessage.To.Add(new MailAddress("macvantan@gmail.com"));//lyra796pharos@m.facebook.com
                mailMessage.To.Add(new MailAddress("lyra796pharos@m.facebook.com"));
                mailMessage.Subject = sContent;// "test tiêu đề";
                mailMessage.Body = string.Format("Send auto at: {0:dd/MM/yyyy HH:mm:ss}",DateTime.Now);
                mailMessage.IsBodyHtml = true;
                smtp.Send(mailMessage);
                mailMessage.Dispose();
                WriteLog("send email:" + sContent);
            }
            catch (ApplicationException ex)
            {
                WriteLog("Lỗi: " + ex.Message);
                throw;
            }

        }
    }
}
