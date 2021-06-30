﻿var check = false;
var countUser = 1;
var GroupName = "";
var up = ['user', 'theme', 'workcontent', 'detailworkcontent', 'affiliation'];
var down = ['monthlytotal', 'dailytotal', 'overalltotal'];

function deleteUserScreenName() {
    var tmp = $('#userScreenName').val();
    $.ajax({
        url: `/ManhourReport/DeleteScreenUser/` + tmp,
        method: 'Delete',
        success: function (response) {
            $('#userScreenName').empty();
            $('#userScreenName').append(`<option>保存名...</option>`);
            $.each(response, function (i, v) {
                $('#userScreenName').append(`<option>${v.Save_name}</option>`);
            });
        }
    });
}

//add User follow GroupCode of first Group (Group init)
function addUserFirstGroup() {
    var GroupCode = $("#GroupId").val();
    $.ajax({
        url: `/ManhourReport/GetsUserName/${GroupCode}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $.each(response, function (i, v) {
                options += (`<option>${v.userCode}[${v.user_Name}]</option>`);
            }
            );
            $('#addUser').append(`<div style="margin-bottom: 15px; height: 30px" id="setfirst${countUser}"><select class="form-control form-control-sm">
                                        ${options}
                                        </select></div>
                                        `);

            $('#trashUser').append(`<div style="margin-bottom: 15px; height: 30px" id="setfirst${countUser}del"><i class="far fa-trash-alt" onclick="delUserFirstGroup(setfirst${countUser})"></i></div>
                                        `);
        }
    })
}

//add 1 User follow GroupCode
function addUser(id) {
    var GroupCode = $(`#GroupId${id}`).val();
    $.ajax({
        url: `/ManhourReport/GetsUserName/${GroupCode}`,
        method: 'Get',
        success: function (response) {
            var options = '';

            $.each(response, function (i, v) {
                options += (`<option>${v.userCode}[${v.user_Name}]</option>`);
            });
            $(`#addUserinGroup${id}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${countUser}"><select class="form-control form-control-sm">
                                        ${options}
                                        </select></div>
                                    `);
            $(`#trashUser${id}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${countUser}del"><i class="far fa-trash-alt" onclick="delUser(addUser${countUser})"></i></div>
                                        `);
        }
    });
    countUser++;
}

var countGroup = 1;

//Delete 1 User
function delUser(GroupCode) {
    console.log(GroupCode)
    $(`#${GroupCode.id}`).remove();
    $(`#${GroupCode.id}del`).remove();
}
function delUserFirstGroup(GroupCode) {
    $(`#${GroupCode.id}`).remove();
    $(`#${GroupCode.id}del`).remove();
}

$.ajax({
    url: `/ManhourReport/GetsGroupName`,
    method: 'Get',
    success: function (response) {
        $.each(response, function (i, v) {
            GroupName += `<option value="${v.group_code}">${v.groupName}</option>`
        })
    }
});

function addGroup() {
    $('#AddGroup').append(`

        <div class="form-group row" id="Group${countGroup}">
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
                        <div id="addUserinGroup${countGroup}"></div>
                        <div class="input-group pl-0">
                            <button class="btn btn-sm btn-outline-secondary mr-2" data-toggle="collapse" data-target="#collapseOne2" onclick="addUser(${countGroup})"><i class="fas fa-plus"></i> ユーザ追加</button>
                        </div>
                    </div>
                    <div class="col-md-4 pl-0" id="firstUser28002000000" style="margin-top:-15px">
                        <div id="trashUser${countGroup}"></div>
                    </div>
                </div>
            </div>
        </div>`);

    countGroup++;
}

function deleteGroup(id) {
    $(`#${id}`).remove();
}

var count = 1;
async function addTheme() {
    var theme = "";
    await $.ajax({
        url: `/ManhourReport/GetsThemeName`,
        method: 'Get',
        success: function (response) {
            $.each(response, function (i, v) {
                theme += `<option value="${v.themeCode}">${v.theme_Name}</option>`
            });
            
            $.ajax({
                url: `/ManhourReport/AddTheme/${count}`,
                method: 'Get',
                success: function (response) { console.log(count); console.log(response); $('#AddTheme').append(response); }
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
            count++;
        }
    });

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

//from unselected to selected
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

//from unselected to selected
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
        $('#Select').append(tmp);
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

// 
async function changeCall() {
    var countUserGroup = 0;
    var surrogatekey = $('#userScreenName').val();
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
    count = 1;
    $('#addUser').empty()
    $('#trashUser').empty()
    $('#AddGroup').empty()
    $('#AddTheme').empty()

    $('#fromDate').val(fromDate)
    $('#toDate').val(toDate)
    $('#SaveName').val(saveName)
    $('#GroupId').val(group_code[0]);
    
    var countfirstGroupUser = 1;
    //get users first group
    for (let i = 0; i < number_user_group_code[0]; i++) {
        await GetsUserFirstGroup(group_code[0], UserNames[countUserGroup], countfirstGroupUser);
        countUserGroup++;
        countfirstGroupUser++;
    }

    countGroup = 1;
    // Get value nhiều User của từng group đã có trong db(ngoai` first group)
    if (numberGroup > 1) {
        for (let i = 1; i < numberGroup; i++) {
            addGroup();
            $(`#GroupId${i}`).val(group_code[i]);
            for (let j = 0; j < number_user_group_code[i]; j++) {
                await GetsUser(countGroup - 1, group_code[i], UserNames[countUserGroup], countUser);
                countUserGroup++;
                countUser++;
            }
                countGroup++;
        }

    }
    for (let i = 0; i < numberTheme; i++) {
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


// get user by groupcode (init Group)
async function GetsUserFirstGroup(group_code, UserNo, countfirstGroupUser) {
    await $.ajax({
        url: `/ManhourReport/GetsUserName/${group_code}`,
        method: 'Get',
        success: function (response) {
            var options = '';

            $.each(response, function (i, v) {
                options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
            });
            $(`#addUserinGroup`).append(`<div style="margin-bottom: 15px; height: 30px" id="setfirst${countfirstGroupUser}"><select class="form-control form-control-sm">
                                        ${options}
                                        </select></div>
                                    `);
            $(`#trashUser`).append(`<div style="margin-bottom: 15px; height: 30px" id="setfirst${countfirstGroupUser}del"><i class="far fa-trash-alt" onclick="delUserFirstGroup(setfirst${countfirstGroupUser})"></i></div>
                                        `);
        }
    });
    $(`#setfirst${countfirstGroupUser} > select`).val(UserNo);
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
            $(`#addUserinGroup${groupCount}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${count_user}"><select class="form-control form-control-sm">
                                        ${options}
                                        </select></div>
                                    `);
            $(`#trashUser${groupCount}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${count_user}del"><i class="far fa-trash-alt" onclick="delUser(addUser${count_user})"></i></div>
                                        `);
        }
    });
    $(`#addUser${count_user} > select`).val(UserNo);
}

// lấy user bằng groupcode
async function addUsertmp1(id, GroupCode, countUser) {
    await $.ajax({
        url: `/ManhourReport/GetsUserName/${GroupCode}`,
        method: 'Get',
        success: function (response) {
            var tmp = '';
            var tmp1 = '';
            $.each(response, function (i, v) {
                tmp += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
            }
            );
            $(`#addUser2${id}`).remove();
            $(`#addFirst${id}`).append(`<div class="col-md-3 input-group pl-0" id="firstUser${countUser1}">
                                                            <select class="form-control form-control-sm" id="setfirstUser${countUser}${GroupCode}">
                                                                    ${tmp}
                                                            </select>
                                                        </div><div class="col-md-4 input-group pl-0" id="firstUser2${countUser1}">
                                                            <i class="far fa-trash-alt" onclick="deletefirstUser(${countUser1})"></i>
                                                        </div>
                                                            `);

            $(`#btn_addUser${id}`).removeAttr('hidden');
        }
    })
    countUser1++;
}
// lấy 1 user bằng groupcode
//async function addUser1(id, GroupCode, countUser) {
//    await $.ajax({
//        url: `/ManhourReport/GetsUserName/${GroupCode}`,
//        method: 'Get',
//        success: function (response) {
//            var tmp = '';
//            $.each(response, function (i, v) {
//                tmp += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
//            }
//            );
//            $(`#addUser1${id}`).append(`<div class="form-group row" id="${countUser1}">
//                                                                <label class="col-form-label col-md-2 text-right"></label>
//                                                                <div class="col-md-10">
//                                                                    <div class="row align-items-center">
//                                                                        <div class="col-md-4 input-group pl-0">
//                                                                        </div>
//                                                                        <div class="col-md-3 input-group pl-0">
//                                                                            <select class="form-control form-control-sm" id="setUser${countUser}${GroupCode}">
//                                                                                 ${tmp}
//                                                                            </select>
//                                                                        </div><div class="col-md-4 input-group pl-0">
//                                                                            <i class="far fa-trash-alt" onclick="deleteUser(${countUser1})"></i>
//                                                                        </div>
//                                                                    </div>
//                                                                </div>
//                                                            </div>
//                                                            `);
//        }
//    })
//    countUser1++;
//}

// get list workcontents của 1 theme đã cho db
async function save(id, workcontent) {
    await $.ajax({
        url: `/ManhourReport/WorkContents/${workcontent}`,
        method: 'Get',
        success: function (response) {
            var tmp1 = '';
            $(`#addWorkContent_${id}`).empty();
            $.each(response, function (i, v) {
                tmp1 += `<option value="${v.workCode}">${v.work_Content}</option>`
            });
            $(`#addWorkContent_${id}`).append(tmp1);
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
    Obj.numberTheme = $('#AddTheme > div').length;
    Obj.themeNos = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.themeNos += $(`#theme_${i + 1}`)[0].value;
        else
            Obj.themeNos += "," + $(`#theme_${i + 1}`)[0].value;
    }
    Obj.workContentCodes = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.workContentCodes += $(`#addWorkContent_${i + 1}`)[0].value;
        else
            Obj.workContentCodes += "," + $(`#addWorkContent_${i + 1}`)[0].value;
    }
    Obj.workContentDetails = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.workContentDetails += $(`#addWorkDetail_${i + 1}`)[0].value;
        else
            Obj.workContentDetails += "," + $(`#addWorkDetail_${i + 1}`)[0].value;
    }
    Obj.numberGroup = countGroup;
    Obj.Groups = "";
    Obj.Users = "";
    Obj.numberUser = "";
    var countUser3 = 0;
    for (let i = 0; i < $('select').length; i++) {
        if ($('select')[i].id.includes("roup") && Obj.Groups == "")
            Obj.Groups += $('select')[i].value;
        else if ($('select')[i].id.includes("roup") && Obj.Groups != "")
            Obj.Groups += "," + $('select')[i].value;

        if ($('select')[i].id.includes("setfirst")) {
            if (countUser3 != 0) {
                if (Obj.numberUser == "") {
                    Obj.numberUser += countUser3;
                }
                else {
                    Obj.numberUser += "," + countUser3;
                }
                countUser3 = 0;
            }
            countUser3++;
            if (Obj.Users == "")
                Obj.Users += $('select')[i].value
            else
                Obj.Users += "," + $('select')[i].value
        }
        else if ($('select')[i].id.includes("setUser")) {
            countUser3++;
            Obj.Users += "," + $('select')[i].value;
        }
    }
    Obj.numberUser += "," + countUser3;

    //check validate or save localStorage
    $.ajax({
        url: '/ManhourReport/CheckReport',
        method: 'POST',
        data: Obj,
        success: function (response) {
            $('#savename').empty();
            $('#toDate1').empty();
            $('#fromDate1').empty();
            console.log(response)
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/CreateData',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        var Object = [];
                        $.each(response, function (i, v) {
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
                            objtmp.column = Obj.selectedHeaderItems;
                            objtmp.total = $("input[name='isTotal']:checked").val();
                            Object.push(objtmp)
                        });
                        sessionStorage.setItem('drawObject', JSON.stringify(Object));
                        window.open('/ManhourReport/ShowReport', '_blank');
                    }
                });
            }
            else {
                if (response.savename != "") {
                    $('#savename').append(`<span style="color:red">${response.savename}</span>`)
                }
                if (response.toDate != "") {
                    $('#toDate1').append(`<span style="color:red">${response.toDate}</span>`)
                }
                if (response.fromDate != "") {
                    $('#fromDate1').append(`<span style="color:red">${response.fromDate}</span>`)
                }
                if (response.Date != "") {
                    $('#fromDate1').append(`<span style="color:red">${response.Date}</span>`)
                }
                if (response.DateCalculate != "") {
                    alert(response.DateCalculate);
                }
            }
        }
    })
}

