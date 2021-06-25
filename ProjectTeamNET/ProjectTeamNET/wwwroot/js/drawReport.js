var localStorage1 = JSON.parse(sessionStorage.getItem('drawObject'));
var thead = "";
var numberDay = ((new Date(localStorage1[0].toDate)).getTime() - (new Date(localStorage1[0].fromDate)).getTime()) / (1000 * 3600 * 24);

var fromDate = new Date(localStorage1[0].fromDate);
var toDate = new Date(localStorage1[0].toDate);
var columnarr = localStorage1[0].column.split(",");

//add header (month and day)
$.each(columnarr, function (i, selectedItem) {
    if (selectedItem != "monthlytotal" && selectedItem != "dailytotal" && selectedItem !="overalltotal") {
        if (selectedItem == "theme") {
            thead += `<th>ThemeNo</th><th>ThemName</th>`
        }
        else if (selectedItem == "user") {
            thead += `<th>${selectedItem}</th><th>UserName</th>`
        }
        else if (selectedItem == "affiliation") {
            thead += `<th>GroupCode</th><th>GroupName</th>`
        }
        else if (selectedItem == "workcontent") {
            thead += `<th>WorkContentCodeName</th><th>WorkContentCode</th>`
        }
        else
            thead += `<th>${selectedItem}</th>`
    }
    if (numberDay <= 31) {
        if (selectedItem == "monthlytotal") {
            if (fromDate.getMonth() == toDate.getMonth()) {
                thead += `<th>thang${fromDate.getMonth()+1}/${fromDate.getFullYear()}</th>`
            }
            else if (fromDate.getFullYear() == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>thang${i}/${fromDate.getFullYear()}</th>`
            }
        }
        if (selectedItem == "dailytotal") {
            if (fromDate.getMonth() == toDate.getMonth()) {
                for (let i = fromDate.getDate(); i <= toDate.getDate(); i++)
                    thead += `<th>${i}/${fromDate.getMonth() + 1}</th>`
            }
            else {
                for (let i = fromDate.getDate(); i <= new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, 0).getDate(); i++)
                    thead += `<th>${i}/${fromDate.getMonth() + 1}</th>`

                for (let i = 1; i <= toDate.getDate(); i++)
                    thead += `<th>${i}/${toDate.getMonth() + 1}</th>`
            }
        }
    }
    else {
        if (selectedItem == "monthlytotal") {
            if (fromDate.getFullYear() == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>thang${i}/${fromDate.getFullYear()}</th>`
            }
            else if (fromDate.getFullYear() + 1 == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= 12; i++)
                    thead += `<th>thang${i}/${fromDate.getFullYear()}</th>`
                for (let i = 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>thang${i}/${toDate.getFullYear()}</th>`
            }
            else {
                for (let i = fromDate.getMonth() + 1; i <= 12; i++)
                    thead += `<th>thang${i}/${fromDate.getFullYear()}</th>`
                var year = fromDate.getFullYear()+1;
                while (year < toDate.getFullYear()) {
                    for (let i = 1; i <= 12; i++) {
                        thead += `<th>thang${i}/${tmp}</th>`
                    }
                    year++;
                }
                for (let i = 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>thang${i}/${toDate.getFullYear()}</th>`
            }
        }
    }
})
var tbody = "";
var total = "<td>Tong</td>";
var numbertd = 0;

//add content
$.each(localStorage1, function (i, manhourScr) {
    numbertd=-1
    var content = "";
    $.each(columnarr, function (j, selectedItem) {
        if (selectedItem != "monthlytotal" && selectedItem != "dailytotal") {
            if (selectedItem == "theme") {
                content += `<td>${manhourScr.themeCode}</td><td>${manhourScr.themeName}</td>`
                numbertd += 2
            }
            else if (selectedItem == "user") {
                content += `<td>${manhourScr.userCode}</td><td>${manhourScr.userName}</td>`
                numbertd += 2
            }
            else if (selectedItem == "detailworkcontent") {
                content += `<td>${manhourScr.workContentDetail}</td>`
                numbertd ++
            }
            else if (selectedItem == "workcontent") {
                content += `<td>${manhourScr.workContentCodeName}</td><td>${manhourScr.workContentCode}</td>`
                numbertd += 2
            }
            else if (selectedItem == "affiliation") {
                content += `<td>${manhourScr.groupCode}</td><td>${manhourScr.groupName}</td>`;
                numbertd += 2
            }
        }
        if (numberDay <= 31) {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < manhourScr.monthly.length; i++) {
                    content += `<td>${manhourScr.monthly[i]}</td>`
                }
            }
            if (selectedItem == "dailytotal") {
                for (let i = 0; i < manhourScr.daily.length; i++) {
                    content += `<td>${manhourScr.daily[i]}</td>`
                }
            }
        }
        else {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < manhourScr.monthly.length; i++) {
                    content += `<td>${manhourScr.monthly[i]}</td>`
                }
            }
        }
    })
    tbody += `<tr>${content}</tr>`;
})

//add total
for (let i = 0; i < numbertd; i++) {

    total+="<td></td>"
}
if (localStorage1[0].total == 1) {
    $.each(columnarr, function (j, selectedItem) {
        if (numberDay <= 31) {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < localStorage1[0].monthly.length; i++) {
                    var month_total = 0;
                    $.each(localStorage1, function (index, value) {
                        month_total += value.monthly[index];
                    })
                    total += `<td>${month_total}</td>`
                }
            }
            if (selectedItem == "dailytotal") {
                for (let i = 0; i < localStorage1[0].daily.length; i++) {
                    var day_total = 0;
                    $.each(localStorage1, function (index, value) {
                        console.log(value.daily[i]);
                        day_total += value.daily[i];
                    })
                    total += `<td>${day_total}</td>`
                }
            }
        }
        else {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < localStorage1[0].monthly.length; i++) {
                    var month_total = 0;
                    $.each(localStorage1, function (i, value) {
                        month_total += value.monthly[i];
                    })
                    total += `<td>${month_total}</td>`
                }
            }
        }
    })
    tbody += `<tr>${total}</tr>`;
}

$('#draw').append(`
<table class="table table-striped table-sm">
    <thead class="thead-light">
        <tr>
            
            ${thead}
        </tr>
    </thead>
    <tbody>
        ${tbody}
    </tbody>
</table>`);