<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CasosEnTramite.aspx.cs" Inherits="MonitorJudicial.CasosEnTramite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page Wrapper -->
    <div id="wrapper">

        <!-- Content Wrapper -->
        <div id="content-wrapper" class="d-flex flex-column">

            <!-- Main Content -->
            <div id="content">

                <!-- Begin Page Content -->
                <div class="container-fluid">

                    <!-- Page Heading -->
                    <h1 class="h3 mb-2 text-gray-800">En Trámite</h1>
                    <%--<p class="mb-4">DataTables is a third party plugin that is used to generate the demo table below.
                        For more information about DataTables, please visit the <a target="_blank"
                            href="https://datatables.net">official DataTables documentation</a>.</p>--%>                  
                    
                    <!-- DataTables -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Tabla de Datos</h6>
                            <asp:Button ID="btnGenerateReport" runat="server" Text="Generar Reporte" OnClick="btnGenerateReport_Click" />

                        </div>
                        <nav class="card-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ddlAccion" runat="server" class="form-control"></asp:DropDownList>
                                        <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnQuitarFiltro" runat="server" Text="Quitar Filtro" OnClick="btnQuitarFiltro_Click" CssClass="btn btn-success" />

                                    </div>
                                </div>
                                <br />
                                <div class="row" id="divGridPrincipal" runat="server" >
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView class="table table-striped table-hover"
                                                ID="gvCasosJudicial"
                                                runat="server"
                                                AutoGenerateColumns="true"
                                                OnRowDataBound="gvCasosJudicial_RowDataBound"
                                                AllowPaging="true"
                                                PageSize="10"
                                                OnPageIndexChanging="gvCasosJudicial_PageIndexChanging"
                                                Width="100%"
                                                CellSpacing="0">
                                                <PagerSettings Mode="NextPrevious" NextPageText="Siguiente" PreviousPageText="Anterior" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="divGridFiltrado" runat="server" >
    <div class="col-md-12">
        <div class="table-responsive">
            <asp:GridView class="table table-striped table-hover"
                ID="gvCasosJudicialFiltrado"
                runat="server"
                AutoGenerateColumns="true"
                OnRowDataBound="gvCasosJudicial_RowDataBound"
                AllowPaging="true"
                PageSize="10"
                OnPageIndexChanging="gvCasosJudicialFiltrado_PageIndexChanging"
                Width="100%"
                CellSpacing="0">
                <PagerSettings Mode="NextPrevious" NextPageText="Siguiente" PreviousPageText="Anterior" />
            </asp:GridView>
        </div>
    </div>
</div>
                            </div>
                        </nav>
                    </div>
                </div>
                <!-- /.container-fluid -->

            </div>
            <!-- End of Main Content -->

            <!-- Footer -->
            <%-- <footer class="sticky-footer bg-white">
                <div class="container my-auto">
                    <div class="copyright text-center my-auto">
                        <span>Copyright &copy; Your Website 2020</span>
                    </div>
                </div>
            </footer>--%>
            <!-- End of Footer -->

        </div>
        <!-- End of Content Wrapper -->

    </div>
    <!-- End of Page Wrapper -->

    <!-- Scroll to Top Button-->
    <a class="scroll-to-top rounded" href="#page-top">
        <i class="fas fa-angle-up"></i>
    </a>

    <!-- Logout Modal-->
    <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
        aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">Select "Logout" below if you are ready to end your current session.</div>
                <div class="modal-footer">
                    <button class="btn btn-secondary" type="button" data-dismiss="modal">Cancel</button>
                    <a class="btn btn-primary" href="login.html">Logout</a>
                </div>
            </div>
        </div>
    </div>


</asp:Content>
