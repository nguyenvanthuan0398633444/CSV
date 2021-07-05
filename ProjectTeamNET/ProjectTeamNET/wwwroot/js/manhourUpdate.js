var deletedThemeArr = [];
let holiday = [];
let today = new Date();
let row = 1;
let listForUpdate = new Array();
let listNeedUpdate = new Array();
const HEADER = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
//format date return string date 2021/06/18
formatDate = function (date) {
    return date.toISOString().slice(0, 10).replace(/-/g, "/");
}
// get user
$("#groups").change(function users() {
    var group = $("#groups").val();
    $.ajax({
        url: "/ManhourUpdate/GetUser/" + group,
        method: 'Post',
        success: function (result) {
            let user = "";
            $.each(result.data, function (i, v) {
                v.forEach(data => {
                    user += `<option value="${data.value}">${data.text}</option>`;  
                })                         
            })
            $('#users').html(user);
        }
        });
})
// search
function search() {
    $('#tbody').empty();
    $('#thead').empty();
    $('#tfoot').empty();
    var obj = {};
    var date = $("#month").val();   
    obj.Year = date.split('/')[0].toString();
    obj.Month = date.split('/')[1].toString();
    obj.Group = $("#groups").val();
    obj.User = $("#users").val();  
    $.ajax({
        url: "/ManhourUpdate/Search",
        method: 'Post',
        data: obj,
        success: function (result) {
            let tbody = "";
            let tfoot = "";
            holiday = result.data.holiday;
            row = 1;
            // add
            let thead = `<tr>
                            <th></th>
                            <th>テーマNo</th>
                            <th style="width:50px;">テーマ名</th>
                            <th>内容</th>
                            <th>月計</th> `
            for (var i of HEADER) {
                thead += `<th style = "width : 42px;">${i}</th>`
            }
            thead += `<th colspan="2">操作</th> </tr>`
            $('.table-striped > #thead').append(thead);
            var tmp = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            result.data.models.forEach(data => {
                tbody += `<tr>
                                <td><div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                                <td class="ThemeNo">${data.theme_no}</td>
                                <td class="ThemeName">${data.theme_name1}</td>
                                <td >${data.work_contents_code}</td>
                                <td class="sum${'row' + row}">${data.total.toFixed(1)}</td>
                                <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                <input type="hidden" class="Group_Code" name="Group_Code" value="${data.group_code}" />
                                <input type="hidden" class="Site_Code" name="Site_Code" value="${data.site_code}" />
                                <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                <input type="hidden" class="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}" />
                                <input type="hidden" class="WorkContentCode" name="WorkContentCode" value="${data.work_contents_code}" />
                                <input type="hidden" class="WorkContentDetail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                <input type="hidden" class="pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="Total Total${'row' + row}" name="Total" value="${0}" />  `
                                for (var i of HEADER) {
                                    tbody += `<td class = "${i}"><input type="text" value="${data['day' + i].toFixed(1)}" class="form-control table-input ${'day' + i} ${'row' + row}"></td>`
                }
                row++;
                tbody +=`<td><div class="text-center"><i class="fas fa-exchange-alt" onclick="changeTheme(this)" ></i></div></td><td >
                             <div class="text-center delete-Theme"><i class="far fa-trash-alt"></i></div></td>
                        </tr >`;             
                tmp[0] += data.total;
                for (var i = 1; i < tmp.length; i++) {
                    tmp[i] += data['day' + i];
                }              
            });
            $('#tbody').append(tbody);
             // daily total calculation                      
            tfoot = `<tr>
                        <td></td>
                        <td></td>
                        <td>合計</td>
                        <td></td>`        
            let day = 0;        
            for (var item in tmp) {
                if (tmp[item] == 8 || holiday.find(element => element == day) != undefined || item == 0 || item > today.getDate()) {
                    tfoot += `<td class="${day} ${'sumday' + day++}">
                                ${tmp[item].toFixed(1)} <i class="error" data-toggle="tooltip" title="合計工数が8h未満です"></i> 
                             </td>`
                }
                else {
                    if (tmp[item] == 0) {
                        tfoot += `<td class="${day} ${'sumday' + day++}">
                                    ${tmp[item].toFixed(1)} <i class="fas fa-exclamation-circle text-danger error" data-toggle="tooltip" title="合計工数が8h未満です"></i> 
                                 </td>`
                    } else {
                        tfoot += `<td class="${day} ${'sumday' + day++}">
                                      ${tmp[item].toFixed(1)}<i class="fas fa-exclamation-circle text-warning error" data-toggle="tooltip" title="合計工数が8h未満です"></i> 
                                  </td>`
                    }
                }               
            }
            tfoot +=` </tr> 
                      <tr>
                        <td></td>
                        <td></td>
                        <td>残工数</td>
                        <td></td>
                        <td></td>`
            // time difference
            for (var i = 1; i < tmp.length; i++) {
                if (8 - (tmp[i]) <= 0 || i > today.getDate() || holiday.find(element => element == i) != undefined) {
                    tfoot += `<td class="${i} ${'missingday' + i}"></td>`
                } else {
                    tfoot += `<td class="${i} ${'missingday' + i}">${(8 - tmp[i]).toFixed(1)}</td> `
                }           
            }
            // color holiday                                  
            $('#tfoot').append(tfoot);
            result.data.holiday.forEach(data => {
                $(`.${data}`).css("background-color", "#f5c6cb");           
            })            
            // color today
            $(`.${today.getDate()}`).css("background-color", "#bee5eb");            
        }
    });
}
// Export file CSV
$('#ExportCsv').click(function () {
    var _group = $('#groups').val();
    var _user = $('#users').val();
    var url = "/ManhourUpdate/ExportCSV?user=" + _user + "&group=" + _group;
    $(location).attr('href', url);
});
// Import file CSV
$("#fileCSVimport").on('change', function () {
    var files = $('#fileCSVimport').prop("files");
    var url = "/ManhourUpdate/ImportCSV";
    formData = new FormData();
    formData.append("file", files[0]);
    $('#fileCSVimport').val("");
    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,     
        contentType: false,
        processData: false,
        success: function (result) {
            console.log(result)
            if (result.messages == "メッセージエリア表示") {
                alert("メッセージエリア表示");
            }
            else if (result.messages == "Header does not match") {
                alert("Header does not match")
            }
            else {
                search();
            }          
        },
        error: function () {
            alert("Error occurs");
        }
    });
}); 
//save information into DB
$("#btSave").on("click",
    function () {
        let dayGet = 'day' + new Date().getDate();
        let listData = new Array();

        //for each tr in table add to obj then add to list data need saved
        $('#tbody tr').each(function () {
            let obj = {};

            obj.Year = parseInt($(this).find(".Year").val());
            obj.Month = parseInt($(this).find(".Month").val());
            obj.User_no = $(this).find(".User_No").val();
            obj.Group_code = $(this).find(".Group_Code").val();
            obj.Site_code = $(this).find(".Site_Code").val();
            obj.Theme_no = $(this).find(".Theme_No").val();
            obj.Work_contents_class = $(this).find(".WorkContentClass").val();
            obj.Work_contents_code = $(this).find(".WorkContentCode").val();
            obj.Work_contents_detail = $(this).find(".WorkContentDetail").val();
            obj.Pin_flg = false;
            obj.Total = parseFloat($(this).find(".Total").val());
            obj.fix_date = formatDate(new Date()).split('/').join([]);
            //get date value for obj
            for (let i = 1; i < 32; i++) {
                obj[`day${i}`] = parseFloat($(this).find(`.day${i}`).val());
            }
            // Add to list data
            listData.push(obj);
        });
        var data = {};
        data.save = listData;
        data.delete = deletedThemeArr;
        deletedThemeArr = [];
        $.ajax({
            url: "/ManhourUpdate/Save",
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            success: function (result) {
                return alert(result);
            }
        });

    });

