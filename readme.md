# .Net core API and .net core web about FCM sample
![Diagram](FCM-diagram.PNG)
## Required
* FirebaseAdmin nuget package
* A firebase project in https://firebase.google.com/
* Firebase credential: `[firebase](https://console.firebase.google.com/)` > `your_firebase_project` > `Project settings` > `Service accounts` > `Generate new private key`
* Copied the Firebase credential json file into project, rename to `google-credential.json`
* In the Statup.cs
```
    // GoogleCredential
    var googleCredentialJson = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "google-credential.json");
    var defaultApp = FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(googleCredentialJson),
    });
```
## Messaging services
### 1. Send message to single device
```
    public class SendMessageRequest
    {
        public string DeviceToken { get; set; }

        public Dictionary<string, string> Data { get; set; }
    }

    public async Task<string> SendMessageAsync(SendMessageRequest request)
    {
        var message = new Message()
        {
            Token = request.DeviceToken,
            Data = request.Data,
        };
        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendAsync(message);

        _logger.LogTrace(FCMConstants.FCM_SENDED, result);
        return result;
    }
```
### 2. Send message to multiple device
```
    public class SendMulticastMessageRequest
    {
        public Dictionary<string, string> Data { get; set; }

        public List<string> DeviceTokens { get; set; }
    }

    public async Task<string> SendMulticastMessageAsync(SendMulticastMessageRequest request)
    {
        var message = new MulticastMessage()
        {
            Data = request.Data,
            Tokens = request.DeviceTokens,
        };
        var messaging = FirebaseMessaging.DefaultInstance;
        var result = await messaging.SendMulticastAsync(message);

        return result.SuccessCount.ToString();
    }
```
# Web FCM project
* This is a project receive FCM from API
## Required
* JS JDK
```
<!-- The core Firebase JS SDK is always required and must be listed first -->
<script src="https://www.gstatic.com/firebasejs/8.8.0/firebase-app.js"></script>
<script src="https://www.gstatic.com/firebasejs/8.8.0/firebase-messaging.js"></script>
```
* Create firebase-messaging-sw.js at root folder
* File contents
```
//DO NOT REMOVE THIS FILE
importScripts('https://www.gstatic.com/firebasejs/8.8.0/firebase-app.js');
importScripts('https://www.gstatic.com/firebasejs/8.8.0/firebase-messaging.js');

//config from firebase project managent (Firebase > Project settings > General > Your app > SDK setup and configuration session)
var firebaseConfig = {
    apiKey: "AIzaSyAcvqVpIZJUxhWJTBUl1uOhA9EKMtFYMI8",
    authDomain: "fcmnetcoreapi.firebaseapp.com",
    projectId: "fcmnetcoreapi",
    storageBucket: "fcmnetcoreapi.appspot.com",
    messagingSenderId: "1077523311985",
    appId: "1:1077523311985:web:de9c433fc6ade37ec91ad0",
    measurementId: "G-C2P1DCF948"
};

firebase.initializeApp(firebaseConfig);
const messaging = firebase.messaging();
```
* Handle background service to listen and show notification from FCM
```
// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
// Get config from firebase project managent (Firebase > Project settings > General > Your app > SDK setup and configuration session)
var firebaseConfig = {
    apiKey: "AIzaSyAcvqVpIZJUxhWJTBUl1uOhA9EKMtFYMI8",
    authDomain: "fcmnetcoreapi.firebaseapp.com",
    projectId: "fcmnetcoreapi",
    storageBucket: "fcmnetcoreapi.appspot.com",
    messagingSenderId: "1077523311985",
    appId: "1:1077523311985:web:de9c433fc6ade37ec91ad0",
    measurementId: "G-C2P1DCF948"
};
// Initialize Firebase
firebase.initializeApp(firebaseConfig);

// Initialize messaging on background
const messaging = firebase.messaging();

// usePublicVapidKey 
// Param: Key pair
// Get Key pair: firebase > Project settings > Cloud Messaging > Web configuration > Key pair
var keyPairInput = document.getElementById("keyPair").value;
var keyPair = keyPairInput;
messaging.usePublicVapidKey(keyPair);

// Request notify permission
messaging.requestPermission()
    .then(function () {
        console.log("Notify permission was granted");
        return messaging.getToken();
    })
    .then(function (token) {
        console.log("Token ", token);
        console.log("keyPair ", keyPair);
        document.getElementById("sendToToken").value = token;
    })
    .catch(function (err) {
        console.log(err);
    });

// Receive message
messaging.onMessage(function (payload) {
    showNotify(payload);
    console.log(payload)
});

// Show notify from message
function showNotify(payload) {
    const title = payload?.data?.title;
    const body = payload?.data?.body;
    const icon = payload?.data?.image;
    const url = payload?.data?.url;

    var notification = new Notification(title, {
        icon: icon,
        body: body,
    });

    if (url) {
        notification.onclick = function () {
            window.open(url);
        };
    }
}
```
#  Task Scheduling and Background Processing using [Hangfire](https://www.hangfire.io/downloads.html)
## Required package
* Hangfire core
* Hangfire SQL Server Storage
## Setup
``` Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    //...

    services.AddHangfire(options =>
    {
        options.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"));
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseHangfireDashboard();
    app.UseHangfireServer();

   // ...
}
```

```Controller
[HttpGet]
public async Task<IActionResult> Send(int seconds)
{
    try
    {
        var onlineAccounts = await _accountService.GetOnlineAccountsAsync();
        var deviceTokens = onlineAccounts.Select(x => x.DeviceToken).ToList();
        var sendMulticastMessageRequest = new SendMulticastMessageRequest()
        {
            Data = new Dictionary<string, string>
            {
                ["title"] = "Message",
                ["body"] = $"Send mutilple device.",
            },
            DeviceTokens = deviceTokens,
        };

        // Hangfire service
        BackgroundJob.Schedule(() =>
            _messagingService.SendMulticastMessageAsync(sendMulticastMessageRequest), TimeSpan.FromSeconds(seconds)
        );

        return Ok("Task scheduled");
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
```