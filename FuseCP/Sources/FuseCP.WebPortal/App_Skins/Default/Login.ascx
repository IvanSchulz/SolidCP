<%@ Control AutoEventWireup="true" %>
<%@ Register TagPrefix="fcp" TagName="SiteFooter" Src="~/DesktopModules/FuseCP/SkinControls/SiteFooter.ascx" %>
<%@ Register TagPrefix="fcp" TagName="Logo" Src="~/DesktopModules/FuseCP/SkinControls/Logo.ascx" %>
<style>
    body {
        background-color: #EFEFEF;
    }
</style>
<div class="middle-content page-login">
    <div class="login-table-row">
        <div class="top-bar text-center login-table-cell">
            <a href='<%= Page.ResolveUrl("~/") %>'>
                <asp:Image runat="server" SkinID="Logo" alt="" />
            </a>
        </div>
    </div>
    <div class="container-fluid primary-content" id="SkinContent">
        <div class="row-fluid">
            <div class="col-sm-4 centering">
                <asp:PlaceHolder ID="ContentPane" runat="server"></asp:PlaceHolder>
                <div class="login-footer">
                    <fcp:SiteFooter ID="SiteFooter2" runat="server" />
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Javascript -->
<script src="/JavaScript/jquery-2.1.0.min.js"></script>
<script src="/JavaScript/bootstrap/bootstrap.js"></script>
<script src="/Javascript/fcp-form-layouts.js"></script>
