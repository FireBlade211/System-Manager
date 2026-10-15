namespace SimpleTaskPane
{
    partial class TaskPanePage
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
            FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup taskPaneGroup1 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink1 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskPanePage));
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink2 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink3 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup taskPaneGroup2 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink4 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink5 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink6 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink7 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup taskPaneGroup3 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneGroup();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink8 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink9 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink10 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            FireBlade.SystemManager.Helpers.Controls.TaskPaneLink taskPaneLink11 = new FireBlade.SystemManager.Helpers.Controls.TaskPaneLink();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.taskPane1 = new FireBlade.SystemManager.Helpers.Controls.TaskPane();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(12, 8, 0, 0);
            this.label1.Size = new System.Drawing.Size(451, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "This is a simple page with a task pane.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(317, 57);
            this.label2.TabIndex = 1;
            this.label2.Text = "This sample showcases how to use the new TaskPane control introduced in System Ma" +
    "nager v1.2.\r\n\r\nSelect a command on the right to display it below.";
            // 
            // taskPane1
            // 
            this.taskPane1.AutoScroll = true;
            this.taskPane1.AutoScrollMinSize = new System.Drawing.Size(0, 239);
            this.taskPane1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            taskPaneLink1.Icon = ((System.Drawing.Image)(resources.GetObject("taskPaneLink1.Icon")));
            taskPaneLink1.Text = "Cut";
            taskPaneLink2.Icon = ((System.Drawing.Image)(resources.GetObject("taskPaneLink2.Icon")));
            taskPaneLink2.Text = "Copy";
            taskPaneLink3.Icon = ((System.Drawing.Image)(resources.GetObject("taskPaneLink3.Icon")));
            taskPaneLink3.Text = "Paste";
            taskPaneGroup1.Links.Add(taskPaneLink1);
            taskPaneGroup1.Links.Add(taskPaneLink2);
            taskPaneGroup1.Links.Add(taskPaneLink3);
            taskPaneGroup1.Title = "Clipboard";
            taskPaneLink4.Text = "Do something";
            taskPaneLink5.Text = "Command 1";
            taskPaneLink6.Text = "Command 2";
            taskPaneLink7.Text = "Command 3";
            taskPaneGroup2.Links.Add(taskPaneLink4);
            taskPaneGroup2.Links.Add(taskPaneLink5);
            taskPaneGroup2.Links.Add(taskPaneLink6);
            taskPaneGroup2.Links.Add(taskPaneLink7);
            taskPaneGroup2.Title = "Sample commands";
            taskPaneGroup3.Expanded = false;
            taskPaneLink8.Text = "Hello, World!";
            taskPaneLink9.Text = "Click me!";
            taskPaneLink10.Text = "Command";
            taskPaneLink11.Text = "Test";
            taskPaneGroup3.Links.Add(taskPaneLink8);
            taskPaneGroup3.Links.Add(taskPaneLink9);
            taskPaneGroup3.Links.Add(taskPaneLink10);
            taskPaneGroup3.Links.Add(taskPaneLink11);
            taskPaneGroup3.Title = "Collapsed by default";
            this.taskPane1.Groups.Add(taskPaneGroup1);
            this.taskPane1.Groups.Add(taskPaneGroup2);
            this.taskPane1.Groups.Add(taskPaneGroup3);
            this.taskPane1.LinkFont = new System.Drawing.Font("Tahoma", 8F);
            this.taskPane1.Location = new System.Drawing.Point(451, 0);
            this.taskPane1.Name = "taskPane1";
            this.taskPane1.Size = new System.Drawing.Size(179, 379);
            this.taskPane1.TabIndex = 2;
            this.taskPane1.Text = "taskPane1";
            this.taskPane1.LinkClicked += new System.EventHandler<FireBlade.SystemManager.Helpers.Controls.TaskPaneLinkClickedEventArgs>(this.taskPane1_LinkClicked);
            this.taskPane1.GroupExpandedChanged += new System.EventHandler<FireBlade.SystemManager.Helpers.Controls.TaskPaneGroupExpandedChangedEventArgs>(this.taskPane1_GroupExpandedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "...";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(25, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Action:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(26, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Item:";
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label7.Location = new System.Drawing.Point(0, 349);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(451, 30);
            this.label7.TabIndex = 7;
            this.label7.Text = "Tip: Change your theme in the System Manager settings and see the task pane chang" +
    "e! You can implement this using the IThemeContract.";
            // 
            // TaskPanePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.taskPane1);
            this.Name = "TaskPanePage";
            this.Size = new System.Drawing.Size(630, 379);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        public FireBlade.SystemManager.Helpers.Controls.TaskPane taskPane1;
    }
}
