const ERR_001 = "メッセージエリア表示";
const ERR_002 = "メッセージエリア表示";
const ERR_005 = "メッセージエリア表示";

let namePageLoad = $('#pageHistory').val();
$(document).ready(function () {
    /* Handle main table event*/
    changeBtActive();

    if (namePageLoad == "Month") {

        loadMonthTable();

    } else if (namePageLoad == "Week") {

        loadWeekTable();

    } else {

        loadDayTable();

    }

    $("#btDay").on("click", function () {
        let btname = $(this).attr("name");
        console.log(btname);
        loadDayTable()
    });

    $("#btWeek").on("click", function () {
        let btname = $(this).attr("name");
        console.log(btname);
        loadWeekTable()
    });

    $("#btMonth").on("click", function () {
        let btname = $(this).attr("name");
        console.log(btname);
        loadMonthTable()
    });

    //load prev day
    $("#btPrev").on("click",
        function () {
            let dateTitle = getDateTitle();
            loadDayTable(changeDate(dateTitle, -1));
        })

    //load next day
    $("#btNext").on("click",
        function () {
            let dateTitle = getDateTitle();
            loadDayTable(changeDate(dateTitle, 1));
        })

    //load local now date
    $("#btNow").on("click",
        function () {
            let nowDate = formatDate(new Date(Date.now()));
            loadDayTable(nowDate);
        })

    //save information into DB
    $("#btSave").on("click",
        function () {

            let dayGet = 'day' + new Date(getDateTitle()).getDate();
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
                obj.Work_contents_class = $(this).find(".WC_Class").val();
                obj.Work_contents_code = $(this).find(".WC_Code").val();
                obj.Work_contents_detail = $(this).find(".WC_Detail").val();
                obj.pin_flg = false;
                obj.total = parseFloat($(this).find(".total").val());
                obj.fix_date = formatDate(new Date()).split('/').join([]);

                //get date value for obj
                for (let i = 1; i < 32; i++) {
                    obj[`day${i}`] = parseFloat($(this).find(`.day${i}`).val());
                }

                if ($(this).find(".InputHour input").val()) {
                    obj[dayGet] = parseFloat($(this).find(".InputHour input").val());
                }

                // Add to list data
                listData.push(obj);
            });
            console.log(listData);
            $.ajax({
                url: "/ManhourInput/Save",
                data: JSON.stringify(listData),
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    return alert(result);
                }

            });

        });

    /* Handle select theme event*/
    // Radio checked display
    $("#cb3").on("click", function () {
        $("#cb3").attr("checked", true);
        $("#cb1").attr('checked', false);
        $("#cb2").attr('checked', false);
    })
    $("#cb2").on("click", function () {
        $("#cb2").attr("checked", true);
        $("#cb1").attr('checked', false);
        $("#cb3").attr('checked', false);
    })
    $("#cb1").on("click", function () {
        $("#cb1").attr("checked", true);
        $("#cb3").attr('checked', false);
        $("#cb2").attr('checked', false);
    })


    //load select theme history modal
    $(".btSelectTheme").on("click", function () {

        $.ajax({
            url: `/ManhourInput/GetHistoryThemes`,
            method: "POST",
            success: function (result) {

                if (result == null) {
                    return;
                }
                console.log(result);
                themeNo = result.historyInput.themeNo;
                themeName = result.historyInput.themeName;
                groupCode = result.historyInput.accountingGroupCode;
                objCode = result.historyInput.salesObjectCode;

                //set history value 
                $('#slThemeNo').val(themeNo != null ? themeNo : '');
                $('#slThemeName').val(themeName != null ? themeName : '')
                $('#comboxGroup').val(groupCode != null ? groupCode : '');
                $('#comboxObject').val(objCode != null ? objCode : '');

                //checked by sold flag
                if (result.historyInput.soldFlg == "未売上") {
                    $("#cb1").attr("checked", true);
                    $("#cb3").attr('checked', false);
                    $("#cb2").attr('checked', false);
                }
                if (result.historyInput.soldFlg == "売上済") {
                    $("#cb2").attr("checked", true);
                    $("#cb1").attr('checked', false);
                    $("#cb3").attr('checked', false);
                }
                if (result.historyInput.soldFlg == "全て") {
                    $("#cb3").attr("checked", true);
                    $("#cb2").attr('checked', false);
                    $("#cb1").attr('checked', false);
                }

                let tbody = '';
                result.themes.forEach(data => {
                    tbody += `<tr>
                            <td>
                            <div class="form-check text-center">
                                <input class="form-check-input position-static Checkbox" type="checkbox" value="option1" aria-label="...">
                            </div>
                            </td>
                            <input type="hidden" class ="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}"/>
                            <input type="hidden" class ="ThemeNo" name="ThemeNo" value="${data.theme_no}"/>
                            <input type="hidden" class ="ThemeName" name="ThemeName" value="${data.theme_name1}"/> 
                             <td>${data.theme_no}</td>
                            <td width="200px">${data.theme_name1}</td>`
                    tbody += data.sold_flg == true ? `<td>売上済</td></tr >` : `<td>未売上</td> </tr >`;

                });
                $('#slThemeBody').html(tbody);
            }
        });
    });

    // Search theme
    $("#searchTheme").on("click",
        function () {

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
            obj.ThemeNo = $('#slThemeNo').val() == "" ? null : $('#slThemeNo').val()
            obj.ThemeName = $('#slThemeName').val() == "" ? null : $('#slThemeName').val()
            obj.AccountingGroupCode = $('#comboxGroup').val() == "" ? null : $('#comboxGroup').val()
            obj.SalesObjectCode = $('#comboxObject').val() == "" ? null : $('#comboxObject').val()
            obj.SoldFlg = soldFlg

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
                                        <input class="form-check-input position-static radio" type="checkbox" id="Checkbox" value="option1" aria-label="..."/>                                         
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
        });

    //get information when onclick add theme form checked row
    var themeNo = null;
    var themeName = null;
    var workContentClass = null;
    $("#choiceTheme").on("click",
        function addTheme() {

            $("#slThemeBody tr").each(function () {
                if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
                    themeNo = $(this).find(".ThemeNo").val();
                    workContentClass = $(this).find(".WorkContentClass").val();
                    themeName = $(this).find(".ThemeName").val();
                    $('#modal1').modal('hide');
                }
            });
        });
    /* Handle select theme evet*/

    //Add new theme to table
    $("#addTheme").on("click",
        function () {
            let workcontent = $('#workContentCode :selected').text()
            let workContentCode = $('#workContentCode :selected').val();
            let workContentDetail = $(detailCode).val();

            if (!themeNo || !themeName || !workContentClass) {
                alert("please choice theme!");
                return;
            }
            if (!workContentDetail ) {
                alert("please choice work contents!");
                return;
            }
            if (!workContentCode) {
                alert("please choice work content code!");
                return;
            }
            
            var rowAdd = '';
            let dateTitle = new Date(getDateTitle());
            rowAdd += `<tr>
                        <td>
                            <div class="text-center">
                                <i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                            </div>
                        </td>
                            <input type="hidden" class="Year"      name="Year" value="${dateTitle.getFullYear()}"/>
                            <input type="hidden" class="Month"     name="Month" value="${dateTitle.getMonth()}"/>
                            <input type="hidden" class="Theme_No"  name="ThemeNo" value="${themeNo}"/>
                            <input type="hidden" class="WC_Class"  name="WorkContentClass" value="${workContentClass}"/>
                            <input type="hidden" class="WC_Code"   name="WorkContentCode" value="${workContentCode}"/>
                            <input type="hidden" class="WC_Detail" name="WorkContentCode" value="${workContentDetail}"/>
                        <td class="ThemeNo">${themeNo}</td>
                        <td class="ThemeName">${themeName}</td>
                        <td class="WContent">${workcontent}</td>
                        <td class="Detail">${workContentDetail}</td>
                        <td  class="InputHour"> <input type="text" value="0.0" class="form-control table-big-input"></td>
                        <td>
                            <div>
                                <button class="btn btn-sm btn-outline-secondary mr-2" onclick="changeTheme(this)">
                                            <i class="fas fa-exchange-alt"></i> テーマ変更
                                </button>
                   
                               <button class="btn btn-sm btn-outline-secondary mr-2" onclick="deleteTheme(this)">
                                <i class="far fa-trash-alt"></i> 削除</button>
                             <div>       
                         </td>
                       </tr>`
            $('#tbody').append(rowAdd);

            //set theme information to null
            themeNo = null;themeName = null;workContentClass = null;
        });

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

});


