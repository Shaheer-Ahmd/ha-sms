using System;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class CourseForm : Form
    {
        private readonly IBusinessLogicFactory _bllFactory;
        private readonly ICourseService _courseService;
        private readonly IDepartmentService _departmentService;
        private int? _selectedCourseId;

        public CourseForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();
            _bllFactory = bllFactory;
            _courseService = _bllFactory.GetCourseService();
            _departmentService = _bllFactory.GetDepartmentService();
            LoadDepartments();
            LoadCourses();
        }

        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentService.GetActiveDepartments();
                cmbDepartment.DataSource = departments;
                cmbDepartment.DisplayMember = "DepartmentName";
                cmbDepartment.ValueMember = "DepartmentID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCourses()
        {
            try
            {
                var courses = _courseService.GetAllCourses();
                dgvCourses.DataSource = courses.Select(c => new
                {
                    c.CourseID,
                    c.CourseCode,
                    c.Title,
                    c.Credits,
                    DepartmentName = c.Department?.DepartmentName
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPrerequisites(int courseId)
        {
            try
            {
                var prereqs = _courseService.GetCoursePrerequisites(courseId);

                dgvPrerequisites.DataSource = prereqs
                    .Select(cp => new
                    {
                        cp.PrerequisiteCourseID,
                        CourseCode = cp.PrerequisiteCourse.CourseCode,
                        Title = cp.PrerequisiteCourse.Title
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                var msg = "Error loading prerequisites: " + ex.Message;
                if (ex.InnerException != null)
                {
                    msg += Environment.NewLine + Environment.NewLine +
                           "Inner exception: " + ex.InnerException.Message;
                }

                MessageBox.Show(msg, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void LoadPrerequisiteCandidates(int courseId)
        {
            try
            {
                var candidates = _courseService.GetAvailablePrerequisiteCourses(courseId);

                cmbPrerequisiteCourse.DataSource = candidates;
                cmbPrerequisiteCourse.DisplayMember = "CourseCode"; // or "Title" if you prefer
                cmbPrerequisiteCourse.ValueMember = "CourseID";

                if (candidates.Count == 0)
                {
                    cmbPrerequisiteCourse.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading available prerequisite courses: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void dgvCourses_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCourses.SelectedRows.Count > 0)
            {
                _selectedCourseId = (int)dgvCourses.SelectedRows[0].Cells["CourseID"].Value;
                LoadCourseDetails(_selectedCourseId.Value);
            }
        }

        private void LoadCourseDetails(int courseId)
        {
            try
            {
                var course = _courseService.GetCourseById(courseId);
                if (course != null)
                {
                    txtCourseCode.Text = course.CourseCode;
                    txtTitle.Text = course.Title;
                    txtDescription.Text = course.Description;
                    numCredits.Value = course.Credits;
                    cmbDepartment.SelectedValue = course.DepartmentID;
                    LoadPrerequisites(courseId);
                    LoadPrerequisiteCandidates(courseId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading course details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddPrerequisite_Click(object sender, EventArgs e)
        {
            if (!_selectedCourseId.HasValue)
            {
                MessageBox.Show("Please select a course first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmbPrerequisiteCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a course to add as a prerequisite.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var courseId = _selectedCourseId.Value;
            var prereqId = (int)cmbPrerequisiteCourse.SelectedValue;

            if (courseId == prereqId)
            {
                MessageBox.Show("A course cannot be its own prerequisite.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _courseService.AddCoursePrerequisite(courseId, prereqId);
                LoadPrerequisites(courseId);
                LoadPrerequisiteCandidates(courseId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding prerequisite: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemovePrerequisite_Click(object sender, EventArgs e)
        {
            if (!_selectedCourseId.HasValue)
            {
                MessageBox.Show("Please select a course first.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvPrerequisites.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a prerequisite to remove.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var courseId = _selectedCourseId.Value;
            var prereqId = (int)dgvPrerequisites.SelectedRows[0].Cells["PrerequisiteCourseID"].Value;

            var confirm = MessageBox.Show(
                "Are you sure you want to remove this prerequisite?",
                "Confirm",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                _courseService.RemoveCoursePrerequisite(courseId, prereqId);
                LoadPrerequisites(courseId);
                LoadPrerequisiteCandidates(courseId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing prerequisite: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var course = new Course
                {
                    CourseCode = txtCourseCode.Text.Trim(),
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Credits = (int)numCredits.Value,
                    DepartmentID = (int)cmbDepartment.SelectedValue
                };

                _courseService.AddCourse(course);
                MessageBox.Show("Course added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCourses();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding course: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!_selectedCourseId.HasValue)
            {
                MessageBox.Show("Please select a course to update.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            try
            {
                var course = new Course
                {
                    CourseID = _selectedCourseId.Value,
                    CourseCode = txtCourseCode.Text.Trim(),
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Credits = (int)numCredits.Value,
                    DepartmentID = (int)cmbDepartment.SelectedValue
                };

                _courseService.UpdateCourse(course);
                MessageBox.Show("Course updated successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCourses();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating course: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!_selectedCourseId.HasValue)
            {
                MessageBox.Show("Please select a course to delete.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show("Are you sure you want to delete this course?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    _courseService.DeleteCourse(_selectedCourseId.Value);
                    MessageBox.Show("Course deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCourses();
                    ClearForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting course: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCourseCode.Text))
            {
                MessageBox.Show("Course code is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCourseCode.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Title is required.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return false;
            }

            if (numCredits.Value < 1 || numCredits.Value > 6)
            {
                MessageBox.Show("Credits must be between 1 and 6.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numCredits.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            _selectedCourseId = null;
            txtCourseCode.Clear();
            txtTitle.Clear();
            txtDescription.Clear();
            numCredits.Value = 3;
            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = 0;
            dgvPrerequisites.DataSource = null;
            cmbPrerequisiteCourse.DataSource = null;
            cmbPrerequisiteCourse.Text = string.Empty;
        }
    }
}
