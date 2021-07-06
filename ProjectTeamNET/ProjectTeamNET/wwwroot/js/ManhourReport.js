var check = false;
var countUser = 0;
var GroupName = "";
var up = ['user', 'theme', 'workcontent', 'detailworkcontent', 'affiliation'];
var down = ['monthlytotal', 'dailytotal', 'overalltotal'];
var countGroup = 1;
var count = 1;
$.ajax({
    url: `/ManhourReport/GetsGroupName`,
    method: 'Get',
    success: function (response) {
        $.each(response, function (i, v) {
            GroupName += `<option value="${v.group_code}">${v.groupName}</option>`
        })
    }
});

$('#GroupId1').on("change", function () {
    if ($('#GroupId1').val() != "") {
        $('#UserAdd1').removeAttr("hidden");
    }
})
//init get screenUserItem by userScreenName last action
if ($('#userScreenName > option').length > 1) {
    var idUserScr = [];
    var userScrSelect = $('#userScreenName > option')[1].value;
    var userScr = userScrSelect.substring(0, userScrSelect.length - 14);
    $.each($('#userScreenName > option'), function (index, value) {
        if(index != 0)
            idUserScr.push(parseInt(value.value.substring(value.value.length - 14, value.value.length)))
    });
    $('#userScreenName').val(userScr + Math.max(...idUserScr));
    changeCall();
}

function deleteUserScreenName() {
    var saveName = $('#userScreenName').val();
    var r = confirm('削除します。よろしいですか？');
    if (r == true) {
        $.ajax({
            url: `/ManhourReport/DeleteScreenUser/` + saveName,
            method: 'Delete',
            success: function (response) {
                toastr.success(response)
                GetScreen();
                $('#SaveName').val("");
                reset();
            }
        });
    }
}

//add 1 User follow GroupCode
function addUser(id) {
    var GroupCode = $(`#GroupId${id}`).val();
    console.log(`#GroupId${id}`)
    $.ajax({
        url: `/ManhourReport/GetsUserName/${GroupCode}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            countUser++;
            $.each(response, function (i, v) {
                options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
            });
            $(`#addUserinGroup${id}`).append(`<div class="UserCount${id}" style="margin-bottom: 15px; height: 30px" id="addUser${countUser}"><select class="form-control form-control-sm" id="selectUser${countUser}">
                                        ${options}
                                        </select></div>
                                    `);
            $(`#trashUser${id}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${countUser}del"><i class="far fa-trash-alt" onclick="delUser(addUser${countUser},${id})"></i></div>
                                        `);
        }
    });
    if ($(`.UserCount${id}`).length >= 9) {
        $(`#UserAdd${id}`).attr("hidden", true);
    }
}

//Delete 1 User
function delUser(GroupCode, id) {
    if ($(`.UserCount${id}`).length <= 10) {
        $(`#UserAdd${id}`).removeAttr("hidden");
    }
    $(`#${GroupCode.id}`).remove();
    $(`#${GroupCode.id}del`).remove();
}
function delUserFirstGroup(GroupCode) {
    $(`#${GroupCode.id}`).remove();
    $(`#${GroupCode.id}del`).remove();
}

function addGroup() {
    countGroup++;
    $('#AddGroup').append(`
        <div class="form-group row addGroup" id="Group${countGroup}">
            <label class="col-form-label col-md-2 text-right"></label>
            <div class="col-md-10">
                <div class="row align-items-center" id="addFirst">
                    <div class="col-md-4 input-group pl-0" style="margin-bottom: auto;">
                        <select class="form-control form-control-sm" id="GroupId${countGroup}">
                            ${GroupName}
                        </select>
                        <div class="col-md-1"><i class="far fa-trash-alt" onclick="deleteGroup('Group${countGroup}')"></i></div>
                    </div>
                    <div class="col-md-3 input-group pl-0">
                        <div class="countUser" id="addUserinGroup${countGroup}" style="width: 100%"></div>
                        <div class="input-group pl-0 UserAdd" id="UserAdd${countGroup}">
                            <button class="btn btn-sm btn-outline-secondary mr-2" data-toggle="collapse" data-target="#collapseOne2" onclick="addUser(${countGroup})"><i class="fas fa-plus"></i> ユーザ追加</button>
                        </div>
                    </div>
                    <div class="col-md-4 pl-0" id="firstUser28002000000" style="margin-top:-15px">
                        <div id="trashUser${countGroup}"></div>
                    </div>
                </div>
            </div>
        </div>`);
    if ($('.addGroup').length >= 9) {
        $('#addGroup').attr("hidden", true);
    }
}

function deleteGroup(id) {
    $(`#${id}`).remove();
    if ($('.addGroup').length < 9) {
        $('#addGroup').removeAttr('hidden');
    }
}

async function addTheme() {
    var theme = "";
    await $.ajax({
        url: `/ManhourReport/GetsThemeName`,
        method: 'Get',
        success: function (response) {
            count++;
            $.each(response, function (i, v) {
                theme += `<option value="${v.themeCode}">${v.theme_Name}</option>`
            });
            
            $.ajax({
                url: `/ManhourReport/AddTheme/${count}`,
                method: 'Get',
                success: function (response) { $('#AddTheme').append(response); }
            });

            $('#AddModal').append(`
                        <div class="modal fade" id="Modal_${count}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <select class="form-control form-control-sm" id="value${count}">${theme}</select>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="SaveTheme(${count})">Save changes</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                `);
        }
    });
    if ($('.themeadd').length >= 8) {
        $('#buttonAddTheme').attr("hidden", true);
    }
}

function SaveTheme(id) {
    $(`#theme_${id}`).val($(`#value${id} option:selected`).text());
    var tmp = $(`#value${id}`).val()
    $.ajax({
        url: `/ManhourReport/WorkContents/${tmp}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $(`#addWorkContent_${id}`).empty();
            $.each(response, function (i, v) {
                options += `<option value="${v.workCode}">${v.work_Content}</option>`
            });
            $(`#addWorkContent_${id}`).append(options);
        }
    })
}

