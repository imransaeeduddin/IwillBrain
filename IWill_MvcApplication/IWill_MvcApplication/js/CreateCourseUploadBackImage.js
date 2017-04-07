
$().ready(function () {



    //*******************************************************************************************************************************************
    //**************************************************Upload background Image Code*************************************************************
    //*******************************************************************************************************************************************

    $("#fuBackgroundImage").change(function () {

        $("#imgBackUpload").css('opacity', '0.3');
        ShowLoading(elemBack);

        var fileName = $("#fuBackgroundImage").val();
        var maxFileSize = 2 * 1024 * 1024;


        if (fileName.length > 0) {
            var ext = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
            if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
                alert('invalid file type!');
                HideLoading(elemBack);
                return;
            }

            if ($("#fuBackgroundImage")[0].files[0].size > maxFileSize) {
                alert('only 2 MB file could be uploaded');
                HideLoading(elemBack);
                return;
            }
            var fileUpload = document.getElementById("fuBackgroundImage");
            var reader = new FileReader();
            reader.readAsDataURL(fileUpload.files[0]);
            var IsHeightandWidthExceeded = false;
            reader.onload = function (e) {
                //Initiate the JavaScript Image object.
                var image = new Image();

                //Set the Base64 string return from FileReader as source.
                image.src = e.target.result;

                //Validate the File Height and Width.
                image.onload = function () {
                    var height = this.height;
                    var width = this.width;
                    if (height < 300 || width < 1000) {
                        alert("Height and Width must be atleast 1000px wide and 300px in height .");
                        if ($("#IsBackPicEdit").val().toLowerCase() == 'false') {
                            $("#imgBackUpload").css('opacity', '1');

                        }
                        //else {
                        //    $("#dvEditIcon").fadeIn();
                        //    $("#fuBackgroundImage").fadeOut();
                        //}
                        $("#dvEditIcon").fadeOut();
                        //$("#fuBackgroundImage").fadeIn();
                        $("#dvfuBackgroundImage").fadeIn();

                        HideLoading(elemBack);
                        $("#fuBackgroundImage").val('');
                        return;
                    }

                    else {
                        UploadBackgroundImage(elemBack);
                    }
                };

            }


        }

    });


    $("#EditBackImageIcon").click(function () {
        $("#imgBackUpload").css('opacity', '0.3');
        $(this).parent().fadeOut();
        // $("#fuBackgroundImage").fadeIn();
        $("#dvfuBackgroundImage").fadeIn();
        $("#IsBackPicEdit").val(true);
        $("#dvEditBackImageOptions").fadeIn();
        $("#dvFrontPicAndData").fadeOut();
    });

    $("#ECancle").click(function () {
        // $("#fuBackgroundImage").fadeOut();
        $("#dvfuBackgroundImage").fadeOut();
        $("#dvEditBackImageOptions").fadeOut();
        $("#imgBackUpload").css('opacity', '1');
        $("#dvEditIcon").fadeIn();
        $("#dvFrontPicAndData").fadeIn();

    });

    $("#ERepositioning").click(function () {
        EnableBackgroundDraggable();


        // $("#fuBackgroundImage").fadeOut();
        $("#dvfuBackgroundImage").fadeOut();
        $("#dvEditBackImageOptions").fadeOut();
        $("#BackImgDragPostionText").fadeIn();
        $("#dvEditBackImgDragOptions").fadeIn();

    });

    $("#RemoveEditDraggedIcon").click(function () {
        $("#dvEditBackImgDragOptions").fadeOut();
        //$("#dvEditBackImageOptions").fadeOut();
        $("#imgBackUpload").css('opacity', '1');
        $("#dvEditIcon").fadeIn();
        DiableBackgroundDraggable();
        $("#BackImgDragPostionText").fadeOut();
        $("#dvFrontPicAndData").fadeIn();
    });

    $("#SaveEditDraggedIcon").click(function () {

        var CategoryNam = $("input[name='CategoryName']").val();
        ShowLoading(elemBack);

        var file = $("#hdnBackgroundFileName").val();
        var imageTrans = $('#imgBackUpload').css("-webkit-transform");
        var results = imageTrans.match(/matrix(?:(3d)\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}\d+))(?:, (-{0,1}\d+))(?:, (-{0,1}\d+)), -{0,1}\d+\)|\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}.+))(?:, (-{0,1}.+))\))/);

        var BCimgTOP = results[6];
        var BCimgLeft = results[5];

        $.ajax({
            url: "/Course/UpdateBackImagePostion",
            type: 'GET',
            data: { CategoryName: CategoryNam, BCimgTOP: BCimgTOP, BCimgLeft: BCimgLeft },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                if (r == "updated") {
                    $("#dvEditBackImgDragOptions").fadeOut();
                    $("#BackImgDragPostionText").fadeOut();
                    $("#imgBackUpload").css('opacity', '1');
                    $("#dvEditIcon").fadeIn();

                    DiableBackgroundDraggable();

                }
                else {
                    if (r.indexOf('Error')) {
                        $("div.error").fadeIn();
                        $("div.error").html(ex.responseText)
                        setTimeout(function () { $('.error').fadeOut('slow'); }, 5000);
                        HideLoading(elemBack);
                    }
                }
                $("#IsBackPicEdit").val(false);
                HideLoading(elemBack);
            },
            error: function (ex) {
                alert(ex.responseText);
                HideLoading(elemBack);
            }
        });
    });


    //********************************************************************ENDED***********************************************************************


    $("#btnCreateCourseCategory").click(function () {

        var elemBack = $("#dvCreateCourseCategory");
        if ($("#fuBackgroundImage").val().length > 0)
            ShowLoading(elemBack);

        if (ValidateControl($("input[name='CategoryName']")) == false)
            return false;

        if ($("#hdnBackgroundFileName").val().length <= 0 || $("#hdnBackgroundFileName").val() == "null") {
            alert("please upload Category Image");
            return false;
        }
        $("#btnCreateCourseCategory").attr("disabled", true);


        var CategoryName = $("input[name='CategoryName']").val();
        var file = $("#hdnBackgroundFileName").val();

        var imageTrans = $('#imgBackUpload').css("-webkit-transform");
        var results = imageTrans.match(/matrix(?:(3d)\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}\d+))(?:, (-{0,1}\d+))(?:, (-{0,1}\d+)), -{0,1}\d+\)|\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}.+))(?:, (-{0,1}.+))\))/);

        var BCimgTOP = results[6];
        var BCimgLeft = results[5];
        var IsEditPic = $("#IsBackPicEdit").val();
        //var mod = { CCName: CategoryName, BackImage: file, BackLeft: BCimgLeft, BackTop: BCimgTOP };
        $.ajax({
            url: "/Course/CreateCategory",
            type: 'POST',
            //data: JSON.stringify(mod),
            data: { CCName: CategoryName, BackImage: file, BackLeft: BCimgLeft, BackTop: BCimgTOP, IsEditPic: IsEditPic },
            //contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                if (r == "saved" || r == "updated") {


                    if (r == "saved") {

                        location.reload();
                    }

                    if (r == "updated") {

                        $("#imgBackUpload").attr("src", "../Images/UserImages/" + file);
                        $("#dvfuBackgroundImage").hide();
                        $("#dvEditIcon").show();
                        $("#btnCreateCourseCategory").fadeOut();
                        $("#hdnBackgroundFileName").val(file);
                        $("input[name='CategoryName']").val(CategoryName);
                        $('#dvImageBackground img').css({
                            transform: "translate3d(" + BCimgLeft + "px, " + BCimgTOP + "px, 0px)"
                        });

                        window.scrollTo(0, 200);
                        $("div.success").fadeIn();
                        $("div.success").html('Course Category has been updated')
                        setTimeout(function () { $('.success').fadeOut('slow'); }, 3000);
                        $("#IsBackPicEdit").val(true);

                        $("#dvBackImgOptions").fadeOut();
                        $("#BackImgDragPostionText").fadeOut();
                        DiableBackgroundDraggable();

                        HideLoading(elemBack);

                        $("#btnCreateCourseCategory").hide();
                        $("#imgBackUpload").css('opacity', '1');
                        $("#btnCreateCourseCategory").attr("disabled", false);
                    }

                }
                else
                    if (r.indexOf("Error") >= 0) {
                        alert(r);
                    }


                //$("input[name='CategoryName']").val('');

            },
            error: function (ex) {
                $("#btnCreateCourseCategory").attr("disabled", false);
                alert(ex.responseText);
                HideLoading(elemBack);
            }

        });

    });


});

