const ERR_001 = "メッセージエリア表示";
const ERR_002 = "メッセージエリア表示";
const ERR_005 = "メッセージエリア表示";

let namePageLoad = $('#pageHistory').val();
let dateTitle = new Date(getDateTitle());
let numDayOfMonth = daysInMonth(dateTitle.getFullYear(), dateTitle.getMonth() - 1);

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
//go to previus time
function gotoPrevius() {

    let pageActive = $('#btNow').text();
    let dateTitle = getDateTitle();
    if (pageActive == '今日') {
        loadDayTable(gotoDate(dateTitle, -1));
    }
    if (pageActive == '今週') {
        loadWeekTable(gotoDate(dateTitle, -1));
    }
    if (pageActive == '今月') {
        loadMonthTable(gotoMonth(dateTitle, -1));
    }

}

//goto next time
function gotoNext() {
    let pageActive = $('#btNow').text();
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

// go to local time now
function gotoNow() {
    let nowDate = formatDate(new Date(Date.now()));
    let pageActive = $('#btNow').text();
    if (pageActive == '今日') {
        loadDayTable(nowDate);
    }
    if (pageActive == '今週') {
        loadWeekTable(nowDate);
    }
    if (pageActive == '今月') {
        loadMonthTable(nowDate);
    }
}

//save information into DB
let listDeleted = new Array();
let listChanged = new Array();
let listInserted = new Array();

// Change theme selected
function changeTheme(el) {

    if (confirm("Do you want to change this theme?") == true) {

        let themName = $(el).closest("tr").find(".ThemeName").text();
        let themeNo = $(el).closest("tr").find(".ThemeNo").text();
        let wContent = $(el).closest("tr").find(".WContent").text();
        let detail = $(el).closest("tr").find(".Detail").text();

        $("#modalThemeNo").val(themeNo);
        $("#modalThemeName").val(themName);
        $("#modalWC").val(wContent);
        $("#modalDetail").val(detail);
        $("#modal3").modal('show');

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

        //convert 2021/06/23 => 20210623
        obj.fix_date = formatDate(new Date()).split('/').join([]);

        //get input hour for month
        for (let i = 1; i <= numDayOfMonth; i++) {
            obj[`day${i}`] = parseFloat($(this).find(`.day${i}`).val());
        }

        //get input hour for day
        if ($(this).find(".inputHour").val()) {
            obj[dayGet] = parseFloat($(this).find(".inputHour").val());
        }

        //get input hour for week
        if (dayGetOfWeek.length > 0) {
            dayGetOfWeek.forEach(date => {
                obj[`${date}`] = parseFloat($(this).find(`.input${date}`).val());
            })
        }
        // Add to list data
        listData.push(obj);

    });

    //list manhour change
    saveData.Update = listData;listdata = [];
    //list manhour deleted
    saveData.Delete = listDeleted;listDeleted = [];
    //list manhour insert
    saveData.Insert = listInserted;listInserted = [];

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
var themeNo = null;
var themeName = null;
var workContentClass = null;
function choiceTheme() {

    $("#slThemeBody tr").each(function () {

        if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
            themeNo = $(this).find(".ThemeNo").val();
            workContentClass = $(this).find(".WorkContentClass").val();
            themeName = $(this).find(".ThemeName").val();
            $('#modal1').modal('hide');
            $('#themeSelected').val(themeNo + ' ' + themeName);
        }

    });
}
/* Handle select theme evet*/

