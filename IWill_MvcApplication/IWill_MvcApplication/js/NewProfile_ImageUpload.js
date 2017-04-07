

$().ready(function () {

    

    var BackTop = $("#hdnBackTop").val();
    var BackLeft = $("#hdnBackLeft").val();

    $('#dvImageBackground img').css({
        transform: "translate3d(" + BackLeft + "px, " + BackTop + "px, 0px)"
    });

    var hdnFrontFileName = $("#hdnFrontFileName").val();
    var hdnBackgroundFileName = $("#hdnBackgroundFileName").val();

    //show User Front Pic
    if (hdnBackgroundFileName == null || hdnBackgroundFileName == "" || hdnBackgroundFileName.length == 0)
        $("#imgBackUpload").attr("src", "../Images/blankImage.png");
    else {
        $("#imgBackUpload").attr("src", "../Images/UserImages/" + hdnBackgroundFileName);
        //$("#fuBackgroundImage").hide();
        $("#dvfuBackgroundImage").hide();
        $(".progress").hide();
        $("#dvEditIcon").show();

    }

    //show User Front Pic
    if (hdnFrontFileName == null || hdnFrontFileName == "" || hdnFrontFileName.length == 0)
        $("#imgUploadThumbnail").attr("src", "../Images/blankImage.png");
    else {
        $("#imgUploadThumbnail").attr("src", "../Images/UserImages/" + hdnFrontFileName);
        $("#dvEditFrontPicIcon").fadeIn();
        $("#dvfuFrontImage").fadeOut();

    }

    GetEmploymentList();
    GetEducationList();



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


    ShowLoading(elemBack);

    var file = $("#hdnBackgroundFileName").val();
    var imageTrans = $('#imgBackUpload').css("-webkit-transform");
    var results = imageTrans.match(/matrix(?:(3d)\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}\d+))(?:, (-{0,1}\d+))(?:, (-{0,1}\d+)), -{0,1}\d+\)|\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}.+))(?:, (-{0,1}.+))\))/);

    var BCimgTOP = results[6];
    var BCimgLeft = results[5];

    $.ajax({
        url: "/Users/UpdateBackImagePostion",
        type: 'GET',
        data: { FileName: file, BCimgTOP: BCimgTOP, BCimgLeft: BCimgLeft },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "updated") {
                $("#dvFrontPicAndData").fadeIn();
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
                    $(".loader").fadeOut();
                }
            }
            $("#IsBackPicEdit").val(false);
            HideLoading(elemBack);
        },
        error: function (ex) {
            alert(ex.responseText);
            $("#loadingBackgroundPic").fadeOut();
        }
    });
});


//********************************************************************ENDED***********************************************************************


//*******************************************************************************************************************************************
//**************************************************Upload Front Image Code*************************************************************
//*******************************************************************************************************************************************




$("#fuFrontImage").change(function () {

    var fileName = $("#fuFrontImage").val();
    var maxFileSize = 2 * 1024 * 1024;
    ShowLoading(elemFront);
    if (fileName.length > 0) {
        var ext = fileName.substr(fileName.lastIndexOf('.') + 1).toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            alert('invalid file type!');
            return;
        }
        if ($("#fuFrontImage")[0].files[0].size > maxFileSize) {
            alert('only 2 MB file could be uploaded');
            return;
        }

        var fileUpload = document.getElementById("fuFrontImage");
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
                if (height < 150 || width < 150) {
                    alert("Front Image Height and Width must be atleast 150 px wide and 150px in height .");

                    // if ($("#IsFrontPicEdit").val().toLowerCase() == 'false') {
                    $("#imgUploadThumbnail").css('opacity', '1');

                    //  }

                    $("#dvEditFrontPicOptions").fadeOut();
                    $("#dvfuFrontImage").fadeIn();

                    HideLoading(elemFront);
                    $("#fuFrontImage").val('');
                    return;
                }
                if (height > 400 || width > 400) {
                    alert("Front Image Height and Width not more than 300px wide and 300px in height .");

                    // if ($("#IsFrontPicEdit").val().toLowerCase() == 'false') {
                    $("#imgUploadThumbnail").css('opacity', '1');

                    //  }

                    $("#dvEditFrontPicOptions").fadeOut();
                    $("#dvfuFrontImage").fadeIn();

                    HideLoading(elemFront);
                    $("#fuFrontImage").val('');
                    return;
                }

                else {

                    var formData = new FormData();
                    var totalFiles = document.getElementById("fuFrontImage").files.length;
                    for (var i = 0; i < totalFiles; i++) {
                        var file = document.getElementById("fuFrontImage").files[i];

                        formData.append("fuFrontImage", file);
                    }

                    $.ajax({
                        url: "/Users/FrontPicUpload",
                        type: 'POST',
                        data: formData,
                        dataType: 'json',
                        contentType: false,
                        processData: false,

                        success: function (data) {
                            var IsSaved = data.split('^')[0];

                            if (IsSaved == 'saved') {
                                var FileName = data.split('^')[1];

                                $("#imgUploadThumbnail").attr("src", "../Images/Temp/" + FileName);
                                $("#dvFrontPicUploadOptions").fadeIn();

                                $("#fuFrontImage").fadeOut();
                                $("#hdnFrontFileName").val(FileName);
                                if ($("#IsFrontPicEdit").val().toLocaleLowerCase() == "true")
                                    $("#dvEditFrontPicOptions").hide();
                                $("#imgUploadThumbnail").css('opacity', '0.6');


                                HideLoading(elemFront);
                            }
                            else {
                                if (data.indexOf("notsaved") == 0) {
                                    $("#dvRemovepicTemp").fadeIn();

                                    $("#dvfuFrontImage").fadeOut();
                                    alert("picture is not saved")
                                }

                                if (data.indexOf("exists") >= 0) {
                                    alert("this file already exists");
                                }

                                HideLoading(elemFront);

                            }

                        },
                        error: function (ex) {

                            alert(ex.responseText)
                            HideLoading(elemFront);
                        }

                    });
                }
            };





        }
    }
    //submit the form here
});



