using FCMPushNotification.Services.FirebaseModels;
using Firebase.Database.Query;
using FirebaseDataServices.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirebaseDataServices.Services
{
    public class StudentService : BaseService, IStudentService
    {
        public StudentService(IConfiguration configuration):base(configuration)
        {
        }

        public async Task AddStudent(Student student)
        {
            await _firebaseClient
              .Child("students")
              .PostAsync(student);
        }

        public async Task<List<KeyValuePair<string, Student>>> GetStudents()
        {
            var students = await _firebaseClient
              .Child("students")
              .OnceAsync<Student>();

            return students?
              .Select(x => new KeyValuePair<string, Student>(x.Key, x.Object))
              .ToList();
        }

        public async Task UpdateStudent(string id, Student student)
        {
            await _firebaseClient
              .Child("students")
              .Child(id)
              .PutAsync(student);
        }

        public async Task RemoveStudent(string id)
        {
             await _firebaseClient
              .Child("students")
              .Child(id)
              .DeleteAsync();
        }
    }
}
