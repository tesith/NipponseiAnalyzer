using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NipponseiAnalyzer
{
    struct MusicInfo
    {
        public int DownloadCount;
        public string Name;

        public MusicInfo(int dlCount, string name)
        {
            DownloadCount = dlCount;
            Name = name;
        }
    }

    class MusicArchive
    {
        private const string NIPPONSEI_PACKLIST_ARCHIVE_URL = "https://nipponsei.minglong.org/packlist/archive/";
        private const string NIPPONSEI_TORRENT_TRACKER_URL = "http://tracker.minglong.org/torrents/";
        private string ArchiveHTML;
        private bool _shouldDownload;
        public bool ShouldDownload
        {
            get
            {
                return _shouldDownload;
            }
        }
        
        public class MyList
        {
            public SortedList<int, MusicInfo> m_list { get; }
            private readonly object m_lock = new object();

            public MyList()
            {
                m_list = new SortedList<int, MusicInfo>();
            }
            public void Add(int key, MusicInfo info)
            {
                lock(m_lock)
                {
                    m_list.Add(key, info);
                }
            }
            public MusicInfo this[int i]
            {
                get => m_list[i];
                set => m_list[i] = value;
            }
        }

        public MyList List { get; set; }

        public MusicArchive()
        {
            _shouldDownload = true;
            List = new MyList();
        }
        
        private void DeleteBlank(ref string str)
        {
            string[] split = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            str = "";
            foreach (string part in split)
            {
                str += part.Trim();
                str += ' ';
            }

            str.Trim();
            /*
            bool beforeIsBlank = false;

            for(int i = 0; ; )
            {
                if (str[i] == '\0')
                    break;
                if (str[i] == ' ')
                {
                    if (beforeIsBlank)
                        str = str.Remove(i, 1);
                    else
                    {
                        beforeIsBlank = true;
                        i++;
                    }
                }
            }*/
        }

        static bool IsInternetConnected()
        {
            const string NCSI_TEST_URL = "http://www.msftncsi.com/ncsi.txt";
            const string NCSI_TEST_RESULT = "Microsoft NCSI";
            const string NCSI_DNS = "dns.msftncsi.com";
            const string NCSI_DNS_IP_ADDRESS = "131.107.255.255";
            
            if (!NetworkInterface.GetIsNetworkAvailable())
                return false;

            try
            {
                // Check NCSI test link
                var webClient = new WebClient();
                string result = webClient.DownloadString(NCSI_TEST_URL);
                if (result != NCSI_TEST_RESULT)
                {
                    return false;
                }

                // Check NCSI DNS IP
                var dnsHost = Dns.GetHostEntry(NCSI_DNS);
                if (dnsHost.AddressList.Count() < 0 || dnsHost.AddressList[0].ToString() != NCSI_DNS_IP_ADDRESS)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

            return true;
        }

        private void TryParse(string s)
        {
            int cutIndex = s.IndexOf(" align");
            if (cutIndex == -1)
                return;

            s = s.Remove(cutIndex, 14);

            string[] info = s.Split(new string[] { "</td><td>" }, StringSplitOptions.RemoveEmptyEntries);
            if (info.Length != 4)
                return;

            string sNum = info[0];
            int number;
            int.TryParse(sNum.Substring(sNum.IndexOf('#') + 1), out number);

            string sDown = info[1];
            int down;
            int.TryParse(sDown.Substring(0, sDown.Length - 1), out down);

            string sName = info[3];
            DeleteBlank(ref sName);

            List.Add(number, new MusicInfo(down, sName));
        }

        // 처음 실행할 경우 다운로드 합니다
        public void TryDownload()
        {
            if(_shouldDownload)
            {
                if (IsInternetConnected())
                {
                    try
                    {
                        if (ArchiveHTML == null)
                        {
                            
                            if (MainForm.MyForm == null)
                                return;

                            MainForm.MyForm.LabelStatusUpdate("곡 정보를 읽는중");

                            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(NIPPONSEI_PACKLIST_ARCHIVE_URL);
                            using (WebResponse response = http.GetResponse())
                            {
                                Stream stream = response.GetResponseStream();

                                string HTML;
                                using (StreamReader sr = new StreamReader(stream))
                                {
                                    HTML = sr.ReadToEnd();
                                }
                                
                                ArchiveHTML = HTML.Substring(HTML.IndexOf("#1"));
                            }
                        }

                        string[] MusicList = ArchiveHTML.Split('\n');

                        int MusicListLength = MusicList.Length;

                        if (MainForm.MyForm == null)
                            return;

                        MainForm.MyForm.ProgressBarMaximum(MusicListLength);
                        MainForm.MyForm.ProgressBarValueUpdate(0);
                        MainForm.MyForm.LabelStatusUpdate("문자열 처리중");

                        Parallel.For(0, MusicListLength, i => {
                            TryParse(MusicList[i]);

                            if (MainForm.MyForm == null) return;
                            MainForm.MyForm.ProgressBarValuePlusOne();
                        });

                        _shouldDownload = false;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("인터넷 연결을 확인해주세요");
                }
            }
        }

        public void TorrentDownload(int index)
        {
            try
            {
                using (SaveFileDialog saveAs = new SaveFileDialog())
                {
                    string targetFileName = List[index - 1].Name.Trim() + ".torrent";

                    saveAs.Filter = "Torrent Files|*.torrent";
                    saveAs.FileName = targetFileName;

                    if(saveAs.ShowDialog() == DialogResult.OK)
                    {
                        //MessageBox.Show(saveAs.OpenFile().ToString());
                        var webClient = new WebClient();
                        webClient.DownloadFile(NIPPONSEI_TORRENT_TRACKER_URL + targetFileName, saveAs.FileName);
                    }
                }
            }
            catch (Exception torrentException)
            {
                MessageBox.Show(torrentException.ToString());
            }
        }
    }
}
