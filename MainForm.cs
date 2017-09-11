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

        private delegate void UpdateMusicListDelegate();
        private delegate string getText();
        
        public MainForm()
        {
            InitializeComponent();
            MusicArchive = new MusicArchive();
        }
        
        private void UpdateMusicList()
        {
            foreach (MusicInfo info in MusicArchive.List)
            {
                MusicListBox.Items.Add(String.Format("#{0}  \t다운로드: {1}      \t{2}", info.Number, info.DownloadCount, info.Name));
            }

            //RankingCheckbox.Enabled = true;
            TorrentButton.Enabled = true;
            //SearchButton.Enabled = true;
            //SearchTextBox.Enabled = true;
        }

        private string SelectedString()
        {
            return MusicListBox.SelectedItem.ToString();
        }

        private void RankingThreadFunc()
        {
            MusicArchive.TryDownload();

            if(MusicArchive._ShouldDownload == false)
            {
                if(MusicListBox.InvokeRequired)
                {
                    UpdateMusicListDelegate d = new UpdateMusicListDelegate(UpdateMusicList);
                    Invoke(d);
                }
                else
                {
                    UpdateMusicList();
                }
            }
        }
        
        private void TorrentThreadFunc()
        {
            string selected;

            if(MusicListBox.InvokeRequired)
            {
                getText d = new getText(SelectedString);
                selected = (string)Invoke(d);
            }
            else
            {
                selected = MusicListBox.SelectedItem.ToString();
            }

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
