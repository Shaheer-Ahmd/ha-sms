namespace StudentManagementSystem.UI.Forms
{
    partial class StudentHoldForm
    {
        /// <inheritdoc />
        private System.ComponentModel.IContainer? components = null;

        private System.Windows.Forms.DataGridView dgvHolds = null!;
        private System.Windows.Forms.TextBox txtHoldId = null!;
        private System.Windows.Forms.TextBox txtStudentId = null!;
        private System.Windows.Forms.ComboBox cmbHoldType = null!;
        private System.Windows.Forms.TextBox txtReason = null!;
        private System.Windows.Forms.DateTimePicker dtpDateApplied = null!;
        private System.Windows.Forms.CheckBox chkIsActive = null!;
        private System.Windows.Forms.Button btnAdd = null!;
        private System.Windows.Forms.Button btnUpdate = null!;
        private System.Windows.Forms.Button btnDelete = null!;
        private System.Windows.Forms.Button btnClose = null!;
        private System.Windows.Forms.Button btnRefresh = null!;
        private System.Windows.Forms.Label lblHoldId = null!;
        private System.Windows.Forms.Label lblStudentId = null!;
        private System.Windows.Forms.Label lblHoldType = null!;
        private System.Windows.Forms.Label lblReason = null!;
        private System.Windows.Forms.Label lblDateApplied = null!;

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.dgvHolds = new System.Windows.Forms.DataGridView();
            this.txtHoldId = new System.Windows.Forms.TextBox();
            this.txtStudentId = new System.Windows.Forms.TextBox();
            this.cmbHoldType = new System.Windows.Forms.ComboBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.dtpDateApplied = new System.Windows.Forms.DateTimePicker();
            this.chkIsActive = new System.Windows.Forms.CheckBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblHoldId = new System.Windows.Forms.Label();
            this.lblStudentId = new System.Windows.Forms.Label();
            this.lblHoldType = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.lblDateApplied = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvHolds)).BeginInit();
            this.SuspendLayout();

            // 
            // dgvHolds
            // 
            this.dgvHolds.AllowUserToAddRows = false;
            this.dgvHolds.AllowUserToDeleteRows = false;
            this.dgvHolds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                    | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvHolds.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHolds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHolds.Location = new System.Drawing.Point(12, 12);
            this.dgvHolds.MultiSelect = false;
            this.dgvHolds.Name = "dgvHolds";
            this.dgvHolds.ReadOnly = true;
            this.dgvHolds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHolds.Size = new System.Drawing.Size(760, 220);
            this.dgvHolds.TabIndex = 0;
            this.dgvHolds.SelectionChanged += new System.EventHandler(this.dgvHolds_SelectionChanged);

            // 
            // lblHoldId
            // 
            this.lblHoldId.AutoSize = true;
            this.lblHoldId.Location = new System.Drawing.Point(12, 245);
            this.lblHoldId.Name = "lblHoldId";
            this.lblHoldId.Size = new System.Drawing.Size(47, 13);
            this.lblHoldId.TabIndex = 1;
            this.lblHoldId.Text = "Hold ID:";

            // 
            // txtHoldId
            // 
            this.txtHoldId.Location = new System.Drawing.Point(100, 242);
            this.txtHoldId.Name = "txtHoldId";
            this.txtHoldId.ReadOnly = true;
            this.txtHoldId.Size = new System.Drawing.Size(100, 20);
            this.txtHoldId.TabIndex = 2;

            // 
            // lblStudentId
            // 
            this.lblStudentId.AutoSize = true;
            this.lblStudentId.Location = new System.Drawing.Point(12, 275);
            this.lblStudentId.Name = "lblStudentId";
            this.lblStudentId.Size = new System.Drawing.Size(61, 13);
            this.lblStudentId.TabIndex = 3;
            this.lblStudentId.Text = "Student ID:";

            // 
            // txtStudentId
            // 
            this.txtStudentId.Location = new System.Drawing.Point(100, 272);
            this.txtStudentId.Name = "txtStudentId";
            this.txtStudentId.Size = new System.Drawing.Size(100, 20);
            this.txtStudentId.TabIndex = 4;

            // 
            // lblHoldType
            // 
            this.lblHoldType.AutoSize = true;
            this.lblHoldType.Location = new System.Drawing.Point(12, 305);
            this.lblHoldType.Name = "lblHoldType";
            this.lblHoldType.Size = new System.Drawing.Size(59, 13);
            this.lblHoldType.TabIndex = 5;
            this.lblHoldType.Text = "Hold Type:";

            // 
            // cmbHoldType
            // 
            this.cmbHoldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHoldType.FormattingEnabled = true;
            this.cmbHoldType.Items.AddRange(new object[] {
                "Financial",
                "Academic",
                "Disciplinary",
                "Administrative"
            });
            this.cmbHoldType.Location = new System.Drawing.Point(100, 302);
            this.cmbHoldType.Name = "cmbHoldType";
            this.cmbHoldType.Size = new System.Drawing.Size(150, 21);
            this.cmbHoldType.TabIndex = 6;

            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(280, 245);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(47, 13);
            this.lblReason.TabIndex = 7;
            this.lblReason.Text = "Reason:";

            // 
            // txtReason
            // 
            this.txtReason.Location = new System.Drawing.Point(340, 242);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReason.Size = new System.Drawing.Size(250, 81);
            this.txtReason.TabIndex = 8;

            // 
            // lblDateApplied
            // 
            this.lblDateApplied.AutoSize = true;
            this.lblDateApplied.Location = new System.Drawing.Point(12, 338);
            this.lblDateApplied.Name = "lblDateApplied";
            this.lblDateApplied.Size = new System.Drawing.Size(72, 13);
            this.lblDateApplied.TabIndex = 9;
            this.lblDateApplied.Text = "Date Applied:";

            // 
            // dtpDateApplied
            // 
            this.dtpDateApplied.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpDateApplied.Location = new System.Drawing.Point(100, 335);
            this.dtpDateApplied.Name = "dtpDateApplied";
            this.dtpDateApplied.Size = new System.Drawing.Size(100, 20);
            this.dtpDateApplied.TabIndex = 10;

            // 
            // chkIsActive
            // 
            this.chkIsActive.AutoSize = true;
            this.chkIsActive.Location = new System.Drawing.Point(340, 335);
            this.chkIsActive.Name = "chkIsActive";
            this.chkIsActive.Size = new System.Drawing.Size(67, 17);
            this.chkIsActive.TabIndex = 11;
            this.chkIsActive.Text = "Is Active";
            this.chkIsActive.UseVisualStyleBackColor = true;

            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(620, 242);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 28);
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(620, 276);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 28);
            this.btnAdd.TabIndex = 13;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(700, 276);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(70, 28);
            this.btnUpdate.TabIndex = 14;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);

            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(620, 310);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 28);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(700, 310);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(70, 28);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // 
            // StudentHoldForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 371);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.chkIsActive);
            this.Controls.Add(this.dtpDateApplied);
            this.Controls.Add(this.lblDateApplied);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.cmbHoldType);
            this.Controls.Add(this.lblHoldType);
            this.Controls.Add(this.txtStudentId);
            this.Controls.Add(this.lblStudentId);
            this.Controls.Add(this.txtHoldId);
            this.Controls.Add(this.lblHoldId);
            this.Controls.Add(this.dgvHolds);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "StudentHoldForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Student Holds";
            this.Load += new System.EventHandler(this.StudentHoldForm_Load);

            ((System.ComponentModel.ISupportInitialize)(this.dgvHolds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