$("#EditFrontImageIcon").click(function () {
    $("#imgUploadThumbnail").css('opacity', '0.6');
    $(this).parent().fadeOut();
    $("#dvEditFrontPicOptions").show();
    $("#fuFrontImage").val('');
    $("#dvfuFrontImage").show();
    $("#IsFrontPicEdit").val(true);

});

$("#EditCancel").click(function () {
    $("#imgUploadThumbnail").css('opacity', '1');
    $("#dvEditFrontPicIcon").show();
    $("#dvEditFrontPicOptions").hide();
    $("#dvfuFrontImage").hide();
    $("#IsFrontPicEdit").val(false);
});


//********************************************************************ENDED***********************************************************************

//https://www.youtube.com/watch?v=f-wXTpbNWoM
//https://css-tricks.com/examples/DragAndDropFileUploading/
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



function RemoveTempFrontPic() {

    ShowLoading(elemFront);
    var FileName = $("#hdnFrontFileName").val();
    $.ajax({
        url: "/Users/RemoveTempFrontPic",
        type: 'GET',
        data: { FileName: FileName },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "deleted") {
                $("#dvFrontPicUploadOptions").fadeOut();
                $("#dvfuFrontImage").fadeIn();
                $("#imgUploadThumbnail").attr("src", "../Images/blankImage.png")
                $("#fuFrontImage").val('');

            }
            HideLoading(elemFront);
        },
        error: function (ex) {
            HideLoading(elemFront);
            alert(ex.responseText)
        }
    });


}

function SaveFrontPic() {

    ShowLoading(elemFront);
    $("#imgUploadThumbnail").css('opacity', '0.5');
    var file = $("#hdnFrontFileName").val();
    //var imageTrans = $('#imgBackUpload').css("-webkit-transform");
    //var results = imageTrans.match(/matrix(?:(3d)\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}\d+))(?:, (-{0,1}\d+))(?:, (-{0,1}\d+)), -{0,1}\d+\)|\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}.+))(?:, (-{0,1}.+))\))/);

    //var BCimgTOP = results[6];
    //var BCimgLeft = results[5];
    //var IsEditPic = $("#IsBackPicEdit").val();
    //alert("BCimgTOP =" + BCimgTOP + "     BCimgLeft = " + BCimgLeft );

    $.ajax({
        url: "/Users/SaveFrontPic",
        type: 'GET',
        //data: { FileName: file, BCimgTOP: BCimgTOP, BCimgLeft, BCimgLeft, IsBackPicEdit: IsEditPic },
        data: { FileName: file },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "saved") {
                $("#dvFrontPicUploadOptions").fadeOut();

                $("#imgUploadThumbnail").css('opacity', '1');
                $("#imgUploadThumbnail").attr("src", "../Images/UserImages/" + file)

                $("#dvfuFrontImage").fadeOut();
                $("#dvEditFrontPicIcon").fadeIn();

            }
            $("#IsFrontPicEdit").val(false);
            HideLoading(elemFront);
        },
        error: function (ex) { alert(ex.responseText); HideLoading(elemBack); }
    });
    //DiableBackgroundDraggable();


}



