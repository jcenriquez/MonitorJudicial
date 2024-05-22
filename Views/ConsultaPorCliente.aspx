<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaPorCliente.aspx.cs" Inherits="MonitorJudicial.ConsultaPorCliente" MaintainScrollPositionOnPostback="true" %>

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
                                        <input type="search" id="idConsulta" class="form-control" placeholder="N° de Cédula o de Cliente" aria-label="Buscar" maxlength="15" validationgroup="grupoNumeroEntero" onkeypress="return soloNumeros(event);" runat="server">
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

                            <!-- Tabla Préstamos Judiciales -->
                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView class="table table-striped table-hover border-left-info border-bottom-info" ID="gvEstadosJudiciales" runat="server" AutoGenerateColumns="true">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="container-fluid">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="form-check">
                                            <asp:CheckBox ID="gridCheck" runat="server" CssClass="form-check-input" Enabled="false" />
                                            <label class="form-check-label" for="gridCheck">
                                                Está Activo
                                            </label>
                                        </div>
                                    </div>
                                </div>
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
                                        <label for="dtAdjudicado" class="form-label">Adjudicado:</label>
                                        <asp:TextBox ID="dtAdjudicado" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="dtUltimoPago" class="form-label">Último Pago:</label>
                                        <asp:TextBox ID="dtUltimoPago" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="dtProxVencimiento" class="form-label">Próximo Vencimiento:</label>
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
                                    <div class="col-md-12">
                                        <label for="ddlJudicatura" class="form-label">Judicatura:</label>
                                        <select class="form-control" id="ddlJudicatura" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <label for="ddlAccion" class="form-label">Acción Desarrollada:</label>
                                        <select class="form-control" id="ddlAccion" runat="server" disabled>
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label class="form-label">Descripción:</label>
                                        <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" ReadOnly="true" EnableViewState="true"></asp:TextBox>
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
                                            <asp:Button ID="btnCancelarEstadoPrestamo" runat="server" CssClass="btn-block btn-dark" Text="Cancelar y Volver" Visible="false" OnClientClick="history.back(); return false;" />
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
