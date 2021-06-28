

// get user
$("#groups").change(function users() {
    var group = document.getElementById('groups').value;
    console.log(group);
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
    var date = document.getElementById('month').value;
    var group = document.getElementById('groups');
    var user = document.getElementById('users');
    var dates = date.split('/');
    obj.Year = dates[0].toString();
    obj.Month = dates[1].toString();
    obj.Group = group.value;
    obj.User = user.value;
    $.ajax({
        url: "/ManhourUpdate/Search",
        method: 'Post',
        data: obj,
        success: function (result) {
            let tbody = "";
            let tfoot = "";
            const header = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
            // add
            let thead = `
                            <tr>
                                <th></th>
                                <th>テーマNo</th>
                                <th>テーマ名</th>
                                <th>内容</th>
                                <th>月計</th>`
            for (var i of header) {
                thead += `<th>${i}</th>`
            }
            thead += `<th colspan="2">操作</th>
                    </tr>`
            $('.table-striped > #thead').append(thead);
            result.data.models.forEach(data => {
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
            });
            $('#tbody').append(tbody);
             // daily total calculation
            var tmp = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            result.data.models.forEach(data => {
                tmp[0] += data.total;
                tmp[1] += data.day1;
                tmp[2] += data.day2;
                tmp[3] += data.day3;
                tmp[4] += data.day4;
                tmp[5] += data.day5;
                tmp[6] += data.day6;
                tmp[7] += data.day7;
                tmp[8] += data.day8;
                tmp[9] += data.day9;
                tmp[10] += data.day10;
                tmp[11] += data.day11;
                tmp[12] += data.day12;
                tmp[13] += data.day13;
                tmp[14] += data.day14;
                tmp[15] += data.day15;
                tmp[16] += data.day16;
                tmp[17] += data.day17;
                tmp[18] += data.day18;
                tmp[19] += data.day19;
                tmp[20] += data.day20;
                tmp[21] += data.day21;
                tmp[22] += data.day22;
                tmp[23] += data.day23;
                tmp[24] += data.day24;
                tmp[25] += data.day25;
                tmp[26] += data.day26;
                tmp[27] += data.day27;
                tmp[28] += data.day28;
                tmp[29] += data.day29;
                tmp[30] += data.day30;
                tmp[31] += data.day31;
            })
            tfoot = `
                            <tr>
                                <td></td>
                                <td></td>
                                <td>合計</td>
                                <td></td>`
         
            
            for (var item of tmp) {
                tfoot += ` <td>${item}</td>`
            }
            tfoot +=` </tr>                          
                        `
            $('#tfoot').append(tfoot);
            console.log(result.data.holiday);
            result.data.holiday.forEach(data => {               
               $(`#tbody td:nth-child(${data + 16})`).css("background-color", "#f5c6cb");
                $(`#tfoot td:nth-child(${data + 5})`).css("background-color", "#f5c6cb");
            })
            var start = new Date();
            // colo today
            $(`#tbody td:nth-child(${start.getDate() + 16})`).css("background-color", "#bee5eb");
            $(`#tfoot td:nth-child(${start.getDate() + 5})`).css("background-color", "#bee5eb");
        }
    });
}
// add .0
function NTS(num) {
    if (num % 1 === 0)
        return num.toFixed(1);
    else
        return num.toString();
}

$('#ExportCsv').click(function () {
    var _group = $('#groups').val();
    var _user = $('#users').val();
    var url = "/ManhourUpdate/ExportCSV?user=" + _user + "&group=" + _group;
    $(location).attr('href', url);
});

$("#fileCSVimport").on('change', function () {
    var files = $('#fileCSVimport').prop("files");
    var url = "/ManhourUpdate/ImportCSV";
    formData = new FormData();
    formData.append("file", files[0]);

    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,     
        contentType: false,
        processData: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        success: function (result) {
            let tbody = '';
            result.data.forEach(data => {               
               
                tbody += `<tr>
                                <td><div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                                <td>${data.theme_no}</td>
                                <td>${data.theme_name1}</td>
                                <td>${data.work_contents_code}</td>
                                <td>${data.total}</td>
                                <td><input type="text" value="${NTS(data.day1)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day2)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day3)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day4)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day5)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day6)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day7)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day8)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day9)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day10)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day11)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day12)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day13)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day14)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day15)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day16)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day17)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day18)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day19)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day20)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day21)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day22)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day23)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day24)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day25)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day26)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day27)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day28)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day29)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day30)}" class="form-control table-input"></td>
                                <td><input type="text" value="${NTS(data.day31)}" class="form-control table-input"></td>
                                <td><div class="text-center"><i class="fas fa-exchange-alt"></i></div></td>
                                <td><div class="text-center"><i class="far fa-trash-alt"></i></div></td>
                              </tr>`
            });
            $('.table-striped').html(tbody);
        },
        error: function () {
            alert("Error occurs");
        }
    });
}); 
//save information into DB
$("#btSave").on("click",
    function () {

        //let dayGet = 'day' + new Date(getDateTitle()).getDate();
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
            //obj.fix_date = formatDate(new Date()).split('/').join([]);

            //get date value for obj
            for (let i = 1; i < 32; i++) {
                obj[`day${i}`] = parseFloat($(this).find(`.day${i}`).val());
            }

            //if ($(this).find(".InputHour input").val()) {
            //    obj[dayGet] = parseFloat($(this).find(".InputHour input").val());
            //}

            // Add to list data
            listData.push(obj);
        });
        console.log(listData);
        $.ajax({
            url: "/ManhourUpdate/Save",
            data: JSON.stringify(listData),
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
        console.log("thuan");
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
        $("#theme").val(theme);
    });
