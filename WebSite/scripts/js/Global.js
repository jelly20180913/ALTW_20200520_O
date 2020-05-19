 
//測試環境連線字串
//var imgServer = 'http://localhost:52006/';
//var apiServer = 'http://localhost:52006/api/';
//var webSiteServer = 'http://localhost:55614/';
$(document).ready(function () {
    var origin = window.location.origin; 
    if (origin == "http://localhost:55614") {
        imgServer = 'http://localhost:52006/';
        apiServer = 'http://localhost:52006/api/';
        webSiteServer = 'http://localhost:55614/';
    } 
    if (sessionStorage.getItem("Token") == null) {
        alert("please login first");
        //  $(location).attr('href', './login.html');
        $(location).attr('href', webSiteServer +'view/login.html');
    }
   
});