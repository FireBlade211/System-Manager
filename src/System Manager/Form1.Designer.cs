namespace FireBlade.SystemManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.contentPanel = new System.Windows.Forms.Panel();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findPagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentPagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemManagerSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewInstalledPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.getMorePluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportimportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutSystemManagerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.systemManagerGitHubRepositoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemManagerPluginRepositoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.pluginDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 49);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.contentPanel);
            this.splitContainer1.Size = new System.Drawing.Size(933, 470);
            this.splitContainer1.SplitterDistance = 310;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.HotTracking = true;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageListSmall;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(310, 470);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseMove);
            // 
            // imageListSmall
            // 
            this.imageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListSmall.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contentPanel
            // 
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Location = new System.Drawing.Point(0, 0);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(618, 470);
            this.contentPanel.TabIndex = 0;
            // 
            // imageListLarge
            // 
            this.imageListLarge.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListLarge.ImageSize = new System.Drawing.Size(32, 32);
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(933, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(933, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findPagesToolStripMenuItem,
            this.goToPageToolStripMenuItem,
            this.recentPagesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // findPagesToolStripMenuItem
            // 
            this.findPagesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("findPagesToolStripMenuItem.Image")));
            this.findPagesToolStripMenuItem.Name = "findPagesToolStripMenuItem";
            this.findPagesToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findPagesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.findPagesToolStripMenuItem.Text = "&Find pages...";
            this.findPagesToolStripMenuItem.Click += new System.EventHandler(this.findPagesToolStripMenuItem_Click);
            // 
            // goToPageToolStripMenuItem
            // 
            this.goToPageToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("goToPageToolStripMenuItem.Image")));
            this.goToPageToolStripMenuItem.Name = "goToPageToolStripMenuItem";
            this.goToPageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToPageToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.goToPageToolStripMenuItem.Text = "&Go to Page...";
            this.goToPageToolStripMenuItem.Click += new System.EventHandler(this.goToPageToolStripMenuItem_Click);
            // 
            // recentPagesToolStripMenuItem
            // 
            this.recentPagesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("recentPagesToolStripMenuItem.Image")));
            this.recentPagesToolStripMenuItem.Name = "recentPagesToolStripMenuItem";
            this.recentPagesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.recentPagesToolStripMenuItem.Text = "&Recent pages";
            this.recentPagesToolStripMenuItem.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(175, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemManagerSettingsToolStripMenuItem,
            this.pluginsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exportimportToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsToolStripMenuItem.Text = "&Settings";
            // 
            // systemManagerSettingsToolStripMenuItem
            // 
            this.systemManagerSettingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("systemManagerSettingsToolStripMenuItem.Image")));
            this.systemManagerSettingsToolStripMenuItem.Name = "systemManagerSettingsToolStripMenuItem";
            this.systemManagerSettingsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.systemManagerSettingsToolStripMenuItem.Text = "&System Manager settings...";
            this.systemManagerSettingsToolStripMenuItem.Click += new System.EventHandler(this.systemManagerSettingsToolStripMenuItem_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewInstalledPluginsToolStripMenuItem,
            this.getMorePluginsToolStripMenuItem});
            this.pluginsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pluginsToolStripMenuItem.Image")));
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            this.pluginsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.pluginsToolStripMenuItem.Text = "&Plugins";
            // 
            // viewInstalledPluginsToolStripMenuItem
            // 
            this.viewInstalledPluginsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("viewInstalledPluginsToolStripMenuItem.Image")));
            this.viewInstalledPluginsToolStripMenuItem.Name = "viewInstalledPluginsToolStripMenuItem";
            this.viewInstalledPluginsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.viewInstalledPluginsToolStripMenuItem.Text = "&Manage installed plugins...";
            this.viewInstalledPluginsToolStripMenuItem.Click += new System.EventHandler(this.viewInstalledPluginsToolStripMenuItem_Click);
            // 
            // getMorePluginsToolStripMenuItem
            // 
            this.getMorePluginsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("getMorePluginsToolStripMenuItem.Image")));
            this.getMorePluginsToolStripMenuItem.Name = "getMorePluginsToolStripMenuItem";
            this.getMorePluginsToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.getMorePluginsToolStripMenuItem.Text = "&Get more plugins...";
            this.getMorePluginsToolStripMenuItem.Click += new System.EventHandler(this.getMorePluginsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
            this.toolStripMenuItem2.Visible = false;
            // 
            // exportimportToolStripMenuItem
            // 
            this.exportimportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSettingsToolStripMenuItem,
            this.importSettingsToolStripMenuItem});
            this.exportimportToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exportimportToolStripMenuItem.Image")));
            this.exportimportToolStripMenuItem.Name = "exportimportToolStripMenuItem";
            this.exportimportToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.exportimportToolStripMenuItem.Text = "&Export/import";
            this.exportimportToolStripMenuItem.Visible = false;
            // 
            // exportSettingsToolStripMenuItem
            // 
            this.exportSettingsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exportSettingsToolStripMenuItem.Name = "exportSettingsToolStripMenuItem";
            this.exportSettingsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportSettingsToolStripMenuItem.Text = "&Export settings";
            // 
            // importSettingsToolStripMenuItem
            // 
            this.importSettingsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importSettingsToolStripMenuItem.Name = "importSettingsToolStripMenuItem";
            this.importSettingsToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.importSettingsToolStripMenuItem.Text = "I&mport settings";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutSystemManagerToolStripMenuItem,
            this.toolStripMenuItem3,
            this.systemManagerGitHubRepositoryToolStripMenuItem,
            this.systemManagerPluginRepositoryToolStripMenuItem,
            this.toolStripMenuItem4,
            this.pluginDocumentationToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutSystemManagerToolStripMenuItem
            // 
            this.aboutSystemManagerToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutSystemManagerToolStripMenuItem.Image")));
            this.aboutSystemManagerToolStripMenuItem.Name = "aboutSystemManagerToolStripMenuItem";
            this.aboutSystemManagerToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.aboutSystemManagerToolStripMenuItem.Text = "&About System Manager...";
            this.aboutSystemManagerToolStripMenuItem.Click += new System.EventHandler(this.aboutSystemManagerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(246, 6);
            // 
            // systemManagerGitHubRepositoryToolStripMenuItem
            // 
            this.systemManagerGitHubRepositoryToolStripMenuItem.Name = "systemManagerGitHubRepositoryToolStripMenuItem";
            this.systemManagerGitHubRepositoryToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.systemManagerGitHubRepositoryToolStripMenuItem.Text = "&System Manager repository...";
            this.systemManagerGitHubRepositoryToolStripMenuItem.Click += new System.EventHandler(this.systemManagerGitHubRepositoryToolStripMenuItem_Click);
            // 
            // systemManagerPluginRepositoryToolStripMenuItem
            // 
            this.systemManagerPluginRepositoryToolStripMenuItem.Name = "systemManagerPluginRepositoryToolStripMenuItem";
            this.systemManagerPluginRepositoryToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.systemManagerPluginRepositoryToolStripMenuItem.Text = "Syst&em Manager plugin repository...";
            this.systemManagerPluginRepositoryToolStripMenuItem.Click += new System.EventHandler(this.systemManagerPluginRepositoryToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(246, 6);
            // 
            // pluginDocumentationToolStripMenuItem
            // 
            this.pluginDocumentationToolStripMenuItem.Name = "pluginDocumentationToolStripMenuItem";
            this.pluginDocumentationToolStripMenuItem.Size = new System.Drawing.Size(249, 22);
            this.pluginDocumentationToolStripMenuItem.Text = "&Plugin documentation...";
            this.pluginDocumentationToolStripMenuItem.Click += new System.EventHandler(this.pluginDocumentationToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 519);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "[Home] System Manager";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.TreeView treeView1;
        public System.Windows.Forms.Panel contentPanel;
        public System.Windows.Forms.ImageList imageListSmall;
        public System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findPagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentPagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemManagerSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInstalledPluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem getMorePluginsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exportimportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutSystemManagerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem systemManagerGitHubRepositoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemManagerPluginRepositoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToPageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem pluginDocumentationToolStripMenuItem;
    }
}

