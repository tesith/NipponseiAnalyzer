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
        public static MainForm MyForm;
        private MusicArchive MusicArchive;
        private Thread RankingThread;

        private delegate void voidDelegate();
        private delegate void voidStrDelegate(string str);
        private delegate void voidIntDelegate(int val);
        private delegate string stringDelegate();

        public MainForm()
        {
            InitializeComponent();
            MyForm = this;
            MusicArchive = new MusicArchive();
        }

        private void _ProgressBarValueUpdate(int val)
        {
            ProgressBar.Value = val;
        }

        public void ProgressBarValueUpdate(int val)
        {
            voidIntDelegate d = _ProgressBarValueUpdate;
            ProgressBar.BeginInvoke(d, val);
        }

        private void _ProgressBarMaximum(int val)
        {
            ProgressBar.Maximum = val;
        }

        public void ProgressBarMaximum(int val)
        {
            voidIntDelegate d = _ProgressBarMaximum;
            ProgressBar.BeginInvoke(d, val);
        }

        private void _ProgressBarValuePlusOne()
        {
            ProgressBar.Value++;
        }

        public void ProgressBarValuePlusOne()
        {
            voidDelegate d = _ProgressBarValuePlusOne;
            ProgressBar.BeginInvoke(d);
        }

        private void _LabelStatusUpdate(string message)
        {
            LabelStatus.Text = message;
        }

        public void LabelStatusUpdate(string message)
        {
            voidStrDelegate d = _LabelStatusUpdate;
            LabelStatus.BeginInvoke(d, message);
        }

        private async void UpdateMusicListAsync()
        {
            try
            {
                ProgressBar.Value = 0;
                ProgressBar.Maximum = MusicArchive.List.m_list.Count;

                LabelStatusUpdate("리스트박스 처리중");

                foreach (KeyValuePair<int, MusicInfo> Pair in MusicArchive.List.m_list)
                {
                    MusicInfo info = Pair.Value;
                    string str = string.Format(
                        "#{0}  \t다운로드: {1}      \t{2}",
                        Pair.Key,
                        info.DownloadCount,
                        info.Name);

                    await AddMusicToList(str);

                    ProgressBar.Value++;
                }

                LabelStatusUpdate("완료");

                TorrentButton.Enabled = true;
                //RankingCheckbox.Enabled = true;
                //SearchButton.Enabled = true;
                //SearchTextBox.Enabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private Task AddMusicToList(string str)
        {
            Action a = new Action(() =>
            {
                voidStrDelegate d = AddMusic;
                MusicListBox.BeginInvoke(d, str);
            });
            Task t = Task.Run(a);
            return t;
        }

        private void AddMusic(string str)
        {
            MusicListBox.Items.Add(str);
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
                ArchiveButton.BeginInvoke(d);
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
            int.TryParse(
                selected.Substring(1, selected.IndexOf(' ') - 1), 
                out musicIndex
                );

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
            TorrentThreadFunc();
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                MyForm = null;
                if (RankingThread != null && RankingThread.IsAlive)
                    RankingThread.Abort();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
