using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace NipponseiAnalyzer
{
    public partial class MainForm : Form
    {
        private MusicArchive MusicArchive;
        private Thread RankingThread;
        private Thread TorrentThread;

        private delegate void voidDelegate();
        private delegate string stringDelegate();
        
        public MainForm()
        {
            InitializeComponent();
            MusicArchive = new MusicArchive();
        }
        
        private async void UpdateMusicListAsync()
        {
            foreach (MusicInfo info in MusicArchive.List)
            {
                string inputString = string.Format("#{0}  \t다운로드: {1}      \t{2}", info.Number, info.DownloadCount, info.Name);
                await AddMusicToList(inputString);
            }

            TorrentButton.Enabled = true;
            //RankingCheckbox.Enabled = true;
            //SearchButton.Enabled = true;
            //SearchTextBox.Enabled = true;
        }

        private Task AddMusicToList(string str)
        {
            Action action = delegate ()
            {
                MusicListBox.Items.Add(str);
            };

            Task task = Task.Factory.StartNew(action);
            return task;
        }

        private string getSelectedString()
        {
            return MusicListBox.SelectedItem.ToString();
        }

        private void RankingThreadFunc()
        {
            MusicArchive.TryDownload();

            if(MusicArchive.ShouldDownload == false)
            {
                voidDelegate d = new voidDelegate(UpdateMusicListAsync);
                MusicListBox.BeginInvoke(d);
            }
            else
            {
                ArchiveButton.BeginInvoke(new Action(() =>
                {
                    ArchiveButton.Enabled = true;
                }));
            }
        }
        
        private void TorrentThreadFunc()
        {
            string selected;
            
            stringDelegate d = new stringDelegate(getSelectedString);
            selected = (string)Invoke(d);

            int musicIndex;
            int.TryParse(selected.Substring(1, selected.IndexOf(' ') - 1), out musicIndex);

            MusicArchive.TorrentDownload(musicIndex);
        }

        private void RankingButton_Click(object sender, EventArgs e)
        {
            ArchiveButton.Enabled = false;
            RankingThread = new Thread(RankingThreadFunc);
            RankingThread.Start();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {

        }

        private void RankingCheckbox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void TorrentButton_Click(object sender, EventArgs e)
        {
            TorrentThread = new Thread(TorrentThreadFunc);
            TorrentThread.Start();
        }
    }
}
