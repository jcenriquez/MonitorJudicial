<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FichaCliente.aspx.cs" Inherits="MonitorJudicial.Tests.FichaCliente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    
       
            <div class="row">
                <div class="col-md-12">
                    <h2>Ficha del Cliente</h2>
                    <div class="customer-status">
                        <label>Estado de la Persona:</label>
                        <div>
                            <asp:Label ID="lblEstado" runat="server" Text="" CssClass="badge"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
       
    
    <script>
        // Este script podría actualizar la clase y el texto del badge basado en el estado del cliente.
        // Por ejemplo:
        document.addEventListener('DOMContentLoaded', function () {
            var estado = '<%= EstadoCliente %>'; // Supongamos que tienes una variable EstadoCliente en el code-behind
            var lblEstado = document.getElementById('<%= lblEstado.ClientID %>');

            if (estado === 'vivo') {
                lblEstado.className = 'badge badge-success';
                lblEstado.innerText = 'VIDA';
            } else if (estado === 'fallecido') {
                lblEstado.className = 'badge badge-danger';
                lblEstado.innerText = 'FALLECIDO';
            }
        });
    </script>

</asp:Content>
