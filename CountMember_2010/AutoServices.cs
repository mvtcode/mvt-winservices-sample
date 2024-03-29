﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;
using Facebook;
using Timer = System.Timers.Timer;
using MailMessage = System.Net.Mail.MailMessage;
using Sercurity;

namespace AutoServices
{
    partial class AutoServices : ServiceBase
    {
        private int iIntervalMinutes;
        private Timer t;
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
            return true;
        }

        private void TimerRuning(object sender, ElapsedEventArgs e)
        {
            //working...
            WriteLog("Server Runing...");

            if (LoadLastSend())
            {
                t.Stop();
                sendemail();
                t.Start();
            }
        }

        private void WriteLog(string s)
        {
            string sLogFile = Application.StartupPath + "\\log\\" + DateTime.Now.ToString("ddMMyyyy") + "_log.txt";
            TextWriter file = new StreamWriter(sLogFile, true);
            file.WriteLine("-> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " : " + s);
            file.Close();
        }

        private bool LoadLastSend()
        {
            try
            {
                bool b = true;
                string sFile = Application.StartupPath + "\\LastSend.txt";
                StreamReader reader = new StreamReader(sFile, true);
                string s = reader.ReadLine();
                if (s != null)
                {
                    string[] arr = s.Split('-');
                    if (arr != null && arr.Length == 6)
                    {
                        if (Convert.ToInt32(arr[0]) == DateTime.Now.Year && Convert.ToInt32(arr[1]) == DateTime.Now.Month && Convert.ToInt32(arr[2]) == DateTime.Now.Day)
                        {
                            b = false;//ko cho gửi tiếp
                        }
                        else
                        {
                            b = true;//cho gửi tiếp
                        }
                    }
                }
                else
                {
                    b = true;
                }
                reader.Close();
                reader.Dispose();
                return b;
            }
            catch (Exception e)
            {
                WriteLog("Lỗi: " + e.Message);
                return false;//ko gửi
                throw;
            }
        }

        private bool UpdateLoadLastSend()
        {
            try
            {
                bool b = false;
                string sFile = Application.StartupPath + "\\LastSend.txt";
                //if (!File.Exists(sFile))
                //{
                //    File.Create(sFile);
                //}
                TextWriter file = new StreamWriter(sFile, false);
                file.Write(string.Format("{0:yyyy-MM-dd-HH-mm-ss}", DateTime.Now));
                file.Close();
                file.Dispose();
                return b;
            }
            catch (Exception e)
            {
                WriteLog("Lỗi: " + e.Message);
                return false;
                throw;
            }
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
                int[] oLina = lunadate.Solar2Lunar(DateTime.Now);
                string s = "";
                if(oLina!=null)
                {
                    s = string.Format("{0}/{1}/{2}",oLina[0],oLina[1],oLina[2]);
                }
                string sContent = string.Format("-Hôm nay: {0}/{1}/{2} (DL) - {3}(AL).", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, s) + System.Environment.NewLine;
                sContent += string.Format("-Ngày: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//lunamonth
                sContent += string.Format("-Tháng: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Năm: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Hoàng đạo: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Ngũ hành: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Sao: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Nên làm: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Không nên làm: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Xuất Hành: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Giờ tốt: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Tuổi xung: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                calenday = calenday.NextNode;//
                sContent += string.Format("-Tiết: {0}.", ((System.Xml.Linq.XElement)calenday).Value) +System.Environment.NewLine;
                sContent += "(Send From AutoServices)";
                
                var fb = new FacebookClient("AAACxM53nypMBABJdzZC3egIPlY6oZAgPUPQTVXnlMHVjD7tD3WoWLYoLwcPqzcsi7G4dV0Udgmw2dnxOCarNw1MHF54JRQlNyVx8JcuwZDZD");

                // make sure to add event handler for PostCompleted.
                fb.PostCompleted += (o, e) =>
                {
                    if (e.Error != null)
                    {
                        WriteLog("Lỗi: " + e.Error.Message);
                    }
                    else
                    {
                        //dynamic result = e.GetResultData();
                        WriteLog("Message Posted successfully");
                    }
                };

                //dynamic parameters = new ExpandoObject();
                var parameters = new Dictionary<string, object>();
                parameters["message"] = sContent;

                fb.PostAsync("me/feed", parameters);

                //var smtp = new SmtpClient("smtp.gmail.com", 587);
                //smtp.Credentials = new NetworkCredential("macvantan@gmail.com", Encrypt.DecryptConn("7qgZIaEIJKPOSnfkg1n5OA=="));
                //smtp.EnableSsl = true;
                //var mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("macvantan@gmail.com");
                ////mailMessage.To.Add(new MailAddress("macvantan@gmail.com"));//lyra796pharos@m.facebook.com
                //mailMessage.To.Add(new MailAddress("lyra796pharos@m.facebook.com"));
                //mailMessage.Subject = sContent;// "test tiêu đề";
                //mailMessage.Body = string.Format("Send auto at: {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
                //mailMessage.IsBodyHtml = true;
                //smtp.Send(mailMessage);
                //mailMessage.Dispose();
                //WriteLog("send email:" + sContent);

                UpdateLoadLastSend();
            }
            catch (ApplicationException ex)
            {
                WriteLog("Lỗi: " + ex.Message);
                throw;
            }

        }
    }
}
