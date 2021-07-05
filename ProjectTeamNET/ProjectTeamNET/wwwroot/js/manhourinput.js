const ERR_001 = "メッセージエリア表示";
const ERR_002 = "メッセージエリア表示";
const ERR_005 = "メッセージエリア表示";

let namePageLoad = $('#pageHistory').val();
let dateTitle = new Date(getDateTitle());
let numDayOfMonth = getDaysInMonth(dateTitle.getFullYear(), dateTitle.getMonth() - 1);
let themeNo = null;
let themeName = null;
let workContentClass = null;
let listDeleted = new Array();
let listInserted = new Array();
let listNeedUpdate = new Array();
let listForUpdate = new Array();
let holidayArr = new Array();

$(document).ready(function () {
    loadInit();
});

//render month table and save user screen item
function loadMonth(el) {

    let pageName = $(el).attr("name");
    savePageHistory(pageName);
    loadMonthTable();
}

//render day table and save user screen item
function loadDay(el) {

    let pageName = $(el).attr("name");
    savePageHistory(pageName);
    loadDayTable();
}
//render week table and save user screen item
function loadWeek(el) {

    let pageName = $(el).attr("name");
    savePageHistory(pageName);
    loadWeekTable();

}

//load init data form database
function loadInit() {

    let daypageLoad = getDateTitle();
    if (namePageLoad == "Month") {

        loadMonthTable(daypageLoad);

    } else if (namePageLoad == "Week") {

        loadWeekTable(daypageLoad);

    } else {
        loadDayTable(daypageLoad);
    }
    changeBtActive();
}

//go to previous time
function gotoPrevious() {
    let pageActive = $('#btnNow').text();
    let dateTitle = getDateTitle();
    let prevDate = gotoDate(dateTitle, -1);
    if (pageActive == '今日') {
        loadDayTable(prevDate);
    }
    if (pageActive == '今週') {
        loadWeekTable(prevDate);
    }
    if (pageActive == '今月') {
        loadMonthTable(gotoMonth(dateTitle, -1));
    }
}

//goto next time
function gotoNext() {

    let pageActive = $('#btnNow').text();
    let dateTitle = getDateTitle();
    if (pageActive == '今日') {
        loadDayTable(gotoDate(dateTitle, 1));
    }
    if (pageActive == '今週') {
        loadWeekTable(gotoDate(dateTitle, 7));
    }
    if (pageActive == '今月') {
        loadMonthTable(gotoDate(dateTitle, numDayOfMonth));
    }
}

function gotoNow() {
    let nowDate = formatDate(new Date(Date.now()));
    displayLoad(nowDate);
}

function displayLoad(dateStr) {
    let pageName = $('#btnNow').text();
    if (pageName == '今日') {
        loadDayTable(dateStr);
    }
    if (pageName == '今週') {
        loadWeekTable(dateStr);
    }
    if (pageName == '今月') {
        loadMonthTable(dateStr);
    }
}

// Change theme selected
function changeTheme(el) {

    if (confirm("Do you want to change this theme?") == true) {
        $("#modalThemeNo").val($(el).closest("tr").find(".ThemeNo").text());
        $("#modalThemeName").val($(el).closest("tr").find(".ThemeName").text());
        $("#modalWC").val($(el).closest("tr").find(".WContent").text());
        $("#modalDetail").val($(el).closest("tr").find(".Detail").text());
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
                        obj.Work_contents_class = $(el).closest("tr").find(".WC_Class").val();
                        obj.Work_contents_code = $(el).closest("tr").find(".WC_Code").val();
                        obj.Work_contents_detail = $(el).closest("tr").find(".WC_Detail").val();

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
                        $(`#workContentCode1`).html('<option value=" ">Please choice theme...</option>');
                        $(`#workContentCode2`).html('<option value=" ">Please choice theme...</option>');
                        themeNo = null; themeName = null; workContentClass = null;

                    } else {
                        alert('Theme is existed!');
                    }
                }
            });
        });

    }

}

// Delete this row clicked 
function deleteTheme(el) {

    if (confirm("Do you want to delete this row?") == true) {
        $(el).closest('tr').remove();

        let obj = {};

        obj.Year = parseInt($(el).closest('tr').find(".Year").val());
        obj.Month = parseInt($(el).closest('tr').find(".Month").val());
        obj.User_no = $(el).closest('tr').find(".User_No").val();
        obj.Theme_no = $(el).closest('tr').find(".Theme_No").val();
        obj.Work_contents_class = $(el).closest('tr').find(".WC_Class").val();
        obj.Work_contents_code = $(el).closest('tr').find(".WC_Code").val();
        obj.Work_contents_detail = $(el).closest('tr').find(".WC_Detail").val();

        listDeleted.push(obj);

        console.log(listDeleted);
    }

}

