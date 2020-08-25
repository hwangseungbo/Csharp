using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Linq.Expressions;
using System.Threading;
using System.IO;
using System.Windows.Forms.VisualStyles;
using System.Security.Principal;
using System.Xml;
using System.Xml.Schema;
using System.Runtime.InteropServices;

namespace Observer
{
    public partial class Form1 : Form
    {
        bool looptriger = true;
        Thread[] Tpro = new Thread[100];    // 관리 프로세스 마다 갖게될 쓰레드
        int Tindex = 0;                     // Tpro객체 인덱스
        Process[] Proc;                     // 전체 프로세스 정보를 담을 객체
        private Process[] myProcess = new Process[100];
        private TaskCompletionSource<bool> eventHandled;
        string[] procName = new string[300];        //전체 프로세스 명을 담을 스트링 배열
        private object lockObject = new object();   //lock문에 사용될 객체.
        string[] startTime = new string[100];   // 시작시간을 저장하고있는 스트링배열

        bool On;
        Point Pos;

        /*
        public async Task PrintDoc(string fileName)
        {
            eventHandled = new TaskCompletionSource<bool>();
            
            using (myProcess[index] = new Process())
            {
                try
                {
                    myProcess[index].StartInfo.FileName = fileName;
                    myProcess[index].StartInfo.Verb = "Print";
                    myProcess[index].StartInfo.CreateNoWindow = true;
                    myProcess[index].EnableRaisingEvents = true;
                    object name = fileName;
                    myProcess[index].Exited += new EventHandler(myProcess_Exited);
                    myProcess[index].Start();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"An error occurred trying to print \"{fileName}\":\n{ex.Message}");
                    return;
                }
                index++;
                // Wait for Exited event, but not more than 30 seconds.
                await Task.WhenAny(eventHandled.Task, Task.Delay(30000));
            }
        }
        

        private void myProcess_Exited(object sender, EventArgs e)
        {
            MessageBox.Show(e.ToString());
            LogWrite("가 종료되었습니다.");
            eventHandled.TrySetResult(true);
        }
        */

        public Form1()
        {
            InitializeComponent();

            MouseDown += (o, e) => { if (e.Button == MouseButtons.Left) { On = true; Pos = e.Location; } };
            MouseMove += (o, e) => { if (On) Location = new Point(Location.X + (e.X - Pos.X), Location.Y + (e.Y - Pos.Y)); };
            MouseUp += (o, e) => { if (e.Button == MouseButtons.Left) { On = false; Pos = e.Location; } };

            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            //대기시간없이 리스트 뷰 뛰우기위해 여기에 추가했다.--------------------------------------------------------------
            Process[] proc = Process.GetProcesses();
            listView1.Items.Clear();
            string path = String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath);
            string[] textvalue = System.IO.File.ReadAllLines(path);
            string ExitLogPath = Environment.CurrentDirectory + @"\ExitLog\ExitLog.log";

            for (int i = 0; i <= (textvalue.Length - 2); i = i + 2)
            {
                string[] LVItem = new string[6];
                LVItem[0] = Path.GetFileNameWithoutExtension(textvalue[i]);
                LVItem[1] = textvalue[i];

                for (int j = 0; j <= proc.Length - 1; j++)
                {
                    if (LVItem[0] == proc[j].ProcessName)
                    {
                        LVItem[2] = proc[j].StartTime.ToString();
                        LVItem[5] = "동작중";
                        break;
                    }
                    else if (j == proc.Length - 1 && LVItem[0] != proc[j].ProcessName)
                    {
                        LVItem[2] = "현재 실행중이지 않습니다.";
                        LVItem[5] = "종료됨";
                    }
                }
                LVItem[3] = textvalue[i + 1];
                string[] textvalue2 = System.IO.File.ReadAllLines(ExitLogPath);
                for (int k = textvalue2.Length - 1; k >= 0; k--)
                {
                    if (textvalue2[k].Contains(LVItem[0]))
                    {
                        LVItem[4] = textvalue2[k].Substring(0, 22);
                        break;
                    }
                    else if (k == 0 && textvalue[k] != LVItem[0])
                    {
                        LVItem[4] = "마지막 종료 정보가 없습니다.";
                        break;
                    }
                }
                ListViewItem lvi = new ListViewItem(LVItem);
                listView1.Items.Add(lvi);
            }
            lblProcNum.Text = "관리중인 프로그램 수 : " + (textvalue.Length / 2).ToString();
            listView1.CheckBoxes = true;
            for(int i=0;i<=listView1.Items.Count-1;i++)
            {
                listView1.Items[i].Checked = true;
            }
            //---------------------------------------------------------------------------------------------------------------
        }
        
        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.bunifuElipse1.ApplyElipse(panel1, 8);
            this.bunifuElipse1.ApplyElipse(panel2, 8);

