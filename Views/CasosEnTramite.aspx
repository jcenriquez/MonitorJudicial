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

                    <!-- DataTables -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Tabla de Datos</h6>
                            <asp:Button ID="btnGenerateReport" runat="server" Text="Generar Reporte" OnClick="btnGenerateReport_Click" />
                        </div>
                        <nav class="card-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlEstado" runat="server" class="form-control"></asp:DropDownList>
                                        <asp:Button ID="btnFiltrarEstado" runat="server" Text="Filtrar" OnClick="btnFiltrarEstado_Click" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnQuitarFiltroEstado" runat="server" Text="Quitar Filtro" OnClick="btnQuitarFiltroEstado_Click" CssClass="btn btn-success" />
                                    </div>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ddlAccion" runat="server" class="form-control"></asp:DropDownList>
                                        <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnQuitarFiltro" runat="server" Text="Quitar Filtro" OnClick="btnQuitarFiltro_Click" CssClass="btn btn-success" />
                                    </div>
                                    <div class="col-md-4" id="divFiltroAbogado" runat="server" visible="false">
                                        <asp:DropDownList ID="ddlFiltroAbogado" runat="server" class="form-control"></asp:DropDownList>
                                        <asp:Button ID="btnFiltrarAbogado" runat="server" Text="Filtrar" OnClick="btnFiltrarAbogado_Click" CssClass="btn btn-primary" />
                                        <asp:Button ID="btnQuitarFiltroAbogado" runat="server" Text="Quitar Filtro" OnClick="btnQuitarFiltroAbogado_Click" CssClass="btn btn-success" />
                                    </div>
                                </div>
                                <br />
                                <div class="row" id="divGridPrincipal" runat="server">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView
                                                class="table table-striped table-hover border-left-primary border-bottom-primary"
                                                ID="gvCasosJudicial"
                                                runat="server"
                                                AutoGenerateColumns="true"
                                                AutoGenerateSelectButton="true"
                                                AllowPaging="true"
                                                PageSize="5"
                                                OnPageIndexChanging="gvCasosJudicial_PageIndexChanging"
                                                OnRowCommand="gvCasosJudicial_RowCommand">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="divGridFiltrado" runat="server">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView
                                                class="table table-striped table-hover border-left-primary border-bottom-primary"
                                                ID="gvCasosJudicialFiltrado"
                                                runat="server"
                                                AutoGenerateColumns="true"
                                                AutoGenerateSelectButton="true"
                                                AllowPaging="true"
                                                PageSize="6"
                                                OnPageIndexChanging="gvCasosJudicialFiltrado_PageIndexChanging"
                                                OnRowCommand="gvCasosJudicial_RowCommandFiltrado">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="divGridAbogado" runat="server">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView
                                                class="table table-striped table-hover border-left-primary border-bottom-primary"
                                                ID="gvFiltradoAbogado"
                                                runat="server"
                                                AutoGenerateColumns="true"
                                                AutoGenerateSelectButton="true"
                                                AllowPaging="true"
                                                PageSize="6"
                                                OnPageIndexChanging="gvCasosJudicialAbogado_PageIndexChanging"
                                                OnRowCommand="gvCasosJudicial_RowCommandFiltrado">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </nav>
                    </div>

                    <!-- DataTales Example -->
                    <div class="card shadow mb-4" id="dvTramitePrestamo" runat="server">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Información y etapas del Préstamo:</h6>
                        </div>
                        <nav class="card-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-xl-3 col-md-6 mb-4">
                                        <div class="card border-left-warning shadow h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                            <asp:Literal ID="litDiasDesdePrestamo" runat="server"></asp:Literal>
                                                        </div>
                                                        <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">
                                                            Días desde registro
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <i class="fas fa-calendar fa-2x text-gray-300"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Tabla Préstamos Judiciales -->
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView class="table table-striped table-hover border-left-info border-bottom-info"
                                                ID="gvEstadosJudiciales" runat="server" AutoGenerateColumns="true"
                                                OnRowCommand="gvEstadosJudiciales_RowCommand">
                                            </asp:GridView>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="txtNumPretamo" class="form-label">N° Préstamo:</label>
                                        <asp:TextBox ID="txtNumPretamo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtCausa" class="form-label">N° Causa:</label>
                                        <asp:TextBox ID="txtCausa" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtOficina" class="form-label">Oficina:</label>
                                        <asp:TextBox ID="txtOficina" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtOficial" class="form-label">Oficial:</label>
                                        <asp:TextBox ID="txtOficial" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="txtTipo" class="form-label">Tipo:</label>
                                        <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="dtAdjudicado" class="form-label">Fecha Adjudicado:</label>
                                        <asp:TextBox ID="dtAdjudicado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="dtUltimoPago" class="form-label">Fecha Último Pago:</label>
                                        <asp:TextBox ID="dtUltimoPago" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="dtProxVencimiento" class="form-label">Fecha Vencimiento:</label>
                                        <asp:TextBox ID="dtProxVencimiento" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="txtDeudaInicial" class="form-label">Deuda Inicial:</label>
                                        <asp:TextBox ID="txtDeudaInicial" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3" hidden>
                                        <label for="txtSaldoActual" class="form-label">Saldo Actual:</label>
                                        <asp:TextBox ID="txtSaldoActual" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtTransferido" class="form-label">Saldo Transferido a Judicial:</label>
                                        <%--<input type="text" class="form-control" id="txtTransferido" runat="server" readonly>--%>
                                        <asp:TextBox ID="txtTransferido" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtSaldoActualCartera" class="form-label">Saldo Actual Cartera:</label>
                                        <asp:TextBox ID="txtSaldoActualCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true" Style="font-weight: bold;"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="txtInteresExigibleCartera" class="form-label">Interés Exigible:</label>
                                        <asp:TextBox ID="txtInteresExigibleCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="txtMoraExigibleCartera" class="form-label">Mora Exigible:</label>
                                        <asp:TextBox ID="txtMoraExigibleCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="ddlAbogado" class="form-label">Abogado:</label>
                                        <select class="form-control" id="ddlAbogado" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="ddlTramite" class="form-label">Trámite:</label>
                                        <asp:DropDownList ID="ddlTramite" runat="server" CssClass="form-control" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="ddlMateria" class="form-label">Materia:</label>
                                        <select class="form-control" id="ddlMateria" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3">
                                        <label for="ddlMedidaCautelar" class="form-label">Medida Cautelar:</label>
                                        <asp:DropDownList ID="ddlMedidaCautelar" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Seleccione..." Value="" Selected="false"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="ddlJudicatura" class="form-label">Judicatura:</label>
                                        <asp:DropDownList ID="ddlJudicatura" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Seleccione..." Value="" Selected="false"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label class="form-label">Descripción:</label>
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" ReadOnly="true" EnableViewState="true"></asp:TextBox>
                                    </div>
                                </div>
                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                                <div class="row">
                                    <div class="col-md-4">
                                        <label for="ddlAccion" class="form-label">Acción Desarrollada:</label>
                                        <asp:DropDownList ID="ddlAccionD" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false">
                                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <!-- Campo de selección de fecha -->
                                    <div class="col-md-4" id="fechaRemateDiv" runat="server" style="display: none;">
                                        <label for="dtFechaRemate" class="form-label">Fecha del Remate:</label>
                                        <asp:TextBox ID="dtFechaRemate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="ceFechaRemate" runat="server" TargetControlID="dtFechaRemate" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="dtFechaIngreso" class="form-label">Fecha Ingreso:</label>
                                        <input type="date" class="form-control" id="dtFechaIngreso" runat="server" disabled>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-floating">
                                            <label for="floatingTextarea">Comentario:</label>
                                            <asp:TextBox ID="txtComentario" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6" hidden>
                                        <label for="dtFechaSistema" class="form-label">Fecha Sistema:</label>
                                        <input type="date" class="form-control" id="dtFechaSistema" runat="server" disabled>
                                    </div>
                                </div>
                            </div>
                            <div class="container-fluid">
                                <br>
                            </div>
                        </nav>
                    </div>
                </div>
                <!-- /.container-fluid -->

            </div>
            <!-- End of Main Content -->
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
