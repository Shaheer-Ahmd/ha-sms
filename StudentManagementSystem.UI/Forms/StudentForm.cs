using System;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class StudentForm : Form
    {
        private readonly IBusinessLogicFactory _bllFactory;
        private readonly IStudentService _studentService;
        private int? _selectedStudentId;

        public StudentForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();
            _bllFactory = bllFactory;
            _studentService = _bllFactory.GetStudentService();
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                dgvStudents.DataSource = students.Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.Email,
                    s.EnrollmentStatus,
                    s.DateOfBirth
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                _selectedStudentId = (int)dgvStudents.SelectedRows[0].Cells["StudentID"].Value;
                LoadStudentDetails(_selectedStudentId.Value);
            }
        }

        private void LoadStudentDetails(int studentId)
        {
            try
            {
                var student = _studentService.GetStudentById(studentId);
                if (student != null)
                {
                    txtFirstName.Text = student.FirstName;
                    txtLastName.Text = student.LastName;
                    txtEmail.Text = student.Email;
                    cmbStatus.Text = student.EnrollmentStatus;
                    dtpDateOfBirth.Value = student.DateOfBirth ?? DateTime.Now.AddYears(-20);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student details: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var student = new Student
                {
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    EnrollmentStatus = cmbStatus.Text,
                    DateOfBirth = dtpDateOfBirth.Value
                };

                _studentService.AddStudent(student);
                MessageBox.Show("Student added successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding student: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_selectedStudentId.HasValue)
            {
                MessageBox.Show("Please select a student to update.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                var student = new Student
                {
                    StudentID = _selectedStudentId.Value,
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    EnrollmentStatus = cmbStatus.Text,
                    DateOfBirth = dtpDateOfBirth.Value
                };

                _studentService.UpdateStudent(student);
                MessageBox.Show("Student updated successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating student: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_selectedStudentId.HasValue)
            {
                MessageBox.Show("Please select a student to delete.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this student? (Uses INSTEAD OF trigger - sets to Inactive)", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _studentService.DeleteStudent(_selectedStudentId.Value);
                    MessageBox.Show("Student deleted (set to Inactive)!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStudents();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting student: {ex.Message}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnViewTranscript_Click(object sender, EventArgs e)
        {
            if (!_selectedStudentId.HasValue)
            {
                MessageBox.Show("Please select a student.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var transcript = _studentService.GetStudentTranscript(_selectedStudentId.Value);
                var gpa = _studentService.GetStudentGPA(_selectedStudentId.Value);

                var message = $"GPA: {gpa:F2}\n\nTranscript:\n";
                foreach (var record in transcript)
                {
                    message += $"{record.CourseCode} - {record.CourseTitle}: {record.Grade}\n";
                }

                MessageBox.Show(message, "Student Transcript", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transcript: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                LoadStudents();
                return;
            }

            try
            {
                var searchText = txtSearch.Text.Trim().ToLower();
                var students = _studentService.GetAllStudents()
                    .Where(s => s.FirstName.ToLower().Contains(searchText) || 
                                s.LastName.ToLower().Contains(searchText) ||
                                s.Email.ToLower().Contains(searchText))
                    .ToList();
                dgvStudents.DataSource = students.Select(s => new
                {
                    s.StudentID,
                    s.FirstName,
                    s.LastName,
                    s.Email,
                    s.EnrollmentStatus,
                    s.DateOfBirth
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching students: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First name is required.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last name is required.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                MessageBox.Show("Valid email is required.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(cmbStatus.Text))
            {
                MessageBox.Show("Enrollment status is required.", "Validation", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            _selectedStudentId = null;
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            cmbStatus.SelectedIndex = 0;
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-20);
            txtSearch.Clear();
        }
    }
}
