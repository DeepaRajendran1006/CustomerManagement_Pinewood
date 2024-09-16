<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerManagement.aspx.cs" Inherits="CustomerApplication.CustomerManagement" Async="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Information</title>
</head>
<body>
    <form id="form1" runat="server">
<%--        <script type="text/javascript">
            function restrictToDigitsOnly(input)
                {
                input.value = input.value.replace(/[^0-9]/g, ''); // Remove non-digit characters
                }
        </script>--%>
        <div>
            <h2><u><b>Customer Information</b></u></h2>

            <table style="align-content:center">
                <tr>
                    <td>Name :</td>
                    <td><asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." Display="None" ForeColor="Red" ValidationGroup="Submit">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Email Id :</td>
                    <td><asp:TextBox ID="txtEmailId" runat="server"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailId" ErrorMessage="Please enter a valid email address." 
                            ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$" Display="None" ForeColor="Red" ValidationGroup="Submit">
                        </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmailId" ErrorMessage="Email is required." Display="None" ForeColor="Red" ValidationGroup="Submit">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>Phone Number :</td>
                    <td><asp:TextBox ID="txtPhoneNumber" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="revPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Please enter a valid 10-digit phone number." 
                            ValidationExpression="^\d{10}$" Display="None" ForeColor="Red" ValidationGroup="Submit">
                        </asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Phone number is required." Display="None" ForeColor="Red" ValidationGroup="Submit">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnAdd" runat="server" Text="Add Customer" OnClick="btnAdd_Click" ValidationGroup="Submit" />
                        <asp:Button ID="btnUpdate" runat="server" Text="Update Customer" OnClick="btnUpdate_Click" ValidationGroup="Submit"/>   
                        <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_Click" CausesValidation="false"/> 
                        
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td colspan ="2">
                        <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary ID="vsSummary" runat="server" DisplayMode="BulletList" HeaderText="Please fix the following errors:" ForeColor="Red" ValidationGroup="Submit"/>
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td colspan="2"></td>
                </tr>
            </table>

            <h3>
                <u>List of Customers</u>
            </h3>

            <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" DataKeyNames="Id" OnRowCommand="gvCustomers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Customer ID" Visible="false"/>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email Id" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                    <asp:TemplateField AccessibleHeaderText="Action" HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" CommandName ="EditCustomer" CommandArgument='<%#Eval("Id") %>' Text="Edit" CausesValidation="false"/>
                            <asp:Button ID="btnDelete" runat="server" CommandName ="DeleteCustomer" CommandArgument='<%#Eval("Id") %>' Text="Delete" CausesValidation="false"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
