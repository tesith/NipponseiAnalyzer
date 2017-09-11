using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public int Number;
        public int DownloadCount;
        public string Name;

        public MusicInfo(int number, int dlCount, string name)
        {
            Number = number;
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
        public bool _ShouldDownload
        {
            get
            {
                return _shouldDownload;
            }
        }

        public List<MusicInfo> List { get; }
        public List<MusicInfo> ListBySort { get; }

        public MusicArchive()
        {
            _shouldDownload = true;
            List = new List<MusicInfo>();
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
                            var webClient = new WebClient();
                            string HTML = webClient.DownloadString(NIPPONSEI_PACKLIST_ARCHIVE_URL);
                            ArchiveHTML = HTML.Substring(HTML.IndexOf("#1"));
                        }

                        string[] MusicList = ArchiveHTML.Split('\n');

                        int MusicListLength = MusicList.Length;
                        for (int i = 0; i < MusicListLength; i++)
                        {
                            string Music = MusicList[i];

                            int removeIndex = Music.IndexOf(" align");
                            if (removeIndex == -1)
                                break;

                            Music = Music.Remove(removeIndex, 14);

                            string[] Information = Music.Split(new string[] { "</td><td>" }, StringSplitOptions.RemoveEmptyEntries);

                            int number;
                            int.TryParse(Information[0].Substring(Information[0].IndexOf('#') + 1), out number);
                            int downloadCount;
                            int.TryParse(Information[1].Substring(0, Information[1].Length - 1), out downloadCount);
                            string name = Information[3];

                            List.Add(new MusicInfo(number, downloadCount, name));
                        }
                        
                        _shouldDownload = false;
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(ee.ToString());
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
                var webClient = new WebClient();
                string targetFileName = List[index - 1].Name + ".torrent";
                webClient.DownloadFile(NIPPONSEI_TORRENT_TRACKER_URL + targetFileName, targetFileName);
            }
            catch (Exception torrentException)
            {
                MessageBox.Show(torrentException.ToString());
            }
        }
    }
}
