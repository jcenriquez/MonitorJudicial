<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaPorCliente.aspx.cs" Inherits="MonitorJudicial.ConsultaPorCliente" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function soloNumeros(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }

    </script>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            var value = evt.target.value;
            var dotPos = value.indexOf('.');

            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            // Permitir solo un punto decimal
            if (dotPos > -1 && charCode == 46) {
                return false;
            }
            return true;
        }
    </script>
    <!-- Page Wrapper -->
    <div id="wrapper">

        <!-- Content Wrapper -->
        <div id="content-wrapper" class="d-flex flex-column">

            <!-- Main Content -->
            <div id="content">

                <!-- Begin Page Content -->
                <div class="container-fluid">

                    <!-- Page Heading -->
                    <h1 class="h3 mb-2 text-gray-800">Consultas</h1>
                    <%--<p class="mb-4">DataTables is a third party plugin that is used to generate the demo table below.
                        For more information about DataTables, please visit the <a target="_blank"
                            href="https://datatables.net">official DataTables documentation</a>.</p>--%>

                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Cliente:</h6>
                        </div>

                        <nav class="card-body">
                            <div class="container-fluid">


                                <div class="row">
                                    <div class="col-md-4">
                                        <input type="search" id="idConsulta" class="form-control" placeholder="N° de Cédula o de Cliente" aria-label="Buscar" maxlength="20" runat="server">
                                    </div>
                                    <div class="col-md-2">
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="flexRadioDefault" id="rbCedula" checked runat="server">
                                            <label class="form-check-label" for="rbCedula">
                                                Por N° Cédula
                                            </label>
                                        </div>
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="flexRadioDefault" id="rbCliente" runat="server">
                                            <label class="form-check-label" for="rbCliente">
                                                Por N° Cliente
                                            </label>
                                        </div>
                                        <div class="form-check">
                                            <input class="form-check-input" type="radio" name="flexRadioDefault" id="rbCaso" runat="server">
                                            <label class="form-check-label" for="rbCaso">
                                                Por N° Causa
                                            </label>
                                        </div>
                                    </div>
                                    <div>
                                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-outline-success" Text="Buscar" OnClick="btnBuscar_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12" id="txtNombresDiv" runat="server">
                                        <label for="txtNombres" class="form-label">Nombre completo:</label>
                                        <input type="text" class="form-control border-left-primary border-bottom-primary" id="txtNombres" runat="server" readonly="readonly">
                                    </div>
                                </div>
                            </div>
                        </nav>
                    </div>

                    <!-- DataTales Example -->
                    <div class="card shadow mb-4">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Préstamos:</h6>
                        </div>
                        <nav class="card-body">
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView class="table table-striped table-hover border-left-primary border-bottom-primary" ID="gvPrestamos" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true" OnRowCommand="gvPrestamos_RowCommand">
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
                            <h6 class="m-0 font-weight-bold text-primary">Trámite Préstamo en Demanda Judicial:</h6>
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
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <%--<asp:LinkButton ID="lnkVer" runat="server" CommandName="Select" ToolTip="Ver" CommandArgument='<%# Eval("ID") %>'
                                                                OnClientClick="return confirm('¿Está seguro de que desea eliminar este registro? Esta acción es irreversible!');">
    <i class="fas fa-eye"></i>
                                                            </asp:LinkButton>--%>
                                                            <asp:LinkButton ID="lnkSelect" runat="server" CommandName="Select" ToolTip="Eliminar" CommandArgument='<%# Eval("ID") %>'
                                                                OnClientClick="return confirm('¿Está seguro de que desea eliminar este registro? Esta acción es irreversible!');">
                        <i class="fas fa-trash-alt"></i>
                                                            </asp:LinkButton>

                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="container-fluid">
                                <%--<div class="row">
                                    <div class="col-6">
                                        <div class="form-check">
                                            <asp:CheckBox ID="gridCheck" runat="server" CssClass="form-check-input" Enabled="false" />
                                            <label class="form-check-label" for="gridCheck">
                                                Está Activo
                                            </label>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="txtNumPretamo" class="form-label">N° Préstamo:</label>
                                        <asp:TextBox ID="txtNumPretamo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="txtCausa" class="form-label">N° Causa:</label>
                                        <asp:TextBox ID="txtCausa" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="txtOficina" class="form-label">Oficina:</label>
                                        <asp:TextBox ID="txtOficina" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="txtOficial" class="form-label">Oficial:</label>
                                        <asp:TextBox ID="txtOficial" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="txtTipo" class="form-label">Tipo:</label>
                                        <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="dtAdjudicado" class="form-label">Fecha Adjudicado:</label>
                                        <asp:TextBox ID="dtAdjudicado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="dtUltimoPago" class="form-label">Fecha Último Pago:</label>
                                        <asp:TextBox ID="dtUltimoPago" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="dtProxVencimiento" class="form-label">Fecha Vencimiento:</label>
                                        <asp:TextBox ID="dtProxVencimiento" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-4">
                                        <label for="txtDeudaInicial" class="form-label">Deuda Inicial:</label>
                                        <asp:TextBox ID="txtDeudaInicial" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="txtSaldoActual" class="form-label">Saldo Actual:</label>
                                        <asp:TextBox ID="txtSaldoActual" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="txtTransferido" class="form-label">Saldo Transferido a Judicial:</label>
                                        <%--<input type="text" class="form-control" id="txtTransferido" runat="server" readonly>--%>
                                        <asp:TextBox ID="txtTransferido" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-4">
                                        <label for="txtSaldoActualCartera" class="form-label">Saldo Actual Cartera:</label>
                                        <asp:TextBox ID="txtSaldoActualCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="txtInteresExigibleCartera" class="form-label">Interés Exigible:</label>
                                        <asp:TextBox ID="txtInteresExigibleCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="txtMoraExigibleCartera" class="form-label">Mora Exigible:</label>
                                        <asp:TextBox ID="txtMoraExigibleCartera" runat="server" CssClass="form-control" inputmode="decimal" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="ddlAbogado" class="form-label">Abogado:</label>
                                        <select class="form-control" id="ddlAbogado" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="ddlTramite" class="form-label">Trámite:</label>
                                        <asp:DropDownList ID="ddlTramite" runat="server" CssClass="form-control" Enabled="false">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="ddlMateria" class="form-label">Materia:</label>
                                        <select class="form-control" id="ddlMateria" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="ddlMedidaCautelar" class="form-label">Medida Cautelar:</label>
                                        <asp:DropDownList ID="ddlMedidaCautelar" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Seleccione..." Value="" Selected="false"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="ddlJudicatura" class="form-label">Judicatura:</label>

                                        <%-- <select class="form-control" id="" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>--%>

                                        <asp:DropDownList ID="ddlJudicatura" runat="server" CssClass="form-control" Enabled="false">
                                            <asp:ListItem Text="Seleccione..." Value="" Selected="false"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Descripción:</label>
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" ReadOnly="true" EnableViewState="true"></asp:TextBox>
                                    </div>
                                </div>

                                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="ddlAccion" class="form-label">Acción Desarrollada:</label>
                                        <asp:DropDownList ID="ddlAccion" runat="server" CssClass="form-control" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlAccion_SelectedIndexChanged">
                                            <asp:ListItem Text="Seleccione..." Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <!-- Campo de selección de fecha -->
                                    <div class="col-md-6" id="fechaRemateDiv" runat="server" style="display: none;">
                                        <label for="dtFechaRemate" class="form-label">Fecha del Remate:</label>
                                        <asp:TextBox ID="dtFechaRemate" runat="server" CssClass="form-control"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="ceFechaRemate" runat="server" TargetControlID="dtFechaRemate" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
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
                                    <div class="col-md-6">
                                        <label for="dtFechaIngreso" class="form-label">Fecha Ingreso:</label>
                                        <input type="date" class="form-control" id="dtFechaIngreso" runat="server" disabled>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="dtFechaSistema" class="form-label">Fecha Sistema:</label>
                                        <input type="date" class="form-control" id="dtFechaSistema" runat="server" disabled>
                                    </div>
                                </div>


                            </div>

                            <div class="container-fluid">


                                <br>
                                <!-- Botones -->
                                <div class="container-fluid">
                                    <div class="row">
                                        <div class="col-md-12 ">
                                            <asp:Button ID="btnActualizarEstadoPrestamo" runat="server" CssClass="btn-block btn-primary" Text="Actualizar Estado Préstamo" OnClick="btnActualizarEstadoPrestamo_Click" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12 ">
                                            <asp:Button ID="btnGuardarEstadoPrestamo" runat="server" CssClass="btn-block btn-success" Text="Guardar Estado Préstamo" OnClick="btnGuardarEstadoPrestamo_Click" Visible="false" AutoPostBack="false" EnableViewState="true" />
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12 t">
                                            <asp:Button ID="btnCancelarEstadoPrestamo" runat="server" CssClass="btn-block btn-dark" Text="Cancelar y Volver" Visible="false" OnClick="btnCancelarEstadoPrestamo_Click" />
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
                <div class="modal-body">Seleccione "Salir" a continuación si está listo para finalizar su sesión actual.</div>
                <div class="modal-footer">
                    <asp:Button ID="btnLogout" runat="server" Text="Cerrar sesión" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
