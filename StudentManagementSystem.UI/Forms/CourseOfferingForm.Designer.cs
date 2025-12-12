namespace StudentManagementSystem.UI.Forms
{
    partial class CourseOfferingForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox grpOfferingList;
        private System.Windows.Forms.DataGridView dgvOfferings;

        private System.Windows.Forms.GroupBox grpOfferingDetails;
        private System.Windows.Forms.Label lblCourse;
        private System.Windows.Forms.ComboBox cmbCourse;
        private System.Windows.Forms.Label lblSemester;
        private System.Windows.Forms.ComboBox cmbSemester;
        private System.Windows.Forms.Label lblMaxCapacity;
        private System.Windows.Forms.NumericUpDown numMaxCapacity;
        private System.Windows.Forms.Label lblCurrentEnrollment;
        private System.Windows.Forms.NumericUpDown numCurrentEnrollment;

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.grpOfferingList = new System.Windows.Forms.GroupBox();
            this.dgvOfferings = new System.Windows.Forms.DataGridView();
            this.grpOfferingDetails = new System.Windows.Forms.GroupBox();
            this.lblCourse = new System.Windows.Forms.Label();
            this.cmbCourse = new System.Windows.Forms.ComboBox();
            this.lblSemester = new System.Windows.Forms.Label();
            this.cmbSemester = new System.Windows.Forms.ComboBox();
            this.lblMaxCapacity = new System.Windows.Forms.Label();
            this.numMaxCapacity = new System.Windows.Forms.NumericUpDown();
            this.lblCurrentEnrollment = new System.Windows.Forms.Label();
            this.numCurrentEnrollment = new System.Windows.Forms.NumericUpDown();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpOfferingList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferings)).BeginInit();
            this.grpOfferingDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCapacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentEnrollment)).BeginInit();
            this.SuspendLayout();
            // 
            // grpOfferingList
            // 
            this.grpOfferingList.Controls.Add(this.dgvOfferings);
            this.grpOfferingList.Location = new System.Drawing.Point(12, 12);
            this.grpOfferingList.Name = "grpOfferingList";
            this.grpOfferingList.Size = new System.Drawing.Size(760, 250);
            this.grpOfferingList.TabIndex = 0;
            this.grpOfferingList.TabStop = false;
            this.grpOfferingList.Text = "Course Offering List";
            // 
            // dgvOfferings
            // 
            this.dgvOfferings.AllowUserToAddRows = false;
            this.dgvOfferings.AllowUserToDeleteRows = false;
            this.dgvOfferings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOfferings.Location = new System.Drawing.Point(15, 25);
            this.dgvOfferings.MultiSelect = false;
            this.dgvOfferings.Name = "dgvOfferings";
            this.dgvOfferings.ReadOnly = true;
            this.dgvOfferings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOfferings.Size = new System.Drawing.Size(730, 210);
            this.dgvOfferings.TabIndex = 0;
            this.dgvOfferings.SelectionChanged += new System.EventHandler(this.dgvOfferings_SelectionChanged);
            // 
            // grpOfferingDetails
            // 
            this.grpOfferingDetails.Controls.Add(this.lblCourse);
            this.grpOfferingDetails.Controls.Add(this.cmbCourse);
            this.grpOfferingDetails.Controls.Add(this.lblSemester);
            this.grpOfferingDetails.Controls.Add(this.cmbSemester);
            this.grpOfferingDetails.Controls.Add(this.lblMaxCapacity);
            this.grpOfferingDetails.Controls.Add(this.numMaxCapacity);
            this.grpOfferingDetails.Controls.Add(this.lblCurrentEnrollment);
            this.grpOfferingDetails.Controls.Add(this.numCurrentEnrollment);
            this.grpOfferingDetails.Location = new System.Drawing.Point(12, 275);
            this.grpOfferingDetails.Name = "grpOfferingDetails";
            this.grpOfferingDetails.Size = new System.Drawing.Size(760, 130);
            this.grpOfferingDetails.TabIndex = 1;
            this.grpOfferingDetails.TabStop = false;
            this.grpOfferingDetails.Text = "Offering Details";
            // 
            // lblCourse
            // 
            this.lblCourse.AutoSize = true;
            this.lblCourse.Location = new System.Drawing.Point(15, 30);
            this.lblCourse.Name = "lblCourse";
            this.lblCourse.Size = new System.Drawing.Size(43, 13);
            this.lblCourse.TabIndex = 0;
            this.lblCourse.Text = "Course:";
            // 
            // cmbCourse
            // 
            this.cmbCourse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCourse.FormattingEnabled = true;
            this.cmbCourse.Location = new System.Drawing.Point(100, 27);
            this.cmbCourse.Name = "cmbCourse";
            this.cmbCourse.Size = new System.Drawing.Size(260, 21);
            this.cmbCourse.TabIndex = 0;
            // 
            // lblSemester
            // 
            this.lblSemester.AutoSize = true;
            this.lblSemester.Location = new System.Drawing.Point(390, 30);
            this.lblSemester.Name = "lblSemester";
            this.lblSemester.Size = new System.Drawing.Size(55, 13);
            this.lblSemester.TabIndex = 2;
            this.lblSemester.Text = "Semester:";
            // 
            // cmbSemester
            // 
            this.cmbSemester.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSemester.FormattingEnabled = true;
            this.cmbSemester.Location = new System.Drawing.Point(460, 27);
            this.cmbSemester.Name = "cmbSemester";
            this.cmbSemester.Size = new System.Drawing.Size(230, 21);
            this.cmbSemester.TabIndex = 1;
            // 
            // lblMaxCapacity
            // 
            this.lblMaxCapacity.AutoSize = true;
            this.lblMaxCapacity.Location = new System.Drawing.Point(15, 75);
            this.lblMaxCapacity.Name = "lblMaxCapacity";
            this.lblMaxCapacity.Size = new System.Drawing.Size(75, 13);
            this.lblMaxCapacity.TabIndex = 4;
            this.lblMaxCapacity.Text = "Max Capacity:";
            // 
            // numMaxCapacity
            // 
            this.numMaxCapacity.Location = new System.Drawing.Point(100, 73);
            this.numMaxCapacity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMaxCapacity.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxCapacity.Name = "numMaxCapacity";
            this.numMaxCapacity.Size = new System.Drawing.Size(120, 20);
            this.numMaxCapacity.TabIndex = 2;
            this.numMaxCapacity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // lblCurrentEnrollment
            // 
            this.lblCurrentEnrollment.AutoSize = true;
            this.lblCurrentEnrollment.Location = new System.Drawing.Point(260, 75);
            this.lblCurrentEnrollment.Name = "lblCurrentEnrollment";
            this.lblCurrentEnrollment.Size = new System.Drawing.Size(98, 13);
            this.lblCurrentEnrollment.TabIndex = 6;
            this.lblCurrentEnrollment.Text = "Current Enrollment:";
            // 
            // numCurrentEnrollment
            // 
            this.numCurrentEnrollment.Location = new System.Drawing.Point(370, 73);
            this.numCurrentEnrollment.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCurrentEnrollment.Name = "numCurrentEnrollment";
            this.numCurrentEnrollment.Size = new System.Drawing.Size(120, 20);
            this.numCurrentEnrollment.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.LightGreen;
            this.btnAdd.Location = new System.Drawing.Point(100, 430);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(120, 35);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Add Offering";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.LightBlue;
            this.btnUpdate.Location = new System.Drawing.Point(250, 430);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(120, 35);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update Offering";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.btnDelete.Location = new System.Drawing.Point(400, 430);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 35);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete Offering";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(550, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 35);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // CourseOfferingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 481);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.grpOfferingDetails);
            this.Controls.Add(this.grpOfferingList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "CourseOfferingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Course Offering Management";
            this.grpOfferingList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOfferings)).EndInit();
            this.grpOfferingDetails.ResumeLayout(false);
            this.grpOfferingDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxCapacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCurrentEnrollment)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
