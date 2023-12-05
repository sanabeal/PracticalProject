
$(document).ready(function () {
    show_data();
});


//Load Data function  
function show_data() {
    $.ajax({
        url: "/Home/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {

                var rows = '<tr>'
                    + '<td>' + item.BillDate + '</td>'
                    + '<td>' + item.CustomerName + '</td>'
                    + '<td>' + item.ContactNo + '</td>'
                    + '<td><a onclick="set_value(\'' + item.BillMasterID + '\')"><i style="cursor: pointer;" class="glyphicon glyphicon-edit"></i></a>&nbsp;<a onclick="delete_value(\'' + item.BillMasterID + '\')"><i style="cursor: pointer;" class="glyphicon glyphicon-trash"></i></a>&nbsp;<a onclick="report_value(\'' + item.BillMasterID + '\')"><i style="cursor: pointer;" class="glyphicon glyphicon-print"></i></a></td>'
                    + '</tr>';
                $('#table_data tbody').append(rows);              
            });
       },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


function GotoPage() {   
    window.location.href = '/Home/Create/'
}

function set_value(id) {
    sessionStorage.setItem("sent", id);
    window.location.href = '/Home/Edit/'
}

function report_value(id) {


        //$.ajax({
        //    url: "/Home/ReportDt",
        //    data: "{id:'" + id + "'}",
        //    type: "post",
        //    contentType: "application/json;charset=utf-8",
        //    dataType: "json",
        //    success: function (msg) {
        //    },            
        //});
  


    //sessionStorage.setItem("sent", id);
    //window.open('/Home/Report/', 'target="_blank"');

}

function delete_value(id) {

    if (confirm("Are you sure delete?")) {    
    $.ajax({
        url: "/Home/DeleteDt",
        data: "{id:'" + id + "'}",
        type: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (msg) {
            window.location.href = '/Home/Index/'
        },
        error: function (xhr, textStatus, errorThrown) {
            $('#msg').html('<div class="alert alert-warning"><a class="close" style="text-decoration: none" data-hide-closest=".alert">×</a>Request Failed</div>');
        }
    });
    }
    else {
        return false;
    }
}