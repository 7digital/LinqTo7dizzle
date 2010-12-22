<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<LinqTo7Dizzle.Entities.Release>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    ASP.NET MVC Music Store
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="promotion"></div>

    <h3><em>Fresh</em> off the grill</h3>

    <ul id="album-list">
        <% foreach (var release in Model)
           { %>
        <li>
            <a href="<%: Url.Action("Details", "Store", new { id = release.Id }) %>">
            <img alt="<%: release.Title %>" src="<%: release.ImageUrl %>" />
            <span><%: release.Title %></span>
            </a>
        </li>
        <% } %>
    </ul>

</asp:Content>
