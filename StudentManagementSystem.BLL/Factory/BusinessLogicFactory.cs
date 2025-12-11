using System;
using Microsoft.Extensions.Configuration;
using StudentManagementSystem.BLL.Interfaces;
using StudentManagementSystem.BLL.LinqImplementation;
using StudentManagementSystem.BLL.StoredProcedureImplementation;

namespace StudentManagementSystem.BLL.Factory
{
    public class BusinessLogicFactory : IBusinessLogicFactory
    {
        private readonly BLLImplementationType _implementationType;
        private readonly string _connectionString;

        public BusinessLogicFactory(BLLImplementationType implementationType, string connectionString = null)
        {
            _implementationType = implementationType;
            
            if (!string.IsNullOrEmpty(connectionString))
            {
                _connectionString = connectionString;
            }
            else
            {
                // Try to get from configuration
                _connectionString = GetConnectionString();
            }

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Connection string 'StudentManagementDB' not found in configuration.");
            }
        }

        private static string GetConnectionString()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                
                var config = builder.Build();
                return config?.GetConnectionString("StudentManagementDB");
            }
            catch
            {
                // If configuration file is not available, use default
                return "Server=localhost,1433;Database=StudentManagementDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";
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
