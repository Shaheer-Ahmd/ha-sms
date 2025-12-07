namespace StudentManagementSystem.BLL.Interfaces
{
    public enum BLLImplementationType
    {
        LINQ,
        StoredProcedure
    }

    public interface IBusinessLogicFactory
    {
        IStudentService GetStudentService();
        ICourseService GetCourseService();
        IDepartmentService GetDepartmentService();
        ISemesterService GetSemesterService();
        IEnrollmentService GetEnrollmentService();
        IStudentHoldService GetStudentHoldService();
        ICourseOfferingService GetCourseOfferingService();
        BLLImplementationType GetImplementationType();
    }
}
