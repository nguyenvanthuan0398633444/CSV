#pragma checksum "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourInput\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cd115eb467f8f19c585d50be821a1a9fd1d51598"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ManhourInput_Index), @"mvc.1.0.view", @"/Views/ManhourInput/Index.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cd115eb467f8f19c585d50be821a1a9fd1d51598", @"/Views/ManhourInput/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa90e289e11958e66328bfa2213bef20d4a46cc8", @"/Views/_ViewImports.cshtml")]
    public class Views_ManhourInput_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ProjectTeamNET.Models.Response.InitDataModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourInput\Index.cshtml"
  
    ViewData["PageName"] = " 工数管理システム";
    ViewBag.Title = "工数入力";
    string dayGet = "Day"+DateTime.Parse(Model.DateSelect).Day;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"row\">\r\n\r\n    <main role=\"main\" class=\"container-fluid px-4\">\r\n        <div class=\"alert alert-warning mb-2\" role=\"alert\">\r\n            <strong>アラート</strong> - 合計工数が8h未満です！\r\n        </div>\r\n\r\n        ");
#nullable restore
#line 15 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourInput\Index.cshtml"
   Write(await Html.PartialAsync("_HeaderInput", Model));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        <!-- Table-->\r\n        <input type=\"hidden\" id=\"pageHistory\"");
            BeginWriteAttribute("value", " value=\"", 525, "\"", 551, 1);
#nullable restore
#line 17 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourInput\Index.cshtml"
WriteAttributeValue("", 533, Model.pageHistory, 533, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@"/>
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
    </main>

</div>
");
#nullable restore
#line 31 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourInput\Index.cshtml"
Write(await Html.PartialAsync("_ManhourInputModal.cshtml", Model));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ProjectTeamNET.Models.Response.InitDataModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
