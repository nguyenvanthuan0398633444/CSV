#pragma checksum "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Shared\_Header.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cfc6e8844b28c4118ac688798fc02d3255e97f64"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__Header), @"mvc.1.0.view", @"/Views/Shared/_Header.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cfc6e8844b28c4118ac688798fc02d3255e97f64", @"/Views/Shared/_Header.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"aa90e289e11958e66328bfa2213bef20d4a46cc8", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared__Header : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"<header>
    <nav class=""navbar navbar-expand-sm navbar-dark bg-primary sticky-top flex-md-nowrap p-0 shadow"">
        <a class=""navbar-brand col-sm-3 col-md-2 mr-0"" href=""/Menu""><i class=""fa fa-home""></i> ????????????????????????</a>
        <div class=""collapse navbar-collapse"" id=""navmenu1"">
            <div class=""navbar-nav"">
                <span class=""form-control-menu"">");
#nullable restore
#line 6 "C:\Users\thuannv\Desktop\projectteamnet\ProjectTeamNET\ProjectTeamNET\Views\Shared\_Header.cshtml"
                                           Write(ViewBag.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</span>
            </div>
        </div>
        <ul class=""navbar-nav px-3 d-flex align-items-center"">
            <li class=""nav-item text-nowrap"">
                <button type=""button"" class=""btn btn-light""><i class=""fas fa-backward""></i> ??????</button>
            </li>
            <li><span class=""form-control-user"">?????????????????????</span></li>
            <li class=""nav-item text-nowrap"">
                <button type=""button"" class=""btn btn-light"" 
                        onclick=""if (confirm('????????????????????????????????????')) {window.location = origin + '/Login/Logout';}"">??????????????? 
                        <i class=""fas fa-sign-out-alt""></i>
                </button>
            </li>
        </ul>
    </nav>
</header>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
