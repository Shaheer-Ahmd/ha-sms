namespace StudentManagementSystem.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnEnrollments;
        private System.Windows.Forms.Button btnDepartments;
        private System.Windows.Forms.Button btnSwitchBLL;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBLLStatus;
        private System.Windows.Forms.Panel panel1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnEnrollments = new System.Windows.Forms.Button();
            this.btnDepartments = new System.Windows.Forms.Button();
            this.btnSwitchBLL = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBLLStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(560, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Student Management System";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnStudents);
            this.panel1.Controls.Add(this.btnCourses);
            this.panel1.Controls.Add(this.btnEnrollments);
            this.panel1.Controls.Add(this.btnDepartments);
            this.panel1.Location = new System.Drawing.Point(50, 80);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 180);
            this.panel1.TabIndex = 1;
            // 
            // btnStudents
            // 
            this.btnStudents.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnStudents.Location = new System.Drawing.Point(20, 20);
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Size = new System.Drawing.Size(200, 60);
            this.btnStudents.TabIndex = 0;
            this.btnStudents.Text = "Manage Students";
            this.btnStudents.UseVisualStyleBackColor = true;
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);
            // 
            // btnCourses
            // 
            this.btnCourses.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnCourses.Location = new System.Drawing.Point(250, 20);
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Size = new System.Drawing.Size(200, 60);
            this.btnCourses.TabIndex = 1;
            this.btnCourses.Text = "Manage Courses";
            this.btnCourses.UseVisualStyleBackColor = true;
            this.btnCourses.Click += new System.EventHandler(this.btnCourses_Click);
            // 
            // btnEnrollments
            // 
            this.btnEnrollments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnEnrollments.Location = new System.Drawing.Point(20, 100);
            this.btnEnrollments.Name = "btnEnrollments";
            this.btnEnrollments.Size = new System.Drawing.Size(200, 60);
            this.btnEnrollments.TabIndex = 2;
            this.btnEnrollments.Text = "Manage Enrollments";
            this.btnEnrollments.UseVisualStyleBackColor = true;
            this.btnEnrollments.Click += new System.EventHandler(this.btnEnrollments_Click);
            // 
            // btnDepartments
            // 
            this.btnDepartments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.btnDepartments.Location = new System.Drawing.Point(250, 100);
            this.btnDepartments.Name = "btnDepartments";
            this.btnDepartments.Size = new System.Drawing.Size(200, 60);
            this.btnDepartments.TabIndex = 3;
            this.btnDepartments.Text = "Manage Departments";
            this.btnDepartments.UseVisualStyleBackColor = true;
            this.btnDepartments.Click += new System.EventHandler(this.btnDepartments_Click);
            // 
            // btnSwitchBLL
            // 
            this.btnSwitchBLL.BackColor = System.Drawing.Color.LightBlue;
            this.btnSwitchBLL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSwitchBLL.Location = new System.Drawing.Point(50, 290);
            this.btnSwitchBLL.Name = "btnSwitchBLL";
            this.btnSwitchBLL.Size = new System.Drawing.Size(480, 45);
            this.btnSwitchBLL.TabIndex = 2;
            this.btnSwitchBLL.Text = "Switch BLL Implementation (LINQ ↔ SP)";
            this.btnSwitchBLL.UseVisualStyleBackColor = false;
            this.btnSwitchBLL.Click += new System.EventHandler(this.btnSwitchBLL_Click);
            // 
            // lblBLLStatus
            // 
            this.lblBLLStatus.AutoSize = false;
            this.lblBLLStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblBLLStatus.Location = new System.Drawing.Point(50, 345);
            this.lblBLLStatus.Name = "lblBLLStatus";
            this.lblBLLStatus.Size = new System.Drawing.Size(480, 25);
            this.lblBLLStatus.TabIndex = 3;
            this.lblBLLStatus.Text = "Current Implementation: LINQ";
            this.lblBLLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(230, 385);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(120, 35);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.lblBLLStatus);
            this.Controls.Add(this.btnSwitchBLL);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Management System - Main Menu";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