function outputCSV() {
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
    Obj.numberTheme = $('#AddTheme > div').length;
    Obj.themeNos = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.themeNos += $(`#theme_${i + 1}`)[0].value;
        else
            Obj.themeNos += "," + $(`#theme${i + 1}`)[0].value;
    }
    Obj.workContentCodes = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.workContentCodes += $(`#addWorkContent_${i + 1}`)[0].value;
        else
            Obj.workContentCodes += "," + $(`#addWorkContent_${i + 1}`)[0].value;
    }
    Obj.workContentDetails = "";
    for (let i = 0; i < $('#AddTheme > div').length; i++) {
        if (i == 0)
            Obj.workContentDetails += $(`#addWorkDetail_${i + 1}`)[0].value;
        else
            Obj.workContentDetails += "," + $(`#addWorkDetail_${i + 1}`)[0].value;
    }
    Obj.numberGroup = countGroup;
    Obj.Groups = "";
    Obj.Users = "";
    Obj.numberUser = "";
    var countUser3 = 0;
    for (let i = 0; i < $('select').length; i++) {
        if ($('select')[i].id.includes("roup") && Obj.Groups == "")
            Obj.Groups += $('select')[i].value;
        else if ($('select')[i].id.includes("roup") && Obj.Groups != "")
            Obj.Groups += "," + $('select')[i].value;

        if ($('select')[i].id.includes("setfirst")) {
            if (countUser3 != 0) {
                if (Obj.numberUser == "") {
                    Obj.numberUser += countUser3;
                }
                else {
                    Obj.numberUser += "," + countUser3;
                }
                countUser3 = 0;
            }
            countUser3++;
            if (Obj.Users == "")
                Obj.Users += $('select')[i].value
            else
                Obj.Users += "," + $('select')[i].value
        }
        else if ($('select')[i].id.includes("setUser")) {
            countUser3++;
            Obj.Users += "," + $('select')[i].value;
        }
    }
    Obj.numberUser += "," + countUser3;

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
                        console.log(response)
                        const data = encodeURI('data:text/csv;charset=utf-8,' + response.data);
                        const link = document.createElement('a');
                        link.setAttribute('href', data);
                        link.setAttribute('download', response.fileName);
                        link.click();
                    }
                });
            }
            else {
                if (response.savename != "") {
                    $('#savename').append(`<span style="color:red">${response.savename}</span>`)
                }
                if (response.toDate != "") {
                    $('#toDate1').append(`<span style="color:red">${response.toDate}</span>`)
                }
                if (response.fromDate != "") {
                    $('#fromDate1').append(`<span style="color:red">${response.fromDate}</span>`)
                }
                if (response.Date != "") {
                    $('#fromDate1').append(`<span style="color:red">${response.Date}</span>`)
                }
                if (response.DateCalculate != "") {
                    alert(response.DateCalculate);
                }
            }
        }
    })
}