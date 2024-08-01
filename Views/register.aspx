<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="MonitorJudicial.Views.register" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>SB Admin 2 - Register</title>

    <!-- Custom fonts for this template-->
    <link href="/Content/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <link href="/Content/css/sb-admin-2.min.css" rel="stylesheet" />
</head>

<body class="bg-gradient-primary">

    <div class="container">
        <div class="card o-hidden border-0 shadow-lg my-5">
            <div class="card-body p-0">
                <!-- Nested Row within Card Body -->
                <div class="row">
                    <div class="col-lg-5 d-none d-lg-block bg-register-image"></div>
                    <div class="col-lg-7">
                        <div class="p-5">
                            <div class="text-center">
                                <h1 class="h4 text-gray-900 mb-4">Crear una Cuenta!</h1>
                            </div>
                            <form class="user" id="registerForm" runat="server">
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtusuario" CssClass="form-control" Placeholder="Usuario" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">

                                        <asp:DropDownList ID="ddlCategories" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Seleccione un Rol:" Value="" Enabled="true" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Administrativo" Value="Administrativo"></asp:ListItem>
                                            <asp:ListItem Text="Judicial" Value="Judicial"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" Placeholder="Nombres" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtLastName" CssClass="form-control" Placeholder="Apellidos" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:TextBox ID="txtEmail" CssClass="form-control" Placeholder="Email" runat="server" TextMode="Email"></asp:TextBox>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtPassword" CssClass="form-control" Placeholder="Contraseña" runat="server" TextMode="Password"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtRepeatPassword" CssClass="form-control" Placeholder="Repita la Contraseña" runat="server" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                                <asp:Button ID="btnRegister" CssClass="btn btn-primary btn-user btn-block" Text="Registrar Usuario" OnClick="btnRegister_Click" runat="server" />
                                <%--<hr />
                                <a href="login.aspx" class="btn btn-primary btn-user btn-block">Register Account</a>
                                <hr />
                                <a href="index.aspx" class="btn btn-google btn-user btn-block">
                                    <i class="fab fa-google fa-fw"></i> Register with Google
                                </a>
                                <a href="index.aspx" class="btn btn-facebook btn-user btn-block">
                                    <i class="fab fa-facebook-f fa-fw"></i> Register with Facebook
                                </a>--%>
                            </form>
                            <hr />
                            <div class="text-center">
                                <a class="small" href="forgot-password.aspx">Forgot Password?</a>
                            </div>
                            <div class="text-center">
                                <a class="small" href="login.aspx">Already have an account? Login!</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap core JavaScript-->
    <script src="/Content/vendor/jquery/jquery.min.js"></script>
    <script src="/Content/vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="/Content/vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="/Content/js/sb-admin-2.min.js"></script>

</body>

</html>