function save() {

    let dayGet = 'day' + new Date(getDateTitle()).getDate();
    let listData = new Array();
    let saveData = {};
    let isStop = false;

    //for each tr in table add to obj then add to list data need saved
    $('#tbody tr').each(function () {
        let obj = {};

        //value
        obj.Year = parseInt($(this).find(".Year").val());
        obj.Month = parseInt($(this).find(".Month").val());
        obj.User_no = $(this).find(".User_No").val();
        obj.Theme_no = $(this).find(".Theme_No").val();
        obj.Work_contents_class = $(this).find(".WC_Class").val();
        obj.Work_contents_code = $(this).find(".WC_Code").val();
        obj.Work_contents_detail = $(this).find(".WC_Detail").val();
        obj.pin_flg = $(this).find(".Pin_flg").val() == 'true' ? true : false;
        obj.total = parseFloat($(this).find(".Total").val());

        //convert 2021/06/23 => 20210623 for fixdate varchar(8)
        obj.fix_date = formatDate(new Date()).split('/').join([]);

        //get input hour for month
        for (let i = 1; i <= numDayOfMonth; i++) {
            let inputHour = parseFloat($(this).find(`.day${i}`).val());
            if (inputHour < 0 || isNaN(inputHour)) { //valid input hour
                isStop = true;
                return false;
            }
            obj[`day${i}`] = inputHour;

        }

        //get input hour for day
        if ($(this).find(".inputHour").val()) {
            let inputHour = parseFloat($(this).find(".inputHour").val());
            if (inputHour < 0 || isNaN(inputHour)) {//valid input hour
                isStop = true;
                return false;
            }
            obj[dayGet] = inputHour;
        }

        //get input hour for week
        if (dayGetOfWeek.length > 0) {
            dayGetOfWeek.forEach(date => {
                let inputHour = parseFloat($(this).find(`.input${date}`).val());
                if (inputHour < 0 || isNaN(inputHour)) {//valid input hour
                    isStop = true;
                    return false;
                }
                obj[`${date}`] = inputHour;
            })
        }
        // Add to list data
        listData.push(obj);

    });

    if (isStop) {
        return alert('input hour is not valid');
    }
    //list manhour change
    saveData.Update = listData; listdata = [];
    //list manhour deleted
    saveData.Delete = listDeleted; listDeleted = [];
    //list manhour insert
    saveData.Insert = listInserted; listInserted = [];
    //list manhour insert
    saveData.NeedUpdate = listNeedUpdate; listNeedUpdate = [];
    //list manhour insert
    saveData.ForUpdate = listForUpdate; listNeedUpdate = [];

    console.log(saveData);

    $.ajax({
        url: "/ManhourInput/Save",
        data: JSON.stringify(saveData),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            return alert(result);
        }

    });
}

/* Handle select theme event*/

//load select theme history modal
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
            $('#slThemeNo').val(result.themeNo != null ? result.themeNo : '');
            $('#slThemeName').val(result.themeName != null ? result.themeName : '')
            $('#comboxGroup').val(result.accountingGroupCode != null ? result.accountingGroupCode : '');
            $('#comboxObject').val(result.salesObjectCode != null ? result.salesObjectCode : '');

            //checked by sold flag to 
            if (result.soldFlg == "未売上") {
                $("#cb1").attr("checked", true);
            }
            if (result.soldFlg == "売上済") {
                $("#cb2").attr("checked", true);
            }
            if (result.soldFlg == "全て") {
                $("#cb3").attr("checked", true);
            }
        }
    });
    $('#modal1').modal('show');
}

//Search theme
function searchTheme() {

    let soldFlg = "";
    if ($("#cb1").is(":checked")) {
        soldFlg = "未売上";
    }
    if ($("#cb2").is(":checked")) {
        soldFlg = "売上済";
    }
    if ($("#cb3").is(":checked")) {
        soldFlg = "全て";
    }

    let obj = {};
    obj.ThemeNo = $('#slThemeNo').val() == "" ? null : $('#slThemeNo').val();
    obj.ThemeName = $('#slThemeName').val() == "" ? null : $('#slThemeName').val();
    obj.AccountingGroupCode = $('#comboxGroup').val() == "" ? null : $('#comboxGroup').val();
    obj.SalesObjectCode = $('#comboxObject').val() == "" ? null : $('#comboxObject').val();
    obj.SoldFlg = soldFlg;

    $.ajax({
        url: `/ManhourInput/SearchThemes`,
        method: "POST",
        data: obj,
        success: function (result) {
            let tbody = '';
            result.themes.forEach(data => {
                tbody += `<tr>
                                    <td>
                                        <div class="form-check text-center">
                                        <input class="form-check-input position-static radio" name="SelectTheme" type="checkbox" id="Checkbox" value="option1" aria-label="..."/>                                         
                                        </div>
                                    </td>   
                                    <input type="hidden" class ="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}"/>
                                    <input type="hidden" class ="ThemeNo" name="ThemeNo" value="${data.theme_no}"/>
                                    <input type="hidden" class ="ThemeName" name="ThemeName" value="${data.theme_name1}"/> 
                                    <td>${data.theme_no}</td>
                                    <td width="200px">${data.theme_name1}</td>`
                tbody += data.sold_flg == true ? `<td>売上済</td></tr >` : `<td>未売上</td></tr >`;

            });
            //render to table body
            $('#slThemeBody').html(tbody);

        }
    });
}