$("#searchTheme").on("click",
    function () {
        let soldFlg = "";
        if ($("#inlineRadio1").is(":checked")) {
            soldFlg = "未売上";
        }
        if ($("#inlineRadio2").is(":checked")) {
            soldFlg = "売上済";
        }
        if ($("#inlineRadio3").is(":checked")) {
            soldFlg = "全て";
        }
        let obj = {};
        obj.ThemeNo = $('#themeNo').val() == "" ? null : $('#themeNo').val()
        obj.ThemeName = $('#themeName').val() == "" ? null : $('#themeName').val()
        obj.AccountingGroupCode = $('#groupThemes').val() == "" ? null : $('#groupThemes').val()
        obj.SalesObjectCode = $('#salesObject').val() == "" ? null : $('#salesObject').val()
        obj.SoldFlg = soldFlg
        $.ajax({
            url: `/ManhourUpdate/SearchThemes`,
            method: "POST",
            data: obj,
            success: function (result) {
                let tbody = '';
                result.themes.forEach(data => {
                    tbody += `<tr>
                                    <td>
                                        <div class="form-check text-center">
                                        <input class="form-check-input position-static radio" type="checkbox" id="Checkbox" value="option1" aria-label="..."/>                                         
                                        </div>
                                    </td>   
                                    <input type="hidden" class ="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}"/>
                                    <input type="hidden" class ="ThemeNo" name="ThemeNo" value="${data.theme_no}"/>
                                    <input type="hidden" class ="ThemeName" name="ThemeName" value="${data.theme_name1}"/> 
                                    <input type="hidden" class ="soldFlag" name="soldFlag" value="${data.sold_flg}"/> 
                                    <td>${data.theme_no}</td>
                                    <td width="200px">${data.theme_name1}</td>`
                    tbody += data.sold_flg == true ? `<td class="soldFlag">売上済</td></tr >` : `<td class="soldFlag">未売上</td></tr >`;
                });
                //render to table body
                $('#slThemeBody').html(tbody);
            }
        });
    });