//format date return string date 2021/06/18
formatDate = function (date) {
    return date.toISOString().slice(0, 10).replace(/-/g, "/");
}

//get date title
getDateTitle = function () {

    return $('#dateTitle').text().trim().substring(0, 10);
}
//previous date
changeDate = function (dateSt, dayNumber) {

    const date = new Date(dateSt);

    const newDate = (date.setDate(date.getDate() + dayNumber));

    const nextDate = new Date(newDate).toLocaleDateString("ja-JP");

    return nextDate;
}

//number to string format (1.0)
function NTS(num) {
    if (num % 1 === 0)
        return num.toFixed(1);
    else
        return num.toString();
}

// Change theme selected
changeTheme = function (el) {

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
deleteTheme = function (el) {

    if (confirm("Do you want to delete this row?") == true) {
        $(el).closest('tr').remove();
    }
}

//valid detail code in 00-99
validDetailCode = function checkDetailCode() {

    let code = $("#detailCode").val();

    if (code.length != 2 || code < 0 || code > 99) {
        $("#detailCode").val('');
        return alert('Giá tri nhập vào khác 00 - 99');
    }

}

//Change button active by table render
changeBtActive = function () {

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

//Render day table by ajax
loadDayTable = function (dateSt) {

    if (dateSt == null) {
        dateSt = $('#dateTitle').text().trim().substring(0, 10);
    }

    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateSt: dateSt },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

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


            let sum = 0;
            let dayGet = 'day' + new Date(result.dateSelect).getDate();


            var tbody = '';
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
                                    <input type="hidden" class="pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                    <input type="hidden" class="total" name="Total" value="${data.total}" />
                                    <input type="hidden" class="day1"  name="Day1"  value="${data.day1}" />
                                    <input type="hidden" class="day2"  name="Day2"  value="${data.day2}" />
                                    <input type="hidden" class="day3"  name="Day3"  value="${data.day3}" />
                                    <input type="hidden" class="day4"  name="Day4"  value="${data.day4}" />
                                    <input type="hidden" class="day5"  name="Day5"  value="${data.day5}" />
                                    <input type="hidden" class="day6"  name="Day6"  value="${data.day6}" />
                                    <input type="hidden" class="day7"  name="Day7"  value="${data.day7}" />
                                    <input type="hidden" class="day8"  name="Day8"  value="${data.day8}" />
                                    <input type="hidden" class="day9"  name="Day9"  value="${data.day9}" />
                                    <input type="hidden" class="day10" name="Day10" value="${data.day10}" />
                                    <input type="hidden" class="day11" name="Day11" value="${data.day11}" />
                                    <input type="hidden" class="day12" name="Day12" value="${data.day12}" />
                                    <input type="hidden" class="day13" name="Day13" value="${data.day13}" />
                                    <input type="hidden" class="day14" name="Day14" value="${data.day14}" />
                                    <input type="hidden" class="day15" name="Day15" value="${data.day15}" />
                                    <input type="hidden" class="day16" name="Day16" value="${data.day16}" />
                                    <input type="hidden" class="day17" name="Day17" value="${data.day17}" />
                                    <input type="hidden" class="day18" name="Day18" value="${data.day18}" />
                                    <input type="hidden" class="day19" name="Day19" value="${data.day19}" />
                                    <input type="hidden" class="day20" name="Day20" value="${data.day20}" />
                                    <input type="hidden" class="day21" name="Day21" value="${data.day21}" />
                                    <input type="hidden" class="day22" name="Day22" value="${data.day22}" />
                                    <input type="hidden" class="day23" name="Day23" value="${data.day23}" />
                                    <input type="hidden" class="day24" name="Day24" value="${data.day24}" />
                                    <input type="hidden" class="day25" name="Day25" value="${data.day25}" />
                                    <input type="hidden" class="day26" name="Day26" value="${data.day26}" />
                                    <input type="hidden" class="day27" name="Day27" value="${data.day27}" />
                                    <input type="hidden" class="day28" name="Day28" value="${data.day28}" />
                                    <input type="hidden" class="day29" name="Day29" value="${data.day29}" />
                                    <input type="hidden" class="day30" name="Day30" value="${data.day30}" />
                                    <input type="hidden" class="day31" name="Day31" value="${data.day31}" />
                                    <td class="ThemeNo">${data.theme_no}</td> 
                                    <td class="ThemeName">${data.theme_name1}</td>
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_class_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>

                                    <td class="InputHour">
                                        <input type="text" value="${NTS(data[dayGet])}" class="form-control table-big-input">
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


            var tfoot = `<tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>合計</td>`
            //render icon warning 
            if (sum == 8) {
                tfoot += `<td>
                                <i data-toggle="tooltip" title="合計工数が8h未満です"></i> ${NTS(sum)}
                          </td>`
            } else
                if (sum == 0) {
                    tfoot += `<td>
                                   <i class="fas fa-exclamation-circle text-danger" data-toggle="tooltip" title="合計工数が8h未満です"></i> ${NTS(sum)}
                              </td>`
                } else {
                    tfoot += `<td>
                                  <i class="fas fa-exclamation-circle text-warning" data-toggle="tooltip" title="合計工数が8h未満です"></i> ${NTS(sum)}
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
                                <td>${(8 - sum) != 0 ? NTS(8 - sum) : ""}</td>
                                <td></td>
                            </tr>`;
            $('#tfoot').html(tfoot);

            //set san date title
            $("#dateTitle").text(result.dateSelect + " (" + dayOfWeek(result.dateSelect) + ")");

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });
    window.history.pushState('page2', 'Title', '/ManhourInput/Day');
    changeBtActive();
    $("#btNow").text('今日');
}
function dayOfWeek(date) {
    return new Date(date).toLocaleString('ja-JP', { weekday: 'long' }).substring(0, 1);
}
//Render week table by ajax
loadWeekTable = function (dateSt) {

    if (dateSt == null) {
        dateSt = getDateTitle();
    }
    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateSt: dateSt },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

            let dayGets = new Array();
            let dateSelected = result.listDateOfWeek;

            let thead = `<tr> 
                                <th style="width:4%" ></th>
                                <th style="width:10%">テーマNo</th>
                                <th style="width:18%">テーマ名</th>
                                <th style="width:6%">内容</th>
                                <th style="width:6%">内容詳細</th>
                                <th style="width:6%">週計</th>`
            dateSelected.forEach(date => {
                thead += `<th style="width:6%">${date.substring(5, date.length)}-${dayOfWeek(date)}</th>`
                dayGets.push("day" + new Date(date).getDate())
            })

            thead += `<th style="width:4%" ></th >
                                <th style="width:4%"></th>
                            </tr>`;
            $('#thead').html(thead);

            //set tfoot for table
            let tfoot = `<tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>合計</td>
                                <td id="totalhour">0.0</td>`
            dayGets.forEach(date => {
                tfoot += `<td></td>`
            });
            tfoot += `<td></td>
                                <td></td>
                                </tr>

                                <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>不足工数</td>
                                <td></td>`
            dayGets.forEach(date => {
                tfoot += `<td></td>`
            });
            tfoot += `<td></td>
                                 <td></td>
                            </tr>`;
            $('#tfoot').html(tfoot);
            // set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {
                tbody += `<tr>
                                    <td>
                                        <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                                    </div>
                                    </td>
                                    <td>${data.theme_no}</td>
                                    <td>${data.theme_name1}</td>                                     
                                    <td>${data.work_contents_code}:${data.work_contents_class_name}</td>
                                    <td>${data.work_contents_code_name}</td>
                                    <td>${data.total}</td>`
                dayGets.forEach(date => {
                    tbody += `<td><input type="text" value="${NTS(data[date])}" class="form-control table-big-input"></td>`
                })

                tbody +=           `<td>
                                        <button class="btn btn-sm btn-outline-secondary mr-2" onclick="changeTheme(this)">
                                            <i class="fas fa-exchange-alt"></i> テーマ変更</button></td>
                                    </td>
                                    <td>
                                        <button class="btn btn-sm btn-outline-secondary mr-2" onclick="deleteTheme(this)">
                                        <i class="far fa-trash-alt"></i> 削除</button>
                                    </td>
                            </tr>`;
            });

            $('#tbody').html(tbody);
            let fromDate = dateSelected[0];
            let toDate = new Date(dateSelected[dateSelected.length - 1]).getDate();
            window.history.pushState('page2', 'Title', '/ManhourInput/Week');
            changeBtActive();
            $("#dateTitle").text(fromDate + '-' + toDate);
            $("#btNow").text('今週');
        }

    });


}

