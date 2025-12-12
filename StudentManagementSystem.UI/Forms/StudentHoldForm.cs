using System;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class StudentHoldForm : Form
    {
        private readonly IStudentHoldService _holdService;

        public StudentHoldForm(IStudentHoldService holdService)
        {
            _holdService = holdService ?? throw new ArgumentNullException(nameof(holdService));
            InitializeComponent();
        }

        private void StudentHoldForm_Load(object? sender, EventArgs e)
        {
            // Default DateApplied to "now"
            dtpDateApplied.Value = DateTime.UtcNow;
            LoadHolds();
        }

        private void LoadHolds()
        {
            try
            {
                var holds = _holdService.GetAllStudentHolds();
                dgvHolds.DataSource = holds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error loading student holds:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvHolds_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvHolds.CurrentRow == null || dgvHolds.CurrentRow.DataBoundItem is not StudentHold hold)
                return;

            txtHoldId.Text = hold.HoldID.ToString();
            txtStudentId.Text = hold.StudentID.ToString();
            cmbHoldType.SelectedItem = hold.HoldType;
            txtReason.Text = hold.Reason ?? string.Empty;
            dtpDateApplied.Value = hold.DateApplied;
            chkIsActive.Checked = hold.IsActive;
        }

        private void btnRefresh_Click(object? sender, EventArgs e)
        {
            LoadHolds();
        }

        private void btnAdd_Click(object? sender, EventArgs e)
        {
            if (!TryBuildHoldFromInputs(includeHoldId: false, out var hold))
                return;

            try
            {
                _holdService.AddStudentHold(hold);
                LoadHolds();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error adding hold:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object? sender, EventArgs e)
        {
            if (!TryBuildHoldFromInputs(includeHoldId: true, out var hold))
                return;

            try
            {
                _holdService.UpdateStudentHold(hold);
                LoadHolds();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error updating hold:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtHoldId.Text, out var holdId))
            {
                MessageBox.Show("Please select a valid hold to delete.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                "Are you sure you want to delete this hold?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                _holdService.DeleteStudentHold(holdId);
                LoadHolds();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error deleting hold:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private bool TryBuildHoldFromInputs(bool includeHoldId, out StudentHold hold)
        {
            hold = new StudentHold();

            if (!int.TryParse(txtStudentId.Text, out var studentId))
            {
                MessageBox.Show("StudentID must be a valid integer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (includeHoldId)
            {
                if (!int.TryParse(txtHoldId.Text, out var holdId))
                {
                    MessageBox.Show("HoldID is invalid. Please select a hold from the list.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                hold.HoldID = holdId;
            }

            if (cmbHoldType.SelectedItem == null)
            {
                MessageBox.Show("Please select a Hold Type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            hold.StudentID = studentId;
            hold.HoldType = cmbHoldType.SelectedItem.ToString() ?? "Financial";
            hold.Reason = string.IsNullOrWhiteSpace(txtReason.Text) ? null : txtReason.Text;
            hold.DateApplied = dtpDateApplied.Value;
            hold.IsActive = chkIsActive.Checked;

            return true;
        }

        private void ClearForm()
        {
            txtHoldId.Text = string.Empty;
            txtStudentId.Text = string.Empty;
            cmbHoldType.SelectedIndex = -1;
            txtReason.Text = string.Empty;
            dtpDateApplied.Value = DateTime.UtcNow;
            chkIsActive.Checked = true;
        }
    }
}
