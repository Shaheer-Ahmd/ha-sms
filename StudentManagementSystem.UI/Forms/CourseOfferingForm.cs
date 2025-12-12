using System;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class CourseOfferingForm : Form
    {
        private readonly IBusinessLogicFactory _bllFactory;
        private readonly ICourseOfferingService _offeringService;
        private readonly ICourseService _courseService;
        private readonly ISemesterService _semesterService;

        private int? _selectedOfferingId;

        public CourseOfferingForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();

            _bllFactory = bllFactory;
            _offeringService = _bllFactory.GetCourseOfferingService();
            _courseService = _bllFactory.GetCourseService();
            _semesterService = _bllFactory.GetSemesterService();

            LoadCourses();
            LoadSemesters();
            LoadOfferings();
        }

        private void LoadCourses()
        {
            try
            {
                var courses = _courseService.GetAllCourses()
                    .OrderBy(c => c.CourseCode)
                    .ToList();

                cmbCourse.DataSource = courses;
                cmbCourse.DisplayMember = "CourseCode";  // or "Title", your choice
                cmbCourse.ValueMember = "CourseID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSemesters()
        {
            try
            {
                var semesters = _semesterService.GetAllSemesters()
                    .OrderBy(s => s.Year)
                    .ThenBy(s => s.Season)
                    .ToList();

                cmbSemester.DataSource = semesters;
                cmbSemester.DisplayMember = "DisplayName"; // see note below
                cmbSemester.ValueMember = "SemesterID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading semesters: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Binds the DataGridView with all course offerings.
        /// </summary>
        private void LoadOfferings()
        {
            try
            {
                var offerings = _offeringService.GetAllOfferings();

                dgvOfferings.DataSource = offerings.Select(o => new
                {
                    o.OfferingID,
                    CourseCode = o.Course?.CourseCode,
                    CourseTitle = o.Course?.Title,
                    Semester = o.Semester != null
                        ? $"{o.Semester.Year} {o.Semester.Season}"
                        : "",
                    o.MaxCapacity,
                    o.CurrentEnrollment
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course offerings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvOfferings_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvOfferings.SelectedRows.Count == 0)
            {
                _selectedOfferingId = null;
                return;
            }

            try
            {
                _selectedOfferingId =
                    (int)dgvOfferings.SelectedRows[0].Cells["OfferingID"].Value;

                LoadOfferingDetails(_selectedOfferingId.Value);
            }
            catch
            {
                _selectedOfferingId = null;
            }
        }

        private void LoadOfferingDetails(int offeringId)
        {
            try
            {
                var offering = _offeringService.GetOfferingById(offeringId);
                if (offering == null)
                    return;

                // Set dropdowns
                cmbCourse.SelectedValue = offering.CourseID;
                cmbSemester.SelectedValue = offering.SemesterID;

                // Set numeric fields
                numMaxCapacity.Value = offering.MaxCapacity;
                numCurrentEnrollment.Value = offering.CurrentEnrollment;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading offering details: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var offering = new CourseOffering
                {
                    CourseID = (int)cmbCourse.SelectedValue,
                    SemesterID = (int)cmbSemester.SelectedValue,
                    MaxCapacity = (int)numMaxCapacity.Value,
                    CurrentEnrollment = (int)numCurrentEnrollment.Value
                };

                if (offering.CurrentEnrollment > offering.MaxCapacity)
                {
                    MessageBox.Show("Current enrollment cannot exceed max capacity.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _offeringService.AddOffering(offering);

                MessageBox.Show("Course offering added successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadOfferings();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding course offering: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_selectedOfferingId.HasValue)
            {
                MessageBox.Show("Please select an offering to update.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                var offering = new CourseOffering
                {
                    OfferingID = _selectedOfferingId.Value,
                    CourseID = (int)cmbCourse.SelectedValue,
                    SemesterID = (int)cmbSemester.SelectedValue,
                    MaxCapacity = (int)numMaxCapacity.Value,
                    CurrentEnrollment = (int)numCurrentEnrollment.Value
                };

                if (offering.CurrentEnrollment > offering.MaxCapacity)
                {
                    MessageBox.Show("Current enrollment cannot exceed max capacity.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _offeringService.UpdateOffering(offering);

                MessageBox.Show("Course offering updated successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadOfferings();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course offering: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_selectedOfferingId.HasValue)
            {
                MessageBox.Show("Please select an offering to delete.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show(
                "Are you sure you want to delete this course offering?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                _offeringService.DeleteOffering(_selectedOfferingId.Value);

                MessageBox.Show("Course offering deleted successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadOfferings();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting course offering: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (cmbCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a course.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCourse.Focus();
                return false;
            }

            if (cmbSemester.SelectedItem == null)
            {
                MessageBox.Show("Please select a semester.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSemester.Focus();
                return false;
            }

            if (numMaxCapacity.Value <= 0)
            {
                MessageBox.Show("Max capacity must be greater than 0.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numMaxCapacity.Focus();
                return false;
            }

            if (numCurrentEnrollment.Value < 0)
            {
                MessageBox.Show("Current enrollment cannot be negative.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numCurrentEnrollment.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            _selectedOfferingId = null;

            if (cmbCourse.Items.Count > 0)
                cmbCourse.SelectedIndex = 0;

            if (cmbSemester.Items.Count > 0)
                cmbSemester.SelectedIndex = 0;

            numMaxCapacity.Value = 50;
            numCurrentEnrollment.Value = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