//get information when onclick add theme form checked row
let themeNo = null;
let themeName = null;
let workContentClass = null;
let soldFlag = null;
$("#choiceTheme").on("click",
    function addTheme() {
        $("#slThemeBody tr").each(function () {
            if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
                themeNo = $(this).find(".ThemeNo").val();
                workContentClass = $(this).find(".WorkContentClass").val();
                themeName = $(this).find(".ThemeName").val();               
                soldFlag = $(this).find(".soldFlag").val();
                $('#modal1').modal('hide');
            }
        });
        var theme = themeName + ", " + themeName + ", " + workContentClass + ", " + soldFlag;
        $('#modal1').modal('hide');
        $('#themeSelected2').val(theme);
        $("#theme").val(theme);
    });
/* Handle select theme evet*/
$("#addTheme").on("click",
    function () {
        let workContentCode = $('#wordContents').val();
        let workContentDetail = $('#workContentDetail').val();
        if (!themeNo || !themeName || !workContentClass) {
            alert("please choice theme!");
            return;
        }
        if (!workContentDetail) {
            alert("please choice work contents!");
            return;
        }
        if (!workContentCode) {
            alert("please choice work content code!");
            return;
        }

        var rowAdd = '';
        let dateTitle = new Date();
        let year = dateTitle.getFullYear();
        let month = dateTitle.getMonth() + 1;
        rowAdd += `<tr>
                        <td>
                            <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                        <td>${themeNo}</td>
                        <td>${themeName}</td>
                        <td>${workContentCode}</td>
                        <td class="sum${'row' + row}">0.0</td>
                        <input type="hidden" class="Year"   name="Year" value="${year}" />
                        <input type="hidden" class="Month"  name="Month" value="${month}" />
                        <input type="hidden" class="User_No" name="User_No" value="${$('#users').val()}" />
                        <input type="hidden" class="Group_Code" name="Group_Code" value="${$('#groups').val()}" />
                        <input type="hidden" class="Site_Code" name="Site_Code" value="${siteCode}" />
                        <input type="hidden" class="Theme_No" name="Theme_No" value="${themeNo}" />
                        <input type="hidden" class="WorkContentClass" name="WorkContentClass" value="${workContentClass}" />
                        <input type="hidden" class="WorkContentCode" name="WorkContentCode" value="${workContentCode}" />
                        <input type="hidden" class="WorkContentDetail" name="WorkContentDetail" value="${workContentDetail}" />
                        <input type="hidden" class="pin_flg" name="Pin_flg" value="" />
                        <input type="hidden" class="Total" name="Total" value="0.0" /> `
                        for (var i of HEADER) {
                            rowAdd += `<td class = "${i}">
                                        <input type="text" value="0.0" class="form-control table-input ${'day' + i} ${'row' + row}">
                                    </td>`
                                     
                        }  
                        row++;
                        rowAdd +=`<td>
                                    <div class="text-center"><i class="fas fa-exchange-alt" onclick="changeTheme(this)" ></i></div></td>
                                <td >
                                    <div class="text-center delete-Theme"><i class="far fa-trash-alt"></i></div></td>
                                </tr>`;
        $('#tbody').append(rowAdd);
       holiday.forEach(data => {
            $(`.${data}`).css("background-color", "#f5c6cb");
        })
        var start = new Date();
        // colo today
        $(`.${start.getDate()}`).css("background-color", "#bee5eb");       
        //set theme information to null
        themeNo = null; themeName = null; workContentClass = null;
    });

