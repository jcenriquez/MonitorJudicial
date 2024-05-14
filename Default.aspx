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

                    <!-- Page Heading -->
                    <div class="d-sm-flex align-items-center justify-content-between mb-4">
                        <h1 class="h3 mb-0 text-gray-800">Dashboard</h1>
                        <a href="#" class="d-none d-sm-inline-block btn btn-sm btn-primary shadow-sm"><i
                            class="fas fa-download fa-sm text-white-50"></i>Generate Report</a>
                    </div>

                    <!-- Content Row -->
                    <div class="row">

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-primary shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-primary text-uppercase mb-1">
                                                Earnings (Monthly)
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">$40,000</div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-calendar fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-success shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-success text-uppercase mb-1">
                                                Earnings (Annual)
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">$215,000</div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-dollar-sign fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Earnings (Monthly) Card Example -->
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-info shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-info text-uppercase mb-1">
                                                Tasks
                                           
                                            </div>
                                            <div class="row no-gutters align-items-center">
                                                <div class="col-auto">
                                                    <div class="h5 mb-0 mr-3 font-weight-bold text-gray-800">50%</div>
                                                </div>
                                                <div class="col">
                                                    <div class="progress progress-sm mr-2">
                                                        <div class="progress-bar bg-info" role="progressbar"
                                                            style="width: 50%" aria-valuenow="50" aria-valuemin="0"
                                                            aria-valuemax="100">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-clipboard-list fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Pending Requests Card Example -->
                        <div class="col-xl-3 col-md-6 mb-4">
                            <div class="card border-left-warning shadow h-100 py-2">
                                <div class="card-body">
                                    <div class="row no-gutters align-items-center">
                                        <div class="col mr-2">
                                            <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                Pending Requests
                                            </div>
                                            <div class="h5 mb-0 font-weight-bold text-gray-800">18</div>
                                        </div>
                                        <div class="col-auto">
                                            <i class="fas fa-comments fa-2x text-gray-300"></i>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Content Row -->

                    <div class="row">

                        <!-- Area Chart -->
                        <div class="col-xl-8 col-lg-7">
                            <div class="card shadow mb-4">
                                <!-- Card Header - Dropdown -->
                                <div
                                    class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                    <h6 class="m-0 font-weight-bold text-primary">Earnings Overview</h6>
                                    <div class="dropdown no-arrow">
                                        <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink"
                                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in"
                                            aria-labelledby="dropdownMenuLink">
                                            <div class="dropdown-header">Dropdown Header:</div>
                                            <a class="dropdown-item" href="#">Action</a>
                                            <a class="dropdown-item" href="#">Another action</a>
                                            <div class="dropdown-divider"></div>
                                            <a class="dropdown-item" href="#">Something else here</a>
                                        </div>
                                    </div>
                                </div>
                                <!-- Card Body -->
                                <div class="card-body">
                                    <div class="chart-area">
                                        <canvas id="myAreaChart"></canvas>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Pie Chart -->
                        <div class="col-xl-4 col-lg-5">
                            <div class="card shadow mb-4">
                                <!-- Card Header - Dropdown -->
                                <div
                                    class="card-header py-3 d-flex flex-row align-items-center justify-content-between">
                                    <h6 class="m-0 font-weight-bold text-primary">Revenue Sources</h6>
                                    <div class="dropdown no-arrow">
                                        <a class="dropdown-toggle" href="#" role="button" id="dropdownMenuLink"
                                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                            <i class="fas fa-ellipsis-v fa-sm fa-fw text-gray-400"></i>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right shadow animated--fade-in"
                                            aria-labelledby="dropdownMenuLink">
                                            <div class="dropdown-header">Dropdown Header:</div>
                                            <a class="dropdown-item" href="#">Action</a>
                                            <a class="dropdown-item" href="#">Another action</a>
                                            <div class="dropdown-divider"></div>
                                            <a class="dropdown-item" href="#">Something else here</a>
                                        </div>
                                    </div>
                                </div>
                                <!-- Card Body -->
                                <div class="card-body">
                                    <div class="chart-pie pt-4 pb-2">
                                        <canvas id="myPieChart"></canvas>
                                    </div>
                                    <div class="mt-4 text-center small">
                                        <span class="mr-2">
                                            <i class="fas fa-circle text-primary"></i>Direct
                                        </span>
                                        <span class="mr-2">
                                            <i class="fas fa-circle text-success"></i>Social
                                        </span>
                                        <span class="mr-2">
                                            <i class="fas fa-circle text-info"></i>Referral
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Content Row -->
                    <div class="row">

                        <!-- Content Column -->
                        <div class="col-lg-6 mb-4">

                            <!-- Project Card Example -->
                            <div class="card shadow mb-4">
                                <div class="card-header py-3">
                                    <h6 class="m-0 font-weight-bold text-primary">Por Transacción</h6>
                                </div>
                                <div class="card-body">
                                    <h4 class="small font-weight-bold">Abandono <span class="float-right"><%: abandono.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: abandono.ToString("0.00") %>%"
                                            aria-valuenow="<%: abandono.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Abstención de Trámite por Parte Del Juez <span class="float-right"><%: abstencionTrmiteParteJuez.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: abstencionTrmiteParteJuez.ToString("0.00") %>%"
                                            aria-valuenow="<%: abstencionTrmiteParteJuez.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Adjudicación <span class="float-right"><%: adjudicacion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: adjudicacion.ToString("0.00") %>%"
                                            aria-valuenow="<%: adjudicacion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Alegatos <span class="float-right"><%: alegatos.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: alegatos.ToString("0.00") %>%"
                                            aria-valuenow="<%: alegatos.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Apelación <span class="float-right"><%: apelacion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: apelacion.ToString("0.00") %>%"
                                            aria-valuenow="<%: apelacion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Avaluó de Bienes <span class="float-right"><%: avaluoBienes.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: avaluoBienes.ToString("0.00")  %>%"
                                            aria-valuenow="<%: avaluoBienes.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Calificación Demanda <span class="float-right"><%: calificacionDemanda.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: calificacionDemanda.ToString("0.00") %>%"
                                            aria-valuenow="<%: calificacionDemanda.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    <h4 class="small font-weight-bold">Cambio de Casillero Judicial <span class="float-right"><%: cambioCasilleroJudicial.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: cambioCasilleroJudicial.ToString("0.00") %>%"
                                            aria-valuenow="<%: cambioCasilleroJudicial.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Citación a los Demandados <span class="float-right"><%: citacionDemandados.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: citacionDemandados.ToString("0.00") %>%"
                                            aria-valuenow="<%: citacionDemandados.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Contestación Demanda <span class="float-right"><%: contestaciónDemanda.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: contestaciónDemanda.ToString("0.00") %>%"
                                            aria-valuenow="<%: contestaciónDemanda.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Desistimiento <span class="float-right"><%: desistimiento.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: desistimiento.ToString("0.00") %>%"
                                            aria-valuenow="<%: desistimiento.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Embargo <span class="float-right"><%: embargo.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: embargo.ToString("0.00") %>%"
                                            aria-valuenow="<%: embargo.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Junta de Conciliación <span class="float-right"><%: juntaConciliacion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: juntaConciliacion.ToString("0.00") %>%"
                                            aria-valuenow="<%: juntaConciliacion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Liquidación <span class="float-right"><%: liquidacion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: liquidacion.ToString("0.00") %>%"
                                            aria-valuenow="<%: liquidacion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Mandamiento de Ejecución <span class="float-right"><%: mandamientoEjecucion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: mandamientoEjecucion.ToString("0.00") %>%"
                                            aria-valuenow="<%: mandamientoEjecucion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Ninguno <span class="float-right"><%: ninguno.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: ninguno.ToString("0.00") %>%"
                                            aria-valuenow="<%: ninguno.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">No Contesta Demanda <span class="float-right"><%: noContestaDemanda.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: noContestaDemanda.ToString("0.00") %>%"
                                            aria-valuenow="<%: noContestaDemanda.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Otros <span class="float-right"><%: otros.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: otros.ToString("0.00") %>%"
                                            aria-valuenow="<%: otros.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Presentación Demanda <span class="float-right"><%: presentacionDemanda.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: presentacionDemanda.ToString("0.00") %>%"
                                            aria-valuenow="<%: presentacionDemanda.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Prueba <span class="float-right"><%: prueba.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: prueba.ToString("0.00") %>%"
                                            aria-valuenow="<%: prueba.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    <h4 class="small font-weight-bold">Remate <span class="float-right"><%: remate.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: remate.ToString("0.00") %>%"
                                            aria-valuenow="<%: remate.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>
                                    
                                    
                                    <h4 class="small font-weight-bold">Sentencia <span class="float-right"><%: sentencia.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: sentencia.ToString("0.00") %>%"
                                            aria-valuenow="<%: sentencia.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Suspendido por Acuerdo <span class="float-right"><%: suspendidoAcuerdo.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: suspendidoAcuerdo.ToString("0.00") %>%"
                                            aria-valuenow="<%: suspendidoAcuerdo.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Terminado por Acuerdo o Pago de Obligaciones <span class="float-right"><%: terminadoAcuerdoPagoObligaciones.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: terminadoAcuerdoPagoObligaciones.ToString("0.00") %>%"
                                            aria-valuenow="<%: terminadoAcuerdoPagoObligaciones.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Aprehencion Vehicular <span class="float-right"><%: aprehencionVehicular.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: aprehencionVehicular.ToString("0.00") %>%"
                                            aria-valuenow="<%: aprehencionVehicular.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Audiencia <span class="float-right"><%: audiencia.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: audiencia.ToString("0.00") %>%"
                                            aria-valuenow="<%: audiencia.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Razón de no Pago <span class="float-right"><%: razonNoPago.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: razonNoPago.ToString("0.00") %>%"
                                            aria-valuenow="<%: razonNoPago.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                    
                                    <h4 class="small font-weight-bold">Audiencia Ejecución <span class="float-right"><%: audienciaEjecucion.ToString("0.00") %>%</span></h4>
                                    <div class="progress mb-4">
                                        <%-- También para el atributo 'style' del elemento div --%>
                                        <div class="progress-bar bg-danger" role="progressbar" style="width: <%: audienciaEjecucion.ToString("0.00") %>%"
                                            aria-valuenow="<%: audienciaEjecucion.ToString("0.00") %>" aria-valuemin="0" aria-valuemax="100">
                                        </div>
                                    </div>

                                </div>
                            </div>

                            <!-- Color System -->
                            <div class="row">
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-primary text-white shadow">
                                        <div class="card-body">
                                            Primary
                                           
                                            <div class="text-white-50 small">#4e73df</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-success text-white shadow">
                                        <div class="card-body">
                                            Success
                                           
                                            <div class="text-white-50 small">#1cc88a</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-info text-white shadow">
                                        <div class="card-body">
                                            Info
                                           
                                            <div class="text-white-50 small">#36b9cc</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-warning text-white shadow">
                                        <div class="card-body">
                                            Warning
                                           
                                            <div class="text-white-50 small">#f6c23e</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-danger text-white shadow">
                                        <div class="card-body">
                                            Danger
                                           
                                            <div class="text-white-50 small">#e74a3b</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-secondary text-white shadow">
                                        <div class="card-body">
                                            Secondary
                                           
                                            <div class="text-white-50 small">#858796</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-light text-black shadow">
                                        <div class="card-body">
                                            Light
                                           
                                            <div class="text-black-50 small">#f8f9fc</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 mb-4">
                                    <div class="card bg-dark text-white shadow">
                                        <div class="card-body">
                                            Dark
                                           
                                            <div class="text-white-50 small">#5a5c69</div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>

                        <div class="col-lg-6 mb-4">

                            <!-- Illustrations -->
                            <div class="card shadow mb-4">
                                <div class="card-header py-3">
                                    <h6 class="m-0 font-weight-bold text-primary">Illustrations</h6>
                                </div>
                                <div class="card-body">
                                    <div class="text-center">
                                        <img class="img-fluid px-3 px-sm-4 mt-3 mb-4" style="width: 25rem;"
                                            src="img/undraw_posting_photo.svg" alt="...">
                                    </div>
                                    <p>
                                        Add some quality, svg illustrations to your project courtesy of <a
                                            target="_blank" rel="nofollow" href="https://undraw.co/">unDraw</a>, a
                                        constantly updated collection of beautiful svg images that you can use
                                        completely free and without attribution!
                                    </p>
                                    <a target="_blank" rel="nofollow" href="https://undraw.co/">Browse Illustrations on
                                        unDraw &rarr;</a>
                                </div>
                            </div>

                            <!-- Approach -->
                            <div class="card shadow mb-4">
                                <div class="card-header py-3">
                                    <h6 class="m-0 font-weight-bold text-primary">Development Approach</h6>
                                </div>
                                <div class="card-body">
                                    <p>
                                        SB Admin 2 makes extensive use of Bootstrap 4 utility classes in order to reduce
                                        CSS bloat and poor page performance. Custom CSS classes are used to create
                                        custom components and custom utility classes.
                                    </p>
                                    <p class="mb-0">
                                        Before working with this theme, you should become familiar with the
                                        Bootstrap framework, especially the utility classes.
                                    </p>
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
