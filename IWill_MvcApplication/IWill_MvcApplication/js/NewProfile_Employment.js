
function ResetEmployment() {

    $("#txtCompanyName").val('');
    $("#txtPost").val('');
    $("#txtEmploymentLocation").val('');
    $("#txtAboutExperience").val('');

    $("#ddlFromYearEmployment")[0].selectedIndex = 0;
    $("#ddlFromMonthEmployment")[0].selectedIndex = 0;
    $("#ddlToYearEmployment")[0].selectedIndex = 0;
    $("#ddlToMonthEmployment")[0].selectedIndex = 0;

    $("#ddlToYearEmployment").attr("disabled", false);
    $("#ddlToMonthEmployment").attr("disabled", false);

    $("#chIsPresentForEmployment").prop('checked', false);



}

function ClearBorderError() {
    $("#txtCompanyName").css("border", "");
    $("#txtPost").css("border", "");
    $("#txtEmploymentLocation").css("border", "");
    $("#txtAboutExperience").css("border", "");

    $("#ddlFromYearEmployment").css("border", "");
    $("#ddlFromMonthEmployment").css("border", "");
    $("#ddlToYearEmployment").css("border", "");
    $("#ddlToMonthEmployment").css("border", "");
}

function AddEmployment() {

    if (ValidateControl($("#txtCompanyName")) == false) {
        return false;
    }
    if (ValidateControl($("#txtPost")) == false) {
        return false;
    }
    if (ValidateControl($("#txtEmploymentLocation")) == false) {
        return false;
    }

    if (ValidateControl($("#ddlFromYearEmployment")) == false)
        return false;

    var FromYear = parseInt($("#ddlFromYearEmployment").val());
    var ToYear = parseInt($("#ddlToYearEmployment").val());

    if (ToYear != NaN) {
        if (FromYear > ToYear) {
            alert("Error: From Year must be greater than or equal to To Year!");
            $("#ddlFromYearEmployment").focus();
            $("#ddlFromYearEmployment").css("border", "1px solid red");
            $("#ddlToYearEmployment").css("border", "1px solid red");
            return false;
        }
        else {
            $("#ddlFromYearEmployment").css("border", "");
            $("#ddlToYearEmployment").css("border", "");
        }
    }

    if (ValidateControl($("#ddlFromMonthEmployment")) == false)
        return false;


    var isChecked = $("#chIsPresentForEmployment").prop('checked');

    if (!isChecked) {

        if (FromYear == ToYear) {
            var FromMonth = parseInt($("#ddlFromMonthEmployment").val());
            var ToMonth = parseInt($("#ddlToMonthEmployment").val());

            if (FromMonth != NaN) {
                if (FromMonth > ToMonth) {
                    alert("Error: From Month must be greater than or equal to To Month !");
                    $("#ddlFromMonthEmployment").focus();
                    $("#ddlFromMonthEmployment").css("border", "1px solid red");
                    $("#ddlToMonthEmployment").css("border", "1px solid red");
                    return false;
                }
                else {
                    $("#ddlFromMonthEmployment").css("border", "");
                    $("#ddlToMonthEmployment").css("border", "");
                }
            }

        }

        if (ValidateControl($("#ddlToYearEmployment")) == false)
            return false;
        if (ValidateControl($("#ddlToMonthEmployment")) == false)
            return false;
    }

    var elem = $('#dvModalEmployment');
    elem.css("opacity", "0.5");
    ShowLoading(elem);
    $("#btnAddEmployment").attr("disabled", true);

    $.ajax({
        url: "/Users/AddExperience",
        data: $('form[id="frmExperience"]').serialize(),
        type: 'POST',
        success: function (data) {

            if (data.indexOf("saved") != null) {

                $("#btnAddEmployment").attr("disabled", false);
                GetEmploymentList();
            }

            if (data.toString().indexOf("Error") >= 0) {
                ErrorMsgTimeOut(data);

            }
            $("#btnAddEmployment").attr("disabled", false);
            $('#dvModalEmployment').modal('hide');
            HideLoading(elem);
            elem.css("opacity", "1");

        },
        error: function (e) {
            ErrorMsgTimeOut(e.error);
            HideLoading(elem);
            elem.css("opacity", "1");
            $("#btnSubmitSocialLinksAndLocation").attr("disabled", false);
        }
    });

}

