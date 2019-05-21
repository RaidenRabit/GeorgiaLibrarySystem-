using System;
using System.Diagnostics;
using System.IO;

namespace Tests.AcceptanceTest
{
    class HostServices
    {
        private Process _websiteIisProcess;
        private Process _wcfIisProcess;
        
        public void StartServer()
        {
            var websitePath = GetApplicationPath("GTLService");
            var wcfServicePath = GetApplicationPath("GtlWebsite");

            KillAllIIS();
            _websiteIisProcess = IISProcess(55400, websitePath);
            _wcfIisProcess = IISProcess(52690, wcfServicePath);

        }

        private Process IISProcess(int iisPort, string path)
        {
            var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            Process iisProcess = new Process();
            iisProcess.StartInfo.FileName = programFiles + @"\IIS Express\iisexpress.exe";
            iisProcess.StartInfo.Arguments = string.Format("/path:{0} /port:{1}", path, iisPort);
            iisProcess.StartInfo.CreateNoWindow = true;
            iisProcess.StartInfo.RedirectStandardOutput = true;
            iisProcess.StartInfo.UseShellExecute = false;
            iisProcess.Start();
            return iisProcess;
        }

        protected virtual string GetApplicationPath(string app)
        {
            var solutionFolder = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory))));
            return Path.Combine(solutionFolder, app);
        }

        private void KillAllIIS()
        {
            Process[] ps = Process.GetProcessesByName("iisexpress");

            foreach (Process p in ps)
            {
                try
                {
                    if (!p.HasExited)
                    {
                        p.Kill();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(String.Format("Unable to kill process {0}, exception: {1}", p.ToString(), ex.ToString()));
                }
            }
        }


        public void Dispose()
        {
            // Ensure IISExpress is stopped
            if (_websiteIisProcess.HasExited == false)
            {
                _websiteIisProcess.Kill();
            }

            if (_wcfIisProcess.HasExited == false)
            {
                _wcfIisProcess.Kill();
            }

        }

    
    }
}
