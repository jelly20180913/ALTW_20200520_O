$(function () {
    //apiserver address need put in global 
    // var apiServer = 'http://localhost:52006/api/';
   // var imgServer = 'http://localhost:52006/';
    var _PicName = getURLParameter('PicName');
    var _Date = getURLParameter('Date');
    // var _Url = '../../Report/201904/' + _PicName + '.JPG';
    var _Url = imgServer + 'Success/AmphenolReport/' + _Date + "/" + _PicName;
    $('.con').css("background-image", "url('" + _Url + "') ");
})
function getURLParameter(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}
