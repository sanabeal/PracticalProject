$(document).ready(function () {

    initial_work();
});

function initial_work() {
    $("#tbl_details tbody tr").remove();

    var rows = '<tr class="input_fields_box">'
    + '<td><select class="form-control" style="width:100%" id="product" name="product"></select></td>'
    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Price" name="txt_Price"/></td>'
    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Quantity" name="txt_Quantity"/></td>'
    + '<td><input class="form-control TotalPrice" type="text" disabled id="txtTotalPrice" name="txtTotalPrice"/></td>'
    + '<td><span id="btn_temp_add12" class="glyphicon glyphicon-plus add_field_button1" style="cursor:pointer"></span></td>'
    + '<td><span class="glyphicon glyphicon-trash remove_field" style="cursor:pointer"></span></td>'
    + '</tr>';

    $('#tbl_details tbody').append(rows);

    set_drop_down_list_Product('product', 'Select Item', '', 'ItemInfo', 'ItemID', 'ItemName', 'IsActive', 'true');
}

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
                //ddl_element.append($('<option></option>').val(this['Value']).html(this['Text']));
                if (selected_item == this['Value'])
                    ddl_element.append($('<option selected="selected"></option>').val(this['Value']).html(this['Text']));
                else
                    ddl_element.append($('<option></option>').val(this['Value']).html(this['Text']));
            });
        }
    });
}

$(document).ready(function () {

    var id = sessionStorage.getItem("sent");

    if (id != "") {
        $.ajax({
            url: "/Home/GetData",
            data: "{id: " + id + "}",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response != "") {
                    $.each(response, function (index, item) {
                        $('#hfID').val(item.BillMasterID);
                        $('#date').val(item.BillDate);
                        $('#txtCustomerName').val(item.CustomerName);
                        $('#txtContactNo').val(item.ContactNo);

                        show_details_data(item.BillMasterID);
                    });
                }
            }
        });
    }
});

//------------------Show Details Data-------------------//
function show_details_data(search_data) {

    $.ajax({
        url: "/Home/show_details_data",
        data: "{ 'id': '" + search_data + "'}",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var sl_no = 1;

            if (response != "") {
                $("#tbl_details tbody tr").remove();
                $.each(response, function (index, item) {

                    ++sl_no;
                    var rows = '<tr class="input_fields_box">'
                    + '<td><select class="form-control" style="width:100%" id="product' + sl_no + '" name="product"></select></td>'
                    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Price' + sl_no + '" name="txt_Price" value="' + item.UnitPrice + '"/></td>'
                    + '<td><input class="form-control" type="text" onkeyup="TotalSellPrice(this.id)" id="txt_Quantity' + sl_no + '" name="txt_Quantity" value="' + item.ItemQty + '"/></td>'
                    + '<td><input class="form-control TotalPrice" type="text" disabled id="txtTotalPrice' + sl_no + '" name="txtTotalPrice" value="' + item.TotalPrice + '"/></td>'
                    + '<td><span id="btn_temp_add12" class="glyphicon glyphicon-plus add_field_button1" style="cursor:pointer"></span></td>'
                    + '<td><span id="' + item.BillDetailsID + '" class="glyphicon glyphicon-trash remove_field" style="cursor:pointer"></span></td>'
                    + '</tr>';

                    $('#tbl_details tbody').append(rows);
                    set_drop_down_list_Product('product' + sl_no, 'Select Product', item.ItemID, 'ItemInfo', 'ItemID', 'ItemName', '', '');

                });

                sl_no = sl_no - 1;
            }
            else {
                empty_details_data();
            }
            document.getElementById('hdn_count_element').value = sl_no;
        },
        error: function (data, success, error) {
            $('#msg').html("Error:" + error);
        }
    });
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

//-----------------Add Multiple Element--------------------//
jQuery_1_7_2(window).load(function () {

    var x = 1; //initilal text box count
    var max_elements = 50; //maximum input elements allowed      
    //--------------------------//
    jQuery_1_7_2(".add_field_button1").live('click', function () {

        var x = document.getElementById('hdn_count_element').value;
        if (x <= max_elements) {
            var $tr = $(this).closest('.input_fields_box');
            //-------------------------//
            var div = $("#tbl_details tr");            //find all select2 and destroy them   

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

            //-------------------------//
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
            //---------------------------------//
            //summation_price();
        }
    });
});
//--------Get Details table data dynamically----------//
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
function Edit() {
      
    var BillMasterID = $('#hfID').val();
    var CustomerName = $('#txtCustomerName').val();
    var BillDate = $('#date').val();
    var contactNo = $('#txtContactNo').val();

    if (CustomerName != "" && BillDate != "") {

        var data = {
            BillMasterID,
            CustomerName,
            BillDate,
            contactNo
        };

        var parameters = {};
        parameters.master = data;

        var remove_all_id = $('#hdn_remove_all_id').val();

        var identify_data = [["master_id", '1'], ["remove_all_id", remove_all_id]];
        var Detaildata = DetailsTable.getData($('#tbl_details'));
      
        parameters.array = Detaildata;
        parameters.array1 = identify_data;

        var request = $.ajax({
            url: "/Home/UpdateDetails",
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