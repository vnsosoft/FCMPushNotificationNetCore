using Firebase.Database;
using Microsoft.Extensions.Configuration;

namespace FirebaseDataServices.Services
{
    public class BaseService
    {
        protected readonly FirebaseClient _firebaseClient;
        private readonly IConfiguration _config;

        public BaseService(IConfiguration configuration)
        {
            _config = configuration;
            var firebaseDatabaseUrl = _config.GetConnectionString("FirebaseDatabaseUrl"); ;
            _firebaseClient = new FirebaseClient(firebaseDatabaseUrl);
        }
    }
}
