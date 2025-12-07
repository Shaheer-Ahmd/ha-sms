namespace StudentManagementSystem.UI.Forms
{
    partial class EnrollmentForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox cmbStudent;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.ComboBox cmbGrade;
        private System.Windows.Forms.DataGridView dgvOfferings;
        private System.Windows.Forms.DataGridView dgvEnrollments;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Button btnUpdateGrade;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblStudent;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.Label lblGrade;
        private System.Windows.Forms.GroupBox grpRegistration;
        private System.Windows.Forms.GroupBox grpEnrollments;

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
            this.cmbStudent = new System.Windows.Forms.ComboBox();
            this.cmbSemester = new System.Windows.Forms.ComboBox();
            this.cmbGrade = new System.Windows.Forms.ComboBox();
            this.dgvOfferings = new System.Windows.Forms.DataGridView();
            this.dgvEnrollments = new System.Windows.Forms.DataGridView();
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnUpdateGrade = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblStudent = new System.Windows.Forms.Label();
            this.lblSemester = new System.Windows.Forms.Label();
            this.lblGrade = new System.Windows.Forms.Label();
            this.grpRegistration = new System.Windows.Forms.GroupBox();
            this.grpEnrollments = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).BeginInit();
            this.grpRegistration.SuspendLayout();
            this.grpEnrollments.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpRegistration
            // 
            this.grpRegistration.Controls.Add(this.lblSemester);
            this.grpRegistration.Controls.Add(this.cmbSemester);
            this.grpRegistration.Controls.Add(this.dgvOfferings);
            this.grpRegistration.Controls.Add(this.btnRegister);
            this.grpRegistration.Location = new System.Drawing.Point(12, 60);
            this.grpRegistration.Name = "grpRegistration";
            this.grpRegistration.Size = new System.Drawing.Size(760, 280);
            this.grpRegistration.TabIndex = 1;
            this.grpRegistration.TabStop = false;
            this.grpRegistration.Text = "Available Course Offerings";
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(15, 25);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(54, 13);
            this.lblSemester.TabIndex = 0;
            this.lblSemester.Text = "Semester:";
            // 
            // cmbSemester
            // 
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.Location = new System.Drawing.Point(80, 22);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(200, 21);
            this.cmbSemester.TabIndex = 0;
            this.cmbSemester.SelectedIndexChanged += new System.EventHandler(this.cmbSemester_SelectedIndexChanged);
            // 
            // dgvOfferings
            // 
            this.dgvOfferings.AllowUserToAddRows = false;
            this.dgvOfferings.AllowUserToDeleteRows = false;
            this.dgvOfferings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOfferings.Location = new System.Drawing.Point(15, 55);
            this.dgvOfferings.MultiSelect = false;
            this.dgvOfferings.Name = "dgvOfferings";
            this.dgvOfferings.ReadOnly = true;
            this.dgvOfferings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOfferings.Size = new System.Drawing.Size(730, 170);
            this.dgvOfferings.TabIndex = 1;
            // 
            // btnRegister
            // 
            this.btnRegister.BackColor = System.Drawing.Color.LightGreen;
            this.btnRegister.Location = new System.Drawing.Point(300, 230);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(160, 35);
            this.btnRegister.TabIndex = 2;
            this.btnRegister.Text = "Register for Selected Course";
            this.btnRegister.UseVisualStyleBackColor = false;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblStudent
            // 
            this.lblStudent.AutoSize = true;
            this.lblStudent.Location = new System.Drawing.Point(15, 20);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(47, 13);
            this.lblStudent.TabIndex = 0;
            this.lblStudent.Text = "Student:";
            // 
            // cmbStudent
            // 
            this.cmbStudent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStudent.FormattingEnabled = true;
            this.cmbStudent.Location = new System.Drawing.Point(75, 17);
            this.cmbStudent.Name = "cmbStudent";
            this.cmbStudent.Size = new System.Drawing.Size(400, 21);
            this.cmbStudent.TabIndex = 0;
            this.cmbStudent.SelectedIndexChanged += new System.EventHandler(this.cmbStudent_SelectedIndexChanged);
            // 
            // grpEnrollments
            // 
            this.grpEnrollments.Controls.Add(this.dgvEnrollments);
            this.grpEnrollments.Controls.Add(this.lblGrade);
            this.grpEnrollments.Controls.Add(this.cmbGrade);
            this.grpEnrollments.Controls.Add(this.btnUpdateGrade);
            this.grpEnrollments.Location = new System.Drawing.Point(12, 355);
            this.grpEnrollments.Name = "grpEnrollments";
            this.grpEnrollments.Size = new System.Drawing.Size(760, 200);
            this.grpEnrollments.TabIndex = 2;
            this.grpEnrollments.TabStop = false;
            this.grpEnrollments.Text = "Student Enrollments";
            // 
            // dgvEnrollments
            // 
            this.dgvEnrollments.AllowUserToAddRows = false;
            this.dgvEnrollments.AllowUserToDeleteRows = false;
            this.dgvEnrollments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEnrollments.Location = new System.Drawing.Point(15, 25);
            this.dgvEnrollments.MultiSelect = false;
            this.dgvEnrollments.Name = "dgvEnrollments";
            this.dgvEnrollments.ReadOnly = true;
            this.dgvEnrollments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEnrollments.Size = new System.Drawing.Size(730, 120);
            this.dgvEnrollments.TabIndex = 0;
            // 
            // lblGrade
            // 
            this.lblGrade.AutoSize = true;
            this.lblGrade.Location = new System.Drawing.Point(200, 160);
            this.lblGrade.Name = "lblGrade";
            this.lblGrade.Size = new System.Drawing.Size(40, 13);
            this.lblGrade.TabIndex = 1;
            this.lblGrade.Text = "Grade:";
            // 
            // cmbGrade
            // 
            this.cmbGrade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrade.FormattingEnabled = true;
            this.cmbGrade.Location = new System.Drawing.Point(250, 157);
            this.cmbGrade.Name = "cmbGrade";
            this.cmbGrade.Size = new System.Drawing.Size(100, 21);
            this.cmbGrade.TabIndex = 1;
            // 
            // btnUpdateGrade
            // 
            this.btnUpdateGrade.BackColor = System.Drawing.Color.LightBlue;
            this.btnUpdateGrade.Location = new System.Drawing.Point(370, 152);
            this.btnUpdateGrade.Name = "btnUpdateGrade";
            this.btnUpdateGrade.Size = new System.Drawing.Size(120, 30);
            this.btnUpdateGrade.TabIndex = 2;
            this.btnUpdateGrade.Text = "Update Grade";
            this.btnUpdateGrade.UseVisualStyleBackColor = false;
            this.btnUpdateGrade.Click += new System.EventHandler(this.btnUpdateGrade_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(340, 570);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // EnrollmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 621);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grpEnrollments);
            this.Controls.Add(this.lblStudent);
            this.Controls.Add(this.cmbStudent);
            this.Controls.Add(this.grpRegistration);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "EnrollmentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enrollment & Registration Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEnrollments)).EndInit();
            this.grpRegistration.ResumeLayout(false);
            this.grpRegistration.PerformLayout();
            this.grpEnrollments.ResumeLayout(false);
            this.grpEnrollments.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
