GetEducationList = function () {

    $.ajax({
        url: "/Users/UserEducationList",
        type: 'POST',
        datatype: "json",
        success: function (data) {
            try {
                if (data != null) {


                    var dvEducation = $("#dvEducation");
                    dvEducation.empty();
                    var EmplHeading = $("<div class='panel-heading iwill-panel-heading'>" +
                        "<h3 class='panel-title iwill-panel-title'>Education<a href='#' data-toggle='modal' data-target='#dvModalEducation' onclick='ResetEducation()' style='float:right;'><i class='fa fa-plus' aria-hidden='true'></i></a></h3>" +
                        "</div>");
                    dvEducation.append(EmplHeading);

                    var dvEducationBody = $("<div class='panel-body dvEmploymentBody' style='text-align:justify;'></div>");
                    $.each(data, function (i, v) {
                        var row;

                        row = "<div class='row employ-row'>" +
                            "<div class='col-lg-11 col-md-11'>" +
                            "<h4 class='employ-desig'>" + v.CourseTitle + "</h4>" +
                            "<h5 class='employ-addres'>" + v.NameOfInstitute + ", " + v.EducationLocation + " </h5>";

                        if (v.EducationToYear != null)
                            row += "<h6 class='employ-date'>" + v.EducationFromYear + " - " + v.EducationToYear + "</h6>";

                        else
                            row += "<h6 class='employ-date'>" + v.EducationFromYear + " - Present</h6>";


                        if (v.EducationDescription != null) {
                            if (v.EducationDescription.toString().length >=40)
                                row += "<p>" + v.EducationDescription.toString().substring(0, 40) + "... </p>";
                            else 
                                row += "<p>" + v.EducationDescription.toString()+  "</p>";
                        }

                        row += "<input class='hdnCourseTitle' type='hidden'  value='" + v.CourseTitle + "'/>" +
                            "<input class='hdnNameOfInstitute' type='hidden'  value='" + v.NameOfInstitute + "'/>" +
                            "<input class='hdnEducationType' type='hidden'  value='" + v.EducationType + "'/>" +
                            "<input class='hdnMajorSubjects' type='hidden'  value='" + v.MajorSubjects + "'/>" +
                            "<input class='hdnEducationLocation' type='hidden'  value='" + v.EducationLocation + "'/>" +
                            "<input class='hdnEducationFromYear' type='hidden'  value='" + v.EducationFromYear + "'/>" +
                            "<input class='hdnEducationFromMonth' type='hidden'  value='" + v.EducationFromMonth + "'/>" +

                            "<input class='hdnEducationToYear' type='hidden'  value='" + v.EducationToYear + "'/>" +
                            "<input class='hdnEducationToMonth' type='hidden'  value='" + v.EducationToMonth + "'/>" +
                            "<input class='hdnEducationDescription' type='hidden'  value='" + v.EducationDescription + "'/>" +
                            "<input class='hdnEducationIsPresent' type='hidden'  value='" + v.EducationIsPresent + "'/>" +



                            "<input class='hdnUEDID' name='UEDID' type='hidden'  value='" + v.UEDID + "'/>" +

                            "</div>" +
                            "<div class='col-lg-1 col-md-1 employ-edit'><a href='#' name='" + v.UEDID + "' onclick='EditEducation(this)'><i class='fa fa-pencil' aria-hidden='true'></i></a></div>" +
                            "<div class='col-lg-1 col-md-1 employ-edit'><a href='#'  onclick='DeleteEducation(" + v.UEDID + ")'><i class='fa fa-pencil' aria-hidden='true'></i></a></div>" +
                            "</div>";
                        dvEducationBody.append($(row));
                    });

                    dvEducation.append(dvEducationBody);
                }




                if (data.toString().indexOf("Error") >= 0) {
                    ErrorMsgTimeOut(data);
                }
            }
            catch (e) {
                alert(e.message)
            }


        },
        error: function (e) {
            ErrorMsgTimeOut(e.error);
        }
    });
}

