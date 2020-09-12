    var cls;
    $('#classId').change(function () {
        var cls = $(this).val();
        $('#Section').empty();
        $('#Section').append("  <option selected disabled>Select Section</option>");
        $.get("/TestResult/FindSection/", { ClsId: cls }, function (data) {
            $.each(data, function (index, row) {
                $('#Section').append("<option value='" + row.SectionId + "'>" + row.SectionName + "</option>")
            })
        });

    });
    $('#Section').change(function () {

         cls = $('#classId').val();
         var sectionId = $(this).val()
         
         $('#Subject').empty();
         $('#Subject').append("  <option selected disabled>Select Subject</option>");
         $.get("/TestResult/FindSubject/", { ClassId: cls, Section: sectionId }, function (data) {

             $.each(data, function (index, row) {
                 $('#Subject').append("<option value='" + row.SubjectId + "'>" + row.SubjectName + "</option>")
             })
         })
    });