//get information when onclick add theme form checked row
function choiceTheme() {

    $("#slThemeBody tr").each(function () {

        if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
            themeNo = $(this).find(".ThemeNo").val();
            workContentClass = $(this).find(".WorkContentClass").val();
            themeName = $(this).find(".ThemeName").val();

            //load select list by class code
            $(`#workContentCode1`).html('<option value=" ">内容選択...</option>');
            $(`#workContentCode2`).html('<option value=" ">内容選択...</option>');
            $.ajax({
                url: "/ManhourInput/GetWorkContentByClass",
                data: { classCode: workContentClass },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    result.forEach(item => {
                        var option = new Option(`${item.work_contents_code} [${item.work_contents_code_name}]`, `${item.work_contents_code}`);

                        $(`#workContentCode2`).append(option);
                        var option = new Option(`${item.work_contents_code} [${item.work_contents_code_name}]`, `${item.work_contents_code}`);
                        $(`#workContentCode1`).append(option);
                    })
                }

            });

            $('#modal1').modal('hide');
            $('#themeSelected1').val(themeNo + ' ' + themeName);
            $('#themeSelected2').val(themeNo + ' ' + themeName);
        }

    });
}
/* Handle select theme evet*/

//Add new theme to table
function addTheme() {

    let workcontent = $(`#workContentCode1 :selected`).text();
    let workContentCode = $(`#workContentCode1 :selected`).val();
    let workContentDetail = $(`#detailCode1`).val();
    //let userNo = 'BAOTQ';
    let dateTitle = new Date(getDateTitle());
    let dayGet = new Date(getDateTitle()).getDate();
    let year = dateTitle.getFullYear();
    let month = dateTitle.getMonth() + 1;
    //valid data 
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

                var rowAdd = '';
                rowAdd += `<tr>
                        <td>
                            <div class="text-center">
                                <i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                            </div>
                        </td>
                            <input type="hidden" class="Year"       name="Year" value="${year}"/>
                            <input type="hidden" class="Month"      name="Month" value="${month}"/>
-                            <input type="hidden" class="Theme_No"   name="ThemeNo" value="${themeNo}"/>
                            <input type="hidden" class="WC_Class"   name="WorkContentClass" value="${workContentClass}"/>
                            <input type="hidden" class="WC_Code"    name="WorkContentCode" value="${workContentCode}"/>
                            <input type="hidden" class="WC_Detail"  name="WorkContentCode" value="${workContentDetail}"/>
                            <input type="hidden" class="pin_flg"    name="Pin_flg" value="${false}" />
                            <input type="hidden" class="Total"      name="Total" value="0.0" />`
                for (let i = 1; i <= numDayOfMonth; i++) {
                    rowAdd += `<input type="hidden" name="${'day' + i}" value="0.0" class="${'day' + i}">`
                }
                rowAdd += ` <td class="ThemeNo">${themeNo}</td>
                                <td class="ThemeName">${themeName}</td>`
                let pageActive = $('#btnNow').text();
                //add row for day table
                if (pageActive == '今日') {
                    rowAdd += `<td class="WContent">${workcontent}</td>
                            <td class="Detail">${workContentDetail}</td>
                            <td>
                                        <input type="text" onclick = "this.select();setOldVal(this,'${`inputHourday${dayGet}`}');"
                                             onchange = "onChangeValid(this,'inputHourday${dayGet}')"
                                             onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        value="0.0" class="form-control table-big-input inputHourday${dayGet}">
                            </td>
                            <td>
                                <div>
                                    <button class="btn btn-sm btn-outline-secondary mr-2" onclick="changeTheme(this)">
                                                <i class="fas fa-exchange-alt"></i> テーマ変更
                                    </button>
                   
                                   <button class="btn btn-sm btn-outline-secondary mr-2" onclick="deleteTheme(this)">
                                    <i class="far fa-trash-alt"></i> 削除</button>
                                 <div>       
                             </td>
                           </tr>`;
                    $('#tbody').append(rowAdd);
                }
                //add row for week table 
                if (pageActive == '今週') {
                    rowAdd += `<td class="WContent">${workcontent}</td>
                                <td class="Detail">${workContentDetail}</td>
                                <td class="total">0.0</td>`;
                    dayGetOfWeek.forEach(date => {
                        rowAdd += `<td class="${getNumInStr(date)}" >
                                        <input onclick="this.select();setOldVal(this,'${'input' + date}');"
                                        onchange="onChangeValid(this,'input${date}')"
                                        onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122" type="text"
                                        value="0.0" class="form-control table-big-input input${date}">
                                   </td>`
                    });

                    rowAdd += `<td>
                                    <button class="btn btn-sm mr-2" onclick="changeTheme(this)">
                                                <i class="fas fa-exchange-alt"></i> 
                                    </button>
                                </td>
                                <td>
                                   <button class="btn btn-sm mr-2" onclick="deleteTheme(this)">
                                    <i class="far fa-trash-alt"></i> </button>
                                 <div>       
                             </td>
                           </tr>`;
                    $('#tbody').append(rowAdd);
                    setHolidayBackground(holidayArr);
                }
                //add row for month table
                if (pageActive == '今月') {
                    rowAdd = `<tr>
                        <td>
                            <div class="text-center">
                                <i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                            </div>
                        </td>
                            <input type="hidden" class="Year"       name="Year" value="${year}"/>
                            <input type="hidden" class="Month"      name="Month" value="${month}"/>
                            <input type="hidden" class="User_No"    name="Month" value="${userNo}"/>
                            <input type="hidden" class="Theme_No"   name="ThemeNo" value="${themeNo}"/>
                            <input type="hidden" class="WC_Class"   name="WorkContentClass" value="${workContentClass}"/>
                            <input type="hidden" class="WC_Code"    name="WorkContentCode" value="${workContentCode}"/>
                            <input type="hidden" class="WC_Detail"  name="WorkContentCode" value="${workContentDetail}"/>
                            <input type="hidden" class="pin_flg"    name="Pin_flg" value="${false}" />
                            <input type="hidden" class="Total"      name="Total" value="0.0" />
                        <td class="ThemeNo">${themeNo}</td>
                        <td class="ThemeName">${themeName}</td>
                        <td class="Detail">${workContentDetail}</td>
                        <td class="total">0.0</td>`;

                    for (let i = 1; i <= numDayOfMonth; i++) {
                        rowAdd += `<td class="${i}">
                                        <input  onclick="this.select();setOldVal(this,'${'day' + i}');" 
                                        onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        onchange="onChangeValid(this,'${'day' + i}')"
                                        type="text" value="0.0" 
                                        class="form-control table-input ${'day' + i}">
                                    </td>`

                    }
                    rowAdd += `   <td><i onclick="changeTheme(this)" class="fas fa-exchange-alt"></i></td>    
                                  <td><i onclick="deleteTheme(this)" class="far fa-trash-alt"></i></td>
                            </tr>`;
                    $('#tbody').append(rowAdd);
                    setHolidayBackground(holidayArr);
                }

                let obj = {};
                obj.Year = parseInt(year);
                obj.Month = parseInt(month);
                obj.Theme_no = themeNo;
                obj.User_no = 'BAOTQ';
                obj.Work_contents_class = workContentClass;
                obj.Work_contents_code = workContentCode;
                obj.Work_contents_detail = workContentDetail;

                listInserted.push(obj);
                //set theme information to null and default option
                $(`#themeSelected1`).val('');
                $(`#themeSelected2`).val('');
                $(`#workContentCode1`).html('<option value=" ">Please choice theme...</option>');
                $(`#workContentCode2`).html('<option value=" ">Please choice theme...</option>');
                $(`#detailCode1`).val('');
                themeNo = null; themeName = null; workContentClass = null;
            } else {
                alert("This theme is exist!")
            }
        }
    });


}

