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
