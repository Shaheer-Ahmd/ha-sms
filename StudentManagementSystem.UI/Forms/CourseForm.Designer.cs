namespace StudentManagementSystem.UI.Forms
{
    partial class CourseForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvCourses;
        private System.Windows.Forms.TextBox txtCourseCode;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.NumericUpDown numCredits;
        private System.Windows.Forms.ComboBox cmbDepartment;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCourseCode;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblCredits;
        private System.Windows.Forms.Label lblDepartment;
        private System.Windows.Forms.GroupBox grpCourseDetails;
        private System.Windows.Forms.GroupBox grpCourseList;
        private System.Windows.Forms.GroupBox grpPrerequisites;
        private System.Windows.Forms.DataGridView dgvPrerequisites;
        private System.Windows.Forms.ComboBox cmbPrerequisiteCourse;
        private System.Windows.Forms.Button btnAddPrerequisite;
        private System.Windows.Forms.Button btnRemovePrerequisite;
        private System.Windows.Forms.Label lblPrerequisiteList;
        private System.Windows.Forms.Label lblAddPrerequisite;

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
            this.dgvCourses = new System.Windows.Forms.DataGridView();
            this.txtCourseCode = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.numCredits = new System.Windows.Forms.NumericUpDown();
            this.cmbDepartment = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCourseCode = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblCredits = new System.Windows.Forms.Label();
            this.lblDepartment = new System.Windows.Forms.Label();
            this.grpCourseDetails = new System.Windows.Forms.GroupBox();
            this.grpCourseList = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCredits)).BeginInit();
            this.grpCourseDetails.SuspendLayout();
            this.grpCourseList.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpCourseList
            // 
            this.grpCourseList.Controls.Add(this.dgvCourses);
            this.grpCourseList.Location = new System.Drawing.Point(12, 12);
            this.grpCourseList.Name = "grpCourseList";
            this.grpCourseList.Size = new System.Drawing.Size(760, 250);
            this.grpCourseList.TabIndex = 0;
            this.grpCourseList.TabStop = false;
            this.grpCourseList.Text = "Course List";
            // 
            // dgvCourses
            // 
            this.dgvCourses.AllowUserToAddRows = false;
            this.dgvCourses.AllowUserToDeleteRows = false;
            this.dgvCourses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCourses.Location = new System.Drawing.Point(15, 25);
            this.dgvCourses.MultiSelect = false;
            this.dgvCourses.Name = "dgvCourses";
            this.dgvCourses.ReadOnly = true;
            this.dgvCourses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCourses.Size = new System.Drawing.Size(730, 210);
            this.dgvCourses.TabIndex = 0;
            this.dgvCourses.SelectionChanged += new System.EventHandler(this.dgvCourses_SelectionChanged);
            // 
            // grpCourseDetails
            // 
            this.grpCourseDetails.Controls.Add(this.lblCourseCode);
            this.grpCourseDetails.Controls.Add(this.txtCourseCode);
            this.grpCourseDetails.Controls.Add(this.lblTitle);
            this.grpCourseDetails.Controls.Add(this.txtTitle);
            this.grpCourseDetails.Controls.Add(this.lblDescription);
            this.grpCourseDetails.Controls.Add(this.txtDescription);
            this.grpCourseDetails.Controls.Add(this.lblCredits);
            this.grpCourseDetails.Controls.Add(this.numCredits);
            this.grpCourseDetails.Controls.Add(this.lblDepartment);
            this.grpCourseDetails.Controls.Add(this.cmbDepartment);
            this.grpCourseDetails.Location = new System.Drawing.Point(12, 275);
            this.grpCourseDetails.Name = "grpCourseDetails";
            this.grpCourseDetails.Size = new System.Drawing.Size(760, 150);
            this.grpCourseDetails.TabIndex = 1;
            this.grpCourseDetails.TabStop = false;
            this.grpCourseDetails.Text = "Course Details";
            // 
            // grpPrerequisites
            // 
            this.grpPrerequisites = new System.Windows.Forms.GroupBox();
            this.dgvPrerequisites = new System.Windows.Forms.DataGridView();
            this.cmbPrerequisiteCourse = new System.Windows.Forms.ComboBox();
            this.btnAddPrerequisite = new System.Windows.Forms.Button();
            this.btnRemovePrerequisite = new System.Windows.Forms.Button();
            this.lblPrerequisiteList = new System.Windows.Forms.Label();
            this.lblAddPrerequisite = new System.Windows.Forms.Label();

            this.grpPrerequisites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrerequisites)).BeginInit();

            // 
            // grpPrerequisites
            // 
            this.grpPrerequisites.Controls.Add(this.lblPrerequisiteList);
            this.grpPrerequisites.Controls.Add(this.dgvPrerequisites);
            this.grpPrerequisites.Controls.Add(this.lblAddPrerequisite);
            this.grpPrerequisites.Controls.Add(this.cmbPrerequisiteCourse);
            this.grpPrerequisites.Controls.Add(this.btnAddPrerequisite);
            this.grpPrerequisites.Controls.Add(this.btnRemovePrerequisite);
            this.grpPrerequisites.Location = new System.Drawing.Point(12, 440);
            this.grpPrerequisites.Name = "grpPrerequisites";
            this.grpPrerequisites.Size = new System.Drawing.Size(760, 170);
            this.grpPrerequisites.TabIndex = 2;
            this.grpPrerequisites.TabStop = false;
            this.grpPrerequisites.Text = "Prerequisites";

            // 
            // lblPrerequisiteList
            // 
            this.lblPrerequisiteList.AutoSize = true;
            this.lblPrerequisiteList.Location = new System.Drawing.Point(15, 25);
            this.lblPrerequisiteList.Name = "lblPrerequisiteList";
            this.lblPrerequisiteList.Size = new System.Drawing.Size(106, 13);
            this.lblPrerequisiteList.TabIndex = 0;
            this.lblPrerequisiteList.Text = "Existing prerequisites";

            // 
            // dgvPrerequisites
            // 
            this.dgvPrerequisites.AllowUserToAddRows = false;
            this.dgvPrerequisites.AllowUserToDeleteRows = false;
            this.dgvPrerequisites.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrerequisites.Location = new System.Drawing.Point(15, 45);
            this.dgvPrerequisites.MultiSelect = false;
            this.dgvPrerequisites.Name = "dgvPrerequisites";
            this.dgvPrerequisites.ReadOnly = true;
            this.dgvPrerequisites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPrerequisites.Size = new System.Drawing.Size(430, 110);
            this.dgvPrerequisites.TabIndex = 1;

            // 
            // lblAddPrerequisite
            // 
            this.lblAddPrerequisite.AutoSize = true;
            this.lblAddPrerequisite.Location = new System.Drawing.Point(465, 25);
            this.lblAddPrerequisite.Name = "lblAddPrerequisite";
            this.lblAddPrerequisite.Size = new System.Drawing.Size(88, 13);
            this.lblAddPrerequisite.TabIndex = 2;
            this.lblAddPrerequisite.Text = "Add prerequisite:";

            // 
            // cmbPrerequisiteCourse
            // 
            this.cmbPrerequisiteCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrerequisiteCourse.FormattingEnabled = true;
            this.cmbPrerequisiteCourse.Location = new System.Drawing.Point(468, 45);
            this.cmbPrerequisiteCourse.Name = "cmbPrerequisiteCourse";
            this.cmbPrerequisiteCourse.Size = new System.Drawing.Size(270, 21);
            this.cmbPrerequisiteCourse.TabIndex = 3;

            // 
            // btnAddPrerequisite
            // 
            this.btnAddPrerequisite.BackColor = System.Drawing.Color.Honeydew;
            this.btnAddPrerequisite.Location = new System.Drawing.Point(468, 80);
            this.btnAddPrerequisite.Name = "btnAddPrerequisite";
            this.btnAddPrerequisite.Size = new System.Drawing.Size(120, 30);
            this.btnAddPrerequisite.TabIndex = 4;
            this.btnAddPrerequisite.Text = "Add";
            this.btnAddPrerequisite.UseVisualStyleBackColor = false;
            this.btnAddPrerequisite.Click += new System.EventHandler(this.btnAddPrerequisite_Click);

            // 
            // btnRemovePrerequisite
            // 
            this.btnRemovePrerequisite.BackColor = System.Drawing.Color.MistyRose;
            this.btnRemovePrerequisite.Location = new System.Drawing.Point(618, 80);
            this.btnRemovePrerequisite.Name = "btnRemovePrerequisite";
            this.btnRemovePrerequisite.Size = new System.Drawing.Size(120, 30);
            this.btnRemovePrerequisite.TabIndex = 5;
            this.btnRemovePrerequisite.Text = "Remove selected";
            this.btnRemovePrerequisite.UseVisualStyleBackColor = false;
            this.btnRemovePrerequisite.Click += new System.EventHandler(this.btnRemovePrerequisite_Click);

            ((System.ComponentModel.ISupportInitialize)(this.dgvPrerequisites)).EndInit();
            this.grpPrerequisites.ResumeLayout(false);
            this.grpPrerequisites.PerformLayout();
            // 
            // lblCourseCode
            // 
            this.lblCourseCode.AutoSize = true;
            this.lblCourseCode.Location = new System.Drawing.Point(15, 30);
            this.lblCourseCode.Name = "lblCourseCode";
            this.lblCourseCode.Size = new System.Drawing.Size(71, 13);
            this.lblCourseCode.TabIndex = 0;
            this.lblCourseCode.Text = "Course Code:";
            // 
            // txtCourseCode
            // 
            this.txtCourseCode.Location = new System.Drawing.Point(100, 27);
            this.txtCourseCode.MaxLength = 255;
            this.txtCourseCode.Name = "txtCourseCode";
            this.txtCourseCode.Size = new System.Drawing.Size(150, 20);
            this.txtCourseCode.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(280, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(30, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "Title:";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(330, 27);
            this.txtTitle.MaxLength = 255;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(400, 20);
            this.txtTitle.TabIndex = 1;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(15, 60);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 4;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(100, 57);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(630, 45);
            this.txtDescription.TabIndex = 2;
            // 
            // lblCredits
            // 
            this.lblCredits.AutoSize = true;
            this.lblCredits.Location = new System.Drawing.Point(15, 115);
            this.lblCredits.Name = "lblCredits";
            this.lblCredits.Size = new System.Drawing.Size(42, 13);
            this.lblCredits.TabIndex = 6;
            this.lblCredits.Text = "Credits:";
            // 
            // numCredits
            // 
            this.numCredits.Location = new System.Drawing.Point(100, 113);
            this.numCredits.Maximum = new decimal(new int[] { 6, 0, 0, 0 });
            this.numCredits.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.numCredits.Name = "numCredits";
            this.numCredits.Size = new System.Drawing.Size(80, 20);
            this.numCredits.TabIndex = 3;
            this.numCredits.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblDepartment
            // 
            this.lblDepartment.AutoSize = true;
            this.lblDepartment.Location = new System.Drawing.Point(240, 115);
            this.lblDepartment.Name = "lblDepartment";
            this.lblDepartment.Size = new System.Drawing.Size(65, 13);
            this.lblDepartment.TabIndex = 8;
            this.lblDepartment.Text = "Department:";
            // 
            // cmbDepartment
            // 
            this.cmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepartment.FormattingEnabled = true;
            this.cmbDepartment.Location = new System.Drawing.Point(315, 112);
            this.cmbDepartment.Name = "cmbDepartment";
            this.cmbDepartment.Size = new System.Drawing.Size(250, 21);
            this.cmbDepartment.TabIndex = 4;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.Location = new System.Drawing.Point(100, 440);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add Course";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightBlue;
            this.btnUpdate.Location = new System.Drawing.Point(250, 440);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 35);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update Course";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.Location = new System.Drawing.Point(400, 440);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 35);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete Course";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(550, 440);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CourseForm
            // 
            // CourseForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // increase height for new group
            this.ClientSize = new System.Drawing.Size(784, 661);

            this.Controls.Add(this.grpPrerequisites);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpCourseDetails);
            this.Controls.Add(this.grpCourseList);

            // move buttons down below prereq group
            this.btnAdd.Location = new System.Drawing.Point(100, 630);
            this.btnUpdate.Location = new System.Drawing.Point(250, 630);
            this.btnDelete.Location = new System.Drawing.Point(400, 630);
            this.btnClose.Location = new System.Drawing.Point(550, 630);
            
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CourseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Course Management";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCourses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCredits)).EndInit();
            this.grpCourseDetails.ResumeLayout(false);
            this.grpCourseDetails.PerformLayout();
            this.grpCourseList.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
