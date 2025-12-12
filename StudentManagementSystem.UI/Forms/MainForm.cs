using System;
using System.Drawing; // Added for UI logic
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.BLL.Factory;
using StudentManagementSystem.BLL.Interfaces;

namespace StudentManagementSystem.UI.Forms
{
    public partial class MainForm : Form
    {
        private IBusinessLogicFactory _bllFactory;
        private BLLImplementationType _currentImplementation;
        private IConfiguration _configuration;

        public MainForm()
        {
            InitializeComponent();
            InitializeConfiguration();
            InitializeBLL();
        }

        private void InitializeConfiguration()
        {
            // Ensure you have the Microsoft.Extensions.Configuration.Json NuGet package installed
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        private void InitializeBLL()
        {
            string bllType = _configuration["AppSettings:BLLImplementation"] ?? "LINQ";
            
            _currentImplementation = bllType == "StoredProcedure"
                ? BLLImplementationType.StoredProcedure
                : BLLImplementationType.LINQ;

            _bllFactory = new BusinessLogicFactory(_currentImplementation);
            UpdateStatusLabel();
        }

        private void UpdateStatusLabel()
        {
            lblBLLStatus.Text = $"Current Backend: {_currentImplementation}";
            
            // Visual feedback on status
            if (_currentImplementation == BLLImplementationType.LINQ)
                lblBLLStatus.ForeColor = Color.Teal;
            else
                lblBLLStatus.ForeColor = Color.DarkOrange;
        }

        private void btnSwitchBLL_Click(object sender, EventArgs e)
        {
            _currentImplementation = _currentImplementation == BLLImplementationType.LINQ
                ? BLLImplementationType.StoredProcedure
                : BLLImplementationType.LINQ;

            _bllFactory = new BusinessLogicFactory(_currentImplementation);
            UpdateStatusLabel();

            MessageBox.Show($"Switched to {_currentImplementation} implementation!",
                "System Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Navigation Events ---

        private void btnStudents_Click(object sender, EventArgs e)
        {
            var form = new StudentForm(_bllFactory);
            form.ShowDialog();
        }

        private void btnCourses_Click(object sender, EventArgs e)
        {
            var form = new CourseForm(_bllFactory);
            form.ShowDialog();
        }

        private void btnEnrollments_Click(object sender, EventArgs e)
        {
            var form = new EnrollmentForm(_bllFactory);
            form.ShowDialog();
        }

        private void btnDepartments_Click(object sender, EventArgs e)
        {
            var form = new DepartmentForm(_bllFactory);
            form.ShowDialog();
        }

        private void btnCourseOfferings_Click(object sender, EventArgs e)
        {
            using (var form = new CourseOfferingForm(_bllFactory))
            {
                form.ShowDialog(this);
            }
        }

        private void btnStudentHolds_Click(object sender, EventArgs e)
        {
            try
            {
                var holdService = _bllFactory.GetStudentHoldService();
                using (var form = new StudentHoldForm(holdService))
                {
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening Student Holds form:\n{ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewGradeAudit_Click(object sender, EventArgs e)
        {
            using (var form = new GradeAuditForm(_bllFactory))
            {
                form.ShowDialog(this);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}