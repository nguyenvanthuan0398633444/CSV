#pragma checksum "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d2d9676777fad6cec4ef1d68a5c901483ef48c3e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ManhourUpdate_Index), @"mvc.1.0.view", @"/Views/ManhourUpdate/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d2d9676777fad6cec4ef1d68a5c901483ef48c3e", @"/Views/ManhourUpdate/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa90e289e11958e66328bfa2213bef20d4a46cc8", @"/Views/_ViewImports.cshtml")]
    public class Views_ManhourUpdate_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProjectTeamNET.Models.Response.ManhourUpdate>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/css/ManhourUpdate.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "ManhourUpdate", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ImportCSV", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formImport"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/manhourUpdate.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
  
    ViewData["Title"] = "Index";
    Layout = "~/Views/Shared/_LayoutUpdate.cshtml";
    string group = Model.groupId;
    string user = Model.userId;


#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d2d9676777fad6cec4ef1d68a5c901483ef48c3e6933", async() => {
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
        <main role=""main"" class=""container-fluid px-4"">
            <div class=""alert alert-warning mb-2"" role=""alert"" style=""display:none;"">
                <strong>アラート</strong> - 合計工数が8h未満のデータが存在します！
            </div>
            <div class=""d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-1 pb-2 mb-3 border-bottom"">
                <div class=""form-row align-items-center"">
                    <div class=""col-auto"">
                        <div class=""input-group date"">
                            <input type=""text"" class=""form-control form-control-sm""");
            BeginWriteAttribute("value", " value=\"", 964, "\"", 984, 1);
#nullable restore
#line 21 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
WriteAttributeValue("", 972, Model.today, 972, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" readonly id=\"month\" />\r\n                        </div>\r\n                    </div>\r\n                    <div class=\"col-auto\">\r\n                        ");
#nullable restore
#line 25 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                   Write(Html.DropDownListFor(Model => Model.groups,
                            new SelectList(Model.groups, "Value", "Text"),
                            new { @class = "form-control form-control-sm selectGroup" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </div>\r\n                    <div class=\"col-auto\">\r\n                        ");
#nullable restore
#line 30 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                   Write(Html.DropDownListFor(Model => Model.users,
                            new SelectList(Model.users, "Value", "Text"),
                            new { @class = "form-control form-control-sm selectUser" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                    </div>
                    <div class=""col-auto"">
                        <button class=""btn btn-sm btn-outline-secondary mr-4"" onclick=""search()""><i class=""fas fa-search""></i> 検索</button>
                    </div>
                </div>
                <div class=""btn-toolbar mb-2 mb-md-0"">
                    <button id=""ExportCsv"" class=""btn btn-sm btn-outline-secondary mr-2""><i class=""fas fa-file-download""></i> CSVダウンロード</button>
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d2d9676777fad6cec4ef1d68a5c901483ef48c3e10788", async() => {
                WriteLiteral("\r\n                        <input id=\"fileCSVimport\" type=\"file\" name=\"Csv\" style=\"display:none;\" />\r\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_4.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
                    <button id=""import"" class=""btn btn-sm btn-outline-secondary mr-2"" onclick="" document.getElementById('fileCSVimport').click();""><i class=""fas fa-file-upload""></i> CSVアップロード</button>
                    <button class=""btn btn-sm btn-outline-secondary mr-2"" id=""btSave""><i class=""far fa-save""></i> 保存</button>
                </div>
            </div>
            <div class=""row mb-2 align-items-center"">
                <div class=""col-md-4 input-group"">
                    <input type=""text"" id=""theme"" class=""form-control form-control-sm"" placeholder=""テーマ選択..."" aria-label=""..."" aria-describedby=""button-addon2"" readonly >
                    <div class=""input-group-append"">
                        <button type=""button"" class=""btn btn-sm btn-outline-secondary"" data-toggle=""modal"" data-target=""#modal1""><i class=""fas fa-search""></i></button>
                    </div>
                </div>
                <div class=""col-md-2 input-group pr-2"">
                    ");
#nullable restore
#line 55 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
               Write(Html.DropDownListFor(Model => Model.wordContents,
                                              new SelectList(Model.wordContents, "Value", "Text"),
                                              new { @class = "form-control form-control-sm selectGroup" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"                   
                </div>
                <div class=""col-md-2 input-group"">
                    <input type=""text""  class=""form-control form-control-sm"" placeholder=""内容詳細..."" aria-label=""..."" aria-describedby=""button-addon2"">
                </div>
                <div class=""col-md-4 pl-0"">
                    <button id=""addTheme"" class=""btn btn-sm btn-outline-secondary mr-2""><i class=""fas fa-arrow-down""></i> テーマ追加</button>
                </div>
            </div>
            <div class=""table-responsive"">
                <table class=""table table-striped table-sm"">
                    <thead class=""thead-light"" id=""thead"">
                    </thead>
                    <tbody id=""tbody"">
                    </tbody>
                    <tfoot id=""tfoot"">
                    </tfoot>
                </table>
            </div>
            <!-- Modal -->
            <div class=""modal fade"" id=""modal1"" tabindex=""-1"" role=""dialog"" aria-labelledby=""label1"" aria-hidden=""t");
            WriteLiteral(@"rue"">
                <div class=""modal-dialog modal-lg"" role=""document"">
                    <div class=""modal-content"">
                        <div class=""modal-header"">
                            <h5 class=""modal-title"" id=""label1"">テーマ追加</h5>
                            <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                                <span aria-hidden=""true"">&times;</span>
                            </button>
                        </div>
                        <div class=""modal-body"">
                            <div class=""container-fluid"">
                                <div class=""row"">
                                    <div class=""col-md-4"">
                                        <div class=""form-group"">
                                            <label>テーマNo</label>
                                            <input type=""text"" class=""form-control form-control-sm"" id=""themeNo"">
                                        </div>
      ");
            WriteLiteral(@"                              </div>
                                    <div class=""col-md-8"">
                                        <div class=""form-group"">
                                            <label>テーマ名</label>
                                            <input type=""text"" class=""form-control form-control-sm"" id=""themeName"">
                                        </div>
                                    </div>
                                </div>
                                <div class=""row"">
                                    <div class=""col-md-4"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 105 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                                       Write(Html.DropDownListFor(Model => Model.groupThemes,
                                               new SelectList(Model.groupThemes, "Value", "Text"),
                                               new { @class = "form-control form-control-sm selectGroup" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>
                                    </div>
                                    <div class=""col-md-4"">
                                        <div class=""form-group"">
                                            ");
#nullable restore
#line 112 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                                       Write(Html.DropDownListFor(Model => Model.salesObject,
                                               new SelectList(Model.salesObject, "Value", "Text"),
                                               new { @class = "form-control form-control-sm selectGroup" }));

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
                                        </div>
                                    </div>
                                    <div class=""col-md-4"">
                                        <div class=""form-group"">
                                            <label>売上状況</label>
                                            <div>
                                                <div class=""form-check form-check-inline"">
                                                    <input class=""form-check-input"" type=""radio"" name=""inlineRadioOptions"" id=""inlineRadio1"" value=""option2"" checked>
                                                    <label class=""form-check-label"">未売上</label>
                                                </div>
                                                <div class=""form-check form-check-inline"">
                                                    <input class=""form-check-input"" type=""radio"" name=""inlineRadioOptions"" id=""inlineRadio2"" value=""option3"">
                         ");
            WriteLiteral(@"                           <label class=""form-check-label"">売上済</label>
                                                </div>
                                                <div class=""form-check form-check-inline"">
                                                    <input class=""form-check-input"" type=""radio"" name=""inlineRadioOptions"" id=""inlineRadio3"" value=""option1"">
                                                    <label class=""form-check-label"">全て</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class=""row"">
                                    <button type=""button"" class=""btn btn-primary mt-2 mb-2"" id=""searchTheme"">検索</button>
                                </div>
                                <div class=""row"">
                                    <div clas");
            WriteLiteral(@"s=""table-responsive"">
                                        <table class=""table table-striped table-sm"">
                                            <thead class=""thead-light"">
                                                <tr>
                                                    <th></th>
                                                    <th>テーマNo</th>
                                                    <th>テーマ名</th>
                                                    <th>売上状況</th>
                                                </tr>
                                            </thead>
                                            <tbody id=""slThemeBody"">
                                                
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=""mo");
            WriteLiteral(@"dal-footer"">
                            <button type=""button"" class=""btn btn-secondary"" data-dismiss=""modal"">キャンセル</button>
                            <button type=""button"" class=""btn btn-primary"" id=""choiceTheme"">追加</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal -->
        </main>
    </div>
</div>
");
            DefineSection("scripts", async() => {
                WriteLiteral("\r\n    <script>       \r\n        document.getElementById(\'groups\').value = \"");
#nullable restore
#line 172 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                                              Write(group);

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n        document.getElementById(\'users\').value = \"");
#nullable restore
#line 173 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourUpdate\Index.cshtml"
                                             Write(user);

#line default
#line hidden
#nullable disable
                WriteLiteral("\";\r\n    </script>\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d2d9676777fad6cec4ef1d68a5c901483ef48c3e23152", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public ProjectDbContext context { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProjectTeamNET.Models.Response.ManhourUpdate> Html { get; private set; }
    }
}
#pragma warning restore 1591