//Add new theme to table
function addTheme() {

    let workcontent = $('#workContentCode :selected').text();
    let workContentCode = $('#workContentCode :selected').val();
    let workContentDetail = $(detailCode).val();
    let userNo = 'BAOTQ';
    let dateTitle = new Date(getDateTitle());
    let year = dateTitle.getFullYear();
    let month = dateTitle.getMonth() + 1;

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
    rowAdd += `<tr>
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
                            <input type="hidden" class="Total"      name="Total" value="${0.0}" />`
    for (let i = 1; i <= numDayOfMonth; i++) {
        rowAdd += `<<input type="hidden" name="${'day' + i}" value="0.0" class="${'day' + i}">`
    }
    rowAdd += `         <td class="ThemeNo">${themeNo}</td>
                        <td class="ThemeName">${themeName}</td>
                        <td class="WContent">${workcontent}</td>
                        <td class="Detail">${workContentDetail}</td>
                        <td> <input onclick="this.select();" type="text" value="0.0" class="form-control table-big-input inputHour"></td>
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
    let obj = {};
    obj.Year = parseInt(year);
    obj.Month = parseInt(month);
    obj.Theme_no = themeNo;
    obj.User_no = 'BAOTQ';
    obj.Work_contents_class = workContentClass;
    obj.Work_contents_code = workContentCode;
    obj.Work_contents_detail = workContentDetail;
    listInserted.push(obj);
    //set theme information to null
    themeNo = null; themeName = null; workContentClass = null;
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
    return date.toISOString().slice(0, 10).replace(/-/g, "/");
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
function validDetailCode() {

    let code = $("#detailCode").val();

    if (code.length != 2 || code < 0 || code > 99) {
        $("#detailCode").val('');
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

                sum += data[dayGet];
                tbody += `<tr> 
                                    <td>
                                        <div class="text-center"> ${data.pin_flg == true ? `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                    </td>
                                    <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                    <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                    <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                    <input type="hidden" class="Group_Code" name="Group_Code" value="${data.group_code}" />
                                    <input type="hidden" class="Site_Code" name="Site_Code" value="${data.site_code}" />
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
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_class_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>

                                    <td>
                                        <input type="text"  onclick="this.select();" onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122" value="${data[dayGet].toFixed(1)}" class="form-control table-big-input inputHour">
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
            //render icon warning 
            if (sum == 8) {
                tfoot += `<td>
                                <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${sum.toFixed(1)}
                          </td>`
            } else
                if (sum == 0) {
                    tfoot += `<td>
                                   <i class="fas fa-exclamation-circle text-danger" data-toggle="tooltip" title="合計工数が8h未満です"></i> ${sum.toFixed(1)}
                              </td>`
                } else {
                    tfoot += `<td>
                                  <i class="fas fa-exclamation-circle text-warning" data-toggle="tooltip" title="合計工数が8h未満です"></i> ${sum.toFixed(1)}
                              </td>`
                }

            tfoot += `  <td></td>
                                </tr>
                                <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>不足工数</td>
                                <td>${(8 - sum) != 0 ? (8 - sum).toFixed(1) : ""}</td>
                                <td></td>
                            </tr>`;
            $('#tfoot').html(tfoot);
            //set san date title
            $("#dateTitle").text(result.dateSelect + " (" + dayOfWeek(result.dateSelect) + ")");
            window.history.pushState('page2', 'Title', '/ManhourInput/Day');
            changeBtActive();
            $("#btNow").text('今日');
            
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
}

//Render week table by ajax
let dayGetOfWeek = new Array();
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
                dayGetOfWeek.push("day" + new Date(date).getDate())
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
                                        <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                                    </div>
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
                        tbody+=     `<td class="ThemeNo">${data.theme_no}</td> 
                                    <td class="ThemeName">${data.theme_name1}</td>
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_class_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>
                                    <td class="Total">${data.total.toFixed(1)}</td>` 

                dayGetOfWeek.forEach(date => {
                    tbody += `<td class="${date.replace(/^.*?(\d+).*/, '$1')}"><input  onclick="this.select();" onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"  type="text" value="${data[date].toFixed(1)}" class="form-control table-big-input input${date}"></td>`
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

            let sumArr = [];
            dayGetOfWeek.forEach(data=> {
                sumArr.push($(`.${data}`).sum());
            })
            // sub by column
            let subArr = [];
            dayGetOfWeek.forEach(data => {
                    subArr.push(8 - $(`.${data}`).sum());
            });

            //set tfoot for table
            let tfoot = `<tr><td></td><td></td><td></td><td></td><td>合計</td><td id="totalhour">${$(`.Total`).sum()}</td>`;

            let x = 0;
            let nowDate = new Date().getDate(); 
            sumArr.forEach(item => {
                let numDayGet = dayGetOfWeek[x].replace(/^.*?(\d+).*/, '$1');
                let index = isHorliday(result.horlidays, numDayGet);
                if (item == 0) {

                    if (index === -1 && numDayGet <= nowDate) { //is not a horliday
                        tfoot += `<td class="${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-danger"
                                        data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i> ${item.toFixed(1)}
                                   </td>`;
                    }

                    else {
                        tfoot += `<td class="${numDayGet}">
                                       <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                                   </td>`;
                    }

                }
                else if (item < 8) { //is not normal working time

                        tfoot += `<td class="${numDayGet}">
                                <i class="fas fa-exclamation-circle text-warning"
                                  data-toggle="tooltip" title="合計工数が8h未満です">
                                </i> ${item.toFixed(1)}
                              </td>`
                    } else {
                        tfoot += `<td class="${numDayGet}">
                                <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                              </td>`;
                    } 
                x++;
            });

            // Handle event in tfoot
            tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td></td><td></td><td>不足工数</td><td></td>`;
            x = 0;
            subArr.forEach(item => {
                let numDayGet = dayGetOfWeek[x].replace(/^.*?(\d+).*/, '$1');
                let index = isHorliday(result.horlidays, numDayGet);
                if (item == 0 || index !== -1 || item==8) {
                    tfoot += `<td></td>` 
                } else {
                    tfoot += `<td class="${numDayGet}">${item.toFixed(1)}</td>`
                } 
                x++;
            });
                tfoot += `<td></td>
                          <td></td>
                          </tr>`;

            $('#tfoot').html(tfoot);
          
            let fromDate = dateSelected[0];
            let toDate = new Date(dateSelected[dateSelected.length - 1]).getDate();
            window.history.pushState('page2', 'Title', '/ManhourInput/Week');
            changeBtActive();
            $("#dateTitle").text(fromDate + '-' + toDate);
            $("#btNow").text('今週');

            //Set backgorund for horliday
            horlidayBackground(result.horlidays);
        }

    });

}

function isHorliday(horlidayArr, num) {
    return horlidayArr.findIndex(e => e == num);
}

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
                                    <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                                <td>${data.theme_no}</td>
                                <td>${data.theme_name1}</td>
                                <td>${data.work_contents_code}</td>
                                <td id="total">${data.total}</td>
                                <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                <input type="hidden" class="Group_Code" name="Group_Code" value="${data.group_code}" />
                                <input type="hidden" class="Site_Code" name="Site_Code" value="${data.site_code}" />
                                <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="Total" name="Total" value="${data.total}" />`
                for (let i = 1; i <= numDayOfMonth; i++) {
                    tbody += `<td class="${i}"><input  onclick="this.select();" onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"  type="text" value="${data['day' + i].toFixed(1)}" class="form-control table-input ${'day' + i}"></td>`
                }

                tbody += `    <td><div class="text-center"><i class="fas fa-exchange-alt" ></i></div></td>
                                <td><div class="text-center"><i class="far fa-trash-alt"></i></div></td>
                              </tr>`;
            })

            // Handle event in tfoot
            $('#tbody').html(tbody);
            // sum by column
            let sumArr = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                sumArr.push($('.day' + i).sum());
            }
            // sub by column
            let subArr = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                subArr.push(8 - $('.day' + i).sum());
            }
            let tfoot = `<tr><td></td><td></td><td>合計</td><td></td><td>${$('#total').sum().toFixed(1)}</td>`
            let x = 1;
            let nowDate = new Date().getDate(); 
            sumArr.forEach(item => {
                const index = isHorliday(result.horlidays, x);
                if (item == 0) {//hour working equal 0

                    if (index === -1 && x <= nowDate) { //is not a horliday and less than now date
                        tfoot += `<td class="${x}">
                                        <i   class="fas fa-exclamation-circle fa-xs text-danger"
                                            data-toggle="tooltip" title="合計工数が8h未満です">
                                        </i>
                                    ${item.toFixed(1)}
                                   </td>`;
                    }

                    else {
                        tfoot += `<td class="${x}">
                                       <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                                   </td>`;
                    }

                } else
                    if (item < 8 && x <= nowDate) {

                        tfoot += `<td class="${x}">
                                        <i class="fas fa-exclamation-circle fa-xs text-warning"
                                        data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                                  </td>`
                    } else { // is a normal working date

                        tfoot += `<td class="${x}">
                                        <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${item.toFixed(1)}
                                  </td>`;
                    }
                x++;
            });

             tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td>残工数</td><td></td><td></td>`
             x = 1;
            subArr.forEach(item => {
                const index = isHorliday(result.horlidays, x);
                if (item == 0 || index !== -1 ||item ==8) {
                    tfoot += `<td class="${x}"></td>`
                } else {
                    tfoot += `<td class="${x}">${item.toFixed(1)}</td>`
                }
                x++;
            });

            tfoot += `<td></td><td></td></tr>`;
            $('#tfoot').html(tfoot)
            let dateGet = new Date(result.dateSelect);
            numDayOfMonth = daysInMonth(dateGet.getFullYear(), dateGet.getMonth()+1);
            $("#dateTitle").text(result.dateSelect + "-" + numDayOfMonth);

            //Set backgorund for horliday
            horlidayBackground(result.horlidays);

        }

    });
    //Change layout information and animation
    window.history.pushState('page2', 'Title', '/ManhourInput/Month');
    changeBtActive();
    $("#btNow").text('今月');
}

function horlidayBackground(horlidays) {

    horlidays.forEach(data => {
        $(`.${data}`).css("background-color", "#f5c6cb");
    })
    //set background for date now
    let now = new Date().getDate();
    $(`.${now}`).css("background-color", "#bee5eb");
     
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
function daysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}


//show display modal
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})

$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

$(document).on('click', 'input[type="checkbox"]', function () {
    $('input[type="checkbox"]').not(this).prop('checked', false);
});

$('.inputday').on('change', function () {
    if (this.val < 0 || this.val) {
        alert
    }
});