function PresentForEducation_Selected(obj) {

    var isChecked = $(obj).prop('checked');

    if (isChecked) {
        $("#ddlToYearEducation").attr("disabled", true);
        $("#ddlToMonthEducation").attr("disabled", true);
        $(obj).next().val('true');
    }
    else {
        $("#ddlToYearEducation").attr("disabled", false);
        $("#ddlToMonthEducation").attr("disabled", false)
        $(obj).next().val('false');
    }
}


function ClearBorderError() {

    $("#ddlEducationType").css("border", "");
    $("#txtNameOfInstitute").css("border", "");
    $("#txtMajorSubjects").css("border", "");
    $("#txtEducationLocation").css("border", "");
    $("#txtCourseTitle").css("border", "");

    $("#ddlFromYearEducation").css("border", "");
    $("#ddlFromMonthEducation").css("border", "");
    $("#ddlToYearEducation").css("border", "");
    $("#ddlToMonthEducation").css("border", "");
}

function ResetEducation() {

    $("#txtNameOfInstitute").val('');
    $("#txtMajorSubjects").val('');
    $("#txtCourseTitle").val('');
    $("#txtEducationLocation").val('');
    $("#txtAboutEducation").val('');

    $("#ddlEducationType")[0].selectedIndex = 0;
    $("#ddlFromYearEducation")[0].selectedIndex = 0;
    $("#ddlFromMonthEducation")[0].selectedIndex = 0;
    $("#ddlToYearEducation")[0].selectedIndex = 0;
    $("#ddlToMonthEducation")[0].selectedIndex = 0;

    $("#ddlToYearEducation").attr("disabled", false);
    $("#ddlToMonthEducation").attr("disabled", false);

    $("#chIsPresentForEducation").prop('checked', false);



}


function AddEducation() {
    ClearBorderError();

    if (ValidateControl($("#ddlEducationType")) == false) {
        return false;
    }

    if (ValidateControl($("#txtNameOfInstitute")) == false) {
        return false;
    }

    if (ValidateControl($("#ddlFromYearEducation")) == false)
        return false;

    var FromYear = parseInt($("#ddlFromYearEducation").val());
    var ToYear = parseInt($("#ddlToYearEducation").val());

    if (ToYear != NaN) {
        if (FromYear > ToYear) {
            alert("Error: From Year must be greater than or equal to To Year!");
            //$("#ddlFromYearEducation").focus();
            $("#ddlFromYearEducation").css("border", "1px solid red");
            $("#ddlToYearEducation").css("border", "1px solid red");
            return false;
        }
        else {
            $("#ddlFromYearEducation").css("border", "");
            $("#ddlToYearEducation").css("border", "");
        }
    }

    if (ValidateControl($("#ddlFromMonthEducation")) == false)
        return false;


    var isChecked = $("#chIsPresentForEducation").prop('checked');

    if (!isChecked) {

        if (FromYear == ToYear) {
            var FromMonth = parseInt($("#ddlFromMonthEducation").val());
            var ToMonth = parseInt($("#ddlToMonthEducation").val());

            if (FromMonth != NaN) {
                if (FromMonth > ToMonth) {
                    alert("Error: From Month must be greater than or equal to To Month !");
                    $("#ddlFromMonthEducation").focus();
                    $("#ddlFromMonthEducation").css("border", "1px solid red");
                    $("#ddlToMonthEducation").css("border", "1px solid red");
                    return false;
                }
                else {
                    $("#ddlFromMonthEducation").css("border", "");
                    $("#ddlToMonthEducation").css("border", "");
                }
            }

        }

        if (ValidateControl($("#ddlToYearEducation")) == false)
            return false;
        if (ValidateControl($("#ddlToMonthEducation")) == false)
            return false;
    }

    if (ValidateControl($("#txtMajorSubjects")) == false) {
        return false;
    }
    if (ValidateControl($("#txtCourseTitle")) == false) {
        return false;
    }

    if (ValidateControl($("#txtEducationLocation")) == false) {
        return false;
    }
    if (ValidateControl($("#txtAboutEducation")) == false) {
        return false;
    }


    var elem = $('#dvModalEducation');
    elem.css("opacity", "0.5");
    ShowLoading(elem);
    $("#btnAddEducation").attr("disabled", true);

    $.ajax({
        url: "/Users/AddEducation",
        data: $('form[id="frmEducation"]').serialize(),
        type: 'POST',
        success: function (data) {

            if (data.indexOf("saved") != null) {

                $("#btnAddEducation").attr("disabled", false);
                GetEducationList();
            }

            if (data.toString().indexOf("Error") >= 0) {
                ErrorMsgTimeOut(data);

            }
            $("#btnAddEducation").attr("disabled", false);
            $('#dvModalEducation').modal('hide');
            HideLoading(elem);
            elem.css("opacity", "1");

        },
        error: function (e) {
            ErrorMsgTimeOut(e.error);
            HideLoading(elem);
            elem.css("opacity", "1");
            $("#btnAddEducation").attr("disabled", false);
        }
    });

}


