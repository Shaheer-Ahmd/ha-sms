namespace StudentManagementSystem.UI.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // UI Controls
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TableLayoutPanel gridMenu;
        private System.Windows.Forms.Panel pnlFooter;
        
        // Menu Buttons
        private System.Windows.Forms.Button btnStudents;
        private System.Windows.Forms.Button btnCourses;
        private System.Windows.Forms.Button btnEnrollments;
        private System.Windows.Forms.Button btnDepartments;
        private System.Windows.Forms.Button btnCourseOfferings;
        private System.Windows.Forms.Button btnStudentHolds;
        
        // Utility Buttons
        private System.Windows.Forms.Button btnViewGradeAudit;
        private System.Windows.Forms.Button btnSwitchBLL;
        private System.Windows.Forms.Button btnExit;
        
        // Labels
        private System.Windows.Forms.Label lblBLLStatus;

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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.gridMenu = new System.Windows.Forms.TableLayoutPanel();
            this.btnStudents = new System.Windows.Forms.Button();
            this.btnCourses = new System.Windows.Forms.Button();
            this.btnEnrollments = new System.Windows.Forms.Button();
            this.btnDepartments = new System.Windows.Forms.Button();
            this.btnCourseOfferings = new System.Windows.Forms.Button();
            this.btnStudentHolds = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnViewGradeAudit = new System.Windows.Forms.Button();
            this.btnSwitchBLL = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblBLLStatus = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.gridMenu.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            
            // 
            // MAIN FORM SETTINGS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(800, 600); // Increased size for better spacing
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Student Management System - Dashboard";
            
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(62)))), ((int)(((byte)(80))))); // Dark Slate
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 80);
            this.pnlHeader.TabIndex = 0;
            
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Light", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(800, 80);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Student Management System";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            
            // 
            // gridMenu (TableLayoutPanel for Alignment)
            // 
            this.gridMenu.ColumnCount = 2;
            this.gridMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridMenu.Controls.Add(this.btnStudents, 0, 0);
            this.gridMenu.Controls.Add(this.btnCourses, 1, 0);
            this.gridMenu.Controls.Add(this.btnEnrollments, 0, 1);
            this.gridMenu.Controls.Add(this.btnDepartments, 1, 1);
            this.gridMenu.Controls.Add(this.btnCourseOfferings, 0, 2);
            this.gridMenu.Controls.Add(this.btnStudentHolds, 1, 2);
            this.gridMenu.Location = new System.Drawing.Point(50, 100);
            this.gridMenu.Name = "gridMenu";
            this.gridMenu.RowCount = 3;
            this.gridMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.gridMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.gridMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.gridMenu.Size = new System.Drawing.Size(700, 230); // Width matches form padding
            this.gridMenu.TabIndex = 1;
            
            // Helper function to style menu buttons
            void StyleMenuButton(System.Windows.Forms.Button btn) {
                btn.Dock = System.Windows.Forms.DockStyle.Fill;
                btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219))))); // Modern Blue
                btn.ForeColor = System.Drawing.Color.White;
                btn.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
                btn.Margin = new System.Windows.Forms.Padding(10);
                btn.UseVisualStyleBackColor = false;
                btn.Cursor = System.Windows.Forms.Cursors.Hand;
            }

            // 
            // btnStudents
            // 
            this.btnStudents.Name = "btnStudents";
            this.btnStudents.Text = "Manage Students";
            StyleMenuButton(this.btnStudents);
            this.btnStudents.Click += new System.EventHandler(this.btnStudents_Click);
            
            // 
            // btnCourses
            // 
            this.btnCourses.Name = "btnCourses";
            this.btnCourses.Text = "Manage Courses";
            StyleMenuButton(this.btnCourses);
            this.btnCourses.Click += new System.EventHandler(this.btnCourses_Click);
            
            // 
            // btnEnrollments
            // 
            this.btnEnrollments.Name = "btnEnrollments";
            this.btnEnrollments.Text = "Manage Enrollments";
            StyleMenuButton(this.btnEnrollments);
            this.btnEnrollments.Click += new System.EventHandler(this.btnEnrollments_Click);
            
            // 
            // btnDepartments
            // 
            this.btnDepartments.Name = "btnDepartments";
            this.btnDepartments.Text = "Manage Departments";
            StyleMenuButton(this.btnDepartments);
            this.btnDepartments.Click += new System.EventHandler(this.btnDepartments_Click);
            
            // 
            // btnCourseOfferings
            // 
            this.btnCourseOfferings.Name = "btnCourseOfferings";
            this.btnCourseOfferings.Text = "Course Offerings";
            StyleMenuButton(this.btnCourseOfferings);
            this.btnCourseOfferings.Click += new System.EventHandler(this.btnCourseOfferings_Click);
            
            // 
            // btnStudentHolds
            // 
            this.btnStudentHolds.Name = "btnStudentHolds";
            this.btnStudentHolds.Text = "Student Holds";
            StyleMenuButton(this.btnStudentHolds);
            this.btnStudentHolds.Click += new System.EventHandler(this.btnStudentHolds_Click);

            // 
            // pnlFooter (Controls underneath the main menu)
            // 
            this.pnlFooter.Controls.Add(this.btnViewGradeAudit);
            this.pnlFooter.Controls.Add(this.lblBLLStatus);
            this.pnlFooter.Controls.Add(this.btnSwitchBLL);
            this.pnlFooter.Controls.Add(this.btnExit);
            this.pnlFooter.Location = new System.Drawing.Point(50, 350);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(700, 200);
            this.pnlFooter.TabIndex = 2;

            // 
            // btnViewGradeAudit
            // 
            this.btnViewGradeAudit.BackColor = System.Drawing.Color.White;
            this.btnViewGradeAudit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewGradeAudit.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnViewGradeAudit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnViewGradeAudit.Location = new System.Drawing.Point(10, 10);
            this.btnViewGradeAudit.Name = "btnViewGradeAudit";
            this.btnViewGradeAudit.Size = new System.Drawing.Size(680, 40);
            this.btnViewGradeAudit.TabIndex = 0;
            this.btnViewGradeAudit.Text = "View Grade Audit Logs";
            this.btnViewGradeAudit.UseVisualStyleBackColor = false;
            this.btnViewGradeAudit.Click += new System.EventHandler(this.btnViewGradeAudit_Click);

            // 
            // btnSwitchBLL
            // 
            this.btnSwitchBLL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18))))); // Flat Orange
            this.btnSwitchBLL.FlatAppearance.BorderSize = 0;
            this.btnSwitchBLL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwitchBLL.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSwitchBLL.ForeColor = System.Drawing.Color.White;
            this.btnSwitchBLL.Location = new System.Drawing.Point(10, 65);
            this.btnSwitchBLL.Name = "btnSwitchBLL";
            this.btnSwitchBLL.Size = new System.Drawing.Size(680, 40);
            this.btnSwitchBLL.TabIndex = 1;
            this.btnSwitchBLL.Text = "Switch Backend (LINQ ↔ Stored Procedure)";
            this.btnSwitchBLL.UseVisualStyleBackColor = false;
            this.btnSwitchBLL.Click += new System.EventHandler(this.btnSwitchBLL_Click);

            // 
            // lblBLLStatus
            // 
            this.lblBLLStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblBLLStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblBLLStatus.Location = new System.Drawing.Point(10, 108);
            this.lblBLLStatus.Name = "lblBLLStatus";
            this.lblBLLStatus.Size = new System.Drawing.Size(680, 20);
            this.lblBLLStatus.TabIndex = 2;
            this.lblBLLStatus.Text = "Current Implementation: LINQ";
            this.lblBLLStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60))))); // Flat Red
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(250, 140);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 40);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit System";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            
            // 
            // Add Controls
            // 
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.gridMenu);
            this.Controls.Add(this.pnlFooter);

            this.pnlHeader.ResumeLayout(false);
            this.gridMenu.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}