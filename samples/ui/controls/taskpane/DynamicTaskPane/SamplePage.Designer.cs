namespace DynamicTaskPane
{
    partial class SamplePage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup taskPaneGroup9 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink13 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink14 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink15 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup taskPaneGroup10 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup();
            this.taskPane1 = new FireBlade.SystemManager.Helpers.Controls.TaskPane();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // taskPane1
            // 
            this.taskPane1.AutoScroll = true;
            this.taskPane1.AutoScrollMinSize = new System.Drawing.Size(0, 146);
            this.taskPane1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            taskPaneLink13.Tag = "add1";
            taskPaneLink13.Text = "Add item";
            taskPaneLink14.Tag = "add5";
            taskPaneLink14.Text = "Add 5 items";
            taskPaneLink15.Tag = "clear";
            taskPaneLink15.Text = "Clear items";
            taskPaneGroup9.Links.Add(taskPaneLink13);
            taskPaneGroup9.Links.Add(taskPaneLink14);
            taskPaneGroup9.Links.Add(taskPaneLink15);
            taskPaneGroup9.Title = "Common tasks";
            taskPaneGroup10.Tag = "dyna";
            taskPaneGroup10.Title = "Dynamic group";
            this.taskPane1.Groups.Add(taskPaneGroup9);
            this.taskPane1.Groups.Add(taskPaneGroup10);
            this.taskPane1.LinkFont = new System.Drawing.Font("Tahoma", 8F);
            this.taskPane1.Location = new System.Drawing.Point(503, 0);
            this.taskPane1.Name = "taskPane1";
            this.taskPane1.Size = new System.Drawing.Size(179, 414);
            this.taskPane1.TabIndex = 0;
            this.taskPane1.Text = "taskPane1";
            this.taskPane1.LinkClicked += new System.EventHandler<FireBlade.SystemManager.Helpers.Controls.TaskPaneLinkClickedEventArgs>(this.taskPane1_LinkClicked);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.IntegralHeight = false;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(503, 414);
            this.listBox1.TabIndex = 1;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // SamplePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.taskPane1);
            this.Name = "SamplePage";
            this.Size = new System.Drawing.Size(682, 414);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBox1;
        public FireBlade.SystemManager.Helpers.Controls.TaskPane taskPane1;
    }
}
