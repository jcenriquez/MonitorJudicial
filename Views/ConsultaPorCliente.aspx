<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaPorCliente.aspx.cs" Inherits="MonitorJudicial.ConsultaPorCliente" %>

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
                                    <div >
                                        <asp:Button ID="btnBuscar" runat="server" CssClass="btn btn-outline-success" Text="Buscar" OnClick="btnBuscar_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                <div class="col-md-12" id="txtNombresDiv" runat="server">
                                    <label for="txtNombres" class="form-label">Nombre completo:</label>
                                    <input type="text" class="form-control border-bottom-info" id="txtNombres" runat="server" readonly="readonly">
                                </div></div>
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
                                            <asp:GridView class="table table-striped table-hover" ID="gvPrestamos" runat="server" AutoGenerateColumns="true" AutoGenerateSelectButton="true" OnRowCommand="gvPrestamos_RowCommand">
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </nav>
                    </div>

                    <!-- DataTales Example -->
                    <div class="card shadow mb-4" id="divTramitePrestamo" runat="server">
                        <div class="card-header py-3">
                            <h6 class="m-0 font-weight-bold text-primary">Trámite Préstamo en Demanda Judicial:</h6>
                        </div>

                        <nav class="card-body">
                            <div class="container-fluid">
                                <form class="row g-3">
                                    <div class="col-md-6">
                                        <label for="numPretamo" class="form-label">N° Préstamo:</label>
                                        <input type="text" class="form-control" id="numPretamo" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="causa" class="form-label">N° Causa:</label>
                                        <input type="text" class="form-control" id="causa" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="oficina" class="form-label">Oficina:</label>
                                        <input type="text" class="form-control" id="oficina" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="oficial" class="form-label">Oficial:</label>
                                        <input type="text" class="form-control" id="oficial" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="tipo" class="form-label">Tipo:</label>
                                        <input type="text" class="form-control" id="tipo" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="adjudicado" class="form-label">Adjudicado:</label>
                                        <input type="text" class="form-control" id="adjudicado" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="ultimoPago" class="form-label">Último Pago:</label>
                                        <input type="text" class="form-control" id="ultimoPago" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="proxVencimiento" class="form-label">Próximo Vencimiento:</label>
                                        <input type="text" class="form-control" id="proxVencimiento" runat="server">
                                    </div>
                                    <div class="col-md-4">
                                        <label for="deudaInicial" class="form-label">Deuda Inicial:</label>
                                        <input type="text" class="form-control" id="deudaInicial" inputmode="decimal" runat="server">
                                    </div>
                                    <div class="col-md-4">
                                        <label for="saldoActual" class="form-label">Saldo Actual:</label>
                                        <input type="text" class="form-control" id="saldoActual" inputmode="decimal" runat="server">
                                    </div>
                                    <div class="col-md-4">
                                        <label for="transferido" class="form-label">Saldo Transferido a Judicial:</label>
                                        <input type="text" class="form-control" id="transferido" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="inlineAbogado" class="form-label">Abogado:</label>
                                        <select class="form-control" id="inlineAbogado" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="inlineTramite" class="form-label">Trámite:</label>
                                        <select class="form-control" id="inlineTramite" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="inlineMateria" class="form-label">Materia:</label>
                                        <select class="form-control" id="inlineMateria" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="inlineMedidaCautelar" class="form-label">Medida Cautelar:</label>
                                        <select class="form-control" id="inlineMedidaCautelar" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-12">
                                        <label for="inlineJudicatura" class="form-label">Judicatura:</label>
                                        <select class="form-control" id="inlineJudicatura" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-12">
                                        <label for="descripcion" class="form-label">Descripción:</label>
                                        <input type="text" class="form-control" id="descripcion" runat="server">
                                    </div>
                                    <div class="col-md-12">
                                        <label for="inlineAccion" class="form-label">Acción Desarrollada:</label>
                                        <select class="form-control" id="inlineAccion" runat="server">
                                            <option selected>Seleccione...</option>
                                        </select>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="form-floating">
                                            <label for="floatingTextarea">Comentario:</label>
                                            <textarea class="form-control" placeholder="Comentario..." id="txtComentario" runat="server"></textarea>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="fechaIngreso" class="form-label">Fecha Ingreso:</label>
                                        <input type="date" class="form-control" id="fechaIngreso" runat="server">
                                    </div>
                                    <div class="col-md-6">
                                        <label for="fechaSistema" class="form-label">Fecha Sistema:</label>
                                        <input type="date" class="form-control" id="fechaSistema" runat="server">
                                    </div>
                                    <div class="col-12">
                                        <div class="form-check">
                                            <input class="form-check-input" type="checkbox" id="gridCheck">
                                            <label class="form-check-label" for="gridCheck">
                                                Está Activo
                                            </label>
                                        </div>
                                    </div>
                                    <%--<div class="col-12">
                                        <button type="Actualizar" class="btn btn-primary">Sign in</button>
                                    </div>--%>
                                </form>
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