function deleteTheme(id) {
    document.getElementById(`Theme_${id}`).remove();
    if ($('.themeadd').length < 9) {
        $('#buttonAddTheme').removeAttr('hidden');
    }
}

function WorkContent() {
    $.ajax({
        url: `/ManhourReport/GetsUserName/${GroupCode}`,
        method: 'Get',
        success: function (response) {
            var tmp = '';
            $.each(response, function (i, v) {
            })
        }
    })
};

//add value option from unselected to selected
function AddSelect() {
    if ($('#NotSelect option:selected').length == 1) {
        var value = $('#NotSelect').val()[0]
        var text = $(`#NotSelect option:selected`).text();
        if (up.includes(value)) {
            var check = true
            var tmp = "";
            if ($("#Select option").length != 0) {
                $("#Select option").each(function (i, v) {
                    // push 3 total down
                    if (up.includes(v.value)) {
                        tmp += (`<option value="${v.value}">${v.text}</option>`);
                    }
                    else if (down.includes(v.value)) {
                        if (check) {
                            tmp += (`<option value="${value}">${text}</option>`);
                            check = false
                        }

                        tmp += (`<option value="${v.value}">${v.text}</option>`);
                    }
                })
                $('#Select').empty();
                $('#Select').append(tmp);
            }
            else {
                $('#Select').append(`<option value="${value}">${text}</option>`);
            }
        }
        else {
            $('#Select').append(`<option value="${value}">${text}</option>`);
        }
        $(`#NotSelect option:selected`).remove()
    }
}

//add value option from unselected to selected
function RemoveSelect() {
    if ($('#Select option:selected').length==1) {
        var value = $('#Select').val()[0]
        var text = $(`#Select option:selected`).text();
        var upcount = 0
        var downcount = 0
        $("#Select option").each(function (i, v) {
            if (up.includes(v.value))
                upcount++
            else if (down.includes(v.value))
                downcount++;
        })
        if (upcount <= 1 && up.includes(value) || downcount <= 1 && down.includes(value)) {

        }
        else {
            $('#NotSelect').append(`<option value="${value}">${text}</option>`);
            $(`#Select option:selected`).remove();
        }
    }
    
}

