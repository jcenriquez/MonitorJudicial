<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MonitorJudicial._Default" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Page Wrapper -->
    <div id="wrapper">

        <!-- Content Wrapper -->
        <div id="content-wrapper" class="d-flex flex-column">

            <!-- Main Content -->
            <div id="content">

                <!-- Begin Page Content -->
                <div class="container-fluid">

                    <!-- Título -->
                    <div class="d-sm-flex align-items-center justify-content-between mb-4">
                        <h1 class="h3 mb-0 text-gray-800">Tablero</h1>
                        <%--<a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" ><i
                            class="fas fa-download fa-sm text-white-50"></i>Generar Reporte</a>--%>
                        <%--<asp:LinkButton ID="btnGenerarReporte" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerarReporte_Click">
    <i class="fas fa-download"></i> Reporte Resumen
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnGenerarReporteDetalle" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerarReporteDetalle_Click">
<i class="fas fa-download"></i> Reporte Casos Actualizados
                        </asp:LinkButton>--%>
                    </div>
                    <%--<div class="row">
                        <div class="col-xl-6 col-md-6 mb-4">
                            <div class="card border-left-warning shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litPrestamoJudicial" runat="server"></asp:Literal>
                                            </div>
                                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                Préstamos en Estado Judicial
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-gavel fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>--%>

                    <div class="row">
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnGenerarReporte" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerarReporte_Click">
                            <i class="fas fa-download"></i>Reporte Resumen
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnReporteDetallado" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerateReportDetalle_Click">
<i class="fas fa-download"></i> Reporte Detalle
                            </asp:LinkButton>
                        </div>
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnGenerarReporteDetalle" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerarReporteDetalle_Click">
            <i class="fas fa-download"></i> Reporte Casos Actualizados
                            </asp:LinkButton>
                        </div>
                                    <div class="col-md-2">
                <asp:LinkButton ID="btnReporteGarantia" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm" runat="server" OnClick="btnGenerarReporteExtendido_Click">
<i class="fas fa-download"></i> Reporte Extendido
                </asp:LinkButton>
            </div>
                    </div>
                    <br />

                    <div class="row">

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                Total Casos
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litPrestamoJudicial" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-suitcase fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Prejudicial -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-dark shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-dark text-uppercase mb-1">
                                                Prejudicial
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalPrejudicial" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-balance-scale-left fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Judicial -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-success shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                Judicial
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalJudicial" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-balance-scale fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Judicial con Acuerdo Al Día -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-info shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                Judicial con Acuerdo Al Día
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalAlDia" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-wallet fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Judicial con Acuerdo Vencido -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-danger shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">
                                                Judicial con Acuerdo Vencido
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalVencido" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-frown-open fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-secondary shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-secondary text-uppercase mb-1">
                                                Castigado
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalCastigado" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-gavel fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <!-- Content Row -->
                    <div class="row">
                        <!-- Porcentajes -->
                        <div class="col-xl-12 col-lg-12">
                            <div class="row">
                            </div>
                            <div class="row">
                                <div class="col-lg-12 mb-4">

                                    <!-- Project Card Example -->
                                    <div class="card shadow mb-4">
                                        <div class="card-header py-3">
                                            <h6 class="m-0 font-weight-bold text-primary">Casos por Abogado</h6>
                                        </div>
                                        <div class="card-body">
                                            <div class="col-md-12">
                                                <div class="table-responsive">
                                                    <asp:GridView class="table table-striped table-hover"
                                                        ID="gvCasosAbogado"
                                                        runat="server"
                                                        AutoGenerateColumns="true"
                                                        Width="100%"
                                                        CellSpacing="0">
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                        
                                    </div>
                                </div>
                            </div>


                        </div>
                        

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
                    <h5 class="modal-title" id="exampleModalLabel">¿Listo para salir?</h5>
                    <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">×</span>
                    </button>
                </div>
                <div class="modal-body">Seleccione "Cerrar Sesión" a continuación si está listo para finalizar su sesión actual.</div>
                <div class="modal-footer">
                    <asp:Button ID="btnLogout" runat="server" Text="Cerrar sesión" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>
