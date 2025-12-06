<%@ Control AutoEventWireup="true" %>
<%@ Register TagPrefix="fcp" TagName="SiteFooter" Src="~/DesktopModules/FuseCP/SkinControls/SiteFooter.ascx" %>
<%@ Register TagPrefix="fcp" TagName="TopMenu" Src="~/DesktopModules/FuseCP/SkinControls/TopMenu.ascx" %>
<%@ Register TagPrefix="fcp" TagName="Logo" Src="~/DesktopModules/FuseCP/SkinControls/Logo.ascx" %>
<%@ Register TagPrefix="fcp" TagName="SignedInUser" Src="~/DesktopModules/FuseCP/SkinControls/SignedInUser.ascx" %>
<%@ Register TagPrefix="fcp" TagName="GlobalSearch" Src="~/DesktopModules/FuseCP/SkinControls/GlobalSearch.ascx" %>
<%@ Register TagPrefix="fcp" TagName="GlobalSearchTop" Src="~/DesktopModules/FuseCP/SkinControls/GlobalSearch.ascx" %>
<%@ Register TagPrefix="fcp" TagName="UserSpaceBreadcrumb" Src="~/DesktopModules/FuseCP/SkinControls/UserSpaceBreadcrumb.ascx" %>

<asp:ScriptManager ID="scriptManager" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    <Services>
        <asp:ServiceReference Path="~/DesktopModules/FuseCP/TaskManager.asmx" />
    </Services>
</asp:ScriptManager>
<!-- WRAPPER -->
<div class="wrapper" id="SkinOutline">
    <nav class="top-bar navbar-fixed-top" role="navigation">
        <div class="search-top" style="display:none;">
            <fcp:GlobalSearch ID="GlobalSearchTop" runat="server" />
        </div>
		<div class="logo-area">
			<a class="btn btn-link btn-off-canvas pull-left"><i class="icon ion-navicon"></i></a>
			<div class="logo pull-left">
                <fcp:Logo ID="logo" runat="server" />
			</div>
		</div>
        <fcp:SignedInUser ID="signedInUser1" runat="server" />
        <div class="hidden-xs">
            <fcp:GlobalSearch ID="globalSearch" runat="server" />
        </div>
	</nav>
	<!-- END TOP NAV BAR -->
    <!-- COLUMN LEFT -->
	<div id="col-left" class="col-left">
		<div class="main-nav-wrapper">
			<nav id="main-nav" class="main-nav">
				<a href="#" id="btn-nav-sidebar-minified" class="btn btn-link btn-nav-sidebar-minified"><i class="icon ion-arrow-swap"></i></a>
                <h3>Main Menu</h3>
                <fcp:TopMenu ID="leftMenu" runat="server" Align="left" />
                <asp:PlaceHolder ID="LeftPane" runat="server"></asp:PlaceHolder>
                <fcp:TopMenu ID="rightMenu" runat="server" Align="right" />
			</nav>
		</div>
	</div>
	<!-- END COLUMN LEFT -->
    <!-- COLUMN RIGHT -->
	<div id="col-right" class="col-right ">
		<div class="container-fluid primary-content" id="SkinContent">
			<!-- PRIMARY CONTENT HEADING -->
			<div class="primary-content-heading clearfix">
				<fcp:UserSpaceBreadcrumb ID="breadcrumb" runat="server"/>
                <div id="ContentOneColumn" class="row">
                    <div id="Center" class="col-md-12">
                        <asp:PlaceHolder ID="ContentPane" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </div>
   			<!-- END PRIMARY CONTENT HEADING -->
			<div class="widget widget-no-header widget-transparent bottom-30px"></div>
        </div>
        <div id="Footer" class="content-footer">
            <fcp:SiteFooter ID="footer" runat="server" />
        </div>
    </div>
</div>
<!-- Javascript -->
<script src="/JavaScript/jquery-2.1.0.min.js"></script>
<script src="/JavaScript/bootstrap/bootstrap.js"></script>
<script src="/JavaScript/fcp-common.js"></script>
<script src="/JavaScript/fcp-charts.js"></script>
<script src="/JavaScript/fcp-elements.js"></script>
<script src="/JavaScript/plugins/plugins.js"></script>
<script src="/JavaScript/jquery-ui/jquery-ui-1.10.4.custom.min.js"></script>
<script src="/JavaScript/jquery.matchHeight.js"></script>