function selectUp() {
    var select = $('#Select option');
    if (up.includes($("#Select")[0].value)) {
        var options = "";
        for (let i = 0; i < select.length - 1; i++) {
            if ($("#Select")[0].value == $("#Select option")[i + 1].value) {
                var value = $("#Select option")[i + 1].value;
                var text = $("#Select option")[i + 1].text;
                options += `<option value='${value}'>${text}</option>`;
                value = $("#Select option")[i].value;
                text = $("#Select option")[i].text;
                options += `<option value='${value}'>${text}</option>`;
                i++
            }
            else {
                var value = $("#Select option")[i].value;
                var text = $("#Select option")[i].text;
                options += `<option value='${value}'>${text}</option>`
            }
        }
        var value = $("#Select option")[select.length - 1].value;
        var text = $("#Select option")[select.length - 1].text;
        options += `<option value='${value}'>${text}</option>`
        $('#Select').empty();
        $('#Select').append(options);
    }
   
}

function selectDown() {
    var select = $('#Select option');
    if (up.includes($("#Select")[0].value)) {
        var options = "";
        var downcount = 0;
        var check = true;
        for (let i = 0; i < select.length - 1; i++) {
            if (down.includes($("#Select option")[i].value)) {
                downcount++;
            }
        }
        for (let i = 0; i < select.length - 1; i++) {
            if ($("#Select")[0].value != $("#Select option")[select.length - downcount - 2].value) {
                check = false;
                if ($("#Select")[0].value == $("#Select option")[i].value) {
                    var value = $("#Select option")[i + 1].value;
                    var text = $("#Select option")[i + 1].text;
                    options += `<option value='${value}'>${text}</option>`;
                    value = $("#Select option")[i].value;
                    text = $("#Select option")[i].text;
                    options += `<option value='${value}'>${text}</option>`;
                    i++;
                }
                else {
                    var value = $("#Select option")[i].value;
                    var text = $("#Select option")[i].text;
                    options += `<option value='${value}'>${text}</option>`
                }
            }
        }
        if (!check) {
            var value = $("#Select option")[select.length - 1].value;
            var text = $("#Select option")[select.length - 1].text;
            options += `<option value='${value}'>${text}</option>`
            $('#Select').empty();
            $('#Select').append(options);
        }
        
    }
}

