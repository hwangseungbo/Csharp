using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;

namespace Observer
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /* 관리자 권한 상승을 위한 코드 */
            //if (IsAdministrator() == false)
            //{
            //    try
            //    {
            //        ProcessStartInfo procinfo = new ProcessStartInfo();
            //        procinfo.UseShellExecute = true;
            //        procinfo.FileName = Application.ExecutablePath;
            //        procinfo.WorkingDirectory = Environment.CurrentDirectory;
            //        procinfo.Verb = "runas";
            //        Process.Start(procinfo);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message.ToString());
            //    }

            //    return;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }


        //관리자 권한 확인 함수
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (null != identity)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }

    }
}