//Sum number in a class
(function ($) {
    $.fn.sum = function () {
        var sum = 0;
        $(this).each(function (index, element) {
            if ($(element).val() != "")
                sum += parseFloat($(element).val());
        });
        return sum;
    };
})(jQuery);

//format date return string date 2021/06/18
function formatDate(date) {
    return date.toLocaleDateString("ja-JP");
}

//get date title
function getDateTitle() {
    return $('#dateTitle').text().trim().substring(0, 10);
}
//previous date
function gotoDate(dateSt, dayNumber) {

    const date = new Date(dateSt);
    const newDate = date.setDate(date.getDate() + dayNumber);
    const nextDate = new Date(newDate).toLocaleDateString("ja-JP");

    return nextDate;
}

function gotoMonth(dateStr, monthNumber) {

    const date = new Date(dateStr);
    const newDate = date.setMonth(date.getMonth() + monthNumber);
    const nextMonth = new Date(newDate).toLocaleDateString("ja-JP");
    return nextMonth;
}

function dayOfWeek(date) {
    return new Date(date).toLocaleString('ja-JP', { weekday: 'long' }).substring(0, 1);
}

//valid detail code in 00-99
function validDetailCode(name) {

    let code = $(`#${name}`).val();

    if (code.length != 2 || code < 0 || code > 99) {
        $(`#${name}`).val('');
        return alert('Giá tri nhập vào khác 00 - 99');
    }

}