// change condition 絞り込み条件 and レイアウト設定 has save in database by 呼出
async function changeCall() {
    $('#error').empty();
    var surrogatekey = $('#userScreenName').val();
    countUser = 0;
    var numberGroup = 0;
    var group_code = [];
    var number_user_group_code = [];
    var UserNames = [];
    var numberTheme = 0;
    var Themes = [];
    var workContentCode = [];
    var workContentDetail = [];
    var selectedHeaderItems = [];
    var isTotal = '';
    var typeDelimiter = '';
    var isSingleQuote = '';
    var fromDate = '';
    var toDate = '';
    var saveName = '';
        await $.ajax({
            url: `/ManhourReport/GetManhourReport/${surrogatekey}`,
            method: 'Get',
            success: function (response) {
                $.each(response, function (i, v) {
                    if (v.screen_item == "numberGroup") {
                        numberGroup = parseInt(v.screen_input);
                    }
                    else if (v.screen_item.includes('group_') && v.screen_item.length <= 8) {
                        group_code.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('group_') && v.screen_item.length > 8) {
                        UserNames.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('numberUserGroup')) {
                        number_user_group_code.push(parseInt(v.screen_input))
                    }
                    else if (v.screen_item.includes('numberTheme')) {
                        numberTheme = (parseInt(v.screen_input))
                    }
                    else if (v.screen_item.includes('themeNo_')) {
                        Themes.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('workContentCode')) {
                        workContentCode.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('workContentDetail')) {
                        workContentDetail.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('selectedHeaderItem')) {
                        selectedHeaderItems.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('isTotal')) {
                        isTotal = (v.screen_input)
                    }
                    else if (v.screen_item.includes('typeDelimiter')) {
                        typeDelimiter = (v.screen_input)
                    }
                    else if (v.screen_item.includes('isSingleQuote')) {
                        isSingleQuote = (v.screen_input)
                    }
                    else if (v.screen_item.includes('fromDate')) {
                        fromDate = (v.screen_input)
                    }
                    else if (v.screen_item.includes('toDate')) {
                        toDate = (v.screen_input)
                        saveName = (v.save_name)
                    }
                });
            }
        });
    count = 1; countGroup++;
    $('#addUserinGroup1').empty();
    $('#trashUser1').empty();
    $('#AddGroup').empty();
    $('#AddTheme').empty();
                                 
    $('#fromDate').val(fromDate) ;
    $('#toDate').val(toDate);
    $('#SaveName').val(saveName);
    $('#GroupId').val(group_code[0]);
    
    countGroup = 1;
    //get users first group
    for (let i = 0; i < number_user_group_code[0]; i++) {
        countUser++;
        await GetsUser(countGroup, group_code[0], UserNames[countUser-1], countUser);
    }
    // Get value group
    $(`#GroupId1`).val(group_code[0]);

    // add value group Gets value group in db
    if (numberGroup > 1) {
        for (let i = 1; i < numberGroup; i++) {
            addGroup();
            $(`#GroupId${i+1}`).val(group_code[i]);
            for (let j = 0; j < number_user_group_code[i]; j++) {
                countUser++;
                await GetsUser(countGroup, group_code[i], UserNames[countUser-1], countUser);
            }
        }
    }
    // add theme and Gets value theme in db
    for (let i = 0; i < numberTheme; i++) {
        if(i > 0)
            await addTheme();
        await save(i + 1, Themes[i].substring(0,10))
        $(`#theme_${i + 1}`).val(Themes[i]);
        $(`#addWorkContent_${i + 1}`).val(workContentCode[i])
        $(`#addWorkDetail_${i + 1}`).val(workContentDetail[i])
    }

    $('#Select').empty();
    $('#NotSelect').empty();
    $('#NotSelect').append(`<option value="user">ユーザ</option>
                            <option value="theme">テーマ</option>
                            <option value="workcontent">作業内容</option>
                            <option value="detailworkcontent">作業内容詳細</option>
                            <option value="affiliation">所属</option>
                            <option value="monthlytotal">月別合計</option>
                            <option value="dailytotal">日別合計</option>
                            <option value="overalltotal">全体合計</option>
                            `);
    $.each(selectedHeaderItems, function (i, v) {
        $('#NotSelect').val(v);
        AddSelect1();
    });
    if (isTotal == 0) {
        $('#isTotal1').prop('checked', false);
        $('#isTotal').prop('checked', true);
    }
    if (typeDelimiter == 0) {
        $('#typeDelimiter1').prop('checked', false);
        $('#typeDelimiter').prop('checked', true);
    }
    if (isSingleQuote == 0) {
        $('#isSingleQuote1').prop('checked', false);
        $('#isSingleQuote').prop('checked', true);
    }
}

