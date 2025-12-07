using System;
using System.Configuration;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Factory;
using StudentManagementSystem.BLL.Interfaces;

namespace StudentManagementSystem.UI.Forms
{
    public partial class MainForm : Form
    {
        private IBusinessLogicFactory _bllFactory;
        private BLLImplementationType _currentImplementation;

        public MainForm()
        {
            InitializeComponent();
            InitializeBLL();
        }

        private void InitializeBLL()
        {
            string bllType = ConfigurationManager.AppSettings["BLLImplementation"];
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
    }
}