//Render month table
loadMonthTable = function (dateSt) {

    if (dateSt == null) {
        dateSt = $('#dateTitle').text().trim().substring(0, 10);
    }

    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateSt: dateSt },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {

            //Set thead for table
            let thead = `tr>
                                <th></th>
                                <th>テーマNo</th>
                                <th>テーマ名</th>
                                <th>内容</th>
                                <th>月計</th>

                                <th>1</th><th>2</th><th>3</th><th>4</th><th>5</th>
                                <th>6</th><th>7</th><th>8</th><th>9</th><th>10</th>
                                <th>11</th><th>12</th><th>13</th><th>14</th><th>15</th>
                                <th>16</th><th>17</th><th>18</th><th>19</th><th>20</th>
                                <th>21</th><th>22</th><th>23</th><th>24</th><th>25</th>
                                <th>26</th><th>27</th><th>28</th><th>29</th><th>30</th>
                                <th>31</th>

                                <th colspan="2">操作</th>
                          </tr>`;
            $('#thead').html(thead);

            // Set tfoot for table 

            //Set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {
                tbody += `<tr>
                                <td>
                                    <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                                <td>${data.theme_no}</td>
                                <td>${data.theme_name1}</td>
                                <td>${data.work_contents_code}:${data.work_contents_class_name}</td>
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
                                <input type="hidden" class="pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="total" name="Total" value="${data.total}" />         
                                <td>
                                    <input type="text" value="${NTS(data.day1)}" class="form-control table-input day1"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day2)}" class="form-control table-input day2"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day3)}" class="form-control table-input day3"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day4)}" class="form-control table-input day4"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day5)}" class="form-control table-input day5"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day6)}" class="form-control table-input day6"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day7)}" class="form-control table-input day7"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day8)}" class="form-control table-input day8"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day9)}" class="form-control table-input day9"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day10)}" class="form-control table-input day10"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day11)}" class="form-control table-input day11"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day12)}" class="form-control table-input day12"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day13)}" class="form-control table-input day13"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day14)}" class="form-control table-input day14"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day15)}" class="form-control table-input day15"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day16)}.0" class="form-control table-input day16"></td>
                                <td>
                                    <input type="text"  value="${NTS(data.day17)}" class="form-control table-input day17"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day18)}" class="form-control table-input day18"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day19)}" class="form-control table-input day19"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day20)}" class="form-control table-input day20"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day21)}" class="form-control table-input day21"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day22)}" class="form-control table-input day22"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day23)}" class="form-control table-input day23"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day24)}" class="form-control table-input day24"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day25)}" class="form-control table-input day25"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day26)}" class="form-control table-input day26"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day27)}" class="form-control table-input day27"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day28)}" class="form-control table-input day28"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day29)}" class="form-control table-input day29"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day30)}" class="form-control table-input day30"></td>
                                <td>
                                    <input type="text" value="${NTS(data.day31)}" class="form-control table-input day31">
                                </td>
                                <td>
                                    <div class="text-center"><i class="fas fa-exchange-alt" ></i></div></td>
                                <td>
                                    <div class="text-center"><i class="far fa-trash-alt"></i></div></td>
                              </tr>`;
            })

            $('#tbody').html(tbody);
            // sum by column
            let sumArr = [];
            for (i = 1; i < 32; i++) {
                sumArr.push($('.m' + i).sum());
            }

            // sub by column
            let subArr = [];
            for (i = 1; i < 32; i++) {
                subArr.push(8 - $('.m' + i).sum());
            }
            let tfoot = `<tr>
                                <td></td>
                                <td></td>
                                <td>合計</td>
                                <td></td>
                                <td>${NTS($('#total').sum())}</td>`

            sumArr.forEach(item => { tfoot += `<td>${NTS(item)}</td>` });

            tfoot += `<td></td>
                                 <td></td>
                                 </tr>
                                 <tr>
                                 <td></td>
                                 <td></td>
                                 <td>残工数</td>
                                 <td></td>
                                 <td></td>`

            subArr.forEach(item => { tfoot += `<td> ${NTS(item) != 0 ? NTS(item) : ''}</td >` });

            tfoot += `<td></td>
                      <td></td>
                      </tr>`;

            $('#tfoot').html(tfoot)
        }

    });
    //Change layout information and animation
    window.history.pushState('page2', 'Title', '/ManhourInput/Month');
    changeBtActive();

    let dateTitle = new Date(getDateTitle());
    $("#dateTitle").text(getDateTitle() + "-" + daysInMonth(dateTitle.getFullYear(), dateTitle.getMonth() - 1));
    $("#btNow").text('今月');
}

function daysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}

//ajax post function
function ajaxPost(url, sendData) {

    // Ajax処理実行
    var result = $.ajax({
        url: url,
        data: sendData,
        type: "GET",
        contentType: "application/json",
        dataType: "json"
    });

    return result;
}