function EnableBackgroundDraggable() {
    Draggable.create("#dvImageBackground img", {
        type: "x,y",
        bounds: "#dvBackgroundPic"
    });
}

function DiableBackgroundDraggable() {
    Draggable.get("#dvImageBackground img").disable();
}



function UploadBackgroundImage(elemBack) {

    var formData = new FormData();
    var totalFiles = document.getElementById("fuBackgroundImage").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("fuBackgroundImage").files[i];

        formData.append("fuBackgroundImage", file);
    }

    $.ajax({

        xhr: function () {
            var xhr = new window.XMLHttpRequest();
            xhr.upload.addEventListener('progress', function (e) {
                if (e.lengthComputable) {
                    console.log('bytes loaded: ' + e.loaded);
                    console.log('total size : ' + e.total);
                    console.log('percentage: ' + e.loaded / e.total);
                    var percentage = Math.round((e.loaded / e.total) * 100);
                    $("#ProgressBar").attr('aria-valuenow', percentage).css('width', percentage + '%').text(percentage + '%');
                }
            });

            return xhr;
        },
        url: "/Course/CourseCategoryFileUpload",
        type: 'POST',
        data: formData,
        //dataType: 'json',
        contentType: false,
        processData: false,

        success: function (data) {
            var IsSaved = data.split('^')[0];
            if (IsSaved == 'saved') {
                var FileName = data.split('^')[1];

                $("#imgBackUpload").attr("src", "../Images/Temp/" + FileName);

                HideLoading(elemBack); $("#dvFrontPicAndData").fadeOut();

                $("#dvfuBackgroundImage").fadeOut();
                $("#dvBackImgOptions").fadeIn();

                $("#BackImgDragPostionText").fadeIn();

                $("#hdnBackgroundFileName").val(FileName);

                if ($("#IsBackPicEdit").val().toLocaleLowerCase() == "true")
                    $("#dvEditBackImageOptions").fadeOut();


                EnableBackgroundDraggable();


            }
            else {

                if (data.indexOf("notsaved") == 0) {
                    alert("picture is not saved")
                }

                if (data.indexOf("exists") >= 0) {
                    alert("this file already exists");
                }

                if (data.indexOf("limit exceeded") >= 0) {
                    alert(data);
                }
                $("#imgBackUpload").css('opacity', '1');
                //$(".progress").fadeOut();
                HideLoading(elemBack);
            }

        },
        error: function (ex) {
            HideLoading(elemBack);
            alert(ex.responseText)
        }

    });
}