GetEmploymentList = function () {

    $.ajax({
        url: "/Users/UserExprnceList",
        type: 'POST',
        datatype: "json",
        success: function (data) {
            try {
                if (data != null) {


                    var dvEmployment = $("#dvEmployment");
                    dvEmployment.empty();
                    var EmplHeading = $("<div class='panel-heading iwill-panel-heading'>" +
                                            "<h3 class='panel-title iwill-panel-title'>Employment<a href='#' data-toggle='modal' data-target='#dvModalEmployment' onclick='ResetEmployment()' style='float:right;'><i class='fa fa-plus' aria-hidden='true'></i></a></h3>" +
                                        "</div>");
                    dvEmployment.append(EmplHeading);

                    var dvEmploymentBody = $("<div class='panel-body dvEmploymentBody' style='text-align:justify;'></div>");
                    $.each(data, function (i, v) {
                        var row;

                        row = "<div class='row employ-row'>" +
                                       "<div class='col-lg-11 col-md-11'>" +
                                           "<h4 class='employ-desig'>" + v.CompanyName + "</h4>" +
                                           "<h5 class='employ-addres'>" + v.UserPost + ", " + v.Location + " </h5>";

                        if (v.ToYear != null)
                            row += "<h6 class='employ-date'>" + v.FromYear + " - " + v.ToYear + "</h6>";

                        else
                            row += "<h6 class='employ-date'>" + v.FromYear + " - Present</h6>";


                        if (v.Description != null) {
                            if (v.Description.toString().length > 40)
                                row += "<p>" + v.Description.toString().substring(0, 40) + "... </p>";
                            else 
                                row += "<p>" + v.Description.toString() + "</p>";
                        }

                        row += "<input class='hdnUserPost' type='hidden'  value='" + v.UserPost + "'/>" +
                                "<input class='hdnLocation' type='hidden'  value='" + v.Location + "'/>" +
                                "<input class='hdnFromYear' type='hidden'  value='" + v.FromYear + "'/>" +
                                "<input class='hdnFromMonth' type='hidden'  value='" + v.FromMonth + "'/>" +

                                "<input class='hdnToYear' type='hidden'  value='" + v.ToYear + "'/>" +
                                "<input class='hdnToMonth' type='hidden'  value='" + v.ToMonth + "'/>" +
                                "<input class='hdnEmplmntDescription' type='hidden'  value='" + v.Description + "'/>" +
                                "<input class='hdnIsPresent' type='hidden'  value='" + v.IsPresent + "'/>" +
                                "<input class='hdnUEXID' name='UEXID' type='hidden'  value='" + v.UEXID + "'/>" +

                              "</div>" +
                            "<div class='col-lg-1 col-md-1 employ-edit'><a href='#' name='" + v.UEXID + "' onclick='EditEmployment(this)'><i class='fa fa-pencil' aria-hidden='true'></i></a></div>" +
                            "<div class='col-lg-1 col-md-1 employ-edit'><a href='#'  onclick='DeleteEmployment(" + v.UEXID + ")'><i class='fa fa-pencil' aria-hidden='true'></i></a></div>" +
                            "</div>";
                        dvEmploymentBody.append($(row));
                    });

                    dvEmployment.append(dvEmploymentBody);
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

function EditEmployment(id) {
    ClearBorderError();

    $('#dvModalEmployment').modal('show');
    var CompanyName = $(id).parents('.row .employ-row').find('.employ-desig').html()
    var Post = $(id).parents('.row .employ-row').find('.hdnUserPost').val()
    var Location = $(id).parents('.row .employ-row').find('.hdnLocation').val();

    var FromYear = $(id).parents('.row .employ-row').find('.hdnFromYear').val();
    var FromMonth = $(id).parents('.row .employ-row').find('.hdnFromMonth').val();

    var ToYear = $(id).parents('.row .employ-row').find('.hdnToYear').val();
    var ToMonth = $(id).parents('.row .employ-row').find('.hdnToMonth').val();
    var IsPresent = $(id).parents('.row .employ-row').find('.hdnIsPresent').val();
    var EmplmntDescription = $(id).parents('.row .employ-row').find('.hdnEmplmntDescription').val();
    var UEXID = $(id).parents('.row .employ-row').find('.hdnUEXID').val();

    $('#txtCompanyName').val(CompanyName);
    $('#txtPost').val(Post);
    $('#txtEmploymentLocation').val(Location);

    $('#ddlFromYearEmployment').val(FromYear);
    $('#ddlFromMonthEmployment').val(FromMonth);

    $('#ddlToYearEmployment').val(ToYear);
    $('#ddlToMonthEmployment').val(ToMonth);


    if (IsPresent == "false" || IsPresent == "null")
        $('#chIsPresentForEmployment').prop('checked', false);
    else {
        $('#chIsPresentForEmployment').prop('checked', IsPresent);
        $("#ddlToYearEmployment").attr("disabled", true);
        $("#ddlToMonthEmployment").attr("disabled", true);

        $("#ddlToYearEmployment")[0].selectedIndex = 0;
        $("#ddlToMonthEmployment")[0].selectedIndex = 0;

    }

    $('#txtAboutExperience').val(EmplmntDescription);
    $('#hdnUEXID').val(UEXID);

}

function DeleteEmployment(id) {

    var r = confirm("Are you sure, you want to delete?");
    if (r == true) {
        var elem = $('#dvEmployment');
        elem.css("opacity", "0.5");
        ShowLoading(elem);

        $.ajax({
            url: "/Users/DeleteEmployment",
            type: 'POST',
            data: { EID: id },
            datatype: "json",
            success: function (data) {
                try {
                    if (data.toString().indexOf("deleted") >= 0) {
                        GetEmploymentList();
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

function PresentForEmployment_Selected(obj) {

    var isChecked = $(obj).prop('checked');


    if (isChecked) {
        $("#ddlToYearEmployment").attr("disabled", true);
        $("#ddlToMonthEmployment").attr("disabled", true);
        $(obj).next().val('true');
    }
    else {
        $("#ddlToYearEmployment").attr("disabled", false);
        $("#ddlToMonthEmployment").attr("disabled", false)
        $(obj).next().val('false');
    }
}