function EditEducation(id) {
    ClearBorderError();

    $('#dvModalEducation').modal('show');

    var EducationType = $(id).parents('.row .employ-row').find('.hdnEducationType').val()
    var CourseTitle = $(id).parents('.row .employ-row').find('.hdnCourseTitle').val()
    var NameOfInstitute = $(id).parents('.row .employ-row').find('.hdnNameOfInstitute').val();

    var FromYear = $(id).parents('.row .employ-row').find('.hdnEducationFromYear').val();
    var FromMonth = $(id).parents('.row .employ-row').find('.hdnEducationFromMonth').val();

    var ToYear = $(id).parents('.row .employ-row').find('.hdnEducationToYear').val();
    var ToMonth = $(id).parents('.row .employ-row').find('.hdnEducationToMonth').val();

    var IsPresent = $(id).parents('.row .employ-row').find('.hdnEducationIsPresent').val();
    var MajorSubjects = $(id).parents('.row .employ-row').find('.hdnMajorSubjects').val();
    var EducationLocation = $(id).parents('.row .employ-row').find('.hdnEducationLocation').val();

    var EducationDescription = $(id).parents('.row .employ-row').find('.hdnEducationDescription').val();
    var UEDID = $(id).parents('.row .employ-row').find('.hdnUEDID').val();

    $('#txtNameOfInstitute').val(NameOfInstitute);

    $('#ddlEducationType').val(EducationType);
    $('#ddlFromYearEducation').val(FromYear);
    $('#ddlFromMonthEducation').val(FromMonth);

    $('#ddlToYearEducation').val(ToYear);
    $('#ddlToMonthEducation').val(ToMonth);


    if (IsPresent == "false" || IsPresent == "null")
        $('#chIsPresentForEducation').prop('checked', false);
    else {
        $('#chIsPresentForEducation').prop('checked', IsPresent);
        $("#ddlToYearEducation").attr("disabled", true);
        $("#ddlToMonthEducation").attr("disabled", true);

        $("#ddlToYearEducation")[0].selectedIndex = 0;
        $("#ddlToMonthEducation")[0].selectedIndex = 0;

    }




    $('#txtMajorSubjects').val(MajorSubjects);
    $('#txtCourseTitle').val(CourseTitle);
    $('#txtEducationLocation').val(EducationLocation);
    $('#txtAboutEducation').val(EducationDescription);
    $('#hdnUEDID').val(UEDID);

}

function DeleteEducation(id) {

    var r = confirm("Are you sure, you want to delete?");
    if (r == true) {
        var elem = $('#dvEducation');
        elem.css("opacity", "0.5");
        ShowLoading(elem);

        $.ajax({
            url: "/Users/DeleteEducation",
            type: 'POST',
            data: { EID: id },
            datatype: "json",
            success: function (data) {
                try {
                    if (data.toString().indexOf("deleted") >= 0) {
                        GetEducationList();
                    }
                    if (data.toString().indexOf("Error") >= 0) {
                        ErrorMsgTimeOut(data);
                    }
                    else {
                        ErrorMsgTimeOut("Error: Could not be deleted");
                    }

                    HideLoading(elem);
                    elem.css("opacity", "1");


                }
                catch (e) {
                    ErrorMsgTimeOut(e.message)
                }


            },
            error: function (e) {
                ErrorMsgTimeOut(e.error);
            }
        });
    }
}