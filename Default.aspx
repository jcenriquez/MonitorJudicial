﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MonitorJudicial._Default" %>



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
                        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                            class="fas fa-download fa-sm text-white-50"></i>Generar Reporte</a>
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

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                Total Préstamos
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litPrestamoJudicial" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-gavel fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-success shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                Castigados
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalCastigado" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-success shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                Judicial
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalJudicial" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-2 col-md-6 mb-4">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                Al Día
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                <asp:Literal ID="litotalAlDia" runat="server"></asp:Literal>
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
                                        <div class="card-body">
                                            <h4 class="small font-weight-bold">Dr. Vasquez Rivadeneira Carlos Gabriel <span
                                                class="float-right">9.64%</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 9.64%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Dr. Guaranguay Vargas Rolando Javier <span
                                                class="float-right">22.80%</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-warning" role="progressbar" style="width: 22.80%"
                                                    aria-valuenow="40" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Dr. Luis Edison Crespo Almeida <span
                                                class="float-right">26.75%</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar" role="progressbar" style="width: 26.75%"
                                                    aria-valuenow="60" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Dr. Edisson Espinosa Venegas <span
                                                class="float-right">40.78%</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-info" role="progressbar" style="width: 40.78%"
                                                    aria-valuenow="80" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>


                        </div>

                        <!-- Content Column -->
                        <div class="col-lg-6 mb-4" hidden>

                            <div class="row">
                                <div class="col-lg-12 mb-4">
                                    <!-- Por Transacción -->
                                    <div class="card shadow mb-4">
                                        <div class="card-header py-3">
                                            <h6 class="m-0 font-weight-bold text-primary">Porcentaje por Tipo de Transacción</h6>
                                        </div>
                                        <div class="card-body">
                                            <h4 class="small font-weight-bold" hidden="">Abandono <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Abstención de Trámite por Parte Del Juez <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Adjudicación <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Alegatos <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Apelación <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Avaluó de Bienes <span class="float-right"><%= avaluoBienes.ToString() %></span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: <%= avaluoBienes.ToString() %>%"
                                                    aria-valuenow="<%= avaluoBienes.ToString() %>" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>

                                            <h4 class="small font-weight-bold" hidden="">Calificación Demanda <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Cambio de Casillero Judicial <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Citación a los Demandados <span
                                                class="float-right">6</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 2.62%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Contestación Demanda <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Desistimiento <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Embargo <span
                                                class="float-right">1</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0.44%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Junta de Conciliación <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Liquidación <span
                                                class="float-right">2</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0.87%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Mandamiento de Ejecución <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Ingreso de la Demanda <span
                                                class="float-right">218</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-info" role="progressbar" style="width: 94.32%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">No Contesta Demanda <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Otros <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Presentación Demanda <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Remate <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Sentencia <span
                                                class="float-right">2</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0.87%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold">Suspendido por Acuerdo <span
                                                class="float-right">1</span></h4>
                                            <div class="progress mb-4">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0.44%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Terminado por Acuerdo o Pago de Obligaciones <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Aprehencion Vehicular <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Audiencia <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Razón de no Pago <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                            <h4 class="small font-weight-bold" hidden="">Audiencia Ejecución <span
                                                class="float-right">0%</span></h4>
                                            <div class="progress mb-4" hidden="">
                                                <div class="progress-bar bg-danger" role="progressbar" style="width: 0%"
                                                    aria-valuenow="20" aria-valuemin="0" aria-valuemax="100">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 mb-4">
                                    <div class="card shadow mb-4">
                                        <!-- Card Header - Dropdown -->
                                        <div
                                            class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                            <h6 class="m-0 font-weight-bold text-primary">Porcentajes</h6>
                                        </div>
                                        <!-- Card Body -->
                                        <div class="card-body">
                                            <div class="chart-pie pt-4 pb-2">
                                                <canvas id="myPieChart"></canvas>
                                            </div>
                                            <div class="mt-4 text-center small">
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-primary"></i>AvaluO De Bienes
                                                </span>
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-success"></i>"Citación A Los Demandados
                                                </span>
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-info"></i>Embargo
                                                </span>
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-warning"></i>Liquidación
                                                </span>
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-danger"></i>Sentencia
                                                </span>
                                                <span class="mr-2">
                                                    <i class="fas fa-circle text-secondary"></i>Suspendido Por Acuerdo
                                                </span>
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
