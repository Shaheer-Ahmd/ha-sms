using System;
using System.Configuration;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.BLL.LinqImplementation;
using StudentManagementSystem.BLL.StoredProcedureImplementation;

namespace StudentManagementSystem.BLL.Factory
{
    public class BusinessLogicFactory : IBusinessLogicFactory
    {
        private readonly BLLImplementationType _implementationType;
        private readonly string _connectionString;

        public BusinessLogicFactory(BLLImplementationType implementationType)
        {
            _implementationType = implementationType;
            _connectionString = ConfigurationManager.ConnectionStrings["StudentManagementDB"]?.ConnectionString;
            
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'StudentManagementDB' not found in configuration.");
            }
        }

        public IStudentService GetStudentService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqStudentService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPStudentService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public ICourseService GetCourseService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqCourseService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPCourseService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public IDepartmentService GetDepartmentService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqDepartmentService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPDepartmentService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public ISemesterService GetSemesterService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqSemesterService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPSemesterService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public IEnrollmentService GetEnrollmentService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqEnrollmentService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPEnrollmentService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public IStudentHoldService GetStudentHoldService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqStudentHoldService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPStudentHoldService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public ICourseOfferingService GetCourseOfferingService()
        {
            switch (_implementationType)
            {
                case BLLImplementationType.LINQ:
                    return new LinqCourseOfferingService(_connectionString);
                case BLLImplementationType.StoredProcedure:
                    return new SPCourseOfferingService(_connectionString);
                default:
                    throw new ArgumentException($"Unknown BLL implementation type: {_implementationType}");
            }
        }

        public BLLImplementationType GetImplementationType()
        {
            return _implementationType;
        }
    }
}
