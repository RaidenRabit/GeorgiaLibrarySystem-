//on load
$(function () {

    GetMaterials();
    //on search option change get corresponding invoices
    $("#numOfRecords, #isbn, #materialTitle, input[name=jobStatus], #author").change(function () {
        GetMaterials();
    });
});

//gets materials
function GetMaterials() {
    $.ajax({
        type: "GET",
        url: "/Materials/GetMaterialsAjax",
        data: {
            numOfRecords: $("#numOfRecords").val(),
            isbn: $("#isbn").val(),
            materialTitle: $("#materialTitle").val(),
            jobStatus: $("input[name=jobStatus]:checked").val(),
            author: $("#author").val()
        },

        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var htmlText = "";
                    for (var i = 0; i < data.length; i++) {
                        var x = parseInt(data[i].Available_Copies, 10);
                        materialType = ["", "Books", "Maps", "Needed Books", "Rare Books", "Reference Books"];
                        htmlText += "<tr>" +
                            "<td class=\"isbn\">" +
                            data[i].ISBN +
                            "</td>" +
                            "<td>" +
                            data[i].Title +
                            "</td>" +
                            "<td>" +
                            data[i].Author +
                            "</td>" +
                            "<td>" +
                            data[i].TypeName +
                            "</td>" +
                            "<td>" +
                            data[i].Description +
                            "</td>" +
                            "<td>" +
                            data[i].Location +
                            "</td>" +
                            "<td>" +
                            x +
                            "</td>";
                        if (x > 0) {
                            htmlText += "<td>" +
                                "<button class=\"btn btn-primary\"  onclick=\"Borrow(this)\">" +
                                "Borrow Book" +
                                "</td>" +
                                "</tr>";
                        } 
                        else {
                            htmlText += "<td>" +
                                "<button class=\"btn btn-primary\"  onclick=\"Borrow(this)\" disabled>" +
                                "Borrow Book" +
                                "</td>" +
                                "</tr>";
                        }
                    }
            $("#materialTable tbody").html(htmlText);
        }
    });
}


function Borrow(obj) {
    var isbn = $(obj).closest("tr").find("td:nth-child(1)").html();
    $.ajax({
        type: "GET",
        url: "/Material/Borrow",
        data: {
            id: isbn
        },
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function(response) {
            GetMaterials();
            InitializeDialog(response);
        }
    });
};

function InitializeDialog(data) {
    var dialog = $("#dialog").dialog({
        autoOpen: false,
        modal: true,
        title: viewDetails,
        width: 1000,
        buttons: {
            cancel : function () {
                $(this).dialog("close");
            }
        }
    });

    dialog.html(data);
    dialog.dialog("open");
};