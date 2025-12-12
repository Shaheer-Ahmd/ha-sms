using System;
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
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
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
            lblBLLStatus.Text = $"Current Implementation: {_currentImplementation}";
        }

        private void btnSwitchBLL_Click(object sender, EventArgs e)
        {
            _currentImplementation = _currentImplementation == BLLImplementationType.LINQ
                ? BLLImplementationType.StoredProcedure
                : BLLImplementationType.LINQ;

            _bllFactory = new BusinessLogicFactory(_currentImplementation);
            UpdateStatusLabel();

            MessageBox.Show($"Switched to {_currentImplementation} implementation!", 
                "BLL Switcher", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void btnViewGradeAudit_Click(object sender, EventArgs e)
        {
            using (var form = new GradeAuditForm(_bllFactory))
            {
                form.ShowDialog(this);
            }
        }
    }
}