function RevomebackgroundPic() {

    ShowLoading(elemBack);

    var file = $("#hdnBackgroundFileName").val();
    var CategoryNam = $("input[name='CategoryName']").val();
    var IsBackPicEdit = $("#IsBackPicEdit").val();

    $.ajax({
        url: "/Course/RemoveBackgroundPic",
        type: 'GET',
        data: { CategoryName: CategoryNam, FileName: file, IsBackPicEdit: IsBackPicEdit },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "deleted") {
                $("#dvBackImgOptions").fadeOut();
                $("#BackImgDragPostionText").fadeOut();

                if (IsBackPicEdit.toLocaleLowerCase() == "false") {
                    DiableBackgroundDraggable();

                }
                $("#hdnBackgroundFileName").val('');

                //else $("#IsBackPicEdit").val(false);

                $("#dvfuBackgroundImage").fadeIn();
                $("#imgBackUpload").css('opacity', '1');
                $("#imgBackUpload").attr("src", "../Images/blankImage.png")

                //$("#ProgressBar").attr('aria-valuenow', 0).css('width', 0 + '%').text(0 + '%');
                $('#dvImageBackground img').css({
                    transform: "translate3d(0px,0px, 0px)"
                });

                $("#dvEditBackImageOptions").fadeOut();
                $("#btnCreateCourseCategory").fadeIn();
            }
            //$("#loadingBackgroundPic").fadeOut();
            //else $("#IsBackPicEdit").val(false);
            HideLoading(elemBack);
        },
        error: function (ex) {
            alert(ex.responseText);
        }
    });


}

