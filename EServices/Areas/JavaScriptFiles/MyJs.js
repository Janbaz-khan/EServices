$(document).ready(function () {
    var notiIcon = $('#bgMain');
    var hub = $.connection.myHub;
    function startHub() {
   $.connection.hub.start()
        .done(function () {
            console.log("Hub Started");
            testNotification();
            examNotification();
            homeworkNotification();
            attendanceNotification();
            //alert("connected");
        });
    }
    
    //$.connection.hub.disconnected(function () {
    //    setTimeout(function () {
    //        startHub();
    //    }, 5000);
    //});
    function connectIt() {

        setInterval(startHub, 10000);
        //setInterval(connectClient, 5000);
    }
    connectIt();
    connectClient();
    //startHub();
    function connectClient() {
 hub.client.notify = function () {
        testNotification();
        examNotification();
        homeworkNotification();
        attendanceNotification();
    };
    }
   
    //testNotification function
    function testNotification() {
        var readS;
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/GetTestNotification/',
            success: function (data) {
        $('#noti3').empty();

        $.each(data, function (index, value) {

                    if (value.Read) {
                        readS = "";
                    } else {
                        readS = "New";
                        $('#m3 i').show();
                        $('#bgMain').show();
                    }

                    $('#noti3').append("<li class=countnoti ><strong>" + value.Name + " </strong>,Class: <strong> " + value.Class + "</strong>,<br> got :<strong>" + value.Marks + "/" + value.TMarks + "</strong><h6>" + value.date + " , type:<strong> Test</strong><h6>, subject: <strong> " + value.subject + "</strong><h6><span>" + readS + "</span></li> ");
                });
            }
        });
                    StopLoading();

    }


    //examnotification function
    function examNotification() {
        var readS;
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/GetExamNotification/',
            success: function (data) {
        $('#noti4').empty();
                $.each(data, function (index, value) {
                    if (value.Read) {
                        readS = "";
                    } else {
                        readS = "New";
                        $('#m4 i').show();
                        $('#bgMain').show();
                    }
                    $('#noti4').append("<li><strong>" + value.Name + " </strong>,Class: <strong> " + value.Class + "</strong>,<br> got :<strong>" + value.Marks + "/" + value.TMarks + "</strong><h6>" + value.date + " , type:<strong> " + value.type + "</strong>, subject: <strong> " + value.subject + "</strong><h6><span>" + readS + "</span></li>");
                });
            }
        });
                    StopLoading();

    }
    ////homeWorkNotification function
    function homeworkNotification() {
        var readS;
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/GetHomeworkNotification/',
            success: function (data) {
                    $('#noti1').empty();
                $.each(data, function (index, value) {
                    if (value.Read) {
                        readS = "";
                    } else {
                        readS = "New";
                        $('#m1 i').show();
                        $('#bgMain').show();
                    }
                    $('#noti1').append("<li>HomeWork/Assignment announce for  Class: <strong> " + value.Class + "</strong>, Description :<strong>" + value.Content + "<h6>" + value.date + " , type:<strong> " + value.type + "</strong>, subject: <strong> " + value.subject + "</strong><h6><span>" + readS + "</span></li>");
                });
            }
        });
                    StopLoading();
    }
    ////attendanceNotification function
    function attendanceNotification() {
        var readS;
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/GetAttendanceNotification/',
            success: function (data) {
        $('#noti2').empty();
                $.each(data, function (index, value) {
                    if (value.Read) {
                        readS = "";
                    } else {
                        readS = "New";
                        $('#m2 i').show();
                        $('#bgMain').show();
                    }
                    $('#noti2').append("<li>" + value.Name + " of class:" + value.Class + " was <b>Absent</b> at " + value.Date + "<h6>" + value.Date + " <h6><span>" + readS + "</span></li>");
                });
            }
        });
                    StopLoading();

    }

    function StopLoading() {
        //$('.loadin').hide( function () {
        //    $('html, body').css({
        //        'overflow': 'auto',
        //        'height': 'auto'
        //    })
        //});

        $('.lds-ripple').hide(function () {
            $('#bell').show();
        });
    }

    //hiding notification icons
    var m1 = $('#m1 i');
    var m2 = $('#m2 i');
    var m3 = $('#m3 i');
    var m4 = $('#m4 i');
    $('#m4').click(function () {
        $('#m4 i').hide();
        if (m1.is(':hidden') && m2.is(':hidden') && m3.is(':hidden')) {
            $('#bgMain').hide();
        }
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/ReadExam/',
            success: function (response) {
            }
        });
    });
    $('#m3').click(function () {
        $('#m3 i').hide();
        if (m1.is(':hidden') && m2.is(':hidden') && m4.is(':hidden')) {
            $('#bgMain').hide();
        }
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/ReadTest/',
            success: function (response) {
            }
        });
    });
    $('#m2').click(function () {
        if (m1.is(':hidden') && m4.is(':hidden') && m3.is(':hidden')) {
            $('#bgMain').hide();
        }
        $('#m2 i').hide();
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/ReadAtten/',
            success: function (response) {
            }
        });
    });
    $('#m1').click(function () {
        if (m4.is(':hidden') && m2.is(':hidden') && m3.is(':hidden')) {
            $('#bgMain').hide();
        }
        $('#m1 i').hide();
        $.ajax({
            type: 'POST',
            url: '/Parent/Home/ReadHomeWork/',
            success: function (response) {
            }
        });
    });


});
