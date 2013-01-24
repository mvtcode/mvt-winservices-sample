using System.ComponentModel;
using System.Configuration.Install;


namespace AutoServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            serviceProcessInstaller1.Password = null;
            serviceProcessInstaller1.Username = null;

            serviceInstaller1.Description = "Tự động post bài lên tường facebook của Mạc Văn Tân";
            serviceInstaller1.ServiceName = "MVT - Auto post facebook";
            serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
        }
    }
}
