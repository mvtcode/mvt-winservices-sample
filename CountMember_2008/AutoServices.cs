using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Xml;
using Timer = System.Timers.Timer;

namespace CountMember
{
    partial class AutoServices : ServiceBase
    {
        private string sConnectString;
        private string shostname;
        private string sFrom;
        private string sPassword;
        private int iPort = 25;
        private int HospitalID;
        private int iIntervalMinutes = 1;
        private Timer fTimer;
        private string sTimeSend;
        private SmtpClient smtp;
        private int iRefreshSv = 5;

        public AutoServices()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Timer ReTime = new Timer(iRefreshSv * 60000);
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
                WriteLog(string.Format("chờ sau {0} phút Service sẽ khởi động lại.", iRefreshSv));
            }
        }

        private bool StartService()
        {
            try
            {
                if (false)
                {
                    return true;
                }

                WriteLog("Service Starting...");

                GetSetting();

                fTimer = new Timer(iIntervalMinutes * 60000);
                fTimer.Elapsed += TimerRuning;
                fTimer.AutoReset = true;
                fTimer.Enabled = true;
                fTimer.Start();
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
                if (fTimer != null)
                {
                    fTimer.Stop();
                    fTimer.Dispose();
                }

            }
            catch (Exception ex)
            {
                WriteLog("Lỗi Stop Service: " + ex.Message);
            }
        }

        private bool GetSetting()
        {
            //DataSet ds = new DataSet();
            //try
            //{
            //    XmlReader xmlFile = default(XmlReader);
            //    xmlFile = XmlReader.Create(My.Application.Info.DirectoryPath + "\\ConfigFile.xml", new XmlReaderSettings());
            //    ds.ReadXml(xmlFile);
            //    if ((ds != null))
            //    {
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            sConnectString = StringForNull(ds.Tables[0].Rows[0]["ConnectionString"]);
            //            sLogFile = StringForNull(ds.Tables[0].Rows[0]["LogFile"]);
            //            HospitalID = IntegerForNull(ds.Tables[0].Rows[0]["HospitalID"]);
            //            iIntervalMinutes = IntegerForNull(ds.Tables[0].Rows[0]["IntervalMinutes"]);
            //            sTimeSend = StringForNull(ds.Tables[0].Rows[0]["TimeSend"]);
            //        }
            //    }
            //    return true;
            //}
            //catch (Exception ex)
            //{
            //    WriteLog("Lỗi GetSetting: " + ex.Message);
            //    return false;
            //}
            //finally
            //{
            //    if ((ds != null))
            //    {
            //        ds.Clear();
            //        ds.Dispose();
            //    }
            //}
            return true;
        }

        private void TimerRuning(object sender, ElapsedEventArgs e)
        {
            WriteLog("Server Runing...");
            if (!string.IsNullOrEmpty(sTimeSend))
            {
                //string[] sTime = sTimeSend.Split(":");
                //string iHour = IntegerForNull(sTime[0]);
                //string iMinute = IntegerForNull(sTime[1]);
                //DateTime oTimeNow = DateAndTime.Now;
                //if (iHour == oTimeNow.Hour & iMinute == oTimeNow.Minute)
                //{
                //    clsAutoService obj = new clsAutoService(sConnectString);
                //    DataSet ds = obj.Load_ListEmail();
                //    if ((ds != null))
                //    {
                //        if (ds.Tables[0].Rows.Count > 0)
                //        {
                //            t.Stop();
                //            SmtpClient smtp = new SmtpClient(shostname, iPort);
                //            smtp.Credentials = new System.Net.NetworkCredential(sFrom, sPassword);
                //            smtp.EnableSsl = true;
                //            foreach (DataRow dr in ds.Tables[0].Rows)
                //            {
                //                int iID = IntegerForNull(dr["ID"]);
                //                Load_EmailDetail(iID);
                //            }
                //            smtp = null;
                //            t.Start();
                //        }
                //    }
                //}
            }
        }

        private void WriteLog(string s)
        {
            string sLogFile = Application.StartupPath + "\\" + DateTime.Now.ToString("ddMMyyyy") + "_log.txt";
            TextWriter file = new StreamWriter(sLogFile, true);
            file.WriteLine("-> " + DateTime.Now + " : " + s);
            file.Close();
        }

        private int getFromUrl()
        {
            string url = "http://violympic.vn/";
            Uri uri = new Uri(url);
            String host = uri.Scheme + Uri.SchemeDelimiter + uri.Host;// +":" + uri.Port;
            if (uri.Port != 80) host += uri.Port;
            WebClient client = new WebClient();
            string html = client.DownloadString(url);

            return 0;
        }
    }
}