//Change button active by table render
function changeBtActive() {

    let titlePage = window.location.href.split("/")[4];

    if (titlePage.toLowerCase() == "day") {
        $("#btDay").addClass("active");
        $("#btWeek").removeClass("active");
        $("#btMonth").removeClass("active");
        return;
    }
    if (titlePage.toLowerCase() == "week") {
        $("#btWeek").addClass("active");
        $("#btDay").removeClass("active");
        $("#btMonth").removeClass("active");
        return;
    } else {
        $("#btWeek").removeClass("active");
        $("#btDay").removeClass("active");
        $("#btMonth").addClass("active");
        return;
    }

}

/* Handle main table event*/
//Render day table by ajax
function loadDayTable(dateStr) {

    dayGetOfWeek = [];
    listDeleted = [];
    listInserted = [];
    if (dateStr == null) {
        dateStr = getDateTitle();
    }

    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            holidayArr = result.horlidays;
            //thead
            var thead = `<tr>
                                <th style="width:5%" ></th> 
                                <th style="width:15%">テーマNo</th>
                                <th style="width:30%" > テーマ名</th>
                                <th style="width:15%">内容</th>
                                <th style="width:15%">内容詳細</th>
                                <th style="width:10%">工数</th>
                                <th style="width:20%">操作</th> 
                            </tr>`;
            $('#thead').html(thead);

            //tbody

            let sum = 0;
            let dayGet = 'day' + new Date(result.dateSelect).getDate();
            let tbody = '';
            result.manhourDatas.forEach(data => {
                //set san date title
                $("#dateTitle").text(result.dateSelect + " (" + dayOfWeek(result.dateSelect) + ")");
                window.history.pushState('page2', 'Title', '/ManhourInput/Day');
                changeBtActive();
                $("#btnNow").text('今日');

                sum += data[dayGet];
                tbody += `<tr> 
                                    <td>
                                        <div class="text-center"> ${data.pin_flg == true ? `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                    </td>
                                    <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                    <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                    <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                    <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                    <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                    <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                    <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                    <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                    <input type="hidden" class="Total" name="Total" value="${data.total}" />`
                for (let i = 1; i <= numDayOfMonth; i++) {
                    tbody += `<input type="hidden" name="${'day' + i}" value="${data['day' + i]}" class="${'day' + i}">`
                }
                tbody += `<td class="ThemeNo">${data.theme_no}</td> 
                                    <td class="ThemeName">${data.theme_name1}</td>
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_code_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>

                                    <td>
                                        <input type="text" onclick = "this.select();setOldVal(this,'${`inputHour${dayGet}`}');"
                                             onchange = "onChangeValid(this,'inputHour${dayGet}')"
                                             onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        value="${data[dayGet].toFixed(1)}" class="form-control table-big-input inputHour${dayGet}">
                                    </td>                                  
                                    <td>
                                        <button class="btn btn-sm btn-outline-secondary mr-2 showchangeTheme" onclick="changeTheme(this)">
                                        <i class="fas fa-exchange-alt"></i> テーマ変更</button>
                                        <button class="btn btn-sm btn-outline-secondary mr-2 deleteTheme" onclick="deleteTheme(this)">
                                        <i class="far fa-trash-alt"></i> 削除</button>
                                    </td>
                            </tr>`;
            })
            $('#tbody').html(tbody);

            //tfoot
            var tfoot = `<tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>合計</td>`
            let nowDate = new Date().getDate();
            let numDayGet = getNumInStr(dayGet);
            let index = isHoliday(result.horlidays, numDayGet);
            if (sum == 0) {
                if (index === -1 && numDayGet <= nowDate) { //is not a horliday
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-danger"
                                        data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i>
                                    ${sum.toFixed(1)}
                                   </td>`;
                }

                else {
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">${sum.toFixed(1)}</td>`;
                }

            }
            else
                if (sum < 8 && numDayGet <= nowDate) { //is not normal working time

                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-warning"
                                          data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i> ${sum.toFixed(1)}
                                    </td>`
                } else { //is a normal working time
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">${sum.toFixed(1)}</td>`;
                }

            tfoot += `      <td></td>
                                </tr>
                                <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>不足工数</td>
                                <td id="missHour${getNumInStr(dayGet)}">${(8 - sum) > 0 ? ((8 - sum).toFixed(1) == 8 ? "" : (8 - sum).toFixed(1)) : ""}</td>
                                <td></td>
                            </tr>`;
            $('#tfoot').html(tfoot);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
}

