using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using StudentManagementSystem.BLL.Factory;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.DAL.Models;

namespace StudentManagementSystem.UI.Forms
{
    public partial class DepartmentForm : Form
    {
        private readonly IBusinessLogicFactory _bllFactory;
        private readonly IDepartmentService _departmentService;

        public DepartmentForm(IBusinessLogicFactory bllFactory)
        {
            InitializeComponent();
            _bllFactory = bllFactory;
            _departmentService = _bllFactory.GetDepartmentService();
            LoadDepartments();
        }

        private void LoadDepartments()
        {
            try
            {
                var departments = _departmentService.GetAllDepartments();
                
                // Display all departments in grid
                var departmentList = departments.Select(d => new
                {
                    d.DepartmentID,
                    d.DepartmentName,
                    ParentDepartment = d.ParentDepartmentID.HasValue 
                        ? departments.FirstOrDefault(p => p.DepartmentID == d.ParentDepartmentID.Value)?.DepartmentName 
                        : "(Root)",
                    Status = d.IsActive ? "Active" : "Inactive"
                }).OrderBy(d => d.DepartmentID).ToList();

                dgvDepartments.DataSource = departmentList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnViewHierarchy_Click(object sender, EventArgs e)
        {
            try
            {
                // Call sp_GetDepartmentHierarchy to show recursive CTE results
                var hierarchy = _departmentService.GetDepartmentHierarchy();
                
                if (hierarchy != null && hierarchy.Rows.Count > 0)
                {
                    dgvHierarchy.DataSource = hierarchy;
                    
                    // Adjust column widths
                    if (dgvHierarchy.Columns.Contains("IndentedName"))
                    {
                        dgvHierarchy.Columns["IndentedName"].Width = 300;
                    }
                    if (dgvHierarchy.Columns.Contains("HierarchyPath"))
                    {
                        dgvHierarchy.Columns["HierarchyPath"].Width = 400;
                    }
                    
                    tabControl.SelectedTab = tabHierarchy;
                    MessageBox.Show($"Hierarchy loaded successfully! Showing {hierarchy.Rows.Count} departments.", 
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No hierarchy data returned.", "Info", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading hierarchy: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
                {
                    MessageBox.Show("Please enter a department name.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var department = new Department
                {
                    DepartmentName = txtDepartmentName.Text.Trim(),
                    ParentDepartmentID = cmbParentDepartment.SelectedValue != null 
                        ? (int?)cmbParentDepartment.SelectedValue 
                        : null,
                    IsActive = chkIsActive.Checked
                };

                _departmentService.AddDepartment(department);
                MessageBox.Show("Department added successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                ClearForm();
                LoadDepartments();
                LoadParentDepartments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding department: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDepartments.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a department to update.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDepartmentName.Text))
                {
                    MessageBox.Show("Please enter a department name.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentID"].Value);
                var department = _departmentService.GetDepartmentById(departmentId);

                if (department != null)
                {
                    department.DepartmentName = txtDepartmentName.Text.Trim();
                    department.ParentDepartmentID = cmbParentDepartment.SelectedValue != null 
                        ? (int?)cmbParentDepartment.SelectedValue 
                        : null;
                    department.IsActive = chkIsActive.Checked;

                    _departmentService.UpdateDepartment(department);
                    MessageBox.Show("Department updated successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearForm();
                    LoadDepartments();
                    LoadParentDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating department: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDepartments.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select a department to delete.", "Validation", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentID"].Value);
                
                var result = MessageBox.Show("Are you sure you want to delete this department?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _departmentService.DeleteDepartment(departmentId);
                    MessageBox.Show("Department deleted successfully!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    ClearForm();
                    LoadDepartments();
                    LoadParentDepartments();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting department: {ex.Message}\n\nNote: Cannot delete departments with sub-departments or courses.", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                try
                {
                    int departmentId = Convert.ToInt32(dgvDepartments.SelectedRows[0].Cells["DepartmentID"].Value);
                    var department = _departmentService.GetDepartmentById(departmentId);

                    if (department != null)
                    {
                        txtDepartmentName.Text = department.DepartmentName;
                        cmbParentDepartment.SelectedValue = department.ParentDepartmentID ?? -1;
                        chkIsActive.Checked = department.IsActive;
                    }
                }
                catch { }
            }
        }

        private void LoadParentDepartments()
        {
            try
            {
                var departments = _departmentService.GetAllDepartments().ToList();
                
                // Add "(None)" option for root departments
                var parentList = departments.Select(d => new
                {
                    DepartmentID = d.DepartmentID,
                    DepartmentName = d.DepartmentName
                }).ToList();

                parentList.Insert(0, new { DepartmentID = -1, DepartmentName = "(None - Root Level)" });

                cmbParentDepartment.DataSource = parentList;
                cmbParentDepartment.DisplayMember = "DepartmentName";
                cmbParentDepartment.ValueMember = "DepartmentID";
                cmbParentDepartment.SelectedValue = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading parent departments: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtDepartmentName.Clear();
            cmbParentDepartment.SelectedValue = -1;
            chkIsActive.Checked = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadParentDepartments();
        }

        private void DepartmentForm_Load(object sender, EventArgs e)
        {
            LoadParentDepartments();
            chkIsActive.Checked = true;
        }
    }
}