// get user by groupcode
async function GetsUser(groupCount, group_code, UserNo, count_user) {
    await $.ajax({
        url: `/ManhourReport/GetsUserName/${group_code}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $.each(response, function (i, v) {
                options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
            });
            $(`#addUserinGroup${groupCount}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${count_user}"><select id="selectUser${countUser}" class="form-control form-control-sm">
                                        ${options}
                                        </select></div>
                                    `);
            $(`#trashUser${groupCount}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${count_user}del"><i class="far fa-trash-alt" onclick="delUser(addUser${count_user})"></i></div>
                                        `);
        }
    });
    $(`#addUser${count_user} > select`).val(UserNo);
}

// get list workcontents của 1 theme đã cho db
async function save(id, workcontent) {
    await $.ajax({
        url: `/ManhourReport/WorkContents/${workcontent}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $(`#addWorkContent_${id}`).empty();
            $.each(response, function (i, v) {
                options += `<option value="${v.workCode}">${v.work_Content}</option>`
            });
            $(`#addWorkContent_${id}`).append(options);
        }
    })
}
//+ option theo db
function AddSelect1() {
    var value = $('#NotSelect').val()[0]
    var text = $(`#NotSelect option:selected`).text();
    $('#Select').append(`<option value="${value}">${text}</option>`);
    $(`#NotSelect option:selected`).remove();
}

function checkform() {
    $('#error').empty();
    var Obj = setObj();
    //check validate or save localStorage
    $.ajax({
        url: '/ManhourReport/CheckReport',
        method: 'POST',
        data: Obj,
        success: function (response) {
            $('#savename').empty();
            $('#toDate1').empty();
            $('#fromDate1').empty();
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/CreateData',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        var Object = [];
                        if (response.messenge == "") {
                            $.each(response.data, function (i, v) {
                                var objtmp = {};
                                objtmp.groupCode = v.groupCode;
                                objtmp.groupName = v.groupName;
                                objtmp.userCode = v.userCode;
                                objtmp.userName = v.userName;
                                objtmp.themeCode = v.themeCode;
                                objtmp.themeName = v.themeName;
                                objtmp.workContentCode = v.workContentCode;
                                objtmp.workContentCodeName = v.workContentCodeName;
                                objtmp.workContentDetail = v.workContentDetail;
                                objtmp.monthly = v.monthly;
                                objtmp.daily = v.daily;
                                objtmp.fromDate = v.fromDate;
                                objtmp.toDate = v.toDate;
                                objtmp.overalltotal = v.overalltotal;
                                objtmp.column = Obj.selectedHeaderItems;
                                objtmp.total = $("input[name='isTotal']:checked").val();
                                Object.push(objtmp)
                            });
                            sessionStorage.setItem('drawObject', JSON.stringify(Object));
                            window.open('/ManhourReport/ShowReport', '_blank');
                        }
                        else {
                            toastr.warning(response.messenge);
                        }
                    }
                });
            }
            else {
                if (response.toDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.toDate}
                                        </div>`);
                }
                if (response.fromDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.fromDate}
                                        </div>`);
                }
                if (response.date != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.date}
                                        </div>`);
                }
                if (response.dateCalculate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.dateCalculate}
                                        </div>`);
                }
                if (response.theme != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.theme}
                                        </div>`);
                }
                if (response.user != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.user}
                                        </div>`);
                }
                if (response.headerItems != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.headerItems}
                                        </div>`);
                }
            }
        }
    })
}

//Get 絞り込み条件 AND レイアウト設定
function setObj() {
    $('#error').empty();
    var Obj = {} || data1;
    Obj.save = $("#SaveName").val();
    Obj.fromDate = $("#fromDate").val();
    Obj.toDate = $("#toDate").val();
    Obj.isTotal = $(`input[name=isTotal]:checked`).val();
    Obj.typeDelimiter = $(`input[name=typeDelimiter]:checked`).val();
    Obj.isSingleQuote = $(`input[name=isSingleQuote]:checked`).val();
    Obj.numberSelectedHeader = $('#Select option').length;
    Obj.selectedHeaderItems = "";
    for (let i = 0; i < $('#Select option').length; i++) {
        if (i == 0)
            Obj.selectedHeaderItems += $('#Select option')[i].value;
        else
            Obj.selectedHeaderItems += "," + $('#Select option')[i].value;
    }
    if ($("#theme_1").val() == "")
        Obj.numberTheme = 0;
    else {
        Obj.numberTheme = $('#AddTheme > div').length + 1;
    }
    Obj.themeNos = "";
    for (let i = 0; i < Obj.numberTheme; i++) {
        if (i == 0)
            Obj.themeNos += $(`#theme_${i + 1}`)[0].value;
        else
            Obj.themeNos += "," + $(`#theme_${i + 1}`)[0].value;
    }
    Obj.workContentCodes = "";
    for (let i = 0; i < $('#AddTheme > div').length + 1; i++) {
        if (i == 0)
            Obj.workContentCodes += $(`#addWorkContent_${i + 1}`)[0].value;
        else
            Obj.workContentCodes += "," + $(`#addWorkContent_${i + 1}`)[0].value;
    }
    Obj.workContentDetails = "";
    for (let i = 0; i < $('#AddTheme > div').length + 1; i++) {
        if (i == 0)
            Obj.workContentDetails += $(`#addWorkDetail_${i + 1}`)[0].value;
        else
            Obj.workContentDetails += "," + $(`#addWorkDetail_${i + 1}`)[0].value;
    }
    Obj.numberGroup = countGroup;
    Obj.Groups = "";
    Obj.Users = "";
    Obj.numberUser = "";
    var countGetUser = 0;
    for (let i = 0; i < $('select').length; i++) {
        if ($('select')[i].id.includes("GroupId"))
            Obj.Groups += "," + $('select')[i].value;

        else if ($('select')[i].id.includes("selectUser")) {
            Obj.Users += "," + $('select')[i].value;
        }
    }
    for (let i = 0; i < $('.countUser').length; i++) {
        Obj.numberUser += "," + (($('.countUser')[i].innerHTML).match(/<select/g) || []).length;
    }
    return Obj;
}

