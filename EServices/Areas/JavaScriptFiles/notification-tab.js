        $(".Abtn-menu").click(function () {
            $(".menuu").toggleClass("back");
        });
   
        function bClick(digit) {
            var head = $("#notiHead");
            
            if (digit == 1) {
                $('#m1').addClass("active-tab");
                $("#m2").removeClass("active-tab");
                $("#m3").removeClass("active-tab");
                $("#m4").removeClass("active-tab");
                $("#m1 i").removeClass("badge1");
                $('#noti1').css("display", "block");
                $('#noti3').css("display", "none");
                $('#noti2').css("display", "none");
                $('#noti4').css("display", "none");
                head.empty();
                head.html("Homework");

            }
            else if (digit == 2) {
                $('#m2').addClass("active-tab");
                $("#m1").removeClass("active-tab");
                $("#m3").removeClass("active-tab");
                $("#m4").removeClass("active-tab");
                $('#noti2').css("display", "block");
                $('#noti3').css("display", "none");
                $('#noti4').css("display", "none");
                $('#noti1').css("display", "none");
                head.empty();
                head.html("Attendance")
            }
            else if (digit == 3) {
                $('#m3').addClass("active-tab");
                $("#m2").removeClass("active-tab");
                $("#m1").removeClass("active-tab");
                $("#m4").removeClass("active-tab");
                $('#noti3').css("display", "block");
                $('#noti4').css("display", "none");
                $('#noti2').css("display", "none");
                $('#noti1').css("display", "none");
                head.empty();
                head.html("Tests")
            } else if (digit == 4) {
                $('#m4').addClass("active-tab");
                $("#m1").removeClass("active-tab");
                $("#m2").removeClass("active-tab");
                $("#m3").removeClass("active-tab");
                $('#noti4').css("display", "block");
                $('#noti3').css("display", "none");
                $('#noti2').css("display", "none");
                $('#noti1').css("display", "none");
                head.empty();
                head.html("Exams")
            }

        }
 function tabClick(digit) {
           
            
            if (digit == 1) {
                $('#t1').addClass("active-tab1");
                 $("#bell").removeClass("active-tab1");
                $("#t2").removeClass("active-tab1");
                $("#t3").removeClass("active-tab1");
                $('#home').css("display", "block");
                $('#notification').css("display", "none");
                $('#option').css("display", "none");
              
            }
            else if (digit == 2) {
                $('#t1').removeClass("active-tab1");
                $("#bell").addClass("active-tab1");
                $("#t2").removeClass("active-tab1");
                $("#t3").removeClass("active-tab1");
                $('#home').css("display", "none");
                $('#notification').css("display", "block");
                $('#option').css("display", "none");
              
            }
            else if (digit == 3) {
                 $('#t1').removeClass("active-tab1");
                $("#bell").removeClass("active-tab1");
                $("#t2").addClass("active-tab1");
                $("#t3").removeClass("active-tab1");
                $('#home').css("display", "none");
                $('#notification').css("display", "none");
                $('#option').css("display", "block");
                
            } else if (digit == 4) {
               alert("Comming soon");
            }

        }
