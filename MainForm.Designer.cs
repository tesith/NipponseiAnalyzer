namespace NipponseiAnalyzer
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ArchiveButton = new System.Windows.Forms.Button();
            this.MusicListBox = new System.Windows.Forms.ListBox();
            this.TorrentButton = new System.Windows.Forms.Button();
            this.TestButton = new System.Windows.Forms.Button();
            this.RankingCheckbox = new System.Windows.Forms.CheckBox();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ArchiveButton
            // 
            this.ArchiveButton.Location = new System.Drawing.Point(12, 12);
            this.ArchiveButton.Name = "ArchiveButton";
            this.ArchiveButton.Size = new System.Drawing.Size(89, 23);
            this.ArchiveButton.TabIndex = 0;
            this.ArchiveButton.Text = "전곡 검색";
            this.ArchiveButton.UseVisualStyleBackColor = true;
            this.ArchiveButton.Click += new System.EventHandler(this.RankingButton_Click);
            // 
            // MusicListBox
            // 
            this.MusicListBox.FormattingEnabled = true;
            this.MusicListBox.HorizontalScrollbar = true;
            this.MusicListBox.ItemHeight = 12;
            this.MusicListBox.Location = new System.Drawing.Point(12, 41);
            this.MusicListBox.Name = "MusicListBox";
            this.MusicListBox.Size = new System.Drawing.Size(955, 460);
            this.MusicListBox.TabIndex = 1;
            // 
            // TorrentButton
            // 
            this.TorrentButton.Enabled = false;
            this.TorrentButton.Location = new System.Drawing.Point(12, 507);
            this.TorrentButton.Name = "TorrentButton";
            this.TorrentButton.Size = new System.Drawing.Size(89, 23);
            this.TorrentButton.TabIndex = 2;
            this.TorrentButton.Text = "토렌트 받기";
            this.TorrentButton.UseVisualStyleBackColor = true;
            this.TorrentButton.Click += new System.EventHandler(this.TorrentButton_Click);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(417, 509);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(75, 23);
            this.TestButton.TabIndex = 3;
            this.TestButton.Text = "테스트";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // RankingCheckbox
            // 
            this.RankingCheckbox.AutoSize = true;
            this.RankingCheckbox.Enabled = false;
            this.RankingCheckbox.Location = new System.Drawing.Point(147, 511);
            this.RankingCheckbox.Name = "RankingCheckbox";
            this.RankingCheckbox.Size = new System.Drawing.Size(128, 16);
            this.RankingCheckbox.TabIndex = 4;
            this.RankingCheckbox.Text = "다운로드 수로 정렬";
            this.RankingCheckbox.UseVisualStyleBackColor = true;
            this.RankingCheckbox.CheckedChanged += new System.EventHandler(this.RankingCheckbox_CheckedChanged);
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Enabled = false;
            this.SearchTextBox.Location = new System.Drawing.Point(608, 509);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(278, 21);
            this.SearchTextBox.TabIndex = 5;
            // 
            // SearchButton
            // 
            this.SearchButton.Enabled = false;
            this.SearchButton.Location = new System.Drawing.Point(892, 507);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 6;
            this.SearchButton.Text = "검색";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(740, 12);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(227, 23);
            this.ProgressBar.TabIndex = 7;
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.Location = new System.Drawing.Point(546, 17);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(41, 12);
            this.LabelStatus.TabIndex = 8;
            this.LabelStatus.Text = "대기중";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 547);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.RankingCheckbox);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.TorrentButton);
            this.Controls.Add(this.MusicListBox);
            this.Controls.Add(this.ArchiveButton);
            this.Name = "MainForm";
            this.Text = "Nipponsei Analyzer v1.0";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ArchiveButton;
        private System.Windows.Forms.ListBox MusicListBox;
        private System.Windows.Forms.Button TorrentButton;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.CheckBox RankingCheckbox;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label LabelStatus;
    }
}

