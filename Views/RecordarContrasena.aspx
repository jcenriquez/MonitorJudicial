<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordarContrasena.aspx.cs" Inherits="MonitorJudicial.Views.RecordarContrasena" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <title>Monitor Judicial - Recuperar Contraseña</title>
    <link href="/Content/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">
    <link href="/Content/css/sb-admin-2.min.css" rel="stylesheet">
</head>
<body class="bg-gradient-primary">
    <form id="form1" runat="server">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-xl-10 col-lg-12 col-md-9">
                    <div class="card o-hidden border-0 shadow-lg my-5">
                        <div class="card-body p-0">
                            <div class="row">
                                <div class="col-lg-6 d-none d-lg-block bg-login-image">
                                    <img src="/Content/img/password-laptop.jpg" alt="Forgot" style="max-width: 100%;">
                                </div>
                                <div class="col-lg-6">
                                    <div class="p-5">
                                        <div class="text-center">
                                            <h1 class="h4 text-gray-900 mb-4">Restablecer Contraseña</h1>
                                        </div>
                                        <asp:Panel runat="server" CssClass="user">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-user" Placeholder="Correo Electrónico" ></asp:TextBox>
                                            </div>
                                            <asp:Button ID="btnSendResetLink" runat="server" CssClass="btn btn-primary btn-user btn-block" Text="Recordar contraseña" OnClick="btnSendResetLink_Click" />
                                        </asp:Panel>
                                        <hr>
                                        <div class="text-center">
                                            <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Content/vendor/jquery/jquery.min.js"></script>
    <script src="/Content/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="/Content/vendor/jquery-easing/jquery.easing.min.js"></script>
    <script src="/Content/js/sb-admin-2.min.js"></script>
</body>
</html>
