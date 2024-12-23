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

    <style>
        .validation-error {
            color: red;
        }
    </style>
</head>

<body class="bg-gradient-primary">

    <div class="container">
        <div class="card o-hidden border-0 shadow-lg my-5">
            <div class="card-body p-0">
                <!-- Nested Row within Card Body -->
                <div class="row">
                    <div class="col-lg-5 d-none d-lg-block bg-login-image">
                        <img src="/Content/img/register.svg" alt="Login" style="max-width: 100%;">
                    </div>
                    <div class="col-lg-7">
                        <div class="p-5">
                            <div class="text-center">
                                <h1 class="h4 text-gray-900 mb-4">Crear una Cuenta!</h1>
                            </div>
                            <form class="user" id="registerForm" runat="server">
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtUsuario" CssClass="form-control" Placeholder="Usuario" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvUsuario"
                                            ControlToValidate="txtUsuario"
                                            ErrorMessage="El campo Usuario es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="revUsuario"
                                            ControlToValidate="txtUsuario"
                                            ErrorMessage="El campo Usuario no debe exceder los 25 caracteres."
                                            CssClass="validation-error"
                                            ValidationExpression="^.{1,50}$"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                    <div class="col-sm-6">

                                        <asp:DropDownList ID="ddlCategories" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Seleccione un Rol:" Value="" Enabled="true" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="ADMINISTRATIVO COAC" Value="Administrativo"></asp:ListItem>
                                            <asp:ListItem Text="FUNCIONARIO JUDICIAL" Value="Judicial"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            ID="rfvCategories"
                                            ControlToValidate="ddlCategories"
                                            InitialValue=""
                                            ErrorMessage="Debe seleccionar un rol válido."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />

                                    </div>
                                </div>
                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtFirstName" CssClass="form-control" Placeholder="Nombres" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvFirstName"
                                            ControlToValidate="txtFirstName"
                                            ErrorMessage="El campo Nombres es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="revFirstName"
                                            ControlToValidate="txtFirstName"
                                            ErrorMessage="El campo Nombres no debe exceder los 50 caracteres."
                                            CssClass="validation-error"
                                            ValidationExpression="^.{1,50}$"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtLastName" CssClass="form-control" Placeholder="Apellidos" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvLastName"
                                            ControlToValidate="txtLastName"
                                            ErrorMessage="El campo Apellidos es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="revLastName"
                                            ControlToValidate="txtLastName"
                                            ErrorMessage="El campo Nombres no debe exceder los 50 caracteres."
                                            CssClass="validation-error"
                                            ValidationExpression="^.{1,50}$"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                </div>
                                <div class="form-group row">

                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtEmail" CssClass="form-control" Placeholder="Email" runat="server" TextMode="Email"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvEmail"
                                            ControlToValidate="txtEmail"
                                            ErrorMessage="El campo Email es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="revEmail"
                                            ControlToValidate="txtEmail"
                                            ErrorMessage="El formato del Email no es válido."
                                            CssClass="validation-error"
                                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:DropDownList ID="ddlAbogado" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Seleccione Abogado Asignado:" Value="" Enabled="true" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="ADMINISTRATIVO COAC" Value="Administrativo"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            ID="rfvAbogado"
                                            ControlToValidate="ddlAbogado"
                                            InitialValue=""
                                            ErrorMessage="Debe seleccionar un abogado asignado."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                </div>

                                <div class="form-group row">
                                    <div class="col-sm-6 mb-3 mb-sm-0">
                                        <asp:TextBox ID="txtPassword" CssClass="form-control" Placeholder="Contraseña" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvPassword"
                                            ControlToValidate="txtPassword"
                                            ErrorMessage="El campo Contraseña es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:RegularExpressionValidator
                                            ID="revPassword"
                                            ControlToValidate="txtPassword"
                                            ErrorMessage="La contraseña debe tener al menos 8 caracteres, incluir letras minúsculas, mayúsculas, números y caracteres especiales."
                                            CssClass="validation-error"
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                    <div class="col-sm-6">
                                        <asp:TextBox ID="txtRepeatPassword" CssClass="form-control" Placeholder="Repita la Contraseña" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            ID="rfvRepeatPassword"
                                            ControlToValidate="txtRepeatPassword"
                                            ErrorMessage="Repetir la contraseña es obligatorio."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                        <asp:CompareValidator
                                            ID="cvPasswords"
                                            ControlToValidate="txtRepeatPassword"
                                            ControlToCompare="txtPassword"
                                            ErrorMessage="Las contraseñas no coinciden."
                                            CssClass="validation-error"
                                            Display="Dynamic"
                                            runat="server" />
                                    </div>
                                </div>
                                <asp:Button ID="btnRegister" CssClass="btn btn-secondary btn-user btn-block" Text="Registrar Usuario" OnClick="btnRegister_Click" runat="server" />
                                <%--<hr />
                                <a href="Views/Login.aspx" class="btn btn-primary btn-user btn-block">Register Account</a>
                                <hr />
                                <a href="index.aspx" class="btn btn-google btn-user btn-block">
                                    <i class="fab fa-google fa-fw"></i> Register with Google
                                </a>
                                <a href="index.aspx" class="btn btn-facebook btn-user btn-block">
                                    <i class="fab fa-facebook-f fa-fw"></i> Register with Facebook
                                </a>--%>
                            </form>
                            <hr />
                            <%--<div class="text-center">
                                <a class="small" href="forgot-password.aspx">Forgot Password?</a>
                            </div>
                            <div class="text-center">
                                <a class="small" href="Views/Login.aspx">Already have an account? Login!</a>
                            </div>--%>
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
