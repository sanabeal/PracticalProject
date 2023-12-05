
$(document).ready(function () {

    var now = new Date();
    var month = (now.getMonth() + 1);
    var day = now.getDate();
    if (month < 10)
        month = "0" + month;
    if (day < 10)
        day = "0" + day;
    var today = now.getFullYear() + '-' + month + '-' + day;
    $('#dateEntry').val(today);

    initial_work();
});

function initial_work() {
    $("#tbl_details tbody tr").remove();

    var rows = '<tr class="input_fields_box">'
    + '<td><select class="form-control" style="width:100%" id="product" name="product"></select></td>'
    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Price" name="txt_Price"/></td>'
    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Quantity" name="txt_Quantity"/></td>'
    + '<td><input class="form-control TotalPrice" type="text" disabled id="txtTotalPrice" name="txtTotalPrice"/></td>'
    + '<td><span id="btn_temp_add1" class="glyphicon glyphicon-plus add_field_button" style="cursor:pointer"></span></td>'
    + '<td><span class="glyphicon glyphicon-trash remove_field" style="cursor:pointer"></span></td>'
    + '</tr>';

    $('#tbl_details tbody').append(rows);



    set_drop_down_list_Product('product', 'Select Item', '', 'ItemInfo', 'ItemID', 'ItemName', 'IsActive', 'true');
}

function TotalSellPrice(ElementID) {


    if (!!ElementID) {
        var suffix = ElementID.match(/\d+/);
        suffix = (!!suffix) ? suffix : "";

        //if (!!suffix) {
        var qty = $('#txt_Quantity' + suffix).val();
        var price = $('#txt_Price' + suffix).val();

        qty = (!!qty) ? qty : 0;
        price = (!!price) ? price : 0;

        var total_price = parseInt(qty) * parseFloat(price);
        $('#txtTotalPrice' + suffix).val(total_price);
        //}
        //-----------------------------------------//
        var sum = 0;
        $('.TotalPrice').each(function () {
            sum += parseFloat(this.value);
        });
        // $('#txtTotalAmount').val(sum);
    }
}

//--------------Doropdown List------------------//
function set_drop_down_list_Product(element_id, deafult_text, selected_item, db_table, col_value, col_text, condition_field, condition, condition_field1, condition1) {


    var ddl_element = $("#" + element_id);

    $.ajax({
        url: "/Home/set_drop_down_list_Product",
        data: "{ 'db_table': '" + db_table + "','col_value': '" + col_value + "','col_text': '" + col_text + "','condition_field': '" + condition_field + "','condition': '" + condition + "','condition_field1': '" + condition_field1 + "','condition1': '" + condition1 + "'}",
        type: "post",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (StatesList) {

            ddl_element.empty().append('<option value="0">' + deafult_text + '</option>');
            $.each(StatesList, function (key, item) {
                ddl_element.append($('<option></option>').val(this['Value']).html(this['Text']));
            });
        }
    });
}

//-----------Add Multiple Element ---------------//
jQuery_1_7_2(window).load(function () {

    var x = 1; //initilal text box count
    var max_elements = 50; //maximum input elements allowed      
    //--------------------------//
    jQuery_1_7_2(".add_field_button").live('click', function () {

        debugger

        var id = this.id;
        var ele_suffix = id.match(/\d+/);
        var x = document.getElementById('hdn_count_element').value;
        if (x <= max_elements) {
            var $tr = $(this).closest('.input_fields_box');
            //-------------------------//
            var div = $("#tbl_details tr");
            //find all select2 and destroy them   
            div.find(".select2").each(function (index) {
                if ($(this).data('select2')) {
                    $(this).select2('destroy');
                    //alert(this.id);
                }
            });
            //-------------------------//
            var $clone = $tr.clone();

            $clone.find(':text').val('');
            $clone.find('textarea').val('');

            $clone.find('.remove_field').attr("id", ''); //Remove Button ID Set Empty For New Clone Data Update.
            $clone.find('select option:first-child').attr("selected", "selected");
            $clone.find('*').each(function (index, element) {
                var ele_id = element.id;
                $('#' + ele_id).attr("id", ele_id + x);
            });
            $tr.after($clone);
            //--------------------------//

            x++;
            document.getElementById('hdn_count_element').value = x;

        }
    });
});

//------------------Remove Element-----------------//
var total_remove_element_id = "";
jQuery_1_7_2(window).load(function () {

    jQuery_1_7_2(".remove_field").live("click", function () {

        var x = document.getElementById('hdn_count_element').value;
        if (x > 1) {
            $(this).parents(".input_fields_box").remove();
            x--;
            document.getElementById('hdn_count_element').value = x;
            //---------------------------------//
            var remove_single_id = $(this).attr("id");
            total_remove_element_id += remove_single_id + "#";
            $('#hdn_remove_all_id').val(total_remove_element_id);
        }
    });
});

//-------------------Save Data-------------------//
var DetailsTable =
{
    getData: function (table) {
        var data = [];
        table.find('tr').not(':first').each(function (rowIndex, r) {
            var cols = [];
            $(this).find('td').each(function (colIndex, c) {
                if ($(this).children(':text,textarea,select,input[type="hidden"]').length > 0)
                    cols.push($(this).children('input,textarea,select,input[type="hidden"]').val().trim());
                    //if dropdown text is needed then uncomment it and remove SELECT from above IF condition//
                else if ($(this).children('select').length > 0)
                    cols.push($(this).find('option:selected').text());
                else if ($(this).children(':checkbox').length > 0)
                    cols.push($(this).children(':checkbox').is(':checked') ? 1 : 0);
                else {
                    //cols.push($(this).text().trim());
                    cols.push($(this).find('.remove_field').attr('id'));
                }
            });
            data.push(cols);
        });
        return data;
    }
}

//----------------- Save Master Data--------// 
function save() {
        
    var CustomerName = $('#txtCustomerNameEntry').val();
    var BillDate = $('#dateEntry').val();
    var contactNo = $('#txtContactNoEntry').val();

    if (CustomerName != "" && BillDate != "") {
        var data = {
            CustomerName,
            BillDate,
            contactNo
        };

        var parameters = {};
        parameters.master = data;

        var Detaildata = DetailsTable.getData($('#tbl_details'));  // passing that table's ID //   
        var identify_data = [["master_id", '1']];
        parameters.array = Detaildata;
        parameters.array1 = identify_data;
        var request = $.ajax({
            url: "/Home/SaveDetails",
            data: JSON.stringify(parameters),
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
        });
        request.done(function (msg) {
            $('#msg').html('<div class="alert alert-success"><a class="close" style="text-decoration: none" data-hide-closest=".alert">×</a><strong>Success!</strong> Data save.</div>');
            window.location.href = '/Home/Index/';
        });
    }
    else {
        $('#msg').html('<div class="alert alert-info"><a class="close" style="text-decoration: none" data-hide-closest=".alert">×</a>Please fill in required fields.</div>');
    }

}

function goto_list() {
    window.location.href = '/Home/Index/'
}