using System;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class EnrollmentForm : Form
    {
        private readonly IBusinessLogicFactory _bllFactory;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IStudentService _studentService;
        private readonly ISemesterService _semesterService;

        public EnrollmentForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();
            _bllFactory = bllFactory;
            _enrollmentService = _bllFactory.GetEnrollmentService();
            _studentService = _bllFactory.GetStudentService();
            _semesterService = _bllFactory.GetSemesterService();
            LoadStudents();
            LoadSemesters();
            InitializeGradeComboBox();
        }

        private void LoadStudents()
        {
            try
            {
                var students = _studentService.GetActiveStudents();
                cmbStudent.DataSource = students;
                cmbStudent.DisplayMember = "FullName";
                cmbStudent.ValueMember = "StudentID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSemesters()
        {
            try
            {
                var semesters = _semesterService.GetAllSemesters()
                    .OrderByDescending(s => s.Year)
                    .ThenByDescending(s => s.Season)
                    .Take(10)
                    .ToList();
                cmbSemester.DataSource = semesters;
                cmbSemester.DisplayMember = "SemesterName";
                cmbSemester.ValueMember = "SemesterID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading semesters: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeGradeComboBox()
        {
            cmbGrade.Items.AddRange(new object[] { "", "A", "B", "C", "D", "F" });
            cmbGrade.SelectedIndex = 0;
        }

        private void cmbSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSemester.SelectedValue != null && cmbSemester.SelectedValue is int)
            {
                LoadAvailableOfferings((int)cmbSemester.SelectedValue);
            }
        }

        private void LoadAvailableOfferings(int semesterId)
        {
            try
            {
                var offeringService = _bllFactory.GetCourseOfferingService();
                var offerings = offeringService.GetOfferingsBySemester(semesterId)
                    .Where(o => o.CurrentEnrollment < o.MaxCapacity)
                    .ToList();
                dgvOfferings.DataSource = offerings.Select(o => new
                {
                    o.OfferingID,
                    CourseCode = o.Course?.CourseCode,
                    CourseTitle = o.Course?.Title,
                    o.MaxCapacity,
                    o.CurrentEnrollment,
                    o.SeatsRemaining
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course offerings: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedValue != null && cmbStudent.SelectedValue is int)
            {
                LoadStudentEnrollments((int)cmbStudent.SelectedValue);
            }
        }

        private void LoadStudentEnrollments(int studentId)
        {
            try
            {
                var enrollments = _enrollmentService.GetStudentEnrollments(studentId);
                dgvEnrollments.DataSource = enrollments.Select(e => new
                {
                    e.EnrollmentID,
                    e.EnrollmentDate,
                    e.OfferingID,
                    e.Grade
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading enrollments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (cmbStudent.SelectedValue == null || !(cmbStudent.SelectedValue is int))
            {
                MessageBox.Show("Please select a student.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvOfferings.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a course offering.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int studentId = (int)cmbStudent.SelectedValue;
                int offeringId = (int)dgvOfferings.SelectedRows[0].Cells["OfferingID"].Value;

                // Uses sp_RegisterStudentForCourse stored procedure
                _enrollmentService.RegisterStudentForCourse(studentId, offeringId);

                MessageBox.Show("Student registered successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadAvailableOfferings((int)cmbSemester.SelectedValue);
                LoadStudentEnrollments(studentId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateGrade_Click(object sender, EventArgs e)
        {
            if (dgvEnrollments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an enrollment to update.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(cmbGrade.Text))
            {
                MessageBox.Show("Please select a grade.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int enrollmentId = (int)dgvEnrollments.SelectedRows[0].Cells["EnrollmentID"].Value;
                DateTime enrollmentDate = (DateTime)dgvEnrollments.SelectedRows[0].Cells["EnrollmentDate"].Value;

                var enrollment = _enrollmentService.GetEnrollmentById(enrollmentId, enrollmentDate);
                if (enrollment != null)
                {
                    enrollment.Grade = cmbGrade.Text;

                    // Triggers AFTER UPDATE trigger (trg_After_GradeUpdate) - audit trail
                    _enrollmentService.UpdateEnrollment(enrollment);

                    MessageBox.Show("Grade updated successfully! (Triggered trg_After_GradeUpdate for audit)",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    LoadStudentEnrollments((int)cmbStudent.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating grade: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
