using FCMPushNotification.Services.FirebaseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirebaseDataServices.Interfaces
{
    public interface IStudentService
    {
        Task AddStudent(Student student);

        Task<List<KeyValuePair<string, Student>>> GetStudents();

        Task UpdateStudent(string id, Student student);

        Task RemoveStudent(string id);
    }
}
