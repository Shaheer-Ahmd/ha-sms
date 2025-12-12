using System;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class GradeAuditForm : Form
    {
        private readonly IEnrollmentService _enrollmentService;

        public GradeAuditForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();
            _enrollmentService = bllFactory.GetEnrollmentService();
            LoadAuditEntries();
        }

        private void LoadAuditEntries()
        {
            try
            {
                var audits = _enrollmentService.GetGradeAuditLog();
                dgvAudit.DataSource = audits.Select(a => new
                {
                    a.AuditID,
                    a.EnrollmentID,
                    a.EnrollmentDate,
                    a.OldGrade,
                    a.NewGrade,
                    a.ChangeDate
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading grade audit log: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