$("#tbody").on("click", ".delete-Theme", function () {
    var obj = {};
    if (confirm('Do you want to delete this row?')) {
        
        obj.Year = parseInt($(this).closest('tr').find('input[name="Year"]').val());
        obj.Month = parseInt($(this).closest('tr').find('input[name="Month"]').val());
        obj.User_no = $(this).closest('tr').find('input[name="User_No"]').val();
        obj.Theme_no = $(this).closest('tr').find('input[name="Theme_No"]').val();
        obj.Work_contents_class = $(this).closest('tr').find('input[name="WorkContentClass"]').val();
        obj.Work_contents_code = $(this).closest('tr').find('input[name="WorkContentCode"]').val() ;
        obj.Work_contents_detail = $(this).closest('tr').find('input[name="WorkContentDetail"]').val();       
        deletedThemeArr.push(obj);
        $(this).closest('tr').remove();       
    }
});
function loadSelectTheme() {
    $('#slThemeBody').html('');
    $.ajax({
        url: `/ManhourInput/GetHistoryThemes`,
        method: "POST",
        success: function (result) {

            if (result == null) {
                return;
            }
            //set history value 
            $('#themeNo').val(result.themeNo != null ? result.themeNo : '');
            $('#themeName').val(result.themeName != null ? result.themeName : '')
            $('#groupThemes').val(result.accountingGroupCode != null ? result.accountingGroupCode : '');
            $('#salesObject').val(result.salesObjectCode != null ? result.salesObjectCode : '');
            //checked by sold flag to 
            if (result.soldFlg == "未売上") {
                $("#inlineRadio1").attr("checked", true);
            }
            if (result.soldFlg == "売上済") {
                $("#inlineRadio2").attr("checked", true);
            }
            if (result.soldFlg == "全て") {
                $("#inlineRadio3").attr("checked", true);
            }
        }
    });
    $('#modal1').modal('show');

}
//Update manhour when change hour on client
$("#tbody").on("click", "input", function () {
    $(this).select();
});

