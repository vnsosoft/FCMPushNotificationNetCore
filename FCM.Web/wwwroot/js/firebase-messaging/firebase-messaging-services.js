// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
// Get config from firebase project managent (Firebase > Project settings > General > Your app > SDK setup and configuration session)
var firebaseConfig = {
    apiKey: "YOUR_GG_API_KEY",
    authDomain: "fcmnetcoreapi.firebaseapp.com",
    projectId: "fcmnetcoreapi",
    storageBucket: "fcmnetcoreapi.appspot.com",
    messagingSenderId: "1077523311985",
    appId: "1:1077523311985:web:de9c433fc6ade37ec91ad0",
    measurementId: "G-C2P1DCF948"
};

/* 
 * Initialize Firebase
 * */
firebase.initializeApp(firebaseConfig);

/*
 * Initialize Firebase analytics
 * */
//firebase.analytics();

/*
 * Initialize messaging on background
 * */
const messaging = firebase.messaging();

// usePublicVapidKey 
// Param: Key pair
// Get Key pair: firebase > Project settings > Cloud Messaging > Web configuration > Key pair
var keyPairInput = document.getElementById("keyPair").value;
var keyPair = keyPairInput;
messaging.usePublicVapidKey(keyPair);

/*
 * Request notify permission
 * */
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

/*
 * Receive message
 * */
messaging.onMessage(function (payload) {
    showNotify(payload);
    console.log(payload)
});

/*
 * Show notify from message
 * */
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