function RemoveFrontPic() {
    $("#imgUploadThumbnail").css('opacity', '0.5');
    ShowLoading(elemFront);

    var FileName = $("#hdnFrontFileName").val();
    var EmailAddress = $("#txtEmail").val();



    $.ajax({
        url: "/Users/RemoveFrontPic",
        type: 'GET',
        data: { FileName: FileName, Email: EmailAddress },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "deleted") {
                $("#dvFrontPicUploadOptions").hide();
                $("#dvEditFrontPicIcon").hide();
                $("#dvfuFrontImage").show();
                $("#imgUploadThumbnail").attr("src", "../Images/blankImage.png")
                $("#imgUploadThumbnail").css('opacity', '1');
                $("#fuFrontImage").val('');
                $("#dvEditFrontPicOptions").hide();

            }
            HideLoading(elemFront);
        },
        error: function (ex) {
            HideLoading(elemFront);
            alert(ex.responseText)
        }
    });


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
        url: "/Users/UserBackgroundFileUpload",
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
                //$("#fuBackgroundImage").fadeOut();
                $("#dvfuBackgroundImage").fadeOut();

                //$("#dvSaveBackgroundPic").fadeIn();
                // $("#dvBackgroundRemovepic").fadeIn();
                $("#dvBackImgOptions").fadeIn();

                $("#BackImgDragPostionText").fadeIn();

                //$("#fuBackgroundImage").prop('disabled', true);
                //$("#fuBackgroundImage").fadeOut();

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
                $("#fuBackgroundImage").val('');
                $("#imgBackUpload").css('opacity', '1');
                $(".progress").fadeOut();
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
    var IsBackPicEdit = $("#IsBackPicEdit").val();

    $.ajax({
        url: "/Users/RemoveBackgroundPic",
        type: 'GET',
        data: { FileName: file, IsBackPicEdit: IsBackPicEdit },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "deleted") {
                $("#dvBackImgOptions").fadeOut();
                $("#dvFrontPicAndData").fadeIn();
                //$("#dvBackgroundRemovepic").fadeOut();
                //$("#dvSaveBackgroundPic").fadeOut();
                $("#BackImgDragPostionText").fadeOut();

                if (IsBackPicEdit.toLocaleLowerCase() == "false") {
                    DiableBackgroundDraggable();
                }

                //$("#fuBackgroundImage").fadeIn();
                $("#dvfuBackgroundImage").fadeIn();
                $("#imgBackUpload").css('opacity', '1');
                $("#imgBackUpload").attr("src", "../Images/blankImage.png")

                $("#fuBackgroundImage").val('')

                $("#ProgressBar").attr('aria-valuenow', 0).css('width', 0 + '%').text(0 + '%');
                $('#dvImageBackground img').css({
                    transform: "translate3d(0px,0px, 0px)"
                });

                $("#dvEditBackImageOptions").fadeOut();

            }
            //$("#loadingBackgroundPic").fadeOut();
            $("#IsBackPicEdit").val(false);
            HideLoading(elemBack);
        },
        error: function (ex) { alert(ex.responseText); $("#loadingBackgroundPic").fadeOut(); }
    });


}

function SaveBackgroundPic() {

    ShowLoading(elemBack);

    var file = $("#hdnBackgroundFileName").val();
    var imageTrans = $('#imgBackUpload').css("-webkit-transform");
    var results = imageTrans.match(/matrix(?:(3d)\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}\d+))(?:, (-{0,1}\d+))(?:, (-{0,1}\d+)), -{0,1}\d+\)|\(-{0,1}\d+(?:, -{0,1}\d+)*(?:, (-{0,1}.+))(?:, (-{0,1}.+))\))/);

    var BCimgTOP = results[6];
    var BCimgLeft = results[5];
    var IsEditPic = $("#IsBackPicEdit").val();
    //if (IsEditPic == null)
    //alert("BCimgTOP =" + BCimgTOP + "     BCimgLeft = " + BCimgLeft );

    $.ajax({
        url: "/Users/SaveBackgroundPic",
        type: 'GET',
        data: { FileName: file, BCimgTOP: BCimgTOP, BCimgLeft, BCimgLeft, IsBackPicEdit: IsEditPic },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            if (r == "saved") {
                $("#dvBackImgOptions").fadeOut();
                $("#dvFrontPicAndData").fadeIn();
                $("#fuBackgroundImage").val('')
                $("#fuBackgroundImage").fadeOut();
                $("#BackImgDragPostionText").fadeOut();


                $("#imgBackUpload").css('opacity', '1');
                $("#imgBackUpload").attr("src", "../Images/UserImages/" + file)

                $("#dvEditIcon").fadeIn();

            }
            else
                if (r.indexOf("Error") >= 0) {
                    alert(r);
                }
            $("#IsBackPicEdit").val(false);
            HideLoading(elemBack);
        },
        error: function (ex) { alert(ex.responseText); HideLoading(elemBack); }
    });
    DiableBackgroundDraggable();
}

