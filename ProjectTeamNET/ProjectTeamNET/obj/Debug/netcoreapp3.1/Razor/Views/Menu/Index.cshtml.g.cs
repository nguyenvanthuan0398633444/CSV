#pragma checksum "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c5ba7266985b64ecfbe52b5cc03b9885033af63d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Menu_Index), @"mvc.1.0.view", @"/Views/Menu/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\_ViewImports.cshtml"
using ProjectTeamNET;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\_ViewImports.cshtml"
using ProjectTeamNET.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\_ViewImports.cshtml"
using ProjectTeamNET.Models.Request;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\_ViewImports.cshtml"
using ProjectTeamNET.Models.Response;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c5ba7266985b64ecfbe52b5cc03b9885033af63d", @"/Views/Menu/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa90e289e11958e66328bfa2213bef20d4a46cc8", @"/Views/_ViewImports.cshtml")]
    public class Views_Menu_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<MenuViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/Menu.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("h5"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "ManhourInput", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Init", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-route-id", "", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("style", new global::Microsoft.AspNetCore.Html.HtmlString("color:red"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
  
    ViewData["Title"] = "Home page";
    DateTime startDate = new DateTime(Model.processingMonth.Year, Model.processingMonth.Month, 1);
    var daysOfMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month); // get how many day from processing Month , return int
    DateTime today = DateTime.Now;
    int currentMonth = Model.processingMonth.Month; // is Processing Month not Current Month in PC 
    int currentYear = Model.processingMonth.Year; // is Processing Year not Current Year in PC 
    int startIndex = (int)startDate.DayOfWeek; // start (index) from calendar (index 0 = sunday)
    int dayNo = 0;

    // if this month has total index > 35, using this for loop
    int row = daysOfMonth + startIndex;
    if (row > 35)
    {
        row = 42;
    }
    else
    {
        row = 35;
    }

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "c5ba7266985b64ecfbe52b5cc03b9885033af63d7277", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<div class=""container-fluid"">

    <div class=""row"">
        <div id=""calendar""></div>
    </div>

    <div class=""row"">
        <nav class=""col-md-2 d-none d-md-block bg-light sidebar"">
            <div class=""sidebar-sticky"">

                <h6 class=""sidebar-heading d-flex justify-content-between align-items-center px-3 mt-2 mb-1 text-muted"">
                    <span>利用者メニュー</span>
                </h6>

                <ul class=""nav flex-column"">
                    <li class=""nav-item"">
                        <a class=""nav-link sidebar-sub active"" href=""#"">
                            <span class=""fa fa-clock""></span> 工数入力 <span class=""sr-only"">(current)</span>
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a class=""nav-link sidebar-sub"" href=""#"">
                            <span class=""fa fa-check""></span> 工数未入力チェック
                        </a>
                    </li>
                    <li cla");
            WriteLiteral(@"ss=""nav-item"">
                        <a class=""nav-link sidebar-sub"" href=""#"">
                            <span class=""fas fa-file-export""></span> 工数集計
                        </a>
                    </li>
                </ul>

                <h6 class=""sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted"">
                    <span>マスターメンテナンス</span>
                </h6>

                <ul class=""nav flex-column mb-2"">
                    <li class=""nav-item"">
                        <a class=""nav-link sidebar-sub"" href=""#"">
                            <span class=""fas fa-edit""></span> テーマ管理
                        </a>
                    </li>
                </ul>

                <h6 class=""sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted"">
                    <span>管理者メニュー</span>
                </h6>

                <ul class=""nav flex-column mb-2"">
                    <li class=""nav-item ");
            WriteLiteral(@"sidebar-sub"">
                        <a class=""nav-link"" href=""#"">
                            <span class=""fa fa-clock""></span> 工数修正
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a class=""nav-link sidebar-sub"" href=""#"">
                            <span class=""fas fa-edit""></span> 月度切替
                        </a>
                    </li>
                    <li class=""nav-item"">
                        <a class=""nav-link sidebar-sub"" href=""#"">
                            <span class=""fas fa-file-export""></span> 原価計算データ出力
                        </a>
                    </li>
                </ul>
            </div>
        </nav>

        <main role=""main"" class=""col-md-9 ml-sm-auto col-lg-10 px-4"">
");
#nullable restore
#line 93 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
             if (Model.check == false) //if not yet complete input or error
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <div class=\"alert alert-warning mb-2\" role=\"alert\">\r\n                    <strong>アラート</strong> -");
#nullable restore
#line 96 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                      Write(currentMonth);

#line default
#line hidden
#nullable disable
            WriteLiteral(" 月度の工数に未入力日があります！入力をお願いします。\r\n                </div>\r\n");
#nullable restore
#line 98 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"row p-3\">\r\n                <table class=\"table table-bordered\">\r\n                    <tbody>\r\n                        <tr>\r\n                            <td");
            BeginWriteAttribute("class", " class=\"", 4222, "\"", 4230, 0);
            EndWriteAttribute();
            WriteLiteral(" colspan=\"7\"><div class=\"h6 text-center m-0\">");
#nullable restore
#line 103 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                Write(currentYear);

#line default
#line hidden
#nullable disable
            WriteLiteral(" 年 ");
#nullable restore
#line 103 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                               Write(currentMonth);

#line default
#line hidden
#nullable disable
            WriteLiteral(@" 月</div></td>
                        </tr>
                        <tr>
                            <td style=""width:14%"">日</td>
                            <td style=""width:14%"">月</td>
                            <td style=""width:14%"">火</td>
                            <td style=""width:14%"">水</td>
                            <td style=""width:14%"">木</td>
                            <td style=""width:14%"">金</td>
                            <td style=""width:14%"">土</td>
                        </tr>

                        <tr class=""calendar-row"">
");
#nullable restore
#line 116 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                             for (var i = 0; i < row; i++)
                            {
                                if (startIndex > i)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <td><a class=\"h5\"></a></td>\r\n");
#nullable restore
#line 121 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                    continue;
                                }

                                dayNo = i - startIndex + 1;

                                if (i % 7 == 0 && i > 0)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                ");
            WriteLiteral("</tr><tr class=\"calendar-row\">\r\n");
#nullable restore
#line 129 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                }
                                if (dayNo <= daysOfMonth)
                                {   // check key Dictionary Holiday is exist or not and value isHoliday?
                                    if (Model.holidays.ContainsKey(dayNo + 1) && Model.holidays[dayNo])
                                    {
                                        //if holiday is true

#line default
#line hidden
#nullable disable
            WriteLiteral("                                        <td style=\"color: red\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d15634", async() => {
#nullable restore
#line 135 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                                                                          Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<p class=\"workHours\">0h</p></td>\r\n");
#nullable restore
#line 136 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"

                                    }
                                    else
                                    {
                                        if ((int)today.Day > (int)dayNo)
                                        {   //case workHour < 8 
                                            if (Model.totalWorkHour[dayNo - 1] > 0 && Model.totalWorkHour[dayNo - 1] < 8)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <td>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d18701", async() => {
#nullable restore
#line 144 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                              Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<span class=\"badge badge-warning\">入力エラー</span><p class=\"workHours\"><i class=\"fas fa-exclamation-circle text-warning\" data-toggle=\"tooltip\"></i>");
#nullable restore
#line 144 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                                                                                                                                                                                       Write(Model.totalWorkHour[dayNo - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("h</p></td>\r\n");
#nullable restore
#line 145 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                            }
                                            // case not yet input workHour
                                            else if (Model.totalWorkHour[dayNo - 1] == 0)
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <td>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d21597", async() => {
#nullable restore
#line 149 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                              Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<span class=\"badge badge-danger\">未入力</span><p class=\"workHours\"><i class=\"fas fa-exclamation-circle text-danger\" data-toggle=\"tooltip\"");
            BeginWriteAttribute("title", " title=\"", 7292, "\"", 7300, 0);
            EndWriteAttribute();
            WriteLiteral(" data-original-title=\"工数が未入力です\"></i>");
#nullable restore
#line 149 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                                                                                                                                                                                                                           Write(Model.totalWorkHour[dayNo - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("h</p></td>\r\n");
#nullable restore
#line 150 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                            }
                                            //case no problem
                                            else
                                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                                <td>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d24646", async() => {
#nullable restore
#line 154 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                              Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<p class=\"workHours\">");
#nullable restore
#line 154 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                                                             Write(Model.totalWorkHour[dayNo - 1]);

#line default
#line hidden
#nullable disable
            WriteLiteral("h</p></td>\r\n");
#nullable restore
#line 155 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                            }
                                        }
                                        else if ((int)today.Day == (int)dayNo)
                                        {   // is Today

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <td style=\"background-color: lightblue;\">");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d27293", async() => {
#nullable restore
#line 159 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                                                               Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<p class=\"workHours\">0h</p></td>\r\n");
#nullable restore
#line 160 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                        }
                                        else
                                        {  

#line default
#line hidden
#nullable disable
            WriteLiteral("                                            <td>");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c5ba7266985b64ecfbe52b5cc03b9885033af63d29454", async() => {
#nullable restore
#line 163 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                                                                                          Write(dayNo);

#line default
#line hidden
#nullable disable
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("<p class=\"workHours\">0h</p></td>\r\n");
#nullable restore
#line 164 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                        }
                                    }
                                }
                                else
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <td style=\"color: gray\"><a class=\"h5\"></a></td>\r\n");
#nullable restore
#line 170 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Menu\Index.cshtml"
                                }
                            }           

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tr>\r\n                    </tbody>\r\n                </table>\r\n            </div>\r\n        </main>\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<MenuViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
