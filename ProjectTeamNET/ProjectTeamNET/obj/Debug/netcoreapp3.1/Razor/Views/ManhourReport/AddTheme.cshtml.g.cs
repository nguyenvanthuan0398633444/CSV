#pragma checksum "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "46fd331116d5cc17425293e2359d989281cbee1e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_ManhourReport_AddTheme), @"mvc.1.0.view", @"/Views/ManhourReport/AddTheme.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"46fd331116d5cc17425293e2359d989281cbee1e", @"/Views/ManhourReport/AddTheme.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa90e289e11958e66328bfa2213bef20d4a46cc8", @"/Views/_ViewImports.cshtml")]
    public class Views_ManhourReport_AddTheme : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<int>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"form-group row themeadd\"");
            BeginWriteAttribute("id", " id=\"", 48, "\"", 65, 2);
            WriteAttributeValue("", 53, "Theme_", 53, 6, true);
#nullable restore
#line 2 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 59, Model, 59, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@">
    <label class=""col-form-label col-md-2""></label>
    <div class=""col-md-10"">
        <div class=""row align-items-center"">
            <div class=""col-md-4 input-group pl-0"">
                <input type=""text"" class=""form-control form-control-sm"" placeholder=""テーマ選択..."" aria-label=""..."" aria-describedby=""button-addon2""");
            BeginWriteAttribute("id", " id=\"", 394, "\"", 411, 2);
            WriteAttributeValue("", 399, "theme_", 399, 6, true);
#nullable restore
#line 7 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 405, Model, 405, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" readonly>\r\n                <div class=\"input-group-append\">\r\n                    <button type=\"button\"");
            BeginWriteAttribute("id", " id=\"", 515, "\"", 542, 2);
            WriteAttributeValue("", 520, "button-addtheme_", 520, 16, true);
#nullable restore
#line 9 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 536, Model, 536, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"btn btn-sm btn-outline-secondary\" data-toggle=\"modal\" data-target=\"#Modal_");
#nullable restore
#line 9 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
                                                                                                                                                  Write(Model);

#line default
#line hidden
#nullable disable
            WriteLiteral("\"><i class=\"fas fa-search\"></i></button>\r\n                </div>\r\n            </div>\r\n            <div class=\"col-md-2 input-group pr-2\">\r\n                <select class=\"form-control form-control-sm\"");
            BeginWriteAttribute("id", " id=\"", 830, "\"", 856, 2);
            WriteAttributeValue("", 835, "addWorkContent_", 835, 15, true);
#nullable restore
#line 13 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 850, Model, 850, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n                </select>\r\n            </div>\r\n            <div class=\"col-md-2 input-group\">\r\n                <input type=\"number\" class=\"form-control form-control-sm\"");
            BeginWriteAttribute("id", " id=\"", 1028, "\"", 1053, 2);
            WriteAttributeValue("", 1033, "addWorkDetail_", 1033, 14, true);
#nullable restore
#line 17 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 1047, Model, 1047, 6, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"内容詳細...\" aria-label=\"...\" aria-describedby=\"button-addon2\"  min=\"00\" max=\"99\">\r\n            </div>\r\n            <div class=\"col-md-4 input-group pl-0\">\r\n                <i class=\"far fa-trash-alt\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1264, "\"", 1293, 3);
            WriteAttributeValue("", 1274, "deleteTheme(", 1274, 12, true);
#nullable restore
#line 20 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\ManhourReport\AddTheme.cshtml"
WriteAttributeValue("", 1286, Model, 1286, 6, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1292, ")", 1292, 1, true);
            EndWriteAttribute();
            WriteLiteral("></i>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<int> Html { get; private set; }
    }
}
#pragma warning restore 1591