$(".table-responsive").on("change", ".table-input", function (e) {
    e.stopPropagation();
    var valueChange = parseFloat($(this).val());
    if (isNaN(valueChange)) {
        valueChange = 0.0;
    }
    if (valueChange > 24 ) {
        valueChange = 0;
        $(this).val(valueChange.toFixed(1));
        return alert('Input hour must less than 24h!');
    }
    if (valueChange < 0) {
        valueChange = 0;
        $(this).val(valueChange.toFixed(1));
        return alert('Input time must be greater than 0 hours!');
    }
    $(this).val(valueChange.toFixed(1));
    var addressRow;
    var addressCol;
    var sumValueRow = 0;
    var sumValueCol = 0;
    var sumValueMonth = 0;

    var classAll = $(this).attr('class').split(' ');
    // find address column and row.
    $.each(classAll, function (i, className) {
        if (className.indexOf('row') != -1) {
            addressRow = className;
        }
        if (className.indexOf('day') != -1) {
            addressCol = className;
        }
    });
    // calculator sum value row and column.
    $('.' + addressRow).each(function () {
        sumValueRow += parseFloat($(this).val());
    });
    var a = '.sum' + addressRow;
    $('.sum' + addressRow).html(sumValueRow.toFixed(1));
    $('.Total' + addressRow).val(sumValueRow.toFixed(1));
    // change value follow column you choose and change sumValueMonth
    changeValueByColumn(addressCol, sumValueCol, sumValueMonth);

});
//funtion change manhour one colum
function changeValueByColumn(addressCol, sumValueCol, sumValueMonth) {
    $('.' + addressCol).each(function () {
        sumValueCol += parseFloat($(this).val());
    });
    $('.sum' + addressCol).html(sumValueCol.toFixed(1));

    for (let i = 1; i <= 31; i++) {
        sumValueMonth += parseFloat($('.sumday' + i).html());
    }
    $('.sumday0').html(sumValueMonth.toFixed(1));
    let checkHoliday = holiday.find(element => element == i) != undefined;
    //missing manhour 1 date check manhour > 0 && manhour < 8
    if ($(".sum" + addressCol).closest("td").hasClass("table-danger") == false && checkHoliday != undefined) {
        if (sumValueCol > 0 && sumValueCol < 8 && checkHoliday == undefined) {
            $(".missing" + addressCol).html((8 - sumValueCol).toFixed(1));

            var checkTagIRange0To8 = $(".sum" + addressCol).closest("td").find("i.hour-min");
            if (checkTagIRange0To8.html() == undefined) {
                var i = '<i class="fas fa-exclamation-circle text-warning hour-min" data-toggle="tooltip" title="合計工数が8h未満です"></i> ';
               $(".sum" + addressCol).append(i);
            } else {
                checkTagIRange0To8.show();
            }

        }
        else if (sumValueCol == 8 && checkHoliday == undefined) {
            $(".missing" + addressCol).html('');
            $(".sum" + addressCol).closest("td").find("i").hide();
        }
        else if (sumValueCol == 0 && checkHoliday == undefined) {
            var checkTagIRange0To8 = $(".sum" + addressCol).closest("td").find("i");
            $(".missing" + addressCol).html('');
            if (checkTagIRange0To8.html() == undefined) {
                var i = '<i class="fas fa-exclamation-circle text-danger error" data-toggle="tooltip" title="合計工数が8h未満です"></i>';
                $(".sum" + addressCol).append(i);
            }
        }
        else {
            var checkTagIRange0To8 = $(".sum" + addressCol).closest("td").find("i");
            $(".missing" + addressCol).html('');
            if (checkTagIRange0To8.html() == undefined ) {
                var i = '<i class="fas fa-exclamation-circle text-warning hour-min" data-toggle="tooltip" title="合計工数が8h未満です"></i> ';
                $(".sum" + addressCol).append(i);
            }
        }
    }
    //check manhour > 24
    if (sumValueCol > 24) {
        var checkTagIGreater24 = $(".sum" + addressCol).closest("td").find("i.hour-max");
        if (checkTagIGreater24.html() == undefined) {
            var i = '<i class="fas fa-exclamation-circle text-warning hour-max" data-toggle="tooltip" title="労働時間の合計が24hを超えることはできません。"></i> ';
            $(".sum" + addressCol).closest("td").find('span').before(i);
        } else {
            checkTagIGreater24.show();
        }
    } else {
        $(".sum" + addressCol).closest("td").find("i.hour-max").hide();
    }
}
                                                                                                                                                                                                                                    