//render manhour week table
let dayGetOfWeek = new Array();
let sumByDay = new Array();
let subByDay = new Array();
function loadWeekTable(dateStr) {
    listDeleted = [];
    listInserted = [];
    if (!dateStr) {
        dateStr = getDateTitle();
    }

    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

            let dateSelected = result.listDateOfWeek;
            let fromDate = dateSelected[0];
            let toDate = new Date(dateSelected[dateSelected.length - 1]).getDate();
            window.history.pushState('page2', 'Title', '/ManhourInput/Week');
            changeBtActive();
            $("#dateTitle").text(fromDate + '-' + toDate);
            $("#btnNow").text('今週');
            

            holidayArr = result.horlidays;
            dayGetOfWeek = [];
            let thead = `<tr> 
                                <th style="width:4%" ></th>
                                <th style="width:10%">テーマNo</th>
                                <th style="width:18%">テーマ名</th>
                                <th style="width:6%">内容</th>
                                <th style="width:6%">内容詳細</th>
                                <th style="width:6%">週計</th>`
            dateSelected.forEach(date => {
                thead += `<th style="width:6%">${date.substring(5, date.length)}-${dayOfWeek(date)}</th>`
                dayGetOfWeek.push("day" + new Date(date).getDate())//day of week [day1,day2]
            })

            thead += `<th style="width:4%" ></th >
                                <th style="width:4%"></th>
                       </tr>`;
            $('#thead').html(thead);

            // set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {
                tbody += `<tr>
                                     <td>
                                        <div class="text-center"> ${data.pin_flg == true ? `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                    </td>
                                    <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                    <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                    <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                    <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                    <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                    <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                    <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                    <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                    <input type="hidden" class="Total" name="Total" value="${data.total}" />`

                for (let i = 1; i <= numDayOfMonth; i++) {
                    tbody += `<input type="hidden" name="${'day' + i}" value="${data['day' + i].toFixed(1)}" class="${'day' + i}">`
                }
                tbody += `<td class="ThemeNo">${data.theme_no}</td> 
                                    <td class="ThemeName">${data.theme_name1}</td>
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_code_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>
                                    <td class="total">${data.total.toFixed(1)}</td>`

                dayGetOfWeek.forEach(date => {
                    tbody += `<td class="${getNumInStr(date)}">
                                    <input  onclick = "this.select();setOldVal(this,'${'input' + date}');"
                                    onchange = "onChangeValid(this,'input${date}')"
                                    onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"  type="text"
                                    value="${data[date].toFixed(1)}" class="form-control table-big-input input${date}"></td>`
                })
                tbody += `          <td>
                                        <button class="btn btn-sm" onclick="changeTheme(this)">
                                            <i class="fas fa-exchange-alt"></i>
                                        </button>
                                   </td>    
                                   <td>
                                        <button class="btn btn-sm" onclick="deleteTheme(this)">
                                        <i class="far fa-trash-alt"></i>
                                        </button>
                                  </td>
                            </tr>`;
            });
            $('#tbody').html(tbody);

            sumByDay = [];
            dayGetOfWeek.forEach(data => {
                sumByDay.push($(`.${data}`).sum());
            })
            // sub by column
            subByDay = [];
            dayGetOfWeek.forEach(data => {
                subByDay.push(8 - $(`.${data}`).sum());
            });

            //set tfoot for table
            let tfoot = `<tr><td></td><td></td><td></td><td></td><td>合計</td><td id="totalHour">${$(`.Total`).sum()}</td>`;

            let x = 0;
            let nowDate = new Date().getDate();
            sumByDay.forEach(item => {

                let numDayGet = getNumInStr(dayGetOfWeek[x]);
                let index = isHoliday(result.horlidays, numDayGet);
                if (item == 0) {
                    if (index === -1 && numDayGet <= nowDate) { //is not a horliday
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-danger"
                                        data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i>
                                    ${item.toFixed(1)}
                                   </td>`;
                    }

                    else {
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">${item.toFixed(1)}</td>`;
                    }

                }
                else
                    if (item < 8 && numDayGet <= nowDate) { //is not normal working time

                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-warning"
                                          data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i> ${item.toFixed(1)}
                                    </td>`
                    } else { //is a normal working time
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">${item.toFixed(1)}</td>`;
                    }

                x++;
            });

            // Handle event in tfoot
            tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td></td><td></td><td>不足工数</td><td></td>`
            x = 0;
            subByDay.forEach(item => {
                let numDayGet = getNumInStr(dayGetOfWeek[x]);
                let index = isHoliday(result.horlidays, numDayGet);

                if (item <= 0 || index !== -1 || item == 8) {//not display
                    tfoot += `<td id="missHour${numDayGet}"></td>`
                } else {
                    tfoot += `<td class="${numDayGet}" id="missHour${numDayGet}">${item.toFixed(1)}</td>`
                }
                x++;
            });
            tfoot += `<td></td>
                          <td></td>
                          </tr>`;

            $('#tfoot').html(tfoot);

            //Set backgorund for horliday
            setHolidayBackground(result.horlidays);
        }

    });

}

function isHoliday(holidayArr, num) {
    return holidayArr.findIndex(e => e == num);
}

//render manhour month table
function loadMonthTable(dateStr) {

    dayGetOfWeek = [];
    listDeleted = [];
    listInserted = [];
    dayGetOfWeek = [];
    if (!dateStr) {
        dateStr = getDateTitle();
    }
    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

            //Change layout information
            window.history.pushState('page2', 'Title', '/ManhourInput/Month');
            changeBtActive();
            $("#btnNow").text('今月');
            $("#dateTitle").text(result.dateSelect + "-" + numDayOfMonth);

            holidayArr = result.horlidays;
            let dateGet = new Date(result.dateSelect);
            numDayOfMonth = getDaysInMonth(dateGet.getFullYear(), dateGet.getMonth() + 1);

            //Set thead for table
            let thead = `tr><th></th><th>テーマNo</th><th>テーマ名</th><th>内容</th><th>月計</th>`
            for (let i = 1; i <= numDayOfMonth; i++) {
                thead += `<th>${i}</th>`
            }
            thead += `<th colspan ="2"> 操作</th >
                          </tr>`;
            $('#thead').html(thead);

            //Set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {
                tbody += `<tr>
                                <td>
                                     <div class="text-center"> ${data.pin_flg == true ? //display pin icon
                        `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                </td>
                                <td class="ThemeNo">${data.theme_no}</td> 
                                <td class="ThemeName">${data.theme_name1}</td>
                                <td class="Detail">${data.work_contents_detail}</td>
                                <td class="total">${data.total.toFixed(1)}</td>
                                <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="Total" name="Total" value="${data.total}" />`
                for (let i = 1; i <= numDayOfMonth; i++) {

                    tbody += `<td class="${i}">
                                    <input  onclick="this.select();setOldVal(this,'${'day' + i}');" 
                                    onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                    onchange="onChangeValid(this,'${'day' + i}')"
                                    type="text" value="${data['day' + i].toFixed(1)}" 
                                    class="form-control table-input ${'day' + i}">
                             </td>`

                }

                tbody += `   <td><i onclick="changeTheme(this)" class="fas fa-exchange-alt"></i></td>    
                             <td><i class="far fa-trash-alt" onclick="deleteTheme(this)"></i></td>
                          </tr>`;
            })

            // Handle event in tfoot
            $('#tbody').html(tbody);
            // sum by column
            sumByDay = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                sumByDay.push($('.day' + i).sum());
            }
            // sub by column
            subByDay = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                subByDay.push(8 - $('.day' + i).sum());
            }

            let tfoot = `<tr><td></td><td></td><td>合計</td><td></td><td id="totalHour">${$('.Total').sum().toFixed(1)}</td>`

            let x = 1;
            let nowDate = new Date().getDate();
            let nowMonth = new Date().getMonth()+1;
            let month = new Date(getDateTitle()).getMonth()+1;
            sumByDay.forEach(item => {
                const index = isHoliday(result.horlidays, x);

                if (item == 0) {//hour working equal 0
                    if (index === -1 && x <= nowDate && month == nowMonth) { //is not a horliday and less than date now
                        tfoot += `<td class="${x}" id="Totalday${x}">
                                        <i  class="fas fa-exclamation-circle fa-xs text-danger"
                                            data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i>
                                    ${item.toFixed(1)}
                                   </td>`;
                    }

                    else { //is horliday or more than date now

                        tfoot += `<td class="${x}" id="Totalday${x}"> ${item.toFixed(1)}</td>`;
                    }

                } else
                    if (item < 8 && x <= nowDate) { //is not a normal working time and less than date now

                        tfoot += `<td class="${x}" id="Totalday${x}">
                                        <i class="fas fa-exclamation-circle fa-xs text-warning"
                                        data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                                  </td>`
                    } else { // is a normal working time

                        tfoot += `<td class="${x}" id="Totalday${x}">${item.toFixed(1)}</td>`;
                    }
                x++;
            });

            tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td>残工数</td><td></td><td></td>`
            x = 1;
            subByDay.forEach(item => {
                const index = isHoliday(result.horlidays, x);
                if (item <= 0 || index !== -1 || item == 8) {
                    tfoot += `<td class="${x}" id="missHour${x}"></td>`
                } else {
                    tfoot += `<td class="${x}" id="missHour${x}">${item.toFixed(1)}</td>`
                }
                x++;
            });

            tfoot += `<td></td><td></td></tr>`;
            $('#tfoot').html(tfoot)
            //Set backgorund for holiday
            setHolidayBackground(result.horlidays);

        }

    });
  
}

function setHolidayBackground(holidays) {

    holidays.forEach(data => {
        $(`.${data}`).css("background-color", "#f5c6cb");
    })
    //set background for date now
    let now = new Date().getDate();
    let nowmonth = new Date().getMonth() + 1;
    let month = new Date(getDateTitle()).getMonth() + 1;
    if (nowmonth == month) {
        $(`.${now}`).css("background-color", "#bee5eb");
    }
}

function getNumInStr(str) {
    return str.replace(/^.*?(\d+).*/, '$1');
}

//save page clicked
function savePageHistory(pageName) {
    $.ajax({
        url: "/ManhourInput/SavePageHistory",
        data: { pageName: pageName },
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    });
}

//return number day of the month
function getDaysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}


//show display to show tooltip 
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})

//display muilty modal to add z-index
$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

//make checkbox like radio
$(document).on('click', 'input[type="checkbox"]', function () {
    $('input[type="checkbox"]').not(this).prop('checked', false);
});


//show date time picker
$('#datepicker1').datepicker({
    format: "yyyy/mm/dd", language: "ja", autoclose: true, orientation: 'bottom right'
}).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    console.log(date);
    window.location = 'https://localhost:44363/ManhourInput/Index?dateSt=' + formatDate(date);
});

$('[data-toggle="tooltip"]').tooltip();

//valid input hour 
let oldVal = null;
function setOldVal(el, name) { //onclick to save old value
    oldVal = $(el).closest('tr').find(`.${name}`).val();
    console.log(oldVal);
}

//onchange valid value input hour
function onChangeValid(el, name) {

    let inputHour = parseInt($(el).closest('tr').find(`.${name}`).val());
    let nowDate = new Date().getDate();
    let nowMonth = new Date().getMonth();
    let dayGet = getNumInStr(name);

    if (inputHour < 0 || isNaN(inputHour)) {
        alert('Input hour not valid!');
        $(el).closest('tr').find(`.${name}`).val(oldVal); oldVal = null;
        return;
    }
    let totalHourDay = $(`.${name}`).sum(); //sum hour value in class 
    if (totalHourDay > 24)  //sum hour value in class must less than 24h
    {
        alert('Total hour of a day must be less than 24h!');
        $(el).closest('tr').find(`.${name}`).val(oldVal); oldVal = null;
        return;
    }
    //set input valid
    $(el).closest('tr').find(`.day${dayGet}`).val(inputHour.toFixed(1));
    $(el).closest('tr').find(`.inputday${dayGet}`).val(inputHour.toFixed(1));
    $(el).closest('tr').find(`.inputHourday${dayGet}`).val(inputHour.toFixed(1));

    //set vlaue for sum hour by day array
    sumByDay[dayGet - 1] = totalHourDay;
 
    let index = isHoliday(holidayArr, dayGet);
    let text = '';
    //get text with value of totalHourDay
    if (totalHourDay == 0) {//hour working equal 0
        if (dayGet <= nowDate && index ===-1) { //is not a horliday and less than date now
            text += `<i  class="fas fa-exclamation-circle fa-xs text-danger"
                         data-toggle="tooltip" title="合計工数が8h未満です"></i>${totalHourDay.toFixed(1)}`;
        }

        else { //is horliday or more than date now
            text += totalHourDay.toFixed(1);
        }
    } else {
        if (totalHourDay < 8 && dayGet <= nowDate && index === -1) { //is not a normal working time and less than date now and not a holiday

            text += `<i class="fas fa-exclamation-circle fa-xs text-warning"
                        data-toggle="tooltip" title="合計工数が8h未満です"></i> ${totalHourDay.toFixed(1)}`;

        } else { // is a normal working time

            text += `<i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${totalHourDay.toFixed(1)}`;

        }
    }

    //dispaly total hour by day
    $(`#Total${name}`).html(text);

    //total hour by month
    let sumHourMonth = 0;
    for (let i = 1; i <= numDayOfMonth; i++) {
        sumHourMonth += parseInt($(el).closest('tr').find(`.day${i}`).val());
    }
    //display total hour in month 
    $(el).closest('tr').find(`.total`).text(sumHourMonth.toFixed(1));
    $(el).closest('tr').find(`.Total`).val(sumHourMonth.toFixed(1));
    $('#totalHour').text($('.Total').sum().toFixed(1));

    if (index === -1) {
        $(`#missHour${dayGet}`).text((8 - totalHourDay) <= 0 ? '' : (8 - totalHourDay).toFixed(1));
    }
   
}

//export csv
function exportCSV() {
    var url = "/ManhourInput/ExportCSV?dateStr=" + getDateTitle();
    $(location).attr('href', url);
}