            //System.Windows.Forms.Application.StartupPath  ==  C:\hwangseungbo\Csharp\Observer3\Observer\bin\Debug
            Proc = Process.GetProcesses();
            
            //List.txt파일이 없다면 현재 디렉토리에 생성해줌
            FileInfo fileInfo = new FileInfo(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath));
            if (!fileInfo.Exists)   
            {
                using (StreamWriter sw = new StreamWriter(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)))
                {
                    sw.Close();
                }
            }

            /*  
            Thread thread = new Thread(new ThreadStart(Run));
            //프로그램 종료시 쓰레드는 계속동작하여 프로세스가 남는경우가 있었는데 
            //아래 문단을 통해 부모프로세스 종료시 스레드를 종료시킬 수 있으므로 문제를 해결할 수 있다.
            thread.IsBackground = true;
            thread.Start();
            */

            LogWrite("PM프로그램이 실행되었습니다.");

            string path = String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath);
            string[] textvalue = System.IO.File.ReadAllLines(path);
            int num = (textvalue.Length/2); // 프로그램 구동시 관리리스트에 프로그램이 등록되어있을경우 구동하기위해 등록된 갯수를 구함.

            //관리자 권한 확인
            bool right = IsRunningAsLocalAdmin();
            if (right) //관리자 권한인지 확인하여 맞으면 타이틀에 Administrator를 붙여줌
            {
                this.Text += " " + "(Administrator)";
            }

            if (num > 0) // 관리하는 프로그램이 1개라도 있다면
            {
                for (int i = 0; i <= (textvalue.Length - 2); i = i + 2)
                {
                    int idx = i;
                    int peri = int.Parse(textvalue[idx + 1]);
                    Tpro[Tindex] = new Thread(() => BackWork(textvalue[idx], peri));
                    Tpro[Tindex].IsBackground = true;
                    Tpro[Tindex].Start();
                    Tindex++;
                }
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            getListview();
        }

        
        //List.txt를 읽어들여 listview에 정보를 올려주는 함수
        void getListview()
        {
            bool[] check = new bool[100];
            Process[] proc = Process.GetProcesses();
            

            //리스트뷰 각 행이 체크되있는지 내용을 저장함
            for(int i =0; i<= listView1.Items.Count - 1; i++)
            {
                if (listView1.Items[i].Checked == true)
                {
                    check[i] = true;
                }
                else if(listView1.Items[i].Checked == false)
                {
                    check[i] = false;
                }
                startTime[i] = listView1.Items[i].SubItems[2].Text;
            }
            listView1.Items.Clear();
            

            string path = String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath);
            string[] textvalue = System.IO.File.ReadAllLines(path);
            string ExitLogPath = Environment.CurrentDirectory + @"\ExitLog\ExitLog.log";
            for (int i = 0; i <= (textvalue.Length - 2); i = i + 2)
            {
                int x = 0;
                string[] LVItem = new string[6];
                LVItem[0] = Path.GetFileNameWithoutExtension(textvalue[i]);
                LVItem[1] = textvalue[i];

                for (int j = 0; j <= proc.Length - 1; j++)
                {
                    if (LVItem[0] == proc[j].ProcessName)
                    {
                        LVItem[2] = proc[j].StartTime.ToString();
                        LVItem[5] = "동작중";
                        startTime[x] = proc[j].StartTime.ToString();
                        x++;
                        break;
                    }
                    else if (j == proc.Length - 1 && LVItem[0] != proc[j].ProcessName)
                    {
                        LVItem[2] = startTime[i/2];
                        LVItem[5] = "종료됨";
                    }
                }
                LVItem[3] = textvalue[i + 1];
                string[] textvalue2 = System.IO.File.ReadAllLines(ExitLogPath);
                for (int k = textvalue2.Length - 1; k >= 0; k--)
                {
                    if (textvalue2[k].Contains(LVItem[0]))
                    {
                        LVItem[4] = textvalue2[k].Substring(0, 22);
                        break;
                    }
                    else if (k == 0 && textvalue[k] != LVItem[0])
                    {
                        LVItem[4] = "마지막 종료 정보가 없습니다.";
                        break;
                    }
                }
                ListViewItem lvi = new ListViewItem(LVItem);
                listView1.Items.Add(lvi);

                for (int g = 0; g <= listView1.Items.Count - 1; g++)
                {
                    if (check[g] == true)
                    {
                        listView1.Items[g].Checked = true;
                    }
                    else if (check[g] == false)
                    {
                        listView1.Items[g].Checked = false;
                    }
                }
            }
            lblProcNum.Text = "관리중인 프로그램 수 : " + (textvalue.Length / 2).ToString();

            if (textBox1.Text != "")
            {
                for (int i = 0; i <= listView1.Items.Count - 1; i++) //리스트 뷰의 항목선택이 유지되도록 한다.
                {
                    if (listView1.Items[i].SubItems[1].Text == textBox1.Text)
                    {
                        listView1.Items[i].Focused = true;    //안해도 될듯 포커스드와 셀렉티드의 정확한 차이점 공부 필
                        listView1.Items[i].Selected = true;
                        break;
                    }
                }
            }
        }

        //폼의 빈공간 클릭시 이벤트
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //리스트뷰가 선택되어있으면 해제한다.
                if (listView1.SelectedItems[0].Selected==true)
                {
                    listView1.SelectedItems[0].Selected = false;
                    listView1.FocusedItem.Selected = false;
                    button2.Enabled = false;
                    button3.Enabled = false;
                    button4.Enabled = false;
                    textBox1.Text = "";
                    textBox2.Text = "";
                }
                
            }catch { }
        }

        //관리자 권한으로 실행되는지 확인하기 위한 함수
        public bool IsRunningAsLocalAdmin()
        {
            WindowsIdentity cur = WindowsIdentity.GetCurrent();
            foreach (IdentityReference role in cur.Groups)
            {
                if (role.IsValidTargetType(typeof(SecurityIdentifier)))
                {
                    SecurityIdentifier sid = (SecurityIdentifier)role.Translate(typeof(SecurityIdentifier));
                    if (sid.IsWellKnown(WellKnownSidType.AccountAdministratorSid) || sid.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        //관리 프로세스들을 지속적으로 감시하는 쓰레드를 통해 돌아가는 루프함수
        void BackWork(string path, int period)
        {
            looptriger = true;
            string procName = Path.GetFileNameWithoutExtension(path);
            bool flag = true;
            
            while (true)
            {
                int idx=0;
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    if(listView1.Items[i].SubItems[1].Text == path)
                    {
                        idx = i;
                        break;
                    }
                }

                if (listView1.Items[idx].Checked == false)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                lock (lockObject)
                {
                    Proc = Process.GetProcesses();
                    Thread.Sleep(1000);
                    for (int i = 0; i <= (Proc.Length - 1); i++)
                    {
                        if (procName == Proc[i].ProcessName) // 정상 동작중이므로 할게 없다.
                        {
                            //아래의 코딩 통해 프로세스를 종료이벤트에 등록함, 종료시 LogWrite 함수를 호출하여 정확한 종료시간을 기록한다.
                            //프로세스를 실행할 때 넣어주는게 좋으나 정확한 프로세스 객체가 필요하므로 이처럼 정상동작이 확인되었을 때 이 그인덱스를 빌려 쉽게 구현하였다.
                            //이렇게 되면 처음 실행이 되고 한 주기 동안 정상동작한 이후에 종료이벤트를 구독하는 문제가 있으나 타협.    // 이 함수 마지막 주기 설정부분을 플래그로 감싸며 해결
                            if (flag == true)
                            {
                                //MessageBox.Show(string.Format("{0}가 종료이벤트를 구독합니다.", procName));
                                Proc[i].EnableRaisingEvents = true;
                                Proc[i].Exited += (sender, e) =>
                                    {
                                        /*종료이벤트 발생시 수행되는 명령*/
                                        bool exit = Proc[i].WaitForExit(3000);
                                        //MessageBox.Show("종료이벤 로그라잇호출");
                                        LogWrite(string.Format("{0}이 종료되었습니다.", procName));
                                        ExitLogWrite(string.Format("{0}이 종료되었습니다.", procName));
                                        flag = true;
                                    };
                                flag = false;
                            }
                            break;
                        }
                        else if (i == Proc.Length - 1 && procName != Proc[Proc.Length - 1].ProcessName) // 프로세스배열 끝까지 이름비교해도 없면 동작중이 아니므로 실행시킨다.
                        {
                            LogWrite(procName + "이(가) 실행중이지 않아 시작합니다.");
                            Process.Start(path);
                            Thread.Sleep(500);
                            break;
                        }
                        if(looptriger == false)
                        {
                            break;
                        }
                    }
                }
                //원래는 한주기 후에 종료이벤트 구독했었는데 이 코드 덕에 해결. 이젠 프로세스 실행되고 얼마않있어 바로 종료이벤트 구독함.
                if(flag == false)
                {
                    Thread.Sleep(period * 1000);
                }
            }
        }

        //칼럼 클릭을 통한 정렬 ------------------------------------------------------------
        Boolean m_ColumnclickASC = true;

        private void listView1_ColumnClick_1(object sender, ColumnClickEventArgs e)
        {
            if (m_ColumnclickASC == true)
                ((ListView)sender).ListViewItemSorter = new ListViewItemSortASC(e.Column);
            else
                ((ListView)sender).ListViewItemSorter = new ListViewItemSortDESC(e.Column);

            m_ColumnclickASC = !m_ColumnclickASC;
        }

        class ListViewItemSort : IComparer
        {
            private int col;

            public ListViewItemSort()
            {
                col = 0;
            }

            public ListViewItemSort(int column)
            {
                col = column;
            }

            public int Compare(object x, object y)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
        }

        class ListViewItemSortASC : IComparer
        {
            private int col;

            public ListViewItemSortASC()
            {
                col = 0;
            }

            public ListViewItemSortASC(int column)
            {
                col = column;
            }

            public int Compare(object x, object y)
            {
                try
                {
                    if (Convert.ToInt32(((ListViewItem)x).SubItems[col].Text) > Convert.ToInt32(((ListViewItem)y).SubItems[col].Text))
                        return 1;
                    else
                        return -1;
                }
                catch (Exception)
                {
                    if (1 != String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text))
                        return -1;
                    else
                        return 1;
                }
            }
        }

        class ListViewItemSortDESC : IComparer
        {
            private int col;

            public ListViewItemSortDESC()
            {
                col = 0;
            }

            public ListViewItemSortDESC(int column)
            {
                col = column;
            }

            public int Compare(object x, object y)
            {
                try
                {
                    if (Convert.ToInt32(((ListViewItem)x).SubItems[col].Text) < Convert.ToInt32(((ListViewItem)y).SubItems[col].Text))
                        return 1;
                    else
                        return -1;
                }

                catch (Exception)
                {
                    if (String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text) == 1)
                        return -1;
                    else
                        return 1;
                }
            }
        }
        //---------------------------------------------------------------------------------

        // 메뉴탭의 "끝내기(X)" 버튼을 통한 종료
        private void 끝내기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LogWrite("PM프로그램이 종료되었습니다.");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }

        // 프로그램 창의 오른쪽위 "X" 버튼 이벤트
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
            LogWrite("PM프로그램이 종료되었습니다.");
        }

        // 메뉴탭의 "시작 프로그램에 등록(R)" 버튼 이벤트
        private void 시작프로그램등록RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registrySet();
        }
        // 시작 프로그램에 등록 함수
        private void registrySet()
        {

            try

            {
                // 시작프로그램 등록하는 레지스트리
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey);
                if (strUpKey.GetValue("StartupNanumtip") == null)
                {
                    strUpKey.Close();
                    strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                    // 시작프로그램 등록명과 exe경로를 레지스트리에 등록
                    strUpKey.SetValue("StartupNanumtip", Application.ExecutablePath);
                }
                MessageBox.Show("정상적으로 등록되었습니다.");
            }
            catch
            {
                MessageBox.Show("등록에 실패하였습니다.");
            }
            //try
            //{
            //    var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    rkApp.SetValue("Observer", Application.ExecutablePath);

            //    MessageBox.Show("정상적으로 등록되었습니다.");
            //}
            //catch
            //{
            //    MessageBox.Show("레지스트리 등록에 실패하였습니다.");
            //}
        }

        // 메뉴탭의 "시작 프로그램에서 삭제(E)" 버튼 이벤트 
        private void 사작프로그램에서삭제EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registryDel();
        }
        // 시작 프로그램에서의 삭제 함수
        private void registryDel()
        {
            try
            {
                string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
                RegistryKey strUpKey = Registry.LocalMachine.OpenSubKey(runKey, true);
                // 레지스트리값 제거
                strUpKey.DeleteValue("StartupNanumtip");
                MessageBox.Show("정상적으로 삭제되었습니다.");
            }
            catch
            {
                MessageBox.Show("시작 프로그램으로 등록되어있지 않습니다.");
            }
            //try
            //{
            //    var rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            //    rkApp.DeleteValue("Observer");
            //    MessageBox.Show("정상적으로 삭제되었습니다.");
            //}
            //catch
            //{
            //    MessageBox.Show("시작 프로그램으로 등록되어있지 않습니다.");
            //}
        }

       
        //"찾기"버튼 클릭시 이벤트
        private void button1_Click(object sender, EventArgs e)
        {
            int len = Proc.Length;

            for (int i = 0; i <= len - 1; i++)
            {
                procName[i] = Proc[i].ProcessName;  //지역변수로 procName[]을 선언하니 "할당안된변수 사용불가"라 하여 전역변수로 선언함
            }

            OpenFileDialog openProcessFile = new OpenFileDialog();
            openProcessFile.Filter = "실행파일(*.exe)|*.exe";

            if (openProcessFile.ShowDialog() == DialogResult.OK)
            {
                var list = new List<string>();
                list.AddRange(procName);
                string item = Path.GetFileNameWithoutExtension(openProcessFile.FileName); //프로세스 명(ex: mspaint)을 반환받아 item 변수에 저장한다.
                string path = openProcessFile.FileName;        //바로가기 lnk 파일을 대상으로해도 실제 찐경로가 반환된다.

                textBox1.Text = path;
                textBox2.Text = "5";
                for (int i = 0; i <= listView1.Items.Count - 1; i++)
                {
                    if(path == listView1.Items[i].SubItems[1].Text)
                    {
                        listView1.SelectedItems.Clear();
                        textBox1.Text = "이미 등록된 프로그램 입니다.";
                        button2.Enabled = false;
                        button3.Enabled = false;
                        button4.Enabled = false;
                    }
                    else
                    {
                        listView1.SelectedItems.Clear();
                        button2.Enabled = false;
                        button3.Enabled = true;
                        button4.Enabled = false;
                    }
                }
            }
        }

        //"추가"버튼 클릭시 이벤트
        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            if (textBox1.Text == "")
            {
                MessageBox.Show("찾기 버튼을 통해 실행파일을 선택해주세요.");
            }
            else if (textBox1.Text != "" && textBox2.Text == "")
            {
                MessageBox.Show("주기를 입력해주세요.");
            }
            else if(!(int.TryParse(textBox2.Text, out int result)))
            {
                MessageBox.Show("주기는 숫자만 입력해주세요.");
            }
            else
            {
                string path = textBox1.Text;
                ResistProcess(path);
                StopAndDoWork();
            }
            timer1.Start();
        }
       
        //"추가"버튼을 통한 관리 프로세스 등록함수
        private void ResistProcess(string path)
        {
            int len = Proc.Length;
            string procName = Path.GetFileNameWithoutExtension(path);
            int leng = listView1.Items.Count;
                        
            FileInfo fileInfo = new FileInfo(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath));

            if (!fileInfo.Exists)   //List.txt파일이 없다면 현재 디렉토리에 생성해주고 등록
            {
                using (StreamWriter sw = new StreamWriter(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)))
                {
                    sw.WriteLine(path);
                    sw.WriteLine(textBox2.Text);
                    sw.Close();
                }
                Thread.Sleep(10);
            }
            else   //List.txt 존재시
            {
                var lines = new List<string>();
                lines.AddRange(File.ReadAllLines(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)));
                if (lines.Count != 0) //관리로 등록된 프로그램이 하나라도 있을경우
                {
                    for (int i = 0; i < lines.Count; i++)
                    {
                        if (path == lines[i]) //관리 등록된게 내가지금 등록하려는것과 같다면
                        {
                            MessageBox.Show("이미 등록된 프로그램 입니다.");
                            Thread.Sleep(10);
                            break;
                        }
                        else if (path != lines[i] && i == lines.Count - 1)//관리 등록된게 내가 지금 등록하려는것과 다르며 현재 인덱스가 마지막 인덱스일경우
                        {
                            using (StreamWriter sw = new StreamWriter(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath), true))
                            {
                                sw.WriteLine(path);
                                sw.WriteLine(textBox2.Text);
                                sw.Close();
                            }
                            Thread.Sleep(10);
                            break;
                        }
                    }
                }
                else    //List.txt파일은 있으면서 관리 등록된게 하나도 없을경우
                {
                    using (StreamWriter sw = File.AppendText(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)))
                    {
                        sw.WriteLine(path);
                        sw.WriteLine(textBox2.Text);
                        sw.Close();
                    }
                    Thread.Sleep(10);
                }
                getListview();
                listView1.Items[listView1.Items.Count - 1].Checked = true;
                //for(int i =0; i <= listView1.Items.Count - 1; i++)
                //{
                //}
            }    
        }
        
        //리스트뷰 클릭시 텍스트 박스 정보가 올라오는 이벤트
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                button4.Enabled = true;
                button2.Enabled = true;
            }
            button3.Enabled = false;
            textBox1.Text = listView1.FocusedItem.SubItems[1].Text;
            textBox2.Text = listView1.FocusedItem.SubItems[3].Text;
        }

        //"수정"버튼 클릭 이벤트
        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            //if(textBox1.Text == "" || textBox2.Text == "") 이 조건도 사용은 가능하나 공백을 잡지 못하므로 아래의 조건을 사용하였다.
            if (String.IsNullOrWhiteSpace(textBox1.Text) == true || String.IsNullOrWhiteSpace(textBox2.Text) == true)  
            {
                MessageBox.Show("테이블에서 항목을 클릭해 주세요");
            }
            else if(String.IsNullOrWhiteSpace(textBox2.Text) == false) //textBox1은 사용자가 글자를 입력할 수 없으므로 조건을 확인할 필요가 없다.
            {
                listView1.FocusedItem.SubItems[3].Text = textBox2.Text.ToString();
                var lines = new List<string>();
                lines.AddRange(File.ReadAllLines(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)));

                for(int i = 0; i< lines.Count; i++)
                {
                    if(textBox1.Text == lines[i])
                    {
                        lines[i + 1] = textBox2.Text.ToString();

                        using (StreamWriter outputFile = new StreamWriter(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)))
                        {
                            for (int j = 0; j < lines.Count; j++)
                            {
                                outputFile.WriteLine(lines[j]);
                            }
                            outputFile.Close();
                        }
                    }
                }
                StopAndDoWork();
            }
            timer1.Start();
        }

        //모든 쓰레드의 동작을 멈추고 죽였다가 다시 실행시키는 함수
        private void StopAndDoWork()
        {
            for (int i = 0; i < Tindex; i++)
            {
                Tpro[i].Abort();
            }

            string path = String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath);
            string[] textvalue = System.IO.File.ReadAllLines(path);

            for (int i = 0; i <= (textvalue.Length - 2); i = i + 2)
            {
                int idx = i;
                int peri = int.Parse(textvalue[idx + 1]);
                Tpro[Tindex] = new Thread(() => BackWork(textvalue[idx], peri));
                Tpro[Tindex].IsBackground = true;
                Tpro[Tindex].Start();
                Tindex++;
            }


        }


        //로그디렉토리 및 로그파일 생성
        public void LogWrite(string str)
        {
            string DirPath = Environment.CurrentDirectory + @"\Log";
            string FilePath = DirPath + "\\Log_" + DateTime.Today.ToString("yyyyMMdd") + ".log";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("{0} {1}", DateTime.Now, str); //2020-08-05 오후 7:45:45 이런형식과 함수로 전달받은 str 문자열을 기록으로 남기게된다.
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("{0} {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        //마지막 종료 로그
        public void ExitLogWrite(string str)
        {
            string DirPath = Environment.CurrentDirectory + @"\ExitLog";
            string FilePath = DirPath + "\\ExitLog"+".log";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (!di.Exists) Directory.CreateDirectory(DirPath);
                if (!fi.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("{0} {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                    Thread.Sleep(100);
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("{0} {1}", DateTime.Now, str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        //"삭제"버튼 클릭 이벤트
        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            //MessageBox.Show(listView1.FocusedItem.SubItems[1].Text);
            try
            {
                if (listView1.SelectedItems.Count == 0) // 선택된 항목이 없을경우
                {
                    MessageBox.Show("선택된 항목이 없습니다.");
                }
                else if (listView1.SelectedItems.Count != 0)   // 테이블에서 항목을 선택하고 삭제버튼을 눌렀다면
                {
                    var lines = new List<string>();
                    lines.AddRange(File.ReadAllLines(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)));
                    Thread.Sleep(100);

                    for (int i = 0; i <= lines.Count - 2; i = i + 2)
                    {
                        if (listView1.FocusedItem.SubItems[1].Text == lines[i])
                        {
                            LogWrite(lines[i] + "(을)를 관리 테이블에서 삭제합니다.");
                            lines.RemoveAt(i);
                            lines.RemoveAt(i);

                            using (StreamWriter outputFile = new StreamWriter(String.Format(@"{0}\List.txt", System.Windows.Forms.Application.StartupPath)))
                            {
                                for (int j = 0; j < lines.Count; j++)
                                {
                                    outputFile.WriteLine(lines[j]);
                                }
                                outputFile.Close();
                            }
                            Thread.Sleep(100);
                        }
                    }
                    listView1.FocusedItem.Remove();
                    button4.Enabled = false;
                    button3.Enabled = true;
                    StopAndDoWork();
                }
            }
            catch
            {
            }
            timer1.Start();
        }

        //시작프로그램 등록버튼
        private void button6_Click(object sender, EventArgs e)
        {
            registrySet();
        }

        //시작프로그램 등록 삭제 버튼
        private void button7_Click(object sender, EventArgs e)
        {
            registryDel();
        }


        private Point mCurrentPosition = new Point(0, 0);
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                mCurrentPosition = new Point(-e.X, -e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(
                this.Location.X + (mCurrentPosition.X + e.X),
                this.Location.Y + (mCurrentPosition.Y + e.Y));
            }
        }

        //프로그램 종료 버튼
        private void button8_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("프로그램을 종료하시겠습니까?", "종료", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LogWrite("PM프로그램이 종료되었습니다.");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }
        }
        //프로그램 위치열기 버튼
        private void button5_Click(object sender, EventArgs e)
        {
            string filepath = System.Windows.Forms.Application.StartupPath;
            Process.Start(filepath);
        }
    }
}
// 체크박스풀고 삭제시 왜 계속 실행되는지 모르겠다.