$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});
function changeTheme(el) {
    if (confirm("Do you want to change this theme?") == true) {
        $("#modalThemeNo").val($(el).closest("tr").find(".ThemeNo").text());
        $("#modalThemeName").val($(el).closest("tr").find(".ThemeName").text());
        $("#modalWC").val($(el).closest("tr").find(".WorkContentCode").val());
        $("#modalDetail").val($(el).closest("tr").find(".WorkContentDetail").val());
        $("#modal3").modal('show');

        $('#btnChange').on('click', function () {

            let workContentCode = $(`#workContentCode2 :selected`).val();
            let workContentDetail = $(`#detailCode2`).val();
            let year = parseInt($(el).closest('tr').find(".Year").val());
            let month = parseInt($(el).closest("tr").find(".Month").val());
            let userNo = $(el).closest("tr").find(".User_No").val();
            if (!themeNo || !themeName || !workContentClass) {
                alert("Please choice theme!");
                return;
            }
            if (!workContentDetail) {
                alert("Please choice work contents!");
                return;
            }
            if (!workContentCode) {
                alert("Please choice work content code!");
                return;
            }
            let obj = {};
            obj.Theme_no = themeNo;
            obj.Year = year;
            obj.Month = month;
            obj.Work_contents_class = workContentClass;
            obj.Work_contents_code = workContentCode;
            obj.Work_contents_detail = workContentDetail;

            $.ajax({
                url: `/ManhourInput/CheckExistTheme`,
                method: "POST",
                data: obj,
                success: function (result) {
                    console.log(result);
                    if (result == false) {
                        let obj = {};
                        obj.Year = year;
                        obj.Month = month;
                        obj.User_no = userNo;
                        obj.Theme_no = $(el).closest("tr").find(".Theme_No").val();
                        obj.Work_contents_class = $(el).closest("tr").find(".WorkContentClass").val();
                        obj.Work_contents_code = $(el).closest("tr").find(".WorkContentCode").val();
                        obj.Work_contents_detail = $(el).closest("tr").find(".WorkContentDetail").val();

                        listNeedUpdate.push(obj);
                        console.log(listNeedUpdate);

                        obj = {};
                        obj.Theme_no = themeNo;
                        obj.Work_contents_class = workContentClass;
                        obj.Work_contents_code = workContentCode;
                        obj.Work_contents_detail = workContentDetail;

                        listForUpdate.push(obj);
                        console.log(listForUpdate);
                        $('#modal3').modal('hide');
                        //Change information row changed
                        $(el).closest("tr").find(".ThemeName").text(themeName);
                        $(el).closest("tr").find(".ThemeNo").text(themeNo);
                        $(el).closest("tr").find(".WContent").text($(`#workContentCode2 :selected`).text());
                        $(el).closest("tr").find(".Detail").text(workContentDetail);                      
                        //set default data 
                        $(`#themeSelected1`).val('');
                        $(`#themeSelected2`).val('');
                        $(`#workContentCode1`).html('');
                        $(`#detailCode1`).val('');
                        $(`#workContentCode1`).html('<option value=" "></option>');
                        $(`#workContentCode2`).html('<option value=" "></option>');
                        themeNo = null; themeName = null; workContentClass = null;

                    } else {
                        alert('Theme is existed!');
                    }
                }
            });
        });

    }

}