/* Handle select theme evet*/
$("#addTheme").on("click",
    function () {
        let workcontent = $('#workContentCode :selected').text()
        let workContentCode = $('#workContentCode :selected').val();
        let workContentDetail = $(detailCode).val();

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
        let dateTitle = new Date(getDateTitle());
        rowAdd += `<tr>
                        <td>
                            <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                        <td>${themeName}</td>
                        <td>${themeName}</td>
                        <td>${workContentCode}</td>
                        <td id="total">${data.total}</td>
                        <input type="hidden" class="Year"   name="Year" value="${dateTitle.getFullYear()}" />
                        <input type="hidden" class="Month"  name="Month" value="${dateTitle.getMonth()}" />
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
                            <input type="text" value="0.0" class="form-control table-input day1"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day2"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day3"></td>
                        <td>
                            <input type="text"  value="0.0" class="form-control table-input day4"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day5"></td>
                        <td>
                            <input type="text"  value="0.0" class="form-control table-input day6"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day7"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day8"></td>
                        <td>
                            <input type="text"  value="0.0" class="form-control table-input day9"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day10"></td>
                        <td>
                            <input type="text"  value="0.0" class="form-control table-input day11"></td>
                        <td>                           0.0
                            <input type="text"  value="0.0" class="form-control table-input day12"></td>
                        <td>                           0.0
                            <input type="text"  value="0.0" class="form-control table-input day13"></td>
                        <td>                           0.0
                            <input type="text"  value="0.0" class="form-control table-input day14"></td>
                        <td>                           0.0
                            <input type="text"  value="0.0" class="form-control table-input day15"></td>
                        <td>                           0.0
                            <input type="text"  value="0.0" class="form-control table-input day16"></td>
                        <td>
                            <input type="text"  value="0.0" class="form-control table-input day17"></td>
                        <td>
                            <input type="text" value="0.0" class="form-control table-input day18"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day19"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day20"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day21"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day22"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day23"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day24"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day25"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day26"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day27"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day28"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day29"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day30"></td>
                        <td>                          0.0
                            <input type="text" value="0.0" class="form-control table-input day31">
                        </td>
                        <td>
                            <div class="text-center"><i class="fas fa-exchange-alt" ></i></div></td>
                        <td>
                            <div class="text-center"><i class="far fa-trash-alt"></i></div></td>
                        </tr>`;
        $('#tbody').append(rowAdd);

        //set theme information to null
        themeNo = null; themeName = null; workContentClass = null;
    });