function outputCSV() {
    $('#error').empty();
    var Obj = setObj();
    $.ajax({
        url: '/ManhourReport/CheckReport',
        method: 'POST',
        data: Obj,
        success: function (response) {
            $('#savename').empty();
            $('#toDate1').empty();
            $('#fromDate1').empty();
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/GetDataCSV',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        if (response.messenge == "") {
                            const data = encodeURI('data:text/csv;charset=utf-8,' + response.data);
                            const link = document.createElement('a');
                            link.setAttribute('href', data);
                            link.setAttribute('download', response.fileName);
                            link.click();
                        }
                        else
                            $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.messenge}
                                        </div>`);
                    }
                });
            }
            else {
                if (response.toDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.toDate}
                                        </div>`);
                }
                if (response.fromDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.fromDate}
                                        </div>`);
                }
                if (response.date != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.date}
                                        </div>`);
                }
                if (response.dateCalculate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.dateCalculate}
                                        </div>`);
                }
                if (response.theme != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.theme}
                                        </div>`);
                }
                if (response.user != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.user}
                                        </div>`);
                }
                if (response.headerItems != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.headerItems}
                                        </div>`);
                }
            }
        }
    })
}

// set default
function reset() {
    $('#error').empty();
    $('#fromDate').val(null);
    $('#toDate').val(null);
    $('#addUserinGroup1').empty();
    $('#trashUser1').empty();
    $('#AddGroup').empty();
    $('#AddTheme').empty();
    $('#addWorkDetail_1').val(null);
    $('#addWorkContent_1').empty();
    $('#theme_1').val(null);
    $('#Select').empty();
    $('#NotSelect').empty();
    $('#NotSelect').append(`<option value="user">ユーザ</option>
                            <option value="theme">テーマ</option>
                            <option value="workcontent">作業内容</option>
                            <option value="detailworkcontent">作業内容詳細</option>
                            <option value="monthlytotal">月別合計</option>
                            <option value="dailytotal">日別合計</option>
                            `);
    $('#Select').append(`<option value="affiliation">所属</option>
                            <option value="overalltotal">全体合計</option>
                            `);
}

function saveUserScreenName() {
    $('#error').empty();
    var Obj = setObj();
    $.ajax({
        url: '/ManhourReport/CheckSave',
        method: 'POST',
        data: Obj,
        success: function (response) {
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/SaveScreen',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        if (response != "error") {
                            $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response}
                                        </div>`);
                            GetScreen();
                        }
                    }
                });
            }
            else {
                if (response.toDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.toDate}
                                        </div>`);
                }
                if (response.fromDate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.fromDate}
                                        </div>`);
                }
                if (response.date != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.date}
                                        </div>`);
                }
                if (response.dateCalculate != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.dateCalculate}
                                        </div>`);
                }
                if (response.theme != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.theme}
                                        </div>`);
                }
                if (response.user != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.user}
                                        </div>`);
                }
                if (response.savename != "") {
                    $("#error").append(`<div class="alert alert-warning mb-2" role="alert">
                                            <strong>アラート</strong> - ${response.savename}
                                        </div>`);
                }
            }
        }
    });
}

$('#datepicker1').datepicker({ autoclose: true, language: 'ja' }).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    $('#fromDate').val(`${date.getFullYear()}/${date.getMonth() >= 9 ? date.getMonth() + 1 : "0" + (date.getMonth()+1)}/${date.getDate() >= 10 ? date.getDate() : "0" + date.getDate()}`);
  });
$('#datepicker2').datepicker({ format: "yyyy/mm/dd", language: "ja", autoclose: true, orientation: 'bottom left' }).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    $('#toDate').val(`${date.getFullYear()}/${date.getMonth() >= 9 ? date.getMonth() + 1 : "0" + (date.getMonth() + 1)}/${date.getDate() >= 10 ? date.getDate(): "0" + date.getDate()}`);
});

function GetScreen() {
    $.ajax({
        url: '/ManhourReport/GetsScreen',
        method: 'Get',
        success: function (response) {
            $('#userScreenName').empty();
            var options = "";
            $.each(response, function (i, v) {
                options += `<option value="${v.value}">${v.text}</option>`
            });
            $('#userScreenName').append(options);
        }
    });
}