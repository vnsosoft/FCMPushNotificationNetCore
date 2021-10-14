using FCMPushNotification.Services.FirebaseModels;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace FCMPushNotification.Services.Services
{
    public class FirebaseRealtimeDbService
    {
        private readonly IConfiguration _config;
        private readonly FirebaseClient firebaseClient;

        public FirebaseRealtimeDbService(IConfiguration config)
        {
            _config = config;

            // = "https://XXXXXX.firebaseio.com/"
        }

        public async Task AddStudent(Student student)
        {
            await firebaseClient
              .Child("students")
              .PostAsync(student);
        }
    }
}