function EditCanclePic() {

    ShowLoading(elemBack);

    var file = $("#hdnBackgroundFileName").val();
    var CategoryNam = $("input[name='CategoryName']").val();


    $.ajax({
        url: "/Course/RemoveBackgroundPic",
        type: 'GET',
        data: { CategoryName: CategoryNam, FileName: file, IsBackPicEdit: "false" },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "deleted") {
                $("#dvBackImgOptions").fadeOut();
                $("#BackImgDragPostionText").fadeOut();

                //if (IsBackPicEdit.toLocaleLowerCase() == "false") {
                DiableBackgroundDraggable();
                //}

                $("#hdnBackgroundFileName").val('');
                $("#dvfuBackgroundImage").fadeIn();
                $("#imgBackUpload").css('opacity', '1');
                $("#imgBackUpload").attr("src", "../Images/blankImage.png")

                $('#dvImageBackground img').css({
                    transform: "translate3d(0px,0px, 0px)"
                });

                $("#dvEditBackImageOptions").fadeOut();
                $("#btnCreateCourseCategory").fadeIn();
            }
            //$("#loadingBackgroundPic").fadeOut();
            //else $("#IsBackPicEdit").val(false);
            HideLoading(elemBack);
        },
        error: function (ex) {
            alert(ex.responseText);
        }
    });


}


CategoryName_Changed = function () {


    var CategoryNam = $("input[name='CategoryName']").val();
    if (CategoryNam.length == 0)
        return;


    ShowLoading(elemBack);
    $.ajax({
        url: "/Course/GetCourseCategory",
        type: 'POST',
        data: { CategoryName: CategoryNam },
        datatype: "json",
        success: function (data) {
            if (data.toString().indexOf("nothing") < 0) {
                if (data.BackImage != null) {// || data.BackImage.length == 0
                    $("#imgBackUpload").attr("src", "../Images/UserImages/" + data.BackImage);
                    $("#dvfuBackgroundImage").hide();
                    $("#dvEditIcon").show();
                    $("#btnCreateCourseCategory").fadeOut();
                    $("#hdnBackgroundFileName").val(data.BackImage);
                    $("input[name='CategoryName']").val(data.CCName);
                    $('#dvImageBackground img').css({
                        transform: "translate3d(" + data.BackLeft + "px, " + data.BackTop + "px, 0px)"
                    });


                }
                else {
                    $("#IsBackPicEdit").val(true);
                    $("#dvBackImgOptions").fadeOut();
                    $("#BackImgDragPostionText").fadeOut();

                    $("#imgBackUpload").css('opacity', '1');
                    $("#imgBackUpload").attr("src", "../Images/blankImage.png")

                    $('#dvImageBackground img').css({
                        transform: "translate3d(0px,0px, 0px)"
                    });
                    $("#dvfuBackgroundImage").fadeIn();
                    $("#btnCreateCourseCategory").fadeIn();

                    $("#dvEditIcon").hide();

                    //$("#hdnBackgroundFileName").val('');
                }


            }
            else if (data.toString().indexOf("nothing") >= 0) {
                $("#dvBackImgOptions").fadeOut();
                $("#BackImgDragPostionText").fadeOut();

                $("#imgBackUpload").css('opacity', '1');
                $("#imgBackUpload").attr("src", "../Images/blankImage.png")

                $('#dvImageBackground img').css({
                    transform: "translate3d(0px,0px, 0px)"
                });
                $("#dvfuBackgroundImage").fadeIn();
                $("#btnCreateCourseCategory").fadeIn();
                $("#dvEditIcon").hide();
                $("#IsBackPicEdit").val(false);
                $("#hdnBackgroundFileName").val('');


            }
            HideLoading(elemBack);
        },
        error: function (e) {
            ErrorMsgTimeOut(e.error);
        